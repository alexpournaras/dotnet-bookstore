using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenGenerator
{
    public static string GenerateToken(IConfiguration _configuration, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["JWT:SigningKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}