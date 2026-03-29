using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProRental.Data.Module1.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Gateways;

public class NotificationPreferenceGateway : INotificationPreferenceGateway
{
    private readonly AppDbContext _context;

    public NotificationPreferenceGateway(AppDbContext context)
    {
        _context = context;
    }

    public Notificationpreference? FindById(int preferenceId)
    {
        return _context.Notificationpreferences.Find(preferenceId);
    }

    public Notificationpreference? FindByUser(int userId)
    {
        return _context.Notificationpreferences
            .FirstOrDefault(p => EF.Property<int>(p, "Userid") == userId);
    }

    public List<Notificationpreference> FindAll()
    {
        return _context.Notificationpreferences.ToList();
    }

    public void InsertPreference(Notificationpreference preference)
    {
        _context.Notificationpreferences.Add(preference);
        _context.SaveChanges();
    }

    public void UpdatePreference(Notificationpreference preference)
    {
        _context.Notificationpreferences.Update(preference);
        _context.SaveChanges();
    }

    public void DeletePreference(int preferenceId)
    {
        var pref = _context.Notificationpreferences.Find(preferenceId);
        if (pref != null)
        {
            _context.Notificationpreferences.Remove(pref);
            _context.SaveChanges();
        }
    }
}
