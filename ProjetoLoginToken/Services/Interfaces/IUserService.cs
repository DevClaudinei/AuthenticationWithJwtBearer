using ProjetoLoginToken.Models;
using ProjetoLoginToken.Models.Requests;
using ProjetoLoginToken.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetoLoginToken.Services.Interfaces;

public interface IUserService
{
    Task<User> Create(User user, string password);
    Task<List<User>> GetAll();
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByEmail(string email);
    Task<User> GetByUserName(string userName);
    Task<AuthenticatedUserResponse> Login(LoginRequest loginRequest);
}