using ProRental.Domain.Entities;

namespace ProRental.Domain.Module1.P24.Interfaces;

public interface INotificationPreferenceService
{
    Notificationpreference? GetPreference(int userId);
    List<Notificationpreference> GetAllPreferences();
    void SetPreference(Notificationpreference preference);
    void UpdatePreference(Notificationpreference preference);
    void DeletePreference(int preferenceId);
}
