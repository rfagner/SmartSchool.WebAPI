using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.V2.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace SmartSchool.WebAPI.Controllers;

[ApiController]
[ApiVersion("2.0")]
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
    public IActionResult Get()
    {
        var alunos = _repo.GetAllAlunos(true);
        return Ok(_mapper.Map<IEnumerable<AlunoDto>>(alunos));
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
