using System;
using Management.Server.Database.Models.Note;
using Management.Server.Message.Cms.Payload.Note;
using CoreModel = Management.Server.Model.Note;

namespace Management.Server.Message.Cms.Codec.Payload.Note
{
    public static class NoteCodec
    {
        public static Cms.Payload.Note.Note EncodeNote(NoteModel dbModel)
        {
            return new Cms.Payload.Note.Note(
                dbModel.Id,
                dbModel.Title,
                dbModel.Description,
                EncodeNoteStatus(dbModel.NoteStatus)
            );
        }


        public static NoteStatus EncodeNoteStatus(CoreModel.NoteStatus status)
        {
            switch (status)
            {
                case CoreModel.NoteStatus.Deleted:
                    return NoteStatus.Deleted;
                case CoreModel.NoteStatus.Disabled:
                    return NoteStatus.Disabled;
                case CoreModel.NoteStatus.Published:
                    return NoteStatus.Published;
                default:
                    throw new ArgumentException("Undefined encode Note Status: " + status);
            }
        }
    }
}