using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface ICartMapper
{
    Cart? GetCartBySession(string sessionId);
    Cart? GetCartByCustomer(string customerId);
    Cart  GetCart(string cartId);
    void  Save(Cart cart);
    void  Delete(string cartId);


}