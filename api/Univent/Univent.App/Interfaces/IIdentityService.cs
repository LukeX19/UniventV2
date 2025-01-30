using System.Security.Claims;
using Univent.Domain.Models.Users;

namespace Univent.App.Interfaces
{
    public interface IIdentityService
    {
        ClaimsIdentity CreateClaimsIdentity(AppUser user);
        string CreateSecurityToken(ClaimsIdentity identity);
    }
}
