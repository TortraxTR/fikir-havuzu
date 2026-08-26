namespace api.Dtos.User
{
    public class UserDto
    {
        public Guid Id { get; set; } // Kullanıcı ID

        public string Name { get; set; } = null!; // Adı

        public string Surname { get; set; } = null!; // Soyadı

        public string Phone { get; set; } = null!; // Telefon Numarası

        public string RegistrationNo { get; set; } = null!; //Sicil No.

        public string GovernmentId { get; set; } = null!; // T.C. Kimlik No.

        public bool IsActive { get; set; } // Kullanıcı aktif mi?
    }
}