using System;
using System.Linq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        var productDataStore = new ProductDataStore();
        var rebateDataStore = new RebateDataStore();
        var rebateService = new RebateService(rebateDataStore, productDataStore);

        // Do a while loop for taking user input and calculating the rebate.
        // Cancel the loop when the user enters "exit".

        while (true)
        {
            // Show the user the list of products and rebates.
            Console.WriteLine("Products:");
            foreach (var product in productDataStore.GetAll())
            {
                var supportedIncentives = string.Join(", ", Enum.GetValues<SupportedIncentiveType>().Where(x => product.SupportedIncentives.HasFlag(x)));

                Console.WriteLine($"Identifier: {product.Identifier}, Price: {product.Price}, Supported incentives: {supportedIncentives}");
            }

            Console.WriteLine("Rebates:");

            foreach (var rebate in rebateDataStore.GetAll())
            {
                Console.WriteLine($"Identifier: {rebate.Identifier}, Percentage: {rebate.Percentage}, Amount: {rebate.Amount}, Incentive: {rebate.Incentive}");
            }

            Console.WriteLine("Enter the product identifier:");
            var productIdentifier = Console.ReadLine();

            Console.WriteLine("Enter the rebate identifier:");
            var rebateIdentifier = Console.ReadLine();


            Console.WriteLine("Enter the volume:");
            var volume = Convert.ToInt32(Console.ReadLine());

            var request = new CalculateRebateRequest
            {
                RebateIdentifier = rebateIdentifier,
                ProductIdentifier = productIdentifier,
                Volume = volume
            };

            var result = rebateService.Calculate(request);

            if (result.Success)
            {
                Console.WriteLine($"Rebate amount: {rebateDataStore.GetStoredCalculationResult(rebateDataStore.GetRebate(rebateIdentifier))}");
            }
            else
            {
                Console.WriteLine("Rebate calculation failed.");
            }

            Console.WriteLine("Enter 'exit' to exit the program or any other key to continue.");
            var exit = Console.ReadLine();

            if (exit == "exit")
            {
                break;
            }

            Console.WriteLine();
        }
    }
}
