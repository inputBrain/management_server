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
    }
}