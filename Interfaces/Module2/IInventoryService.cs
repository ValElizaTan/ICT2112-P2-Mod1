using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Module2;

// Interface contract for Module 2 Team 3's inventory service
public interface IInventoryService
{
    List<Inventoryitem> GetInventoryItemsByStatus(InventoryStatus status);
}
