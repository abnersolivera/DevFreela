using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Auth;
using DevFreela.Infrastructure.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DevFreela.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly DevFreelaDbContext _dbContext;
    private readonly IAuthService _authService;
    private readonly IMemoryCache _memoryCache;
    private readonly IEmailService _emailService;
    public UsersController(DevFreelaDbContext dbContext, IAuthService authService, IMemoryCache memoryCache, IEmailService emailService)
    {
        _dbContext = dbContext;
        _authService = authService;
        _memoryCache = memoryCache;
        _emailService = emailService;
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
    
    [HttpPost("password-recovery/request")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestPasswordRecovery(PasswordRecoveryRequestInputModel inputModel)
    {
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Email == inputModel.Email);

        if (user is null)
        {
            return BadRequest("Usuário não encontrado.");
        }
        
        var code = new Random().Next(100000, 999999).ToString();
        
        var cacheKey = $"RecoveryCode:{user.Email}";
        
        _memoryCache.Set(cacheKey, code, TimeSpan.FromMinutes(15));
        
        await _emailService.SendEmailAsync(
            user.Email,
            "Código de recuperação de senha",
            $"Seu código de recuperação é {code}"
        );
        
        return NoContent();
    }
    
    [HttpPost("password-recovery/validate")]
    [AllowAnonymous]
    public IActionResult ValidateRecoveryCode(ValidateRecoveryCodeInputModel inputModel)
    {
        var cacheKey = $"RecoveryCode:{inputModel.Email}";
        if (!_memoryCache.TryGetValue(cacheKey, out string? code) || code != inputModel.Code)
        {
            return BadRequest("Código inválido ou expirado.");
        }
        return NoContent();
    }
    
    [HttpPost("password-recovery/change")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword(ChangePasswordInputModel inputModel)
    {
        var cacheKey = $"RecoveryCode:{inputModel.Email}";
        if (!_memoryCache.TryGetValue(cacheKey, out string? code) || code != inputModel.Code)
        {
            return BadRequest("Código inválido ou expirado.");
        }
        _memoryCache.Remove(cacheKey);
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Email == inputModel.Email);
        
        if (user is null)
        {
            return BadRequest("Usuário não encontrado.");
        }
        
        var hash = _authService.ComputeHash(inputModel.NewPassword);
        user.UpdatePassword(hash);
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}