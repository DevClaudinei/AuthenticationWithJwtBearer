namespace ProjetoLoginToken.Models.Requests;

public class RegisterRequest
{
    protected RegisterRequest() { }

    public RegisterRequest(
        string email,
        string username,
        string password,
        string confirmPassword
    )
    {
        Email = email;
        Username = username;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}