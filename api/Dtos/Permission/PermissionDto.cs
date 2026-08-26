namespace api.Dtos.Permission
{
    public class PermissionDto
    {
        public Guid Id { get; set; } // Yetki ID

        public string Name { get; set; } = null!; // Yetki Adı

    }
}