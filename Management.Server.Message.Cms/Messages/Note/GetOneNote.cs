namespace Management.Server.Message.Cms.Messages.Note
{
    public sealed class GetOneNote
    {
        public class Response : IResponse
        {
            public Payload.Note.Note Note { get; }


            public Response(Payload.Note.Note note)
            {
                Note = note;
            }
        }
    }
}