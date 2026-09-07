namespace api.Dtos.Permission
{
    public class PermissionDto
    {
        public Guid Id { get; set; } // Yetki ID

        public string Code { get; set; } = null!; // Makine tarafından kullanılan yetki kodu

        public string Name { get; set; } = null!; // Yetki Adı

    }
}