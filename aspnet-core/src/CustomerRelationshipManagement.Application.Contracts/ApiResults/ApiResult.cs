
namespace CustomerRelationshipManagement.ApiResults;

public enum ResultCode
{
    Success = 200,// 成功
    Fail = 400,// 失败
    NotFound = 404,// 未找到
    Unauthorized = 401,// 未授权
    InternalServerError = 500// 服务器内部错误
}   
public class ApiResult
{
    public bool IsSuc { get; set; }

    public string Msg { get; set; }

    public ResultCode Code { get; set; }

    public ApiResult(bool isSuc, string msg, ResultCode code)
    {
        Code = code;
        Msg = msg;
        IsSuc = isSuc;
    }

    public static ApiResult Success(ResultCode code)
    {
        return new ApiResult(true, "操作成功", code);
    }

    public static ApiResult Fail(string msg, ResultCode code)
    {
        return new ApiResult(false, msg, code);
    }
}

public class ApiResult<T> : ApiResult
{
    public T Data { get; set; }
    public ApiResult(bool isSuc, string msg, ResultCode code, T data) : base(isSuc, msg, code)
    {
        Data = data;
        IsSuc = isSuc;
        Msg = msg;
        Code = code;
    }

    public static ApiResult<T> Success(ResultCode code, T data)
    {
        return new ApiResult<T>(true, "操作成功", code, data);
    }

    public static ApiResult<T> Fail(string msg, ResultCode code)
    {
        return new ApiResult<T>(false, msg, code, default);
    }
}
