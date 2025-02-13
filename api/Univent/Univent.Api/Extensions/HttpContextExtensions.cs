using System.Security.Claims;

namespace Univent.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetUserIdClaimValue(this HttpContext context)
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            return Guid.Parse(claimsIdentity?.FindFirst("uid")?.Value);
        }
    }
}
