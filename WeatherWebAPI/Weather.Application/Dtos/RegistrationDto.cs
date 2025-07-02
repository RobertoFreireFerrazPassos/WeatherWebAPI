namespace Weather.Application.Dtos;

public class RegistrationDto
{
    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Address { get; set; }

    public DateTime Birthdate { get; set; }

    public string PhoneNumber { get; set; }

    public string LivingCountry { get; set; }

    public string CitizenCountry { get; set; }
}
