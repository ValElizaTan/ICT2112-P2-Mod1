using System.Collections.Generic;

namespace ProRental.Controllers.Module1;

public class CatalogueControl
{
    private readonly List<string> _products = new()
    {
        "Camera",
        "Tripod",
        "Lighting Kit",
        "Microphone"
    };

    public List<string> GetAllProducts()
    {
        return _products;
    }

    public string AddProductSelection(int productId, int quantity)
    {
        if (productId <= 0 || productId > _products.Count)
            return "Product not found";

        if (quantity <= 0)
            return "Invalid quantity";

        var product = _products[productId - 1];

        return $"Added {product} (Qty: {quantity})";
    }
}