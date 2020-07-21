using Management.Server.Database.Repositories;
using Management.Server.Database.Services;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database
{
    public static class Factory
    {
        public static ServiceContainer CreateServiceContainer(DatabaseContainer databaseContainer, ILoggerFactory loggerFactory)
        {
            return new ServiceContainer(new UserService(databaseContainer.User, loggerFactory));
        }


        public static DatabaseContainer CreateDatabaseContainer(PostgreSqlContext db, ILoggerFactory loggerFactory)
        {
            var userRepository = new UserRepository(db, loggerFactory);
            return new DatabaseContainer(userRepository);
        }
    }
}