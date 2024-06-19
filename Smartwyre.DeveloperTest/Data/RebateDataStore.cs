using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    private readonly Dictionary<string, Rebate> _rebates = [];
    private readonly List<(Rebate, decimal)> _storedCalculations = [];

    public RebateDataStore()
    {
        _rebates.Add("R1", new Rebate { Identifier = "R1", Percentage = 0.1m, Incentive = IncentiveType.FixedRateRebate });
        _rebates.Add("R2", new Rebate { Identifier = "R2", Amount = 5m, Incentive = IncentiveType.AmountPerUom });
        _rebates.Add("R3", new Rebate { Identifier = "R3", Amount = 10m, Incentive = IncentiveType.FixedCashAmount });
    }

    public IEnumerable<Rebate> GetAll()
    {
        return _rebates.Values;
    }

    public Rebate GetRebate(string rebateIdentifier)
    {
        _rebates.TryGetValue(rebateIdentifier, out var rebate);
        return rebate;
    }

    public decimal GetStoredCalculationResult(Rebate account)
    {
        return _storedCalculations.Find(x => x.Item1.Identifier == account.Identifier).Item2;
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // If the account is already stored, remove it and add it again to update the value

        if (_rebates.ContainsKey(account.Identifier))
            _rebates.Remove(account.Identifier);

        if (_storedCalculations.Contains((account, rebateAmount)))
            _storedCalculations.Remove((account, rebateAmount));


        _rebates.Add(account.Identifier, account);
        _storedCalculations.Add((account, rebateAmount));
    }
}
