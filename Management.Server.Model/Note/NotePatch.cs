namespace Management.Server.Model.Note
{
    public sealed class NotePatch
    {
        public readonly string Title;

        public readonly string Description;

        public readonly NoteStatus NoteStatus;


        public NotePatch(string title, string description, NoteStatus noteStatus)
        {
            Title = title;
            Description = description;
            NoteStatus = noteStatus;
        }
    }
}