﻿using Univent.Domain.Models.Users;

namespace Univent.App.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserById(Guid id, CancellationToken ct = default);
        Task<Dictionary<Guid, double>> GetAverageRatingsAsync(ICollection<Guid> userIds, CancellationToken ct);
    }
}
