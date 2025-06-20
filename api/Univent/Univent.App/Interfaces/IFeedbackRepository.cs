﻿using Univent.Domain.Models.Users;

namespace Univent.App.Interfaces
{
    public interface IFeedbackRepository : IBaseRepository<Feedback>
    {
        Task AddRangeAsync(ICollection<Feedback> feedbacks, CancellationToken ct = default);
    }
}
