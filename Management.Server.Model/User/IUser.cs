namespace Management.Server.Model.User
{
    public interface IUser
    {
        int Id { get; }

        string Email { get; }

        string Phone { get; }

        string Name { get; }
    }
}