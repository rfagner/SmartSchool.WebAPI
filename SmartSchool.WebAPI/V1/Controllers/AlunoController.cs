using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.V1.Dtos;

namespace SmartSchool.WebAPI.V1.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AlunoController : ControllerBase
{
    private readonly IRepository _repo;
    private readonly IMapper _mapper;


    public AlunoController(IRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    /// <summary>
    /// Método responsável por retornar todos os meus alunos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
    {
        var alunos = await _repo.GetAllAlunosAsync(pageParams, true);

        var alunosResult = _mapper.Map<IEnumerable<AlunoDto>>(alunos);

        Response.AddPagination(alunos.CurrentPage, alunos.PageSize, alunos.TotalCount, alunos.TotalPages);

        return Ok(alunosResult);
    }

    /// <summary>
    /// Método responsável por retornar apenas um único AlunoDTO.
    /// </summary>
    /// <returns></returns>
    [HttpGet("getRegister")]
    public IActionResult GetRegister()
    {
        return Ok(new AlunoRegistrarDto());
    }

    /// <summary>
    /// Método responsável por retornar apenas um Aluno por meio do código  ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public IActionResult GetById(int id)
    {
        var aluno = _repo.GetAlunoById(id, false);
        if (aluno == null) return NotFound("O Aluno não encontrado!");

        var alunoDto = _mapper.Map<AlunoDto>(aluno);

        return Ok(alunoDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public IActionResult Post(AlunoRegistrarDto model)
    {
        var aluno = _mapper.Map<Aluno>(model);

        _repo.Add(aluno);
        if (_repo.SaveChanges())
        {
            return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
        }

        return BadRequest("Aluno não cadastrado!");
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public IActionResult Put(int id, AlunoRegistrarDto model)
    {
        var aluno = _repo.GetAlunoById(id);
        if (aluno == null) return NotFound("Aluno não encontrado!");

        _mapper.Map(model, aluno);

        _repo.Update(aluno);
        if (_repo.SaveChanges())
        {
            return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
        }

        return BadRequest("Aluno não Atualizado!");
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public IActionResult Patch(int id, AlunoRegistrarDto model)
    {
        var aluno = _repo.GetAlunoById(id);
        if (aluno == null) return NotFound("Aluno não encontrado!");

        _mapper.Map(model, aluno);

        _repo.Update(aluno);
        if (_repo.SaveChanges())
        {
            return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
        }

        return BadRequest("Aluno não Atualizado!");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public IActionResult Delete(int id)
    {
        var aluno = _repo.GetAlunoById(id);
        if (aluno == null) return NotFound("Aluno não encontrado!");

        _repo.Delete(aluno);
        if (_repo.SaveChanges())
        {
            return Ok("Aluno deletado.");
        }

        return BadRequest("Aluno não deletado.");
    }

}
