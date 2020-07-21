namespace Management.Server.Core
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