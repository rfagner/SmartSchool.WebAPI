using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Dtos;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    public IActionResult GetById(int id)
    {
        var Professor = _repo.GetProfessorById(id, false);
        if (Professor == null) return NotFound("O Professor não encontrado!");

        var professorDto = _mapper.Map<ProfessorDto>(Professor);

        return Ok(Professor);
    }

    [HttpPost]
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
