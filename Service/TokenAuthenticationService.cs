using MESWebAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MESWebAPI.Service;
#nullable disable
public class TokenAuthenticationService : IAuthenticateService
{
    private readonly TokenManagement _tokenManagement;
    public TokenAuthenticationService(IOptions<TokenManagement> tokenManagement)
    {
        _tokenManagement = tokenManagement.Value;
    }
    public string Authenticated(User mesUser)
    {
        var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,mesUser.UserID),
                new Claim(ClaimTypes.Name,mesUser.UserName),
            };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer, _tokenManagement.Audience, claims, expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}
