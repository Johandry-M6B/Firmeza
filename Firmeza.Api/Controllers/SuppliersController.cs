using Application.Suppliers.Commands.CreateSuppliers;
using Application.Suppliers.Commands.DeleteSuppliers;
using Application.Suppliers.Commands.UpdateSupplier;
using Application.Suppliers.DTOs;
using Application.Suppliers.Queries.GetSupplierById;
using Firmeza.Application.Suppliers.Queries.GetSuppliers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers;

[Authorize]
public class SuppliersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSuppliers([FromQuery] bool onlyActive = true)
    {
        return Ok(await Mediator.Send(new GetSuppliersQuery { OnlyActive = onlyActive }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierDto>> GetSupplierById(int id)
    {
        var supplier = await Mediator.Send(new GetSupplierByIdQuery(id));
        if (supplier == null) return NotFound();
        return Ok(supplier);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateSupplierCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateSupplierCommand command)
    {
        if (id != command.Id) return BadRequest();
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteSupplierCommand { Id = id });
        return NoContent();
    }
}
