using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Server.Core;
using Management.Server.Database.Models.User;
using Management.Server.Model.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database.Repositories
{
    public class UserRepository : AbstractRepository<UserModel>
    {
        public UserRepository(PostgreSqlContext db, ILoggerFactory loggerFactory) : base(db, loggerFactory)
        {
        }


        public async Task<UserModel> CreateModel(
            string email,
            string phone,
            string name
        )
        {
            var model = UserModel.Create
            (
                email,
                phone,
                name,
                UserStatus.Ok
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


        public async Task<UserModel> FindModelByEmail(string email)
        {
            return await DbModel.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<UserModel> GetModelByEmail(string email)
        {
            var userModel = await FindModelByEmail(email);

            if (userModel == null)
            {
                throw ErrorException.DbException("Get User Error. User not exist with email " + email);
            }

            return userModel;
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


        public Task<List<UserModel>> FindByIds(List<int> userIds)
        {
            return DbModel
                   .Where(x => userIds.Contains(x.Id))
                   .ToListAsync();
        }
    }
}