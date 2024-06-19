using Xunit;
using Moq;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests
{
    public class RebateServiceTests
    {

        [Fact]
        public void Calculate_ShouldNotSucceed_ForNonExistentRebate()
        {
            // Arrange
            var mockRebateDataStore = new Mock<IRebateDataStore>();
            var mockProductDataStore = new Mock<IProductDataStore>();

            mockRebateDataStore.Setup(m => m.GetRebate(It.IsAny<string>()))
                .Returns((Rebate)null);

            var service = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

            // Act
            var result = service.Calculate(new CalculateRebateRequest
            {
                RebateIdentifier = "R1",
                ProductIdentifier = "P1",
                Volume = 1
            });

            // Assert
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData(IncentiveType.FixedCashAmount)]
        [InlineData(IncentiveType.FixedRateRebate)]
        [InlineData(IncentiveType.AmountPerUom)]
        public void Calculate_ShouldNotSucceed_ForNonExistingProduct(IncentiveType incentiveType)
        {
            // Arrange
            var mockRebateDataStore = new Mock<IRebateDataStore>();
            var mockProductDataStore = new Mock<IProductDataStore>();

            mockRebateDataStore.Setup(m => m.GetRebate(It.IsAny<string>()))
                .Returns(new Rebate
                {
                    Identifier = "R1",
                    Incentive = incentiveType
                });

            mockProductDataStore.Setup(m => m.GetProduct(It.IsAny<string>()))
                .Returns((Product)null);

            var service = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

            // Act
            var result = service.Calculate(new CalculateRebateRequest
            {
                RebateIdentifier = "R1",
                ProductIdentifier = "P1",
                Volume = 1
            });

            // Assert
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.FixedRateRebate)]
        [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.AmountPerUom)]
        [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.FixedCashAmount)]
        [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.AmountPerUom)]
        [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.FixedCashAmount)]
        [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.FixedRateRebate)]

        public void Calculate_ShouldNotSucceed_ForUnsupportedIncentive(IncentiveType supportedIncentiveType, SupportedIncentiveType productSupportedIncentiveType)
        {
            // Arrange
            var mockRebateDataStore = new Mock<IRebateDataStore>();
            var mockProductDataStore = new Mock<IProductDataStore>();

            mockRebateDataStore.Setup(m => m.GetRebate(It.IsAny<string>()))
                .Returns(new Rebate
                {
                    Identifier = "R1",
                    Incentive = supportedIncentiveType
                });

            mockProductDataStore.Setup(m => m.GetProduct(It.IsAny<string>()))
                .Returns(new Product
                {
                    Identifier = "P1",
                    SupportedIncentives = productSupportedIncentiveType
                });

            var service = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

            // Act
            var result = service.Calculate(new CalculateRebateRequest
            {
                RebateIdentifier = "R1",
                ProductIdentifier = "P1",
                Volume = 1
            });

            // Assert
            Assert.False(result.Success);
        }
    }
}

