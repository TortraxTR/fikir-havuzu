using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Mappers.UserMappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
{
    private readonly FikirHavuzuContext _context;
    public UserController(FikirHavuzuContext context)
    {
        _context = context;
        
    }
    
    // GET: api/users
    [HttpGet]
    
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        var usersDto = users.Select(u => u.ToUserDto());
        return Ok(usersDto);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user.ToUserDto());
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] Dtos.User.CreateUserRequestDto userDto)
    {
        var user = userDto.ToUser();

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.ToUserDto());
    }

    // UPDATE: api/users/{id}
    [HttpPut("{id}")]

    public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] Dtos.User.UpdateUserRequestDto userDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Name = userDto.Name;
        user.Surname = userDto.Surname;
        user.Phone = userDto.Phone;
        user.RegistrationNo = userDto.RegistrationNo;
        user.GovernmentId = userDto.GovernmentId;
        user.PasswordHash = userDto.PasswordHash;
        user.IsActive = userDto.IsActive;

        await _context.SaveChangesAsync();

        return Ok(user.ToUserDto());
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}};