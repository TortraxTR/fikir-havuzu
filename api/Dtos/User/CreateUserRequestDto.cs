namespace api.Dtos.User
{
    public class CreateUserRequestDto
    {
        public string Name { get; set; } = null!; // Adı

        public string Surname { get; set; } = null!; // Soyadı

        public string Phone { get; set; } = null!; // Telefon Numarası

        public string RegistrationNo { get; set; } = null!; // Sicil No.

        public string GovernmentId { get; set; } = null!; // T.C. Kimlik No.

        public string Password { get; set; } = null!; // Şifre

        public bool IsActive { get; set; } // Kullanıcı aktif mi?
    }
}