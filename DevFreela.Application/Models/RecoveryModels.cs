namespace DevFreela.Application.Models;

public class PasswordRecoveryRequestInputModel
{
    public string Email { get; set; } = string.Empty;
}

public class ValidateRecoveryCodeInputModel
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class ChangePasswordInputModel
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}