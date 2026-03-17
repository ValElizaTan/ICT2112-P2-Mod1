using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Session
{
    public int SessionId { get; set; }
    public int UserId { get; set; }
    public UserRole RoleEnum => Enum.Parse<UserRole>(Role);
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}