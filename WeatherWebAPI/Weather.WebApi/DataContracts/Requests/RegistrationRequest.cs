namespace Weather.WebApi.DataContracts.Requests;

public class RegistrationRequest
{
    [Required(ErrorMessage = "First name is required")]
    [MinLength(2, ErrorMessage = "First name can not be shorter than 2 characters")]
    [MaxLength(30, ErrorMessage = "First name can not be longer than 30 characters")]
    public string Firstname { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [MinLength(2, ErrorMessage = "Last name can not be shorter than 2 characters")]
    [MaxLength(100, ErrorMessage = "Last name can not be longer than 100 characters")]
    public string Lastname { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password can not be shorter than 6 characters")]
    [MaxLength(20, ErrorMessage = "Password can not be longer than 20 characters")]
    public string Password { get; set; }

    public string Address { get; set; }

    [Required(ErrorMessage = "Birthdate is required")]
    [DataType(DataType.Date, ErrorMessage = "Birthdate is invalid")]
    public DateTime Birthdate { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [MinLength(6, ErrorMessage = "Phone number can not be shorter than 6 characters")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Living country is required")]
    public string LivingCountry { get; set; }

    [Required(ErrorMessage = "Citizen country is required")]
    public string CitizenCountry { get; set; }
}