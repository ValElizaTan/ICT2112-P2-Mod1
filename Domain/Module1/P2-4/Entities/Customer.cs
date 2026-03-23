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
        _address = address;
        _customertype = customerType;
        _userid = user.GetUserInfo().UserId;
        User = user;
    }

    protected Customer() { }

    public CustomerInfo GetCustomerInfo() => new(
        GetCustomerIdInternal(),
        GetAddressInternal(),
        GetCustomerTypeInternal(),
        User.GetUserInfo()
    );

    public void SetCustomerInfo(CustomerInfo info)
    {
        SetAddress(info.Address);
        SetCustomerType(info.CustomerType);
        User.SetUserInfo(info.User);
    }

    private int GetCustomerIdInternal() => _customerid;
    private string GetAddressInternal() => _address;
    private int GetCustomerTypeInternal() => _customertype;

    private void SetAddress(string address) => _address = address;
    private void SetCustomerType(int customerType) => _customertype = customerType;
}