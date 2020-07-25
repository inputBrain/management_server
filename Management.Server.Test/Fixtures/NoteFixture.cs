using System;
using Management.Server.Database.Models.Note;
using Management.Server.Database.Models.User;
using Management.Server.Database.Repositories;
using Management.Server.Model.Note;

namespace Management.Server.Test.Fixtures
{
    internal static class NoteFixture
    {
        public static NoteModel CreateNoteModel(
            NoteRepository noteRepository,
            UserModel userModel,
            NotePatch notePatch = null
        )
        {
            var uniqueName = new Guid().ToString("N");
            if (notePatch == null)
            {
                notePatch = new NotePatch(
                    "Title_" + uniqueName,
                    "Desc_" + uniqueName,
                    NoteStatus.Disabled
                );
            }

            return noteRepository.CreateModel(userModel.Id, notePatch).Result;
        }
    }
}