using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly DevFreelaDbContext _dbContext;
    private readonly IAuthService _authService;
    public UsersController(DevFreelaDbContext dbContext, IAuthService authService)
    {
        _dbContext = dbContext;
        _authService = authService;
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
    [AllowAnonymous]
    public IActionResult Post(CreateUserInputModel inputModel)
    {
        var hash = _authService.ComputeHash(inputModel.Password);
        var user = new User(inputModel.FullName, inputModel.Email, inputModel.BirthDate, hash, inputModel.Role);
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
    
    [HttpPut("login")]
    [AllowAnonymous]
    public IActionResult Login(LoginInputModel inputModel)
    {
        var hash = _authService.ComputeHash(inputModel.Password);
        var user = _dbContext.Users
            .SingleOrDefault(u => u.Email == inputModel.Email && u.Password == hash);
        if (user is null)
        {
            var error = ResultViewModel<LoginViewModel>.Error("Invalid email or password.");
            return BadRequest(error);
        }
        var token = _authService.GenerateToken(user.Email, user.Role);
        var viewModel = new LoginViewModel(token);
        var result = ResultViewModel<LoginViewModel>.Success(viewModel);
        return Ok(result);
    }
}