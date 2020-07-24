using System.ComponentModel.DataAnnotations;

namespace Management.Server.Message.Cms.Payload.User
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public UserStatus Status { get; set; }


        public User(int id, string firstName, string lastName, string email, UserStatus status)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Status = status;
        }
    }
}