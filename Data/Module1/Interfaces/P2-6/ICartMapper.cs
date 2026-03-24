using ProRental.Domain.Entities;

<<<<<<< HEAD
namespace ProRental.Interfaces.Data;

public interface ICartMapper
{
    Cart? FindActiveByCustomerId(int customerId);
    Cart? FindActiveBySessionId(int sessionId);
    Cart? FindById(int cartId);
    void Insert(Cart cart);
    void Update(Cart cart);
    void Delete(int cartId);
=======
namespace ProRental.Interfaces.Domain;

public interface ICartMapper
{
    Cart? GetCartBySession(string sessionId);
    Cart? GetCartByCustomer(string customerId);
    Cart  GetCart(string cartId);
    void  Save(Cart cart);
    void  Delete(string cartId);


>>>>>>> origin/Catalauge,-Cart,-Checkout,-Payment-(Backup)
}