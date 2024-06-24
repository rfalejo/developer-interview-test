using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public interface IRebateExecutor
{
    decimal CalculateRebate(Rebate rebate, Product product, decimal volume);
    bool IsRebateSupportedByProduct(Rebate rebate, Product product);
}