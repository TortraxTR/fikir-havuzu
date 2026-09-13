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
            Email = user.Email,
            Phone = user.Phone,
            IsActive = user.IsActive,
            // Codes, not display names — LandingPage.tsx compares these against permission
            // codes to decide which tabs to show.
            Permissions = user.Permissions
                .Select(permission => permission.Code)
                .ToList()
        };
    }
}