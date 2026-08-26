using api.Dtos.Permission;
using api.Models;

namespace api.Mappers.PermissionMappers
{
    public static class PermissionMapper
    {
        public static PermissionDto ToPermissionDto(this Permission permission)
        {
            return new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name == null ? string.Empty : permission.Name
            };
        }

        public static Permission ToPermission(this CreatePermissionRequestDto createPermissionRequestDto)
        {
            return new Permission
            {
                Name = createPermissionRequestDto.Name
            };
        }

        public static void UpdatePermission(this Permission permission, UpdatePermissionRequestDto updatePermissionRequestDtos)
        {
            permission.Name = updatePermissionRequestDtos.Name;
        }
    }
}