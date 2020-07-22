using Management.Server.Database.Repositories;
using Management.Server.Database.Services;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database
{
    public class DatabaseFacade
    {
        public readonly UserRepository User;
        public readonly NoteRepository Note;
        public readonly ServiceContainer ServiceContainer;


        public DatabaseFacade(PostgreSqlContext db, ILoggerFactory loggerFactory)
        {
            User = new UserRepository(db, loggerFactory);
            Note = new NoteRepository(db, loggerFactory);

            ServiceContainer = new ServiceContainer(
                new UserService(User, loggerFactory)
            );
        }
    }
}