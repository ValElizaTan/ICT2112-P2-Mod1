using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Interfaces;

public interface INotificationPreferenceGateway
{
    Notificationpreference? FindById(int preferenceId);
    Notificationpreference? FindByUser(int userId);
    List<Notificationpreference> FindAll();
    void InsertPreference(Notificationpreference preference);
    void UpdatePreference(Notificationpreference preference);
    void DeletePreference(int preferenceId);
}
