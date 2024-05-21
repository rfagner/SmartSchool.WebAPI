using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SmartSchool.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        public ProfessorController()
        {
            
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Professores: Diana, Pedro Gabriel, Hélvia, Isis");
        }
    }
}
