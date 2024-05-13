namespace Weather.Domain.Dtos;

public class Response<T>
{
    public bool IsSuccessful { get; set; }

    public string ErrorMessage { get; set; }

    public T Data { get; set; }

    public Response(bool isSuccessful, string errorMessage = "", T data = default)
    {
        if (isSuccessful)
        {
            IsSuccessful = true;
            ErrorMessage = string.Empty;
            Data = data;
        }
        else
        {
            IsSuccessful = false;
            ErrorMessage = errorMessage;
            Data = default(T);
        }
    }
}
