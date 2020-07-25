using System.ComponentModel.DataAnnotations;

namespace Management.Server.Message.Cms.Payload.Note
{
    public class NotePatch
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public NoteStatus NoteStatus { get; set; }


        public NotePatch(string title, string description, NoteStatus noteStatus)
        {
            Title = title;
            Description = description;
            NoteStatus = noteStatus;
        }
    }
}