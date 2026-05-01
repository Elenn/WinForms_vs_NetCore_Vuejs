namespace MyApp.Api.Models.Users;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty; // email or username
    public string Password { get; set; } = string.Empty; // NOTE: store a hash in production
}