using Application.Common.Services;
using Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication;

public sealed class JwtGenerator : IJwtGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    public string GenerateJwt(UserBase User)
    {
        SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)), SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("UserType", User.UserType.ToString()),
            new Claim(ClaimTypes.NameIdentifier, User.Id.Value.ToString()),
            new Claim(ClaimTypes.Email, User.Email),
            new Claim(ClaimTypes.GivenName, User.FirstName),
            new Claim(ClaimTypes.Name, User.LastName),
            new Claim(ClaimTypes.MobilePhone, User.MobileNo)
    };

        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_jwtSettings.ExpiresIn),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
