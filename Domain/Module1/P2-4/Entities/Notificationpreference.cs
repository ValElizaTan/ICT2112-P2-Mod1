using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public record NotificationPreferenceInfo(
    int PreferenceId,
    int UserId,
    NotificationFrequency NotificationFrequency,
    NotificationGranularity NotificationGranularity,
    bool EmailEnabled,
    bool SmsEnabled
);

public partial class Notificationpreference
{
    private NotificationFrequency? _notificationfrequency;
    private NotificationFrequency? Notificationfrequency { get => _notificationfrequency; set => _notificationfrequency = value; }
    public void UpdateNotificationfrequency(NotificationFrequency newValue) => _notificationfrequency = newValue;

    private NotificationGranularity? _notificationGranularity;
    private NotificationGranularity? NotificationGranularity { get => _notificationGranularity; set => _notificationGranularity = value; }
    public void UpdateNotificationGranularity(NotificationGranularity newValue) => _notificationGranularity = newValue;

    private int GetPreferenceId() => _preferenceid;
    private int GetUserid() => _userid;
    private bool GetEmailEnabled() => _emailenabled;
    private bool GetSmsEnabled() => _smsenabled;

    private void SetPreferenceId(int preferenceId) => Preferenceid = preferenceId;
    private void SetUserId(int userId) => Userid = userId;
    private void SetEmailEnabled(bool enabled) => Emailenabled = enabled;
    private void SetSmsEnabled(bool enabled) => Smsenabled = enabled;

    public NotificationPreferenceInfo GetNotificationPreferenceInfo() => new NotificationPreferenceInfo(
        GetPreferenceId(),
        GetUserid(),
        _notificationfrequency ?? NotificationFrequency.DAILY,
        _notificationGranularity ?? Enums.NotificationGranularity.ALL,
        GetEmailEnabled(),
        GetSmsEnabled()
    );

    public void SetNotificationPreferenceInfo(NotificationPreferenceInfo info)
    {
        SetUserId(info.UserId);
        UpdateNotificationfrequency(info.NotificationFrequency);
        UpdateNotificationGranularity(info.NotificationGranularity);
        SetEmailEnabled(info.EmailEnabled);
        SetSmsEnabled(info.SmsEnabled);
    }

    public Notificationpreference() { }

    public Notificationpreference(int preferenceId, int userId, NotificationFrequency frequency, NotificationGranularity granularity, bool emailEnabled, bool smsEnabled)
    {
        SetPreferenceId(preferenceId);
        SetUserId(userId);
        UpdateNotificationfrequency(frequency);
        UpdateNotificationGranularity(granularity);
        SetEmailEnabled(emailEnabled);
        SetSmsEnabled(smsEnabled);
    }
}
