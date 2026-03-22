namespace ProRental.Domain.Entities;

public record StaffInfo(
    int StaffId,
    string Department,
    UserInfo User
);

public partial class Staff
{
    public Staff(int staffId, string department)
    {
        _staffid = staffId;
        Department = department;
    }

    //protected Staff() { }
    public Staff() { }

    private int GetStaffId() => _staffid;
    private string GetDepartment() => _department;

    private void SetDepartment(string department) => Department = department;

    public StaffInfo GetStaffInfo() => new(
        GetStaffId(),
        GetDepartment(),
        User.GetUserInfo()
    );

    public void SetStaffInfo(StaffInfo info)
    {
        SetDepartment(info.Department);
        User.SetUserInfo(info.User);
    }

    // MINIMAL PLACEHOLDER ENTITY
    public int StaffId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public int GetStaffIdd() => StaffId;
    public string GetName() => Name;
    public string GetRole() => Role;
}
