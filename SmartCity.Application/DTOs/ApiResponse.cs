using System.Text.Json.Serialization;

namespace SmartCity.Application.DTOs
{
    public class ApiResponse<T>
    {
        public string Message { get; set; } = string.Empty;

        // ✅ FIXED: works for both value types (Guid, int) and reference types
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Data { get; set; }

        // ✅ keep this (only appears when errors exist)
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Errors { get; set; }

        // ✅ Success (with data)
        public static ApiResponse<T> SuccessResponse(string message, T data)
        {
            return new ApiResponse<T>
            {
                Message = message,
                Data = data
            };
        }

        // ✅ Success (no data)
        public static ApiResponse<T> SuccessResponse(string message)
        {
            return new ApiResponse<T>
            {
                Message = message
            };
        }

        // ❌ Failure
        public static ApiResponse<T> FailResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Message = message,
                Errors = errors ?? new List<string> { message }
            };
        }
    }
}