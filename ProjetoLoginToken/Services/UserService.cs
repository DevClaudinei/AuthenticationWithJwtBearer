using ProjetoLoginToken.Models;
using ProjetoLoginToken.Models.Requests;
using ProjetoLoginToken.Services.Interfaces;
using ProjetoLoginToken.Services.PasswordHasher;
using ProjetoLoginToken.Services.TokenGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLoginToken.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new();
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IPasswordHasher passwordHasher, AcessTokenGenerator acessTokenGenerator)
    {
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public Task<User> Create(User user, string password)
    {
        UserAlreadyExists(user);
        var userToCreate = AssignEncryptedPasswordAndId(user, password);
       
        _users.Add(userToCreate);

        return Task.FromResult(user);
    }

    private User AssignEncryptedPasswordAndId(User user, string password)
    {
        user.PasswordHash = _passwordHasher.HashPassword(password);
        user.Id = Guid.NewGuid();
        
        return user;
    }

    private void UserAlreadyExists(User user)
    {
        if (_users.Any(x => x.Username.Equals(user.Username))) throw new ArgumentException($"User for usarname: {user.Username} not found");

        if (_users.Any(x => x.Email.Equals(user.Email))) throw new ArgumentException($"User for usarname: {user.Username} not found");
    }

    public Task<User> GetByEmail(string email)
    {
        return Task.FromResult(_users.FirstOrDefault(x => x.Email.Equals(email)));
    }

    public Task<User> GetByIdAsync(int id)
    {
        return Task.FromResult(_users.FirstOrDefault(x => x.Id.Equals(id)));
    }

    public Task<User> GetByUserName(string userName)
    {
        return Task.FromResult(_users.FirstOrDefault(x => x.Username.Equals(userName)));
    }

    public Task<User> Login(LoginRequest loginRequest)
    {
        var user = GetByUserName(loginRequest.Username);

        if (user is null) throw new ArgumentException($"User for usarname: {user.Result.Username} not found");

        var isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.Result.PasswordHash);

        if (!isCorrectPassword) return null;

        return user;
    }
}