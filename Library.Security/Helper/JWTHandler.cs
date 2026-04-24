using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.Security.Helper;

internal class JWTHandler
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _jwtSettings;
    public JWTHandler(IConfiguration configuration)
    {
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("JwtSettings");
    }

    private SigningCredentials SigningCredentials()
    {
        var Key = Encoding.UTF8.GetBytes(_jwtSettings["Key"]);
        var secretKey = new SymmetricSecurityKey(Key);

        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }

    private List<Claim> GetClaims()
    {
        var claims = new List<Claim>();
        return claims;
    }
        
}