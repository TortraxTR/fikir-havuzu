using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.User
{
    public class CreateUserRequestDto
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string RegistrationNo { get; set; } = null!;

        public string GovernmentId { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}