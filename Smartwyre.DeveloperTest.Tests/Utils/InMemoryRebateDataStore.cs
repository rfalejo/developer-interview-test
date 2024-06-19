using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Tests
{
    public class InMemoryRebateDataStore : IRebateDataStore
    {
        private readonly Dictionary<string, Rebate> _rebates = new();
        private readonly List<(Rebate, decimal)> _storedCalculations = new();

        public Rebate GetRebate(string rebateIdentifier)
        {
            _rebates.TryGetValue(rebateIdentifier, out var rebate);
            return rebate;
        }

        public void StoreCalculationResult(Rebate rebate, decimal rebateAmount)
        {
            _storedCalculations.Add((rebate, rebateAmount));
        }

        // Utility method to add rebates for testing
        public void AddRebate(Rebate rebate)
        {
            _rebates[rebate.Identifier] = rebate;
        }

        // Utility method to check if a calculation result was stored
        public bool HasStoredCalculation(Rebate rebate, decimal rebateAmount)
        {
            return _storedCalculations.Contains((rebate, rebateAmount));
        }

        public IEnumerable<Rebate> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public decimal GetStoredCalculationResult(Rebate account)
        {
            throw new System.NotImplementedException();
        }
    }
}
