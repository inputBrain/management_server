using System.Threading.Tasks;
using Management.Server.Database;
using Management.Server.Message.Cms.Codec.Messages.Note;
using Management.Server.Message.Cms.Messages.Note;
using Microsoft.AspNetCore.Mvc;

namespace Management.Server.Host.Controllers.Admin
{
    public class NoteController : AbstractController<NoteController>
    {
        public NoteController(PostgreSqlContext context) : base(context)
        {
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetOneNote.Response), 200)]
        public async Task<IActionResult> GetOneNote(int id)
        {
            var model = await Db.Note.GetOne(id);
            return SendOk(NoteMessageCodec.EncodeGetOneNote(model));
        }


        [HttpPost]
        [ProducesResponseType(typeof(CreateNote.Response), 200)]
        public async Task<IActionResult> CreateNote([FromBody] CreateNote request)
        {
            // var user = 
            // var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            return null;
        }
    }
}