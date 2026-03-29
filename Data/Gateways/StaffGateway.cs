using ProRental.Data.Module1.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProRental.Data.Module1.Gateways;

public class StaffGateway : IStaffGateway
{
    private readonly AppDbContext _context;

    public StaffGateway(AppDbContext context)
    {
        _context = context;
    }

    public Staff? FindById(int staffId)
    {
        return _context.Staff
            .Include(s => s.User)
            .FirstOrDefault(s => EF.Property<int>(s, "Staffid") == staffId);
    }

    public Staff? FindByEmail(string email)
    {
        return _context.Staff
            .Include(s => s.User)
            .AsEnumerable()
            .FirstOrDefault(s => s.User?.GetUserInfo()?.Email == email);
    }

    public void InsertStaffWithUser(User user, Staff staff)
    {
        _context.Users.Add(user);
        _context.SaveChanges();

        // Link staff to the newly created user via reflection (private field)
        var useridField = typeof(Staff).GetField("_userid",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var newUserId = typeof(User).GetField("_userid",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(user);
        useridField?.SetValue(staff, newUserId);

        _context.Staff.Add(staff);
        _context.SaveChanges();
    }

    public void UpdateStaff(Staff staff)
    {
        _context.Staff.Update(staff);
        _context.SaveChanges();
    }

    public void DeleteStaffAndUser(int staffId)
    {
        var staff = _context.Staff
            .Include(s => s.User)
            .FirstOrDefault(s => EF.Property<int>(s, "Staffid") == staffId);
        if (staff == null) return;

        var user = staff.User;

        // Remove staff first (FK: staff → user)
        _context.Staff.Remove(staff);
        _context.SaveChanges();

        // Then remove the user
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }

    public List<Staff> FindAll()
    {
        return _context.Staff.Include(s => s.User).ToList();
    }
}
