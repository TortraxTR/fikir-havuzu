namespace api.Dtos.Auth;

public class LoginResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public IEnumerable<string> Permissions { get; set; } = [];
}