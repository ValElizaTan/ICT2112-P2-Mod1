using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProRental.Data.Module1.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Data.Module1.Gateways;

public class NotificationGateway : INotificationGateway
{
    private readonly AppDbContext _context;

    public NotificationGateway(AppDbContext context)
    {
        _context = context;
    }

    public Notification? FindById(int notificationId)
    {
        return _context.Notifications.Find(notificationId);
    }

    public List<Notification> FindByUser(int userId)
    {
        return _context.Notifications
            .Where(n => EF.Property<int>(n, "Userid") == userId)
            .ToList();
    }

    public List<Notification> FindByType(NotificationType type)
    {
        return _context.Notifications
            .Where(n => n.GetNotificationInfo().Type == type)
            .ToList();
    }

    public List<Notification> FindUnreadByUser(int userId)
    {
        return _context.Notifications
            .Where(n => EF.Property<int>(n, "Userid") == userId && !EF.Property<bool>(n, "Isread"))
            .ToList();
    }

    public void InsertNotification(Notification notification)
    {
        _context.Notifications.Add(notification);
        _context.SaveChanges();
    }

    public void UpdateNotification(Notification notification)
    {
        _context.Notifications.Update(notification);
        _context.SaveChanges();
    }

    public void DeleteNotification(int notificationId)
    {
        var notification = _context.Notifications.Find(notificationId);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            _context.SaveChanges();
        }
    }
}
