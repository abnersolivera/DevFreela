using DevFreela.Application.Commands.CompleteProject;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.InsertComment;
using DevFreela.Application.Commands.InsertProject;
using DevFreela.Application.Commands.StartProject;
using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Application.Queries.GetProjectById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(string search = "", int page = 0, int rows = 3)
    {
        GetAllProjectsQuery query = new(search, page, rows);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        GetProjectByIdQuery query = new(id);
        
        var result = await _mediator.Send(query);
        
        if(!result.IsSuccess)
            return BadRequest(result.Message);
        
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(InsertProjectCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Data }, command);
    }
    
    [HttpPut]
    public async Task<IActionResult> Put(UpdateProjectCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        DeleteProjectCommand command = new(id);
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return NoContent();
    }
    
    [HttpPut("{id:int}/start")]
    public async Task<IActionResult> Start(int id)
    {
        StartProjectCommand command = new(id);
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return NoContent();
    }
    
    [HttpPut("{id:int}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        CompleteProjectCommand command = new(id);
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return NoContent();
    }
    
    [HttpPost("comments")]
    public async Task<IActionResult> PostComment(InsertCommentCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return NoContent();
    }
}