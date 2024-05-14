namespace Weather.Domain.Entities;

public class User : Entity
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Address { get; set; }
    public DateTime Birthdate { get; set; }
    public string PhoneNumber { get; set; }
    public string LivingCountry { get; set; }
    public string CitizenCountry { get; set; }

    public void GenerateName()
    {
        Username = Firstname.Substring(0, 2).ToLower() + Lastname.Substring(0, 2).ToLower() + (new Random()).Next(10000, 99999);
    }

    public bool IsValidPhoneNumber(string idd)
    {
        return PhoneNumber.StartsWith(idd);
    }
}
