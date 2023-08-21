namespace Metrics;

public record AuthorizationRequest
(
    bool Successful,
    SpendType SpendType
);