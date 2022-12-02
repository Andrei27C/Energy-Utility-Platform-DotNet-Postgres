using DotNet_CSharp_React_EnergyApp_DS.DTOs;
using DotNet_CSharp_React_EnergyApp_DS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet_CSharp_React_EnergyApp_DS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceMappingController : ControllerBase
{
    private readonly DeviceManagerContext _context;

    public DeviceMappingController(DeviceManagerContext context)
    {
        _context = context;
    }

    // GET: api/DeviceMapping
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceMapping>>> GetDeviceMappings()
    {
        if (_context.DeviceMappings == null) return NotFound();
        return await _context.DeviceMappings.ToListAsync();
    }

    // GET: api/DeviceMapping/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceMapping>> GetDeviceMapping(int id)
    {
        if (_context.DeviceMappings == null) return NotFound();
        var deviceMapping = await _context.DeviceMappings.FindAsync(id);

        if (deviceMapping == null) return NotFound();

        return deviceMapping;
    }

    // PUT: api/DeviceMapping/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDeviceMapping(int id, DeviceMapping deviceMapping)
    {
        if (id != deviceMapping.DeviceMappingId) return BadRequest();

        _context.Entry(deviceMapping).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DeviceMappingExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/DeviceMapping
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<DeviceMapping>> PostDeviceMapping(DeviceMappingDTO deviceMappingDTO)
    {
        if (_context.DeviceMappings == null)
            return Problem("Entity set 'DeviceManagerContext.DeviceMappings'  is null.");

        DeviceMapping deviceMapping = new DeviceMapping();
        deviceMapping.User = _context.Users.Find(deviceMappingDTO.userId);
        deviceMapping.Device = _context.Devices.Find(deviceMappingDTO.deviceId);
        
        _context.DeviceMappings.Add(deviceMapping);
        
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDeviceMapping", new { id = deviceMapping.DeviceMappingId }, deviceMapping);
    }

    // DELETE: api/DeviceMapping/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDeviceMapping(int id)
    {
        if (_context.DeviceMappings == null) return NotFound();
        var deviceMapping = await _context.DeviceMappings.FindAsync(id);
        if (deviceMapping == null) return NotFound();

        _context.DeviceMappings.Remove(deviceMapping);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DeviceMappingExists(int id)
    {
        return (_context.DeviceMappings?.Any(e => e.DeviceMappingId == id)).GetValueOrDefault();
    }
}