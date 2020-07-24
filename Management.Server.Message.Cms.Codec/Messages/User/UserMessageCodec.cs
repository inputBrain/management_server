using Management.Server.Database.Models.User;
using Management.Server.Message.Cms.Messages.User;

namespace Management.Server.Message.Cms.Codec.Messages.User
{
    public static class UserMessageCodec
    {
        public static GetOneUser.Response EncodeGetOneUser(UserModel dbModel)
        {
            return new GetOneUser.Response(Payload.User.UserCodec.EncodeUser(dbModel));
        }
    }
}