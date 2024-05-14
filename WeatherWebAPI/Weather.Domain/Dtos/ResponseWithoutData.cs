namespace Weather.Domain.Dtos;

public class ResponseWithoutData
{
    public bool IsSuccessful { get; set; }

    public string ErrorMessage { get; set; }

    public ResponseWithoutData(bool isSuccessful, string errorMessage = "")
    {
        if (isSuccessful)
        {
            IsSuccessful = true;
            ErrorMessage = string.Empty;
        }
        else
        {
            IsSuccessful = false;
            ErrorMessage = errorMessage;
        }
    }
}
