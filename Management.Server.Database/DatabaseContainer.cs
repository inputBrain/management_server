using Management.Server.Database.Repositories;

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