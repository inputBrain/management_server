namespace Management.Server.Message.Cms.Messages.User
{
    public sealed class GetOneUser
    {
        public class Response : IResponse
        {
            public Payload.User.User Model { get; }


            public Response(Payload.User.User model)
            {
                Model = model;
            }
        }
    }
}