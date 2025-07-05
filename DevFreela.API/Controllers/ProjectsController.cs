using DevFreela.Application.Models;
using DevFreela.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    
    [HttpGet]
    public IActionResult Get(string search = "", int page = 1, int size = 3)
    {
        var result = _projectService.GetAll(search, page, size);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var result = _projectService.GetById(id);
        return Ok(result);
    }
    
    [HttpPost]
    public IActionResult Post(CreateProjectInputModel inputModel)
    {
        var result = _projectService.Insert(inputModel);
        return CreatedAtAction(nameof(GetById), new { id = result }, inputModel);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult Put(int id, UpdateProjectInputModel inputModel)
    {
        inputModel.IdProject = id;
        _projectService.Update(inputModel);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _projectService.Delete(id);
        return NoContent();
    }
    
    [HttpPut("{id:int}/start")]
    public IActionResult Start(int id)
    {
        _projectService.Start(id);
        return NoContent();
    }
    
    [HttpPut("{id:int}/complete")]
    public IActionResult Complete(int id)
    {
        _projectService.Complete(id);
        return NoContent();
    }
    
    [HttpPost("{id:int}/comments")]
    public IActionResult PostComment(int id, CreateProjectCommentInputModel inputModel)
    {
        _projectService.InsertComment(id, inputModel);
        return NoContent();
    }
}