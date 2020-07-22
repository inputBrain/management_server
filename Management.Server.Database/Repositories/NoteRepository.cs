using Management.Server.Database.Models.Note;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database.Repositories
{
    public class NoteRepository : AbstractRepository<NoteModel>
    {
        public NoteRepository(PostgreSqlContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }



    }
}