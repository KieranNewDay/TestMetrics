using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Metrics.Controllers;

[ApiController]
[Route("account/{accountId:guid}")]
public class Authorization : ControllerBase
{
    private readonly TransactionMetrics _meters;

    public Authorization(TransactionMetrics meters)
    {
        _meters = meters;
    }

    [HttpPost("auth", Name = "CreateAuthorization")]
    public IActionResult Authorize([FromRoute] Guid accountId, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] AuthorizationRequest authorizationRequest)
    {
        if (authorizationRequest.Successful)
        {
            _meters.AddApprovedAuthorization(authorizationRequest.SpendType);
        }
        else
        {
            _meters.AddDeclinedAuthorization(authorizationRequest.SpendType);
        }
        
        return Ok();
    }
}