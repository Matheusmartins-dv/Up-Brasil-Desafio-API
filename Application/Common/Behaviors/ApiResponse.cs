namespace Application.Common.Behaviors;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    public ApiResponse(T data)
    {
        Success = true;
        Data = data;
        Message = null;
    }

    public ApiResponse(string message)
    {
        Success = false;
        Data = default; 
        Message = message;
    }
}
