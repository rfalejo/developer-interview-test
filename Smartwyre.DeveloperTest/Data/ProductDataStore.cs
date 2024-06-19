using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    private readonly Dictionary<string, Product> _products = [];

    public ProductDataStore()
    {
        _products.Add("P1", new Product { Identifier = "P1", Price = 10m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate });
        _products.Add("P2", new Product { Identifier = "P2", Price = 20m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate | SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.AmountPerUom});
    }

    public IEnumerable<Product> GetAll()
    {
        return _products.Values;
    }

    public Product GetProduct(string productIdentifier)
    {
        _products.TryGetValue(productIdentifier, out var product);
        return product;
    }
}
