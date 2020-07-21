using Management.Server.Database.Services;

namespace Management.Server.Database
{
    public class ServiceContainer
    {
        public readonly UserService User;

        internal ServiceContainer(UserService user)
        {
            User = user;
        }
    }
}