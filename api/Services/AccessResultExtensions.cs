using api.Authorization;
using api.Common;

namespace api.Services
{
    internal static class AccessResultExtensions
    {
        public static Result ToFailure(this AccessResult access) => access.Status switch
        {
            AccessStatus.NoUser => Result.Fail(ResultError.Unauthorized, "Geçerli bir kullanıcı gereklidir."),
            AccessStatus.Inactive => Result.Fail(ResultError.Forbidden, "Aktif bir kullanıcı gereklidir."),
            _ => Result.Fail(ResultError.Forbidden, "Bu işlem için yetkiniz yok."),
        };

        public static Result<T> ToFailure<T>(this AccessResult access) =>
            Result<T>.Fail(access.ToFailure());
    }
}
