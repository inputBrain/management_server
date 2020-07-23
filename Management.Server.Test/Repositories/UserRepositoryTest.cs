using Management.Server.Model.User;
using Xunit;

namespace Management.Server.Test.Repositories
{
    public class UserRepositoryTest : BaseDbTestCase
    {
        [Fact]
        public void CreateModelTest()
        {
            var model = Registry.User.CreateModel(
                "string@string.string",
                "FirstName",
                "LastName",
                UserStatus.NotConfirmed).Result;

            Assert.NotNull(model);
            Assert.Equal("string@string.string", model.Email);
            Assert.Equal("FirstName", model.FirstName);
            Assert.Equal("LastName", model.LastName);
            Assert.Equal(UserStatus.NotConfirmed, model.Status);
        }
    }
}