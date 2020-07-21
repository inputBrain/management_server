using Management.Server.Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database
{
    public class DatabaseContainer
    {

        public readonly UserRepository User;


        internal DatabaseContainer(UserRepository user)
        {
            User = user;
        }
    }
}