using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

public class NotificationPreferenceControl : INotificationPreferenceService
{
    private readonly INotificationPreferenceGateway _preferenceGateway;

    public NotificationPreferenceControl(INotificationPreferenceGateway preferenceGateway)
    {
        _preferenceGateway = preferenceGateway;
    }

    public Notificationpreference? GetPreference(int userId)
    {
        return _preferenceGateway.FindByUser(userId);
    }

    public List<Notificationpreference> GetAllPreferences()
    {
        return _preferenceGateway.FindAll();
    }

    public void SetPreference(Notificationpreference preference)
    {
        var userId = preference.GetNotificationPreferenceInfo().UserId;
        var existing = _preferenceGateway.FindByUser(userId);

        if (existing == null)
        {
            _preferenceGateway.InsertPreference(preference);
        }
        else
        {
            _preferenceGateway.UpdatePreference(preference);
        }
    }

    public void UpdatePreference(Notificationpreference preference)
    {
        _preferenceGateway.UpdatePreference(preference);
    }

    public void DeletePreference(int preferenceId)
    {
        _preferenceGateway.DeletePreference(preferenceId);
    }
}

