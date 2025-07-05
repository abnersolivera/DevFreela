using DevFreela.Application.Models;
using DevFreela.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;
    public ProjectsController(IProjectService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public IActionResult Get(string search = "", int page = 0, int size = 3)
    {
        var result = _service.GetAll(search, page, size);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var result = _service.GetById(id);
        
        if(!result.IsSuccess)
            return BadRequest(result.Message);
        
        return Ok(result);
    }
    
    [HttpPost]
    public IActionResult Post(CreateProjectInputModel inputModel)
    {
        var result = _service.Insert(inputModel);
        return CreatedAtAction(nameof(GetById), new { id = result.Data }, inputModel);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult Put(int id, UpdateProjectInputModel inputModel)
    {
        inputModel.IdProject = id;
        var result = _service.Update(inputModel);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var result = _service.Delete(id);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return NoContent();
    }
    
    [HttpPut("{id:int}/start")]
    public IActionResult Start(int id)
    {
        var result = _service.Start(id);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return NoContent();
    }
    
    [HttpPut("{id:int}/complete")]
    public IActionResult Complete(int id)
    {
        var result = _service.Complete(id);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return NoContent();
    }
    
    [HttpPost("{id:int}/comments")]
    public IActionResult PostComment(int id, CreateProjectCommentInputModel inputModel)
    {
        var result = _service.InsertComment(id, inputModel);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return NoContent();
    }
}