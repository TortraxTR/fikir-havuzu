namespace api.Dtos.Permission
{
    public class UpdatePermissionRequestDto
    {
        public string Code { get; set; } = null!; // Makine tarafından kullanılan yetki kodu

        public string Name { get; set; } = null!; // Yetki Adı

    }
}