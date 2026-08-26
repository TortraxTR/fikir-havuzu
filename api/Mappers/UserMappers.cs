namespace api.Mappers.UserMappers
{
    public static class UserMapper
    {
        public static Dtos.User.UserDto ToUserDto(this Models.User user)
        {
            return new Dtos.User.UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Phone = user.Phone,
                RegistrationNo = user.RegistrationNo,
                GovernmentId = user.GovernmentId,
                IsActive = user.IsActive
            };
        }

        public static Models.User ToUser(this Dtos.User.CreateUserRequestDto userDto)
        {
            return new Models.User
            {
                Name = userDto.Name,
                Surname = userDto.Surname,
                Phone = userDto.Phone,
                RegistrationNo = userDto.RegistrationNo,
                GovernmentId = userDto.GovernmentId,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                IsActive = userDto.IsActive
            };
        }
    }
} 