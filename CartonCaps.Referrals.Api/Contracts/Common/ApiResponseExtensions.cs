using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Referrals.Api.Contracts.Common
{
    public static class ApiResponseExtensions
    {
        public static ActionResult<ApiResponse<T>> ApiOk<T>(this ControllerBase controller, T data)
        {
            return controller.Ok(ApiResponse<T>.Ok(data, status: 200));
        }

        public static ActionResult<ApiResponse<T>> ApiError<T>(this ControllerBase controller, int status, params ApiError[] errors)
        {
            return controller.StatusCode(status, ApiResponse<T>.Fail(status, errors.ToList()));
        }
    }
}
