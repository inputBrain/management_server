using Management.Server.Model.Note;
using Management.Server.Model.User;
using Xunit;

namespace Management.Server.Test.Repositories
{
    public class NoteRepositoryTest : BaseDbTestCase
    {
        [Fact]
        public void CreateModelTest()
        {
            var userModel = Registry.User.CreateModel(
                "string@string.string",
                "FirstName",
                "LastName",
                UserStatus.Confirmed
            ).Result;

            Assert.NotNull(userModel);

            var patch = new NotePatch(
                "Perfect note",
                "Hello world",
                NoteStatus.Disabled
            );

            var model = Registry.Note.CreateModel(
                userModel.Id,
                patch.Title,
                patch.Description,
                patch.NoteStatus
            ).Result;


            Assert.NotNull(model);
            Assert.Equal(userModel.Id, model.AuthorId);
            Assert.Equal(patch.Title, model.Title);
            Assert.Equal(patch.Description, model.Description);
            Assert.Equal(patch.NoteStatus, model.NoteStatus);
        }


        [Fact]
        public void UpdatePatchTest()
        {
            var userModel = Registry.User.CreateModel(
                "string@string.string",
                "FirstName",
                "LastName",
                UserStatus.Confirmed
            ).Result;

            var model = Fixtures.NoteFixture.CreateNoteModel(Registry.Note, userModel);

            var patch = new NotePatch(
                "It is not a perfect note :(",
                "Success or die",
                NoteStatus.Published
            );

            var updatedModel = Registry.Note.UpdatePatch(model, patch).Result;

            Assert.NotNull(updatedModel);
            Assert.Equal(patch.Title, updatedModel.Title);
            Assert.Equal(patch.Description, updatedModel.Description);
            Assert.Equal(patch.NoteStatus, updatedModel.NoteStatus);
        }
    }
}