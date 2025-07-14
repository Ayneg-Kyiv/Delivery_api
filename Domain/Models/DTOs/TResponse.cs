namespace Domain.Models.DTOs
{
    public class TResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }
        public object? Data { get; set; }

        public static TResponse Successful(object? data = null, string? message = null)
        {
            return new TResponse
            {
                StatusCode = 200,
                Success = true,
                Message = message ?? "Operation completed successfully.",
                Data = data
            };
        }

        public static TResponse Failure(int statusCode, string? message = null, object? data = null)
        {
            return new TResponse
            {
                StatusCode = statusCode,
                Success = false,
                Message = message ?? "Operation failed.",
                Data = data
            };
        }
    }
}
