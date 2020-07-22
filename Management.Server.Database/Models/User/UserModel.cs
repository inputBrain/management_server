using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Management.Server.Model.User;

namespace Management.Server.Database.Models.User
{
    public class UserModel : AbstractModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public UserStatus Status { get; set; }


        public static UserModel CreateModel(
            string email,
            string firstName,
            string lastName,
            UserStatus status
        )
        {
            return new UserModel
            {
                Email = email,
                FirstName = firstName,
                Status = status,
                LastName = lastName
            };
        }
    }
}