using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

public class StaffControl : IStaffService
{ 
    private readonly IStaffGateway _staffGateway;

    public StaffControl(IStaffGateway staffGateway)
    {
        _staffGateway = staffGateway;
    }
    
    // Create a new Staff with the details
    public bool CreateStaff(string name, string email, int phoneCountry,
        int phoneNumber, string passwordHash)
    {
        // Check if a staff with this email already exists
        var existing = _staffGateway.FindByEmail(email);
        if (existing != null)
            return false;

        // Let the DB auto-generate IDs (GENERATED ALWAYS columns)
        var user = new User(0, UserRole.STAFF, name, email, passwordHash, phoneCountry, phoneNumber.ToString());
        var staff = new Staff(0, "Default");

        _staffGateway.InsertStaffWithUser(user, staff);
        return true;
    }
    //Delete a staff by ID 
    public void DeleteStaff(int staffId)
    {
        _staffGateway.DeleteStaffAndUser(staffId);
    }
    //Update staff details by ID
    public void UpdateStaff(int staffId, string name, string email, int phoneCountry,
    string phoneNumber, string? passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.");
        if (!email.Contains("@")) throw new ArgumentException("Email is not valid.");
        if (phoneCountry <= 0) throw new ArgumentException("Phone country code is not valid.");
        if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentException("Phone number cannot be empty.");

        var staff = _staffGateway.FindById(staffId);
        if (staff == null)
            throw new InvalidOperationException($"Staff with ID {staffId} not found.");

        var updatedUserInfo = new UserInfo(
            staff.GetStaffInfo().User.UserId,
            staff.GetStaffInfo().User.UserRole,
            name,
            email,
            phoneCountry,
            phoneNumber
        );

        var updatedStaffInfo = new StaffInfo(
            staffId,
            staff.GetStaffInfo().Department,
            updatedUserInfo
        );

        staff.SetStaffInfo(updatedStaffInfo);
        _staffGateway.UpdateStaff(staff);
    }
    //Get staff info by ID
    public Staff GetStaffInformation(int staffId)
    {
        var staff = _staffGateway.FindById(staffId);
        if (staff == null)
            throw new InvalidOperationException($"Staff with ID {staffId} not found.");
        return staff;
    }
    // Get staff info by email
    public Staff? GetStaffByEmail(string email)
    {
        return _staffGateway.FindByEmail(email);
    }
    //Get all staff records
    public List<Staff> GetStaff()
    {
        return _staffGateway.FindAll();
    }
}
