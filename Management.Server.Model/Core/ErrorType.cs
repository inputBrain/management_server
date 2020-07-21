namespace Management.Server.Model.Core
{
    public enum ErrorType
    {
        Undefined = 0,
        Global,
        DbError,
        Auth,
        InvalidData
    }
}