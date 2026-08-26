using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
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
    private readonly IUserRepository _user_repo;
    public UserController(IUserRepository userRepository)
    {
        _user_repo = userRepository;

    }
    
    // GET: api/users
    [HttpGet]
    
    public async Task<IActionResult> GetUsers()
    {
        var users = await _user_repo.GetAllUsersAsync();
        var usersDto = users.Select(u => u.ToUserDto());
        return Ok(usersDto);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        var user = await _user_repo.GetUserByIdAsync(id);
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

        await _user_repo.CreateUserAsync(user);
        await _user_repo.UpdateUserAsync(user.Id, user);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.ToUserDto());
    }

    // UPDATE: api/users/{id}
    [HttpPut("{id}")]

    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] Dtos.User.UpdateUserRequestDto userDto)
    {
        var user = await _user_repo.GetUserByIdAsync(id);
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

        await _user_repo.UpdateUserAsync(id, user);

        return Ok(user.ToUserDto());
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var user = _user_repo.GetUserByIdAsync(id).Result;
        if (user == null)
        {
            return NotFound();
        }

        await _user_repo.DeleteUserAsync(id);

        return NoContent();
    }

}};