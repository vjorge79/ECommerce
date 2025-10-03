using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands;
using OrderService.Application.Orders.DTOs;
using OrderService.Application.Orders.Queries;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/v1/orders")]
public sealed class OrdersController(IMediator mediator, ILogger<OrdersController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Place([FromBody] CreateOrderCommand cmd, CancellationToken ct)
    {
        logger.LogInformation("Recebido pedido do cliente {BuyerId}", cmd.BuyerId);
        var id = await mediator.Send(cmd, ct);
        logger.LogInformation("Pedido criado {OrderId}", id);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var order = await mediator.Send(new GetOrderByIdQuery(id), ct);

        return order is null ? NotFound() : Ok(order);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        var deleted = await mediator.Send(new DeleteOrderCommand(id), ct);

        if (!deleted) return NotFound();

        //logger.LogInformation("Pedido {OrderId} excluído com sucesso", id);
        return NoContent();
    }
}