using System.Threading.Tasks;
using Management.Server.Database;
using Microsoft.AspNetCore.Mvc;

namespace Management.Server.Host.Controllers.Admin
{
    public class UserController : AbstractController<UserController>
    {
        public UserController(PostgreSqlContext context) : base(context)
        {
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var model = await Db.User.GetOne(id);
            return SendOk();
        }

    }
}