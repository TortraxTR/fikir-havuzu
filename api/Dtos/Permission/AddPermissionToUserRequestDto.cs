using System;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Permission
{
    public class AddPermissionToUserRequestDto
    {
        [Required]
        public Guid PermissionId { get; set; } // Eklenecek yetki kimliği
    }
}
