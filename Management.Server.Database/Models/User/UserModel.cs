using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Management.Server.Model.User;

namespace Management.Server.Database.Models.User
{

    public class UserModel : AbstractModel, IUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public UserStatus Status { get; set; }


        public static UserModel Create(
            string email,
            string phone,
            string name,
            UserStatus status
        )
        {
            return new UserModel
            {
                Email = email,
                Phone = phone,
                Status = status,
                Name = name,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }
    }
}