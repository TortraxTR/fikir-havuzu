using api.Dtos.Auth;
using api.Models;

namespace api.Mappers.AuthMappers;

public static class AuthMapper
{
    public static LoginResponseDto ToLoginResponseDto(this User user)
    {
        return new LoginResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Phone = user.Phone,
            IsActive = user.IsActive,
            Permissions = user.Permissions
                .Where(permission => permission.Name != null)
                .Select(permission => permission.Name!)
                .ToList()
        };
    }
}