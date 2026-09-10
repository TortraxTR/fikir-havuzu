using Microsoft.AspNetCore.Mvc;

namespace api.Common
{
    public static class ResultExtensions
    {
        private static int StatusCodeFor(ResultError error) => error switch
        {
            ResultError.Unauthorized => StatusCodes.Status401Unauthorized,
            ResultError.Forbidden => StatusCodes.Status403Forbidden,
            ResultError.NotFound => StatusCodes.Status404NotFound,
            ResultError.Validation => StatusCodes.Status400BadRequest,
            ResultError.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        public static ActionResult ToActionResult(this Result result) =>
            result.Ok ? new NoContentResult() : result.Error();

        public static ActionResult<T> ToActionResult<T>(this Result<T> result) =>
            result.Ok ? new OkObjectResult(result.Value) : result.Error();

        public static ActionResult Error(this Result result) =>
            new ObjectResult(result.Message) { StatusCode = StatusCodeFor(result.Error) };
    }
}
