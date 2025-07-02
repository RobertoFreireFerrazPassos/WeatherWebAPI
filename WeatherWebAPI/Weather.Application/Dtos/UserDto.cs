namespace Weather.Application.Dtos;

public class UserDto
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
}
