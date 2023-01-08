using DotNet_CSharp_React_EnergyApp_DS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCSharpReact_EnergyApp_DS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsumptionPerDeviceController : ControllerBase
{
    private readonly DeviceManagerContext _context;

    public ConsumptionPerDeviceController(DeviceManagerContext context)
    {
        _context = context;
    }

    // GET: api/ConsumptionPerDevice
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConsumptionPerDevice>>> GetConsumptionPerDevice()
    {
        if (_context.ConsumptionPerDevice == null) return NotFound();
        return await _context.ConsumptionPerDevice.ToListAsync();
    }

    // GET: api/ConsumptionPerDevice/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ConsumptionPerDevice>> GetConsumptionPerDevice(int id)
    {
        if (_context.ConsumptionPerDevice == null) return NotFound();
        var consumptionPerDevice = await _context.ConsumptionPerDevice.FindAsync(id);

        if (consumptionPerDevice == null) return NotFound();

        return consumptionPerDevice;
    }

    // PUT: api/ConsumptionPerDevice/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutConsumptionPerDevice(int id, ConsumptionPerDevice consumptionPerDevice)
    {
        if (id != consumptionPerDevice.ConsumptionPerDeviceId) return BadRequest();

        _context.Entry(consumptionPerDevice).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ConsumptionPerDeviceExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/ConsumptionPerDevice
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ConsumptionPerDevice>> PostConsumptionPerDevice(
        ConsumptionPerDevice consumptionPerDevice)
    {
        if (_context.ConsumptionPerDevice == null)
            return Problem("Entity set 'DeviceManagerContext.ConsumptionPerDevice'  is null.");
        _context.ConsumptionPerDevice.Add(consumptionPerDevice);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetConsumptionPerDevice", new { id = consumptionPerDevice.ConsumptionPerDeviceId },
            consumptionPerDevice);
    }

    // DELETE: api/ConsumptionPerDevice/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConsumptionPerDevice(int id)
    {
        if (_context.ConsumptionPerDevice == null) return NotFound();
        var consumptionPerDevice = await _context.ConsumptionPerDevice.FindAsync(id);
        if (consumptionPerDevice == null) return NotFound();

        _context.ConsumptionPerDevice.Remove(consumptionPerDevice);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ConsumptionPerDeviceExists(int id)
    {
        return (_context.ConsumptionPerDevice?.Any(e => e.ConsumptionPerDeviceId == id)).GetValueOrDefault();
    }
}