namespace DevFreela.Application.Models;

public class LoginInputModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginViewModel
{
    public LoginViewModel(string token)
    {
        Token = token;
    }
    
    public string Token { get; set; }
}