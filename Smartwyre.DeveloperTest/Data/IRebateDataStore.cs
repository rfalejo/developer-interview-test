using Smartwyre.DeveloperTest.Types;
using System.Collections;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Data;

public interface IRebateDataStore
{
    Rebate GetRebate(string rebateIdentifier);
    decimal GetStoredCalculationResult(Rebate account);
    void StoreCalculationResult(Rebate account, decimal rebateAmount);
    IEnumerable<Rebate> GetAll();
}
