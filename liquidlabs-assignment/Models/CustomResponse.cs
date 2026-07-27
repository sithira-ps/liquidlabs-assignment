namespace liquidlabs_assignment.Models;

public class ErrorResponse
{
    public string status { get; set; } = string.Empty;
    public string error { get; set; } = string.Empty;
    public string? details { get; set; }
}


public class SuccessResponse<T>
{
    public string status { get; set; } = string.Empty;
    public T? data { get; set; }
}