using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public class FixedCashAmountRebateExecutor : IRebateExecutor
    {
        public decimal CalculateRebate(Rebate rebate, Product product, decimal volume)
        {
            return rebate.Amount;
        }

        public bool IsRebateSupportedByProduct(Rebate rebate, Product product)
        {
            return product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount);
        }
    }

    public class FixedRateRebateExecutor : IRebateExecutor
    {
        public decimal CalculateRebate(Rebate rebate, Product product, decimal volume)
        {
            return product.Price * rebate.Percentage;
        }

        public bool IsRebateSupportedByProduct(Rebate rebate, Product product)
        {
            return product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate);
        }
    }

    public class AmountPerUomRebateExecutor : IRebateExecutor
    {
        public decimal CalculateRebate(Rebate rebate, Product product, decimal volume)
        {
            return rebate.Amount * volume;
        }

        public bool IsRebateSupportedByProduct(Rebate rebate, Product product)
        {
            return product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom);
        }
    }
}
