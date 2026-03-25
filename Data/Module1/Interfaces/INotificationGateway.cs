using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Data.Module1.Interfaces;

public interface INotificationGateway
{
    Notification? FindById(int notificationId);
    List<Notification> FindByUser(int userId);
    List<Notification> FindByType(NotificationType type);
    List<Notification> FindUnreadByUser(int userId);
    void InsertNotification(Notification notification);
    void UpdateNotification(Notification notification);
    void DeleteNotification(int notificationId);
}
