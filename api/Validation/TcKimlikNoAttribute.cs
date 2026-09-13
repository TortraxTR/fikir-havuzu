using System.ComponentModel.DataAnnotations;

namespace api.Validation
{
    public sealed class TcKimlikNoAttribute : ValidationAttribute
    {
        public TcKimlikNoAttribute()
            : base("T.C. Kimlik No geçersiz.")
        {
        }

        public override bool IsValid(object? value) =>
            value is string governmentId && TcKimlikNoValidator.IsValid(governmentId);
    }
}
