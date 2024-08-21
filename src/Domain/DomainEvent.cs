using MediatR;

namespace VerticalSliceMinimalApi.Domain;

public record DomainEvent(Guid Id) : INotification;

