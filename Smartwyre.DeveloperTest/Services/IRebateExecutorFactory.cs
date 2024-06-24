using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public interface IRebateExecutorFactory
    {
        public IRebateExecutor Resolve(IncentiveType incentive);

        public bool HasExecutor(IncentiveType incentive);

        public bool IsRebateSupportedByProduct(Rebate rebate, Product product);

        public void RegisterExecutor(IncentiveType incentive, IRebateExecutor executor);
    }
}
