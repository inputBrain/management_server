using System.ComponentModel.DataAnnotations;

namespace Management.Server.Message.Cms.Payload.Note
{
    public class Note
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public NoteStatus NoteStatus { get; set; }


        public Note(int id, int authorId, string title, string description, NoteStatus noteStatus)
        {
            Id = id;
            AuthorId = authorId;
            Title = title;
            Description = description;
            NoteStatus = noteStatus;
        }
    }
}