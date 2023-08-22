namespace FrontEnd.Models;

public class LoginViewModel
{

}

public class LoginForm
{
    public string? Usn { get; set; }
    public string? Login { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ReturnUrl { get; set; }
    public bool? RememberMe { get; set; }
}

public class CapcthaModel
{
    public bool Success { get; set; }
    public DateTime ChallengeTs { get; set; }
    public string? Hostname { get; set; }
    public double Score { get; set; }
    public string? Action { get; set; }
}