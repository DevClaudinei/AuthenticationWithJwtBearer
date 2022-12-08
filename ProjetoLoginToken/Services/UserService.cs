using Microsoft.Extensions.Options;
using ProjetoLoginToken.Models;
using ProjetoLoginToken.Models.Requests;
using ProjetoLoginToken.Models.Response;
using ProjetoLoginToken.Services.Interfaces;
using ProjetoLoginToken.Services.PasswordHasher;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetoLoginToken.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new();
    private readonly IPasswordHasher _passwordHasher;
    private readonly JwtOptions _jwtOptions;

    public UserService(IPasswordHasher passwordHasher, IOptions<JwtOptions> jwtOptions)
    {
        if (jwtOptions is null)
        {
            throw new ArgumentNullException(nameof(jwtOptions));
        }

        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _jwtOptions = jwtOptions.Value;
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

    public Task<User> GetByIdAsync(Guid id)
    {
        if (!_users.Any(x => x.Id.Equals(id))) throw new ArgumentException($"User for Id: {id} not found");

        return Task.FromResult(_users.FirstOrDefault(x => x.Id.Equals(id)));
    }

    public Task<User> GetByUserName(string userName)
    {
        return Task.FromResult(_users.FirstOrDefault(x => x.Username.Equals(userName)));
    }

    public Task<AuthenticatedUserResponse> Login(LoginRequest loginRequest)
    {
        var user = GetByUserName(loginRequest.Username);

        if (user is null) throw new ArgumentException($"User for usarname: {user.Result.Username} not found");

        var isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.Result.PasswordHash);

        if (!isCorrectPassword) return null;

        return GerarCredenciais(loginRequest.Username);
    }

    private async Task<AuthenticatedUserResponse> GerarCredenciais(string username)
    {
        var user = await GetByUserName(username);

        List<Claim> claims = new()
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username.ToString()),
            new Claim(ClaimTypes.Email, user.Email.ToString()),
        };

        var dataExpiracaoAccessToken = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);
        var acessToken = GerarToken(claims, dataExpiracaoAccessToken);

        return new AuthenticatedUserResponse(acessToken);
    }

    private string GerarToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
    {
        var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: dataExpiracao,
                signingCredentials: _jwtOptions.SigningCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public Task<List<User>> GetAll()
    {
        return Task.FromResult(_users);
    }
}