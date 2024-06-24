using Smartwyre.DeveloperTest.Types;
using System.Collections;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Services
{
    public static class RebateExecutorFactory
    {
        private static IDictionary<IncentiveType, IRebateExecutor> resolveDictionary = new Dictionary<IncentiveType, IRebateExecutor>();

        public static void RegisterExecutor(IncentiveType incentive, IRebateExecutor executor)
        {
            resolveDictionary.Add(incentive, executor);
        }

        public static IRebateExecutor Resolve(IncentiveType incentive)
        {
            return resolveDictionary[incentive];
        }

        public static bool HasExecutor(IncentiveType incentive)
        {
            return resolveDictionary.ContainsKey(incentive);
        }

        public static bool IsRebateSupportedByProduct(Rebate rebate, Product product)
        {
            return Resolve(rebate.Incentive).IsRebateSupportedByProduct(rebate, product);
        }
    }
}
