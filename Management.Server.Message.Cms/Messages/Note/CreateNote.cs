using Management.Server.Message.Cms.Payload.Note;

namespace Management.Server.Message.Cms.Messages.Note
{
    public class CreateNote : NotePatch
    {
        public CreateNote(string title, string description, NoteStatus noteStatus) : base(title, description, noteStatus)
        {
        }


        public class Response : IResponse
        {
            public Payload.Note.Note Note { get; set; }


            public Response(Payload.Note.Note note)
            {
                Note = note;
            }
        }
    }
}