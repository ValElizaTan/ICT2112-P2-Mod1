namespace ProRental.Domain.Entities;

public record CustomerInfo(
    int CustomerId,
    string Address,
    int CustomerType,
    UserInfo User
);

public partial class Customer
{
    public Customer(int customerId, string address, int customerType, User user)
    {
        _customerid = customerId;
        Address = address;
        Customertype = customerType;
        _userid = user.GetUserInfo().UserId;
        User = user;
    }

    protected Customer() { }

    private int GetCustomerId() => _customerid;
    private string GetAddress() => _address;
    private int GetCustomerType() => _customertype;

    private void SetAddress(string address) => Address = address;
    private void SetCustomerType(int customerType) => Customertype = customerType;

    public CustomerInfo GetCustomerInfo() => new(
        GetCustomerId(),
        GetAddress(),
        GetCustomerType(),
        User != null ? User.GetUserInfo() : null!
    );

    public void SetCustomerInfo(CustomerInfo info)
    {
        SetAddress(info.Address);
        SetCustomerType(info.CustomerType);
        User.SetUserInfo(info.User);
    }
}
