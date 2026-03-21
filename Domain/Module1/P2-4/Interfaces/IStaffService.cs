using ProRental.Domain.Entities;

namespace ProRental.Domain.Module1.P24.Interfaces;

public interface IStaffService
{
    bool CreateStaff(int staffId, string name, string email, int phoneCountry,
        int phoneNumber, string passwordHash);
    void DeleteStaff(int staffId);
    void UpdateStaff(int staffId, string name, string email, int phoneCountry,
        int phoneNumber, string passwordHash);
    Staff GetStaffInformation(int staffId);
    List<Staff> GetStaff();
}
