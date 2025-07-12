using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DevFreelaDbContext _dbContext;

    public UsersController(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var user = _dbContext.Users
            .Include(u => u.Skills)
            .ThenInclude(s => s.Skill)
            .SingleOrDefault(u => u.Id == id);

        if (user is null)
            return NotFound();

        var userViewModel = UserViewModel.FromEntity(user);

        return Ok(userViewModel);
    }

    [HttpPost]
    public IActionResult Post(CreateUserInputModel inputModel)
    {
        var user = new User(inputModel.FullName, inputModel.Email, inputModel.BirthDate, inputModel.Password, inputModel.Role);
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpPut("{id:int}/skills")]
    public IActionResult PutSkills(int id, UserSkillsInputModel inputModel)
    {
        var userSkills = inputModel.SkillIds
            .Select(s => new UserSkill(id, s))
            .ToList();

        _dbContext.UserSkills.AddRange(userSkills);
        _dbContext.SaveChanges();
        return NoContent();
    }


    [HttpPut("{id:int}/profile-picture")]
    public IActionResult UpdateProfilePicture(int id, IFormFile file)
    {
        if (file.Length == 0)
        {
            return BadRequest("File cannot be empty.");
        }

        return Ok(new { Message = "Profile picture updated successfully.", FileName = file.FileName });
    }
}