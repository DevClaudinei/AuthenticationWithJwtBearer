using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjetoLoginToken.Models;
using ProjetoLoginToken.Models.Requests;
using ProjetoLoginToken.Services.Interfaces;
using ProjetoLoginToken.Services.TokenGenerator;
using System;
using System.Threading.Tasks;

namespace ProjetoLoginToken.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly AcessTokenGenerator _acessTokenGenerator;

    public AuthenticateController(IUserService userService, IMapper mapper, AcessTokenGenerator acessTokenGenerator)
    {
        _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        _userService = userService ?? throw new System.ArgumentNullException(nameof(userService));
        _acessTokenGenerator = acessTokenGenerator ?? throw new ArgumentNullException(nameof(acessTokenGenerator));
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
            var acessToken = _acessTokenGenerator.GenerateToken(user);

            return Ok(acessToken);
        }
        catch
        {
            return Unauthorized();
        }
    }
}