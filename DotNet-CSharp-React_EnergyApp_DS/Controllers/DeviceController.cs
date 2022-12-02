using DotNet_CSharp_React_EnergyApp_DS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet_CSharp_React_EnergyApp_DS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly DeviceManagerContext _context;

    public DeviceController(DeviceManagerContext context)
    {
        _context = context;
    }

    // GET: api/Device
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
    {
        if (_context.Devices == null) return NotFound();
        return await _context.Devices.ToListAsync();
    }

    // GET: api/Device/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Device>> GetDevice(int id)
    {
        if (_context.Devices == null) return NotFound();
        var device = await _context.Devices.FindAsync(id);

        if (device == null) return NotFound();

        return device;
    }

    // PUT: api/Device/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDevice(int id, Device device)
    {
        if (id != device.DeviceId) return BadRequest();

        _context.Entry(device).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DeviceExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Device
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Device>> PostDevice(Device device)
    {
        if (_context.Devices == null) return Problem("Entity set 'DeviceManagerContext.Devices'  is null.");
        _context.Devices.Add(device);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDevice", new { id = device.DeviceId }, device);
    }

    // DELETE: api/Device/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        if (_context.Devices == null) return NotFound();
        var device = await _context.Devices.FindAsync(id);
        if (device == null) return NotFound();

        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DeviceExists(int id)
    {
        return (_context.Devices?.Any(e => e.DeviceId == id)).GetValueOrDefault();
    }
}