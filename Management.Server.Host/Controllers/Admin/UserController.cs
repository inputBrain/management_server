using System.Threading.Tasks;
using Management.Server.Database;
using Management.Server.Message.Cms.Codec.Messages.User;
using Management.Server.Message.Cms.Messages.User;
using Microsoft.AspNetCore.Mvc;

namespace Management.Server.Host.Controllers.Admin
{
    public class UserController : AbstractController<UserController>
    {
        public UserController(PostgreSqlContext context) : base(context)
        {
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetOneUser.Response), 200)]
        public async Task<IActionResult> GetOne(int id)
        {
            var model = await Db.User.GetOne(id);
            return SendOk(UserMessageCodec.EncodeGetOneUser(model));
        }

    }
}