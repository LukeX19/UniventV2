using MediatR;
using Univent.App.Interfaces;

namespace Univent.App.EventTypes.Commands
{
    public record EnableEventTypeCommand(Guid Id) : IRequest<Unit>;

    public class EnableEventTypeHandler : IRequestHandler<EnableEventTypeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EnableEventTypeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(EnableEventTypeCommand request, CancellationToken ct)
        {
            var eventType = await _unitOfWork.EventTypeRepository.GetByIdAsync(request.Id, ct);

            eventType.IsDeleted = false;

            await _unitOfWork.EventTypeRepository.UpdateAsync(eventType, ct);

            return Unit.Value;
        }
    }
}
