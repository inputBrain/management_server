using System.Threading.Tasks;
using Management.Server.Core;
using Management.Server.Database.Models.User;
using Management.Server.Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database.Services
{
    public sealed class UserService
    {
        private readonly UserRepository _repository;
        private readonly ILogger _logger;


        public UserService(UserRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<UserService>();
        }


        public async Task<UserModel> CreateUser(
            string email,
            string phone,
            string name
        )
        {
            var exist = await _repository.FindModelByEmail(email);
            if (exist != null)
            {
                throw ErrorException.InvalidDataException("User with this name already exist");
            }

            var model = await _repository.CreateModel(
                email,
                phone,
                name
            );

            return model;
        }
    }
}