namespace TestMetrics;

public record AuthorizationRequest
(
    bool Successful,
    SpendType SpendType
);