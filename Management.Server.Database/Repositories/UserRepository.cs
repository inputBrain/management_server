using System.Threading.Tasks;
using Management.Server.Core;
using Management.Server.Database.Models.User;
using Management.Server.Model.User;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database.Repositories
{
    public class UserRepository : AbstractRepository<UserModel>
    {
        public UserRepository(PostgreSqlContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }


        public async Task<UserModel> CreateModel(
            string email,
            string firstName,
            string lastName,
            UserStatus status
        )
        {
            var model = UserModel.CreateModel
            (
                email,
                firstName,
                lastName,
                status
            );

            var result = await CreateModelAsync(model);
            if (result == null)
            {
                throw new ErrorException(
                    Error.Create("User is not create")
                );
            }

            return model;
        }


        public async Task<UserModel> CreateModel(UserModel model)
        {
            var result = await CreateModelAsync(model);
            if (result == null)
            {
                throw new ErrorException(Error.Create("User not created", ErrorType.DbError));
            }

            return result;
        }


        public async Task<UserModel> GetOne(int id)
        {
            var userModel = await FindOne(id);
            if (userModel == null)
            {
                throw ErrorException.DbException("Get User Error. User not exist with id " + id);
            }

            return userModel;
        }

    }
}