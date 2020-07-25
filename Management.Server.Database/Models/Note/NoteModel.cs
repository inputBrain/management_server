using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Management.Server.Database.Models.User;
using Management.Server.Model.Note;

namespace Management.Server.Database.Models.Note
{
    public class NoteModel : AbstractModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public UserModel Author { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public NoteStatus NoteStatus { get; set; }


        public static NoteModel CreateModel(
            int authorId,
            string title,
            string description,
            NoteStatus noteStatus
        )
        {
            return new NoteModel
            {
                AuthorId = authorId,
                Title = title,
                Description = description,
                NoteStatus = noteStatus
            };
        }
    }
}