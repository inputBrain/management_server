namespace Management.Server.Model.Core
{
    public class Error
    {
        public readonly ErrorType Type;

        public readonly string Message;


        public Error(string message, ErrorType type = ErrorType.Undefined)
        {
            Message = message;
            Type = type;
        }


        public static Error Create(string message, ErrorType type = ErrorType.Undefined)
        {
            return new Error(message, type);
        }



        public static Error AuthenticationFailedError(string message)
        {
            return Create(message, ErrorType.Auth);
        }



        public static Error InvalidError(string message)
        {
            return new Error(message, ErrorType.InvalidData);
        }
    }
}