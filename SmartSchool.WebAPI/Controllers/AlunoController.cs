using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartSchool.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        public List<Aluno> Alunos = new List<Aluno>()
        {
            new Aluno()
            {
                Id = 1,
                Nome = "Pedro",
                Sobrenome = "Paulo Soares Pereira",
                Telefone = "74108520"
            },
            new Aluno()
            {
                Id = 2,
                Nome = "Maria",
                Sobrenome = "Clara dos Santos",
                Telefone = "85209654"
            },
            new Aluno()
            {
                Id = 3,
                Nome = "Cítia",
                Sobrenome = "Queiroz",
                Telefone = "74107896"
            }

        };
        public AlunoController()
        {
            
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Alunos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var aluno = Alunos.FirstOrDefault(a => a.Id == id);
            if (aluno == null)
                return BadRequest();

            return Ok(aluno);
        }
    }
}
