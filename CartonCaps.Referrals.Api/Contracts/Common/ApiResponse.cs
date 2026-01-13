namespace CartonCaps.Referrals.Api.Contracts.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public int Status { get; init; }
        public T? Data { get; init; }
        public List<ApiError> Errors { get; init; } = new List<ApiError>();

        public static ApiResponse<T> Ok(T data, int status = 200)
            => new() { Success = true, Status = status, Data = data };

        public static ApiResponse<T> Fail(int status, IEnumerable<ApiError> errors)
            => new() { Success = false, Status = status, Errors = errors.ToList() };

        public static ApiResponse<T> Fail(int status, ApiError error)
            => new() { Success = false, Status = status, Errors = new List<ApiError> { error } };
    }

    public sealed class ApiError
    {
        public string Code { get; init; }
        public string Message { get; init; }
    }
}
