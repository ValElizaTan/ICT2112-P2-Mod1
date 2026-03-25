using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public record UserInfo(
    int UserId,
    UserRole UserRole,
    string Name,
    string Email,
    int? PhoneCountry,
    string? PhoneNumber
);

public partial class User
{
    private UserRole _userRole;
    private UserRole UserRole { get => _userRole; set => _userRole = value; }
    protected User() { }
    public User(int userId, UserRole userRole, string name, string email, string passwordHash, int phoneCountry, string phoneNumber)
    {
        _userid = userId;
        _userRole = userRole;
        Name = name;
        Email = email;
        Passwordhash = passwordHash;
        Phonecountry = phoneCountry;
        Phonenumber = phoneNumber;
    }

    private int GetUserId() => _userid;
    private string GetName() => Name;
    private string GetEmail() => Email;
    private int? GetPhoneCountry() => Phonecountry;
    private string? GetPhoneNumber() => Phonenumber;

    private void SetName(string name) => Name = name;
    private void SetEmail(string email) => Email = email;
    private void SetPasswordHash(string passwordHash) => Passwordhash = passwordHash;
    private void SetPhoneCountry(int phoneCountry) => Phonecountry = phoneCountry;
    private void SetPhoneNumber(string phoneNumber) => Phonenumber = phoneNumber;

    public UserInfo GetUserInfo() => new(
        GetUserId(),
        _userRole,
        GetName(),
        GetEmail(),
        GetPhoneCountry(),
        GetPhoneNumber()
    );

    public void SetUserInfo(UserInfo info)
    {
        _userRole = info.UserRole;
        SetName(info.Name);
        SetEmail(info.Email);
        SetPhoneCountry(info.PhoneCountry ?? 0);
        SetPhoneNumber(info.PhoneNumber ?? "");
    }
}
