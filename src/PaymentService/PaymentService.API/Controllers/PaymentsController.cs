using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Payments.Queries;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("api/v1/payments")]
public sealed class PaymentsController(IMediator mediator) : ControllerBase
{
    [HttpGet("by-order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrderId(Guid orderId, CancellationToken ct)
    {
        var p = await mediator.Send(new GetPaymentByOrderIdQuery(orderId), ct);
        return p is null ? NotFound() : Ok(p);
    }
}