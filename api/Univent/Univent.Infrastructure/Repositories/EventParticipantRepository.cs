﻿using Microsoft.EntityFrameworkCore;
using Univent.App.Interfaces;
using Univent.Domain.Models.Associations;
using Univent.Infrastructure.Exceptions;

namespace Univent.Infrastructure.Repositories
{
    public class EventParticipantRepository : IEventParticipantRepository
    {
        private readonly AppDbContext _context;

        public EventParticipantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(Guid EventId, Guid UserId)> CreateEventParticipantAsync(EventParticipant entity, CancellationToken ct = default)
        {
            if (_context.EventParticipants.Contains(entity))
            {
                throw new EntityAlreadyExistsException(typeof(EventParticipant).Name, entity.EventId, entity.UserId);
            }

            _context.EventParticipants.Add(entity);
            await _context.SaveChangesAsync(ct);
            return (entity.EventId, entity.UserId);
        }

        public async Task<ICollection<EventParticipant>> GetEventParticipantsByEventIdAsync(Guid eventId, CancellationToken ct = default)
        {
            return await _context.EventParticipants
                .AsNoTracking()
                .AsSplitQuery()
                .Where(ep => ep.EventId == eventId)
                .Include(ep => ep.User)
                .ToListAsync(ct);
        }

        public async Task<EventParticipant> GetEventParticipantByIdPairAsync(Guid eventId, Guid userId, CancellationToken ct = default)
        {
            var participant = await _context.EventParticipants
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId, ct);

            return participant ?? throw new EntityNotFoundException(typeof(EventParticipant).Name, eventId, userId);
        }

        public async Task<Dictionary<Guid, bool>> GetFeedbackStatusesAsync(Guid userId, List<Guid> eventIds, CancellationToken ct = default)
        {
            return await _context.EventParticipants
                .Where(ep => ep.UserId == userId && eventIds.Contains(ep.EventId))
                .ToDictionaryAsync(
                    ep => ep.EventId,
                    ep => ep.HasCompletedFeedback,
                    ct
                );
        }

        public async Task UpdateEventParticipantWithoutSavingAsync(EventParticipant updatedEntity, CancellationToken ct = default)
        {
            var entityExists = await _context.EventParticipants.AnyAsync(ep => ep.EventId == updatedEntity.EventId && ep.UserId == updatedEntity.UserId, ct);
            if (!entityExists)
            {
                throw new EntityNotFoundException(typeof(EventParticipant).Name, updatedEntity.EventId, updatedEntity.UserId);
            }

            _context.EventParticipants.Update(updatedEntity);
        }

        public async Task DeleteEventParticipantAsync(Guid eventId, Guid userId, CancellationToken ct = default)
        {
            var entityToDelete = await _context.EventParticipants.FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId, ct);
            if (entityToDelete == null)
            {
                throw new EntityNotFoundException(typeof(EventParticipant).Name, eventId, userId);
            }

            _context.EventParticipants.Remove(entityToDelete);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<int> CountEventParticipantsByEventIdAsync(Guid eventId, CancellationToken ct = default)
        {
            return await _context.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .CountAsync(ct);
        }
    }
}
