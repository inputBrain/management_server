using Management.Server.Database.Models.Note;
using Management.Server.Message.Cms.Messages.Note;

namespace Management.Server.Message.Cms.Codec.Messages.Note
{
    public static class NoteMessageCodec
    {
        public static GetOneNote.Response EncodeGetOneNote(NoteModel dbModel)
        {
            return new GetOneNote.Response(Payload.Note.NoteCodec.EncodeNote(dbModel));
        }
    }
}