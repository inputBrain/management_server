using Management.Server.Database.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database
{
    public sealed class PostgreSqlContext : DbContext
    {
        public DbSet<UserModel> User { get; set; }


        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options, ILoggerFactory loggerFactory) :
            base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}