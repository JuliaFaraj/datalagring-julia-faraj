namespace CoursesManager.Application.Helpers;


public enum ResultStatus
{
    Ok,
    Badrequest,
    NotFound,
    Conflict
}

public class ResponseResult<T>
{
    public ResultStatus Status { get; init; }
    public T? Data { get; init; }
    public string? Message { get; init; }
    public IReadOnlyList<string>? Errors { get; init; }
    public bool IsSucess => Status == ResultStatus.Ok;



    public static ResponseResult<T> OK()
        => new() { Status = ResultStatus.Ok };

    public static ResponseResult<T> OK(T data)
    => new() { Status = ResultStatus.Ok, Data = data };

    public static ResponseResult<T> NotFound(string message)
    => new() { Status = ResultStatus.NotFound, Message = message };

    public static ResponseResult<T> Conflict(string message)
    => new() { Status = ResultStatus.Conflict, Message = message };

    public static ResponseResult<T> BadRequest(string message, IReadOnlyList<string>? errors = null)
    => new() { Status = ResultStatus.Badrequest, Message = message, Errors = errors };
}