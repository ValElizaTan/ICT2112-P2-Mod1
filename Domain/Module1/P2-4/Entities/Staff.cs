namespace ProRental.Domain.Entities;

public record StaffInfo(
    int StaffId,
    string Department,
    UserInfo User
);

public partial class Staff
{
    public Staff(int staffId, string department, User user)
    {
        _staffid = staffId;
        _department = department;
        _userid = user.GetUserInfo().UserId;
        User = user;
    }

    public Staff(int staffId, string department)
    {
        _staffid = staffId;
        _department = department;
    }

    protected Staff() { }

    public StaffInfo GetStaffInfo() => new(
        GetStaffIdInternal(),
        GetDepartmentInternal(),
        User != null ? User.GetUserInfo() : new UserInfo(0, default, "", "", null, null)
    );

    public void SetStaffInfo(StaffInfo info)
    {
        SetDepartment(info.Department);
        User.SetUserInfo(info.User);
    }
    
    private int GetStaffIdInternal() => _staffid;
    private string GetDepartmentInternal() => _department;

    private void SetDepartment(string department) => _department = department;
}