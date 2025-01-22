namespace SagaPattern.Shared;

// PaymentProcessedEvent.cs
public record PaymentProcessedEvent
{
    public Guid OrderId { get; init; }
    public bool Success { get; init; }
    public string? FailureReason { get; init; }
}
