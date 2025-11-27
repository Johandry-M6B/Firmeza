using Application.Sales.Commands.AddPayment;
using Application.Sales.Commands.CancelSale;
using Application.Sales.Commands.CreateSale;
using Application.Sales.DTOs;
using Application.Sales.Queries.GetSaleById;
using Application.Sales.Queries.GetSales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers;

[Authorize]
public class SalesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetSales([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        return Ok(await Mediator.Send(new GetSalesQuery { StartDate = startDate, EndDate = endDate }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetSaleById(int id)
    {
        var sale = await Mediator.Send(new GetSaleByIdQuery(id));
        if (sale == null) return NotFound();
        return Ok(sale);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateSaleCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> Cancel(int id, [FromBody] string reason = "")
    {
        await Mediator.Send(new CancelSaleCommand { SaleId = id, Reason = reason });
        return NoContent();
    }

    [HttpPost("{id}/payments")]
    public async Task<ActionResult> AddPayment(int id, AddPaymentCommand command)
    {
        if (id != command.SaleId) return BadRequest();
        await Mediator.Send(command);
        return NoContent();
    }
}
