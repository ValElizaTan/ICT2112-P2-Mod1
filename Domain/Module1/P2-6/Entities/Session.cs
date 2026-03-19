// Your editable Session.cs (P2-6)
using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Session
{
    // Computed from the auto-generated Role property
    public UserRole RoleEnum => Enum.Parse<UserRole>(Role);
    public bool IsExpired() => DateTime.UtcNow >= Expiresat;

    // Factory method — avoids needing to set private properties directly
    public static Session Create(int userId, UserRole role)
    {
        // Use reflection to bypass private set
        var session = new Session();
        typeof(Session).GetProperty("Userid")!.SetValue(session, userId);
        typeof(Session).GetProperty("Role")!.SetValue(session, role.ToString());
        typeof(Session).GetProperty("Createdat")!.SetValue(session, DateTime.UtcNow);
        typeof(Session).GetProperty("Expiresat")!.SetValue(session, DateTime.UtcNow.AddHours(2));
        return session;
    }
}