using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Data;

public interface ICheckoutMapper
{
    Checkout? FindById(int checkoutId);
    Checkout? FindActiveByCustomerId(int customerId);
    void Insert(Checkout checkout);
    void Update(Checkout checkout);
    void Delete(int checkoutId);
}