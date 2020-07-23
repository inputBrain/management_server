using System;
using System.IO;
using Management.Server.Database;
using Management.Server.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Management.Server.Test
{
    public abstract class BaseDbTestCase : IDisposable
    {
        private readonly DbContextOptions<PostgreSqlContext> _options;
        protected readonly PostgreSqlContext Context;
        protected readonly DatabaseFacade Registry;
        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }


        public BaseDbTestCase()
        {
            var config = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json")
                         .Build();

            if (config == null)
            {
                throw new Exception("File config not found");
            }

            _options = new DbContextOptionsBuilder<PostgreSqlContext>()
                       .UseNpgsql(
                           config.GetConnectionString("PostgreSqlConnection"),
                           b => b.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name)
                       )
                       .Options;

            LoggerFactory = new LoggerFactory();
            Context = new PostgreSqlContext(_options, LoggerFactory);
            Context.Database.Migrate();
            var logger = LoggerFactory.CreateLogger<BaseDbTestCase>();
            Logger = logger;
            Registry = Context.Db;
        }


        public void Dispose()
        {
            using (var context = new PostgreSqlContext(_options, LoggerFactory))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}