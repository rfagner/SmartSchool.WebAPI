using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.V1.Dtos;

namespace SmartSchool.WebAPI.V1.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProfessorController : ControllerBase
{
    private readonly IRepository _repo;
    private readonly IMapper _mapper;
    public ProfessorController(IRepository repo, IMapper mapper)
    {
        _mapper = mapper;
        _repo = repo;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var Professor = _repo.GetAllProfessores(true);
        return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(Professor));
    }

    [HttpGet("getRegister")]
    public IActionResult GetRegister()
    {
        return Ok(new ProfessorRegistrarDto());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public IActionResult GetById(int id)
    {
        var Professor = _repo.GetProfessorById(id, false);
        if (Professor == null) return NotFound("O Professor não encontrado!");

        var professorDto = _mapper.Map<ProfessorDto>(Professor);

        return Ok(Professor);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public IActionResult Post(ProfessorRegistrarDto model)
    {
        var prof = _mapper.Map<Professor>(model);

        _repo.Add(prof);
        if (_repo.SaveChanges())
        {
            return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(prof));
        }

        return BadRequest("Professor não cadastrado!");
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public IActionResult Put(int id, ProfessorRegistrarDto model)
    {
        var prof = _repo.GetProfessorById(id, false);
        if (prof == null) return NotFound("Professor não encontrado!");

        _mapper.Map(model, prof);

        _repo.Update(prof);
        if (_repo.SaveChanges())
        {
            return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(prof));
        }

        return BadRequest("Professor não atualizado!");
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public IActionResult Patch(int id, ProfessorRegistrarDto model)
    {
        var prof = _repo.GetProfessorById(id, false);
        if (prof == null) return NotFound("Professor não encontrado!");

        _mapper.Map(model, prof);

        _repo.Update(prof);
        if (_repo.SaveChanges())
        {
            return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(prof));
        }

        return BadRequest("Professor não atualizado!");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public IActionResult Delete(int id)
    {
        var prof = _repo.GetProfessorById(id, false);
        if (prof == null) return NotFound("Professor não encontrado!");

        _repo.Delete(prof);
        if (_repo.SaveChanges())
        {
            return Ok("Professor deletado!");
        }

        return BadRequest("Professor não deletado!");
    }
}
