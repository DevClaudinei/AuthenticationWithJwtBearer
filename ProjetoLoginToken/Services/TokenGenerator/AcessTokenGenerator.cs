using Microsoft.IdentityModel.Tokens;
using ProjetoLoginToken.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjetoLoginToken.Services.TokenGenerator;

public class AcessTokenGenerator
{
    public string GenerateToken(User user)
    {
        List<Claim> claims = new ()
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username.ToString()),
            new Claim(ClaimTypes.Email, user.Email.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("appsettings.json"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            "https://localhost:7071",
            "https://localhost:7071",
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(45),
            credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}