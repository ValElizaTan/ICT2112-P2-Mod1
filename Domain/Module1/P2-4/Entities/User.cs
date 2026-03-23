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

    private User() { }

    public User(int userId, UserRole userRole, string name, string email, string passwordHash, int phoneCountry, string phoneNumber)
    {
        _userid = userId;
        _userRole = userRole;
        _name = name;
        _email = email;
        _passwordhash = passwordHash;
        _phonecountry = phoneCountry;
        _phonenumber = phoneNumber;
    }

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
        SetPhoneNumber(info.PhoneNumber ?? string.Empty);
    }
    
    private int GetUserId() => _userid;
    private string GetName() => _name;
    private string GetEmail() => _email;
    private int? GetPhoneCountry() => _phonecountry;
    private string? GetPhoneNumber() => _phonenumber;

    private void SetName(string name) => _name = name;
    private void SetEmail(string email) => _email = email;
    private void SetPasswordHash(string passwordHash) => _passwordhash = passwordHash;
    private void SetPhoneCountry(int phoneCountry) => _phonecountry = phoneCountry;
    private void SetPhoneNumber(string phoneNumber) => _phonenumber = phoneNumber;
}