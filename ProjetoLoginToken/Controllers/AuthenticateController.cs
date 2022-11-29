using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoLoginToken.Models;
using ProjetoLoginToken.Models.Requests;
using ProjetoLoginToken.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ProjetoLoginToken.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public AuthenticateController(IUserService userService, IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        try
        {
            var user = _mapper.Map<User>(registerRequest);
            var userCreated = await _userService.Create(user, registerRequest.Password);

            return Created("", userCreated.Id);
        }
        catch (ArgumentException exception)
        {
            var message = exception.InnerException?.Message ?? exception.Message;
            return BadRequest(message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        try
        {
            var user = await _userService.Login(loginRequest);
            return Ok(user);
        }
        catch
        {
            return Unauthorized();
        }
    }

    [Authorize]
    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var acessToken = await _userService.GetByIdAsync(id);
            return Ok(acessToken);
        }
        catch (ArgumentException exception)
        {
            var message = exception.InnerException?.Message ?? exception.Message;
            return BadRequest(message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }
}