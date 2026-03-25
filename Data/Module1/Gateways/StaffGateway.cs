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

    public void InsertStaff(Staff staff)
    {
        _context.Staff.Add(staff);
        _context.SaveChanges();
    }

    public void UpdateStaff(Staff staff)
    {
        _context.Staff.Update(staff);
        _context.SaveChanges();
    }

    public void DeleteStaff(int staffId)
    {
        var staff = _context.Staff.Find(staffId);
        if (staff != null)
        {
            _context.Staff.Remove(staff);
            _context.SaveChanges();
        }
    }

    public List<Staff> FindAll()
    {
        return _context.Staff.Include(s => s.User).ToList();
    }
}
