using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

/// <summary>
/// Service to calculate rebates based on the rebate type and product type.
/// </summary>
public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;

    /// <summary>
    /// Constructor for RebateService.
    /// </summary>
    /// <param name="rebateDataStore">Rebate data store instance.</param>
    /// <param name="productDataStore">Product data store instance.</param>
    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {

        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = _productDataStore.GetProduct(request.ProductIdentifier);

        var result = new CalculateRebateResult();

        // Organize previous if statements
        if (rebate == null || product == null || !IsRebateSupportedByProduct(rebate, product) || !IsRebateDataValid(rebate) || !IsProductDataValid(product) || !IsRequestDataValid(request))
        {
            result.Success = false;
            return result;
        }

        // The result is guaranteed to be successful at this point
        result.Success = true;

        var rebateAmount = rebate.Incentive switch
        {
            IncentiveType.FixedCashAmount => rebate.Amount,
            IncentiveType.FixedRateRebate => product.Price * rebate.Percentage * request.Volume,
            IncentiveType.AmountPerUom => rebate.Amount * request.Volume,
            _ => 0m
        };

        _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

        return result;
    }

    // Method to check if the rebate is supported by the product.
    private static bool IsRebateSupportedByProduct(Rebate rebate, Product product)
    {
        return rebate.Incentive switch
        {
            IncentiveType.FixedCashAmount => product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount),
            IncentiveType.FixedRateRebate => product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate),
            IncentiveType.AmountPerUom => product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom),
            _ => false,
        };
    }

    private static bool IsRebateDataValid(Rebate rebate)
    {
        return rebate switch
        {
            { Incentive: IncentiveType.FixedCashAmount, Amount: > 0 } => true,
            { Incentive: IncentiveType.FixedRateRebate, Percentage: > 0 } => true,
            { Incentive: IncentiveType.AmountPerUom, Amount: > 0 } => true,
            _ => false,
        };
    }

    private static bool IsProductDataValid(Product product)
    {
        return product switch
        {
            { Price: > 0 } => true,
            _ => false,
        };
    }

    private static bool IsRequestDataValid(CalculateRebateRequest request)
    {
        return request switch
        {
            { Volume: > 0 } => true,
            _ => false,
        };
    }
}
