using System.ComponentModel.DataAnnotations;

namespace api.Dtos.User
{
    public class UpdateUserRequestDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!; // Adı

        [Required]
        [StringLength(100, ErrorMessage = "Surname cannot exceed 100 characters.")]
        public string Surname { get; set; } = null!; // Soyadı

        [Required]
        [Phone]
        [StringLength(15, ErrorMessage = "Phone cannot exceed 15 characters.")]
        public string Phone { get; set; } = null!; // Telefon Numarası

        [Required]
        [StringLength(50, ErrorMessage = "Registration number cannot exceed 50 characters.")]
        public string RegistrationNo { get; set; } = null!; // Sicil No.

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Government ID must be exactly 11 characters.")]
        public string GovernmentId { get; set; } = null!; // T.C. Kimlik No.

        public bool IsActive { get; set; } // Kullanıcı aktif mi?
    }
}