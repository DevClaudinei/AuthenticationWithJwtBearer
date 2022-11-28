using ProjetoLoginToken.Models;
using ProjetoLoginToken.Models.Requests;
using System.Threading.Tasks;

namespace ProjetoLoginToken.Services.Interfaces;

public interface IUserService
{
    Task<User> Create(User user, string password);
    Task<User> GetByIdAsync(int id);
    Task<User> GetByEmail(string email);
    Task<User> GetByUserName(string userName);
    Task<User> Login(LoginRequest loginRequest);
}