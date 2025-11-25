using Application.Measurements.Commands.DeleteMeasurements;
using Application.Measurements.Commands.UpdateMeasurements;
using Application.Measurements.DTOs;
using Application.Measurements.Queries.CreateMeasurement;
using Application.Measurements.Queries.GetMeasurementById;
using Application.Products.Queries.GetMeasurements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers;

[Authorize]
public class MeasurementsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeasurementDto>>> GetMeasurements([FromQuery] bool onlyActive = true)
    {
        return Ok(await Mediator.Send(new GetMeasurementsQuery { OnlyActive = onlyActive }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MeasurementDto>> GetMeasurementById(int id)
    {
        var measurement = await Mediator.Send(new GetMeasurementByIdQuery(id));
        if (measurement == null) return NotFound();
        return Ok(measurement);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateMeasurementCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateMeasurementCommand command)
    {
        if (id != command.Id) return BadRequest();
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteMeasurementCommand { Id = id });
        return NoContent();
    }
}
