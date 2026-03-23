using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Data;

public interface ICartMapper
{
    Cart? FindActiveByCustomerId(int customerId);
    Cart? FindActiveBySessionId(string sessionId);
    Cart? FindById(int cartId);
    void Insert(Cart cart);
    void Update(Cart cart);
    void Delete(int cartId);
}