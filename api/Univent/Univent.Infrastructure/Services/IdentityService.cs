using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Univent.App.Interfaces;
using Univent.Domain.Models.Users;
using Univent.Infrastructure.Options;

namespace Univent.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly JwtSettings? _settings;
        private readonly byte[] _key;

        public IdentityService(IOptions<JwtSettings> jwtOptions)
        {
            _settings = jwtOptions.Value;
            ArgumentNullException.ThrowIfNull(_settings);
            ArgumentNullException.ThrowIfNull(_settings.SigningKey);
            ArgumentNullException.ThrowIfNull(_settings.Audiences);
            ArgumentNullException.ThrowIfNull(_settings.Audiences[0]);
            ArgumentNullException.ThrowIfNull(_settings.Issuer);
            _key = Encoding.ASCII.GetBytes(_settings?.SigningKey!);
        }

        public JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

        public ClaimsIdentity CreateClaimsIdentity(AppUser user)
        {
            return new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim(ClaimTypes.Role, ((int)user.Role).ToString(), ClaimValueTypes.Integer32)
            });
        }

        public string CreateSecurityToken(ClaimsIdentity identity)
        {
            var tokenDescription = GetTokenDescriptor(identity);
            var token = TokenHandler.CreateToken(tokenDescription);
            return TokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
        {
            return new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(10),
                Audience = _settings!.Audiences?[0]!,
                Issuer = _settings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
        }
    }
}
