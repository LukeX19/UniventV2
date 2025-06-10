using MediatR;
using Univent.App.Exceptions;
using Univent.App.Feedbacks.Dtos;
using Univent.App.Interfaces;
using Univent.Domain.Models.Users;

namespace Univent.App.Feedbacks.Commands
{
    public record CreateMultipleFeedbacksCommand(Guid SenderUserId, Guid EventId, CreateMultipleFeedbacksDto feedbacksDto) : IRequest<Unit>;

    public class CreateMultipleFeedbacksHandler : IRequestHandler<CreateMultipleFeedbacksCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMultipleFeedbacksHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateMultipleFeedbacksCommand request, CancellationToken ct)
        {
            // Start transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(request.EventId, ct);
                var eventParticipant = await _unitOfWork.EventParticipantRepository.GetEventParticipantByIdPairAsync(request.EventId, request.SenderUserId, ct);

                if (request.feedbacksDto.Recipients.Any(r => r.UserId == request.SenderUserId))
                {
                    throw new InvalidOperationException("A user can not give feedback to themselves.");
                }

                if (request.feedbacksDto.Recipients == null || !request.feedbacksDto.Recipients.Any())
                {
                    throw new InvalidOperationException("Feedback recipient list is empty.");
                }

                if (eventParticipant.HasCompletedFeedback == true)
                {
                    throw new FeedbackAlreadySubmittedException(request.SenderUserId, request.EventId);
                }

                if (DateTime.UtcNow < eventEntity.StartTime || eventEntity.IsCancelled == true)
                {
                    throw new FeedbackClosedException(request.EventId);
                }

                var feedbacks = request.feedbacksDto.Recipients.Select(r => new Feedback
                {
                    Id = Guid.NewGuid(),
                    UserId = r.UserId,
                    Rating = r.Rating
                }).ToList();

                await _unitOfWork.FeedbackRepository.AddRangeAsync(feedbacks, ct);

                eventParticipant.HasCompletedFeedback = true;
                await _unitOfWork.EventParticipantRepository.UpdateEventParticipantWithoutSavingAsync(eventParticipant, ct);

                await _unitOfWork.SaveChangesAsync();

                // Commit transaction if everything succeeds
                await _unitOfWork.CommitTransactionAsync();

                return Unit.Value;
            }
            catch
            {
                // Undo everything if something fails
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
