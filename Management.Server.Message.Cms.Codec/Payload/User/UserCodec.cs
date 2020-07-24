using System;
using Management.Server.Database.Models.User;
using Management.Server.Message.Cms.Payload.User;
using CoreModel = Management.Server.Model.User;

namespace Management.Server.Message.Cms.Codec.Payload.User
{
    public static class UserCodec
    {
        //TODO: return type have to be shorter like just "User" but not Cms.Payload...
        public static Cms.Payload.User.User EncodeUser(UserModel dbModel)
        {
            return new Cms.Payload.User.User(
                dbModel.Id,
                dbModel.FirstName,
                dbModel.LastName,
                dbModel.Email,
                EncodeUserStatus(dbModel.Status)
            );
        }


        public static UserStatus EncodeUserStatus(CoreModel.UserStatus status)
        {
            switch (status)
            {
                case CoreModel.UserStatus.NotConfirmed:
                    return UserStatus.NotConfirmed;
                case CoreModel.UserStatus.Confirmed:
                    return UserStatus.Confirmed;
                case CoreModel.UserStatus.Deleted:
                    return UserStatus.Deleted;
                default:
                    throw new ArgumentException("Undefined encode user status: " + status);
            }
        }
    }
}