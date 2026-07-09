namespace Application.DTOs.Response;


public class ResponseResult
{
    public bool Success {get; set;}
    public string Message  {get; set;} = string.Empty;
    public object? Data {get; set;}


    public static ResponseResult Sucesso(string mensagem, object? data = null)
    {
        return new ResponseResult
        {
            Success = true,
            Message = mensagem,
            Data = data
        };
    }


    public static ResponseResult Erro(string mensagem)
    {
        return new ResponseResult
        {
            Success = false,
            Message = mensagem   
        };
    }
}