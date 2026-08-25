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

    [HttpGet]
    
    public IActionResult GetUsers()
    {
        var users = _context.Users.ToList().Select(u => u.ToUserDto());
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user.ToUserDto());
    }

    [HttpPost]

    public IActionResult CreateUser([FromBody] Dtos.User.CreateUserRequestDto userDto)
    {
        var user = new User
        {
            Name = userDto.Name,
            Surname = userDto.Surname,
            Phone = userDto.Phone,
            RegistrationNo = userDto.RegistrationNo,
            GovernmentId = userDto.GovernmentId,
            PasswordHash = userDto.PasswordHash,
            IsActive = userDto.IsActive
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.ToUserDto());
    }

}};