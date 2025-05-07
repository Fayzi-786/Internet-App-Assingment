using soft20181_starter.Models;

public class UserEventRegistration
{
    public int Id { get; set; }
    public string UserId { get; set; } // Assuming you're using ASP.NET Core Identity
    public int EventId { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;


    public Event Event { get; set; }
    public UsersInfo User { get; set; } // Assuming you have ApplicationUser
}