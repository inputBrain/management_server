using Management.Server.Database;
using Microsoft.AspNetCore.Mvc;

namespace Management.Server.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AbstractController<T> : ControllerBase
        where T : ControllerBase
    {
        protected readonly DatabaseFacade Db;


        public AbstractController(PostgreSqlContext context)
        {
            Db = context.Db;
        }


        protected IActionResult SendOk()
        {
            return Ok();
        }

        protected IActionResult SendOk(object response)
        {
            return Ok(response);
        }

    }
}