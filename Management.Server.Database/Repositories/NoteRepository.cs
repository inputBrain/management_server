using System.Threading.Tasks;
using Management.Server.Core;
using Management.Server.Database.Models.Note;
using Management.Server.Model.Note;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database.Repositories
{
    public class NoteRepository : AbstractRepository<NoteModel>
    {
        public NoteRepository(PostgreSqlContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }


        public async Task<NoteModel> CreateModel(NoteModel model)
        {
            var result = await CreateModelAsync(model);
            if (result == null)
            {
                throw new ErrorException(Error.Create("Note is not created", ErrorType.DbError));
            }

            return result;
        }


        public Task<NoteModel> CreateModel(
            int authorId,
            NotePatch notePatch
        )
        {
            return CreateModel(
                authorId,
                notePatch.Title,
                notePatch.Description,
                notePatch.NoteStatus
            );
        }


        public async Task<NoteModel> CreateModel(
            int authorId,
            string title,
            string description,
            NoteStatus status
        )
        {
            var model = NoteModel.CreateModel(
                authorId,
                title,
                description,
                status
            );

            var result = await CreateModelAsync(model);
            if (result == null)
            {
                throw new ErrorException(Error.Create("Note is not created", ErrorType.DbError));
            }

            return await GetOne(model.Id);
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


        public async Task<NoteModel> UpdatePatch(NoteModel model, NotePatch notePatch)
        {
            if (model.IsSame(notePatch))
            {
                return model;
            }

            model.UpdateByNotePatch(notePatch);

            var result = await UpdateModelAsync(model);
            if (result == 0)
            {
                throw new ErrorException(Error.Create("Note Model not updated", ErrorType.DbError));
            }

            return model;
        }
    }
}