using DotNet_CSharp_React_EnergyApp_DS.DTOs;
using DotNet_CSharp_React_EnergyApp_DS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet_CSharp_React_EnergyApp_DS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DeviceManagerContext _context;

    public UserController(DeviceManagerContext context)
    {
        _context = context;
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        if (_context.Users == null) return NotFound();
        // Console.WriteLine(_context.Users.ToListAsync().Id);
        return await _context.Users.ToListAsync();
    }

    [HttpPost]
    [Route("login")]
    public async Task<User> LoginUserAsync(UserLoginDTO userLoginDTO)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userLoginDTO.Username);
        if (user != null)
            if (user.Password == userLoginDTO.Password)
                return user;
        return null;
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        if (_context.Users == null) return NotFound();
        var user = await _context.Users.FindAsync(id);

        if (user == null) return NotFound();

        return user;
    }

    // PUT: api/User/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.UserId) return BadRequest();

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/User
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        if (_context.Users == null) return Problem("Entity set 'DeviceManagerContext.Users'  is null.");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = user.UserId }, user);
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (_context.Users == null) return NotFound();
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
    }
}