using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TestMetrics.Controllers;

[ApiController]
[Route("account")]
public class Authorization : ControllerBase
{
    private readonly TransactionMetrics _meters;

    public Authorization(TransactionMetrics meters)
    {
        _meters = meters;
    }

    [HttpPost("auth", Name = "CreateAuthorization")]
    public IActionResult Authorize([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] AuthorizationRequest authorizationRequest)
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