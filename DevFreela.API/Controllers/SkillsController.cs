using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SkillsController : ControllerBase
{
    private readonly DevFreelaDbContext _dbContext;
    
    public SkillsController(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var skills = _dbContext.Skills.ToList();
        
        return Ok(skills);
    }
    
    [HttpPost]
    public IActionResult Post(CreateSkillInputModel inputModel)
    {
        var skill = new Skill(inputModel.Description);
        
        _dbContext.Skills.Add(skill);
        _dbContext.SaveChanges();
        
        return NoContent();
    }
}