namespace Weather.Application.DataContracts.Requests;

public class RegistrationRequest
{
    public string FullName { get; set; }

    public string Password { get; set; }

    public (bool IsValid, string ErrorMessage) IsValid()
    {
        if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Password))
        {
            return (false, "Full name and password are mandatory fields");
        }

        foreach (var name in FullName.Split(' '))
        {
            foreach (var i in name)
            {
                if (!char.IsLetter(i))
                {
                    return (false, "Full name contains non-letter characters");
                }
            }
        }

        foreach (var i in Password)
        {
            if (!char.IsLetterOrDigit(i))
            {
                return (false, "Full name contains non-letter digit characters");
            }
        }

        var names = FullName.Split(' ');

        if (names.Length < 2)
        {
            return (false, "Full name has less than 2 names");
        }

        return (true, string.Empty);
    }
}