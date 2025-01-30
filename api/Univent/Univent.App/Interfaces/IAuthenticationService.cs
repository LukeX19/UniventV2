using Univent.Domain.Models.Users;

namespace Univent.App.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AppUser> RegisterAsync(AppUser user, string password, CancellationToken ct = default);
        Task<AppUser> LoginAsync(string email, string password, CancellationToken ct = default);
    }
}
