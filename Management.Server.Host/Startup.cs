using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Management.Server.Core;
using Management.Server.Database;
using Management.Server.Host.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Management.Server.Host
{
    /*
     * TODO: This class was not written by me.
     */
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

            static readonly string _RequireAuthenticatedUserPolicy =
                            "RequireAuthenticatedUserPolicy";

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            TunePerformance();
            Task.Run(() => StartThreadPoolMonitoringTask(Configuration.GetSection("ThreadPoolMonitoring").Get<bool>()));
            ConfigureJwtAuth(services);

            services.AddCors(
                options =>
                {
                    options.AddPolicy(
                        "CorsPolicy",
                        policy =>
                            policy.WithOrigins(Configuration.GetSection("AllowedHosts").Value)
                                .WithMethods("POST", "GET", "DELETE")
                                .WithHeaders("*")
                    );
                    options.AddPolicy(
                        "apiDocumentation",
                        policy =>
                            policy.WithOrigins("*")
                                .WithMethods("POST", "GET", "DELETE")
                                .WithHeaders("*")
                    );
                }
            );

            Type typeOfContent = typeof(Startup);
            services.AddDbContext<PostgreSqlContext>(
                opt => opt.UseNpgsql(
                    Configuration.GetConnectionString("PostgreSqlConnection"),
                    b => b.MigrationsAssembly(typeOfContent.Assembly.GetName().Name)
                )
            );

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddAuthorization(o => o.AddPolicy(_RequireAuthenticatedUserPolicy,
                builder => builder.RequireAuthenticatedUser()));

            services.AddMvc(
                        options => { options.Filters.Add(new ErrorHandlingFilter()); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(
                    options =>
                    {
                        // options.SerializerSettings.Converters.Add(new ValidEnumConverter());
                        // options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    }
                );

            ConfigureApi(services);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseBuffering();
            app.UseDeveloperExceptionPage();

            if (Convert.ToBoolean(Configuration.GetSection("Swagger:Enable")?.Value))
            {
                app.UseCors("apiDocumentation");
                app.UseSwagger(c => { c.RouteTemplate = "api-docs/{documentName}/swagger.json"; });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/api-docs/v1/swagger.json", "Client Api");
                    c.SwaggerEndpoint("/api-docs/admin/swagger.json", "Admin Api");
                    c.RoutePrefix = string.Empty;
                });
            }


            app.UseRouting();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseCors("CorsPolicy");
            app.UseMiddleware<ErrorWrappingMiddleware>();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,
                RequireHeaderSymmetry = false,
                ForwardLimit = null
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private void ConfigureJwtAuth(IServiceCollection services)
        {
            var signingKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(Configuration.GetSection("JWTSettings:SecretKey").Value)
            );

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                ValidateIssuer = true,
                ValidIssuer = Configuration.GetSection("JWTSettings:Issuer").Value,

                ValidateAudience = true,
                ValidAudience = Configuration.GetSection("JWTSettings:Audience").Value,

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero,
            };


            services.Configure<JwtSettings>(Configuration.GetSection("JWTSettings"));

            services.AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                )
                .AddJwtBearer(
                    jwtBearerOptions =>
                    {
                        jwtBearerOptions.TokenValidationParameters = tokenValidationParameters;
                        jwtBearerOptions.Events = new JwtBearerEvents()
                        {
                            OnTokenValidated = context =>
                            {
                                // Add the access_token as a claim, as we may actually need it
                                if (context.SecurityToken is JwtSecurityToken accessToken)
                                {
                                    if (context.Principal.Identity is ClaimsIdentity identity)
                                    {
                                        identity.AddClaim(new Claim("access_token", accessToken.RawData));
                                        identity.AddClaim(new Claim("email", accessToken.Subject));
                                    }
                                }

                                return Task.CompletedTask;
                            },
                            OnChallenge = context =>
                            {
                                var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                                    .CreateLogger(nameof(JwtBearerEvents));
                                logger.LogError("OnChallenge error" + context.Error + ' ' + context.ErrorDescription);
                                throw new ErrorException(
                                    Error.Create("need authorization or access forbidden", ErrorType.Auth)
                                );
                            }
                        };
                    }
                );
        }


        private void ConfigureApi(IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<AuthorizationOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "JWT Authorization header using the Bearer scheme" +
                                  "Example: \"Authorization: Bearer {token}\" "
                });

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Management Client API",
                        Version = "v1",
                        TermsOfService = null,
                        Description = "Api documentation for Management Client" +
                                      "<p>All data in GMT(type : number-int)</p>" +
                                      "<p>OK Response: 200</p>" +
                                      "<p>Error response: 406</p>" +
                                      "<p>Server error: 500</p>"
                    }
                );

                c.SwaggerDoc("admin", new OpenApiInfo
                {
                    Title = "Management Admin Panel",
                    Version = "admin",
                    Description = "Api for Management Admin panel CMS",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }


        private static async void StartThreadPoolMonitoringTask(bool isEnable = false)
        {
            if (isEnable == false)
            {
                return;
            }

            while (true)
            {
                ThreadPool.GetMaxThreads(out var maxWorker, out var maxEvent);
                ThreadPool.GetAvailableThreads(out var availableWorker, out var availableEvent);
                Console.WriteLine(
                    $"{DateTime.UtcNow}[{Thread.CurrentThread.ManagedThreadId}]: cur={maxWorker - availableWorker}:{maxEvent - availableEvent}, available threads={availableWorker}:{availableEvent}, max={maxWorker}:{maxEvent}"
                );
                await Task.Delay(5000);
            }
        }


        private static void TunePerformance()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 256; // Max concurrent outbound requests

            ThreadPool.GetMaxThreads(out _, out var completionThreads);
            ThreadPool.SetMinThreads(256, completionThreads); // or higher
        }
    }
}