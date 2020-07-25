using System.Threading.Tasks;
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

            Task.Delay(1000).Wait();

            var anotherPatch = new NotePatch(
                "It is not a perfect note :(",
                "Success or die",
                NoteStatus.Published
            );

            var updatedModel = Registry.Note.UpdatePatch(model, anotherPatch).Result;

            Assert.NotNull(updatedModel);
            Assert.Equal(anotherPatch.Title, updatedModel.Title);
            Assert.Equal(anotherPatch.Description, updatedModel.Description);
            Assert.Equal(anotherPatch.NoteStatus, updatedModel.NoteStatus);
        }
    }
}