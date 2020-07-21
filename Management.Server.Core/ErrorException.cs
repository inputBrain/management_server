using System;

namespace Management.Server.Core
{
    public class ErrorException : Exception
    {
        public readonly Error Error;


        public ErrorException(Error error)
        {
            Error = error;
        }


        public static ErrorException DbException(string message)
        {
            return new ErrorException(new Error(message, ErrorType.DbError));
        }


        public static ErrorException InvalidDataException(string message)
        {
            return new ErrorException(new Error(message, ErrorType.InvalidData));
        }
    }
}