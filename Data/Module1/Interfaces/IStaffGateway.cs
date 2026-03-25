using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Interfaces;

public interface IStaffGateway
{
    Staff? FindById(int staffId);
    Staff? FindByEmail(string email);
    void InsertStaff(Staff staff);
    void UpdateStaff(Staff staff);
    void DeleteStaff(int staffId);
    List<Staff> FindAll();
}
