namespace SmartCity.Application.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public List<string>? Errors { get; set; }

        // ✅ Success
        public static ApiResponse<T> SuccessResponse(string message, T data)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = null
            };
        }

        // ❌ Failure (single error)
        public static ApiResponse<T> FailResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = new List<string> { message }
            };
        }

        // ❌ Failure (multiple errors)
        public static ApiResponse<T> FailResponse(string message, List<string> errors)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
    }
}