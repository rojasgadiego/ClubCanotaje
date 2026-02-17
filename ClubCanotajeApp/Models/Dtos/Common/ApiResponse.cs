namespace ClubCanotajeAPI.Models.Dtos.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public string[] Errors { get; set; } = [];
        public static ApiResponse<T> Ok(T data, string msg = "OK")
            => new() { Success = true, Message = msg, Data = data };
        public static ApiResponse<T> Fail(string error)
            => new() { Success = false, Message = error, Errors = [error] };
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string[] Errors { get; set; } = [];
        public static ApiResponse Ok(string msg = "OK")
            => new() { Success = true, Message = msg };
        public static ApiResponse Fail(string error)
            => new() { Success = false, Message = error, Errors = [error] };
    }
}
