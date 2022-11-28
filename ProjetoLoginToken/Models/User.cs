using System;

namespace ProjetoLoginToken.Models;

public class User
{
    protected User() { }

    public User(string email, string username, string passwordHash)
    {
        Email = email;
        Username = username;
        PasswordHash = passwordHash;
    }

    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
}