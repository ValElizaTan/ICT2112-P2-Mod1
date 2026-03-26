using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Data;

public interface IOrderMapper
{
    void Insert(Order order);
    void Update(Order order);
    void InsertHistory(Orderstatushistory history);
    Order? FindById(int orderId);
    List<Order> FindAll();
    List<Order> FindByCustomer(int customerId);
    void Delete(int orderId);
    void InsertHistory(Orderstatushistory history);
}