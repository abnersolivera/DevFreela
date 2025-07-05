using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly DevFreelaDbContext _dbContext;
    
    public ProjectsController(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public IActionResult Get(string search = "", int page = 1, int size = 3)
    {
        var projects = _dbContext.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Where(p => !p.IsDeleted && (search == "" || p.Title.Contains(search) || p.Description.Contains(search)))
            .Skip(page * size)
            .Take(3)
            .ToList();
        
        var projectViewModels = projects
            .Select(ProjectItemViewModel.FromEntity)
            .ToList();
        
        return Ok(projectViewModels);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var project = _dbContext.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Include(p => p.Comments)
            .SingleOrDefault(p => p.Id == id);
        
        var projectViewModel = ProjectViewModel.FromEntity(project);
        
        return Ok(projectViewModel);
    }
    
    [HttpPost]
    public IActionResult Post(CreateProjectInputModel inputModel)
    {
        var project = inputModel.ToEntity();
        
        _dbContext.Projects.Add(project);
        _dbContext.SaveChanges();
        
        return CreatedAtAction(nameof(GetById), new { id = 1 }, inputModel);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult Put(int id, UpdateProjectInputModel inputModel)
    {
        inputModel.IdProject = id;
        
        var project = _dbContext.Projects.SingleOrDefault(p => p.Id == inputModel.IdProject);
        
        if (project is null)
            return NotFound();
        
        project.Update(inputModel.Title, inputModel.Description, inputModel.TotalCost);
        
        _dbContext.Projects.Update(project);
        
        _dbContext.SaveChanges();
        
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
        
        if (project is null)
            return NotFound();
        
        project.SetAsDeleted();
        _dbContext.Projects.Update(project);
        _dbContext.SaveChanges();
        
        return NoContent();
    }
    
    [HttpPut("{id:int}/start")]
    public IActionResult Start(int id)
    {
        var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
        
        if (project is null)
            return NotFound();
        
        project.Start();
        _dbContext.Projects.Update(project);
        _dbContext.SaveChanges();
        
        return NoContent();
    }
    
    [HttpPut("{id:int}/complete")]
    public IActionResult Complete(int id)
    {
        var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
        
        if (project is null)
            return NotFound();
        
        project.Complete();
        _dbContext.Projects.Update(project);
        _dbContext.SaveChanges();
        
        return NoContent();
    }
    
    [HttpPost("{id:int}/comments")]
    public IActionResult PostComment(int id, CreateProjectCommentInputModel inputModel)
    {
        var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
        
        if (project is null)
            return NotFound();
        
        var comment = new ProjectComment(inputModel.Content, inputModel.IdProject, inputModel.IdUser);
        
        project.Comments.Add(comment);
        _dbContext.SaveChanges();
        
        return CreatedAtAction(nameof(GetById), new { id = 1 }, inputModel);
    }
    
    
}