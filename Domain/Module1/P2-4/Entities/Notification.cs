using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public record NotificationInfo(
    int NotificationId, int UserId, string Message, NotificationType Type, bool IsRead, DateTime DateSent
);

public partial class Notification
{
    private NotificationType? _notificationType;
    private NotificationType? NotificationType { get => _notificationType; set => _notificationType = value; }
    public void UpdateNotificationType(NotificationType newValue) => _notificationType = newValue;

    private int GetNotificationId() => _notificationid;
    private int GetUserid() => _userid;
    private string GetMessage() => _message;
    private bool GetIsRead() => _isread;
    private DateTime GetDateSent() => _datesent;
    private void SetNotificationId(int notificationId) => Notificationid = notificationId;
    private void SetUserId(int userId) => Userid = userId;
    private void SetMessage(string message) => Message = message;
    private void SetIsRead(bool isRead) => Isread = isRead;
    private void SetDateSent(DateTime dateSent) => Datesent = dateSent;

    public NotificationInfo GetNotificationInfo() => new NotificationInfo(
        GetNotificationId(), GetUserid(), GetMessage(), _notificationType ?? Enums.NotificationType.SYSTEM, GetIsRead(), _datesent
    );

    public void SetNotificationInfo(NotificationInfo info)
    {
        SetUserId(info.UserId);
        SetMessage(info.Message);
        UpdateNotificationType(info.Type);
        SetIsRead(info.IsRead);
        SetDateSent(info.DateSent);
    }

    public Notification() { }
}