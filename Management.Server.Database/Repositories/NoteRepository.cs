using System.Threading.Tasks;
using Management.Server.Core;
using Management.Server.Database.Models.Note;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database.Repositories
{
    public class NoteRepository : AbstractRepository<NoteModel>
    {
        public NoteRepository(PostgreSqlContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }


        public async Task<NoteModel> GetOne(int id)
        {
            var model = await FindOne(id);
            if (model == null)
            {
                throw new ErrorException(Error.InvalidError("Note not found"));
            }

            return model;
        }

    }
}