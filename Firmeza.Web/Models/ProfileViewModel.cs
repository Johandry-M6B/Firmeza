namespace Firmeza.Web.Models;

public class ProfileViewModel
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
}