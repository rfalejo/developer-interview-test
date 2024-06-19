using Xunit;
using Moq;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests
{
    public class RebateServiceTests
    {

        #region Calculate negative tests

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

        [Theory]
        [InlineData(IncentiveType.FixedCashAmount, -1.0, -1.0)]
        [InlineData(IncentiveType.FixedRateRebate, -1.0, -1.0)]
        [InlineData(IncentiveType.AmountPerUom, -1.0, -1.0)]
        public void Calculate_ShouldNotSucceed_ForInvalidRebateData(IncentiveType incentiveType, decimal amount, decimal percentage)
        {
            // Arrange
            var mockRebateDataStore = new Mock<IRebateDataStore>();
            var mockProductDataStore = new Mock<IProductDataStore>();

            mockRebateDataStore.Setup(m => m.GetRebate(It.IsAny<string>()))
                .Returns(new Rebate
                {
                    Identifier = "R1",
                    Incentive = incentiveType,
                    Amount = amount,
                    Percentage = percentage
                });

            mockProductDataStore.Setup(m => m.GetProduct(It.IsAny<string>()))
                .Returns(new Product
                {
                    Identifier = "P1",
                    SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
                    Price = 1.0m,
                    Uom = "U1"
                });

            var service = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

            // Act
            var result = service.Calculate(new CalculateRebateRequest
            {
                RebateIdentifier = "R1",
                ProductIdentifier = "P1",
                Volume = 1,
            });

            // Assert
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData(IncentiveType.FixedCashAmount)]
        [InlineData(IncentiveType.FixedRateRebate)]
        [InlineData(IncentiveType.AmountPerUom)]
        public void Calculate_ShouldNotSucceed_ForInvalidProductData(IncentiveType incentiveType)
        {
            // Arrange
            var mockRebateDataStore = new Mock<IRebateDataStore>();
            var mockProductDataStore = new Mock<IProductDataStore>();

            mockRebateDataStore.Setup(m => m.GetRebate(It.IsAny<string>()))
                .Returns(new Rebate
                {
                    Identifier = "R1",
                    Incentive = incentiveType,
                    Amount = 1.0m
                });

            mockProductDataStore.Setup(m => m.GetProduct(It.IsAny<string>()))
                .Returns(new Product
                {
                    Identifier = "P1",
                    SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
                    Price = -1.0m,
                    Uom = "U1"
                });

            var service = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

            // Act
            var result = service.Calculate(new CalculateRebateRequest
            {
                RebateIdentifier = "R1",
                ProductIdentifier = "P1",
                Volume = 1,
            });

            // Assert
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData(IncentiveType.FixedCashAmount)]
        [InlineData(IncentiveType.FixedRateRebate)]
        [InlineData(IncentiveType.AmountPerUom)]
        public void Calculate_ShouldNotSucceed_ForInvalidRequestData(IncentiveType incentiveType)
        {
            // Arrange
            var mockRebateDataStore = new Mock<IRebateDataStore>();
            var mockProductDataStore = new Mock<IProductDataStore>();

            mockRebateDataStore.Setup(m => m.GetRebate(It.IsAny<string>()))
                .Returns(new Rebate
                {
                    Identifier = "R1",
                    Incentive = incentiveType,
                    Amount = 1.0m
                });

            mockProductDataStore.Setup(m => m.GetProduct(It.IsAny<string>()))
                .Returns(new Product
                {
                    Identifier = "P1",
                    SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
                    Price = 1.0m,
                    Uom = "U1"
                });

            var service = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

            // Act
            var result = service.Calculate(new CalculateRebateRequest
            {
                RebateIdentifier = "R1",
                ProductIdentifier = "P1",
                Volume = -1,
            });

            // Assert
            Assert.False(result.Success);
        }

        #endregion

        #region Persistence tests

        [Fact]
        public void Rebate_ShouldNotBeStored_ForInvalidRequestData()
        {
            // Arrange
            var mockRebateDataStore = new Mock<IRebateDataStore>();
            var mockProductDataStore = new Mock<IProductDataStore>();

            mockRebateDataStore.Setup(m => m.GetRebate(It.IsAny<string>()))
                .Returns(new Rebate
                {
                    Identifier = "R1",
                    Incentive = IncentiveType.FixedCashAmount,
                    Amount = 1.0m
                });

            mockProductDataStore.Setup(m => m.GetProduct(It.IsAny<string>()))
                .Returns(new Product
                {
                    Identifier = "P1",
                    SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
                    Price = 1.0m,
                    Uom = "U1"
                });

            var service = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

            // Act
            var result = service.Calculate(new CalculateRebateRequest
            {
                RebateIdentifier = "R1",
                ProductIdentifier = "P1",
                Volume = -1,
            });

            // Assert
            mockRebateDataStore.Verify(m => m.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void Rebate_ShouldBeStored_ForValidRequestData()
        {
            // Arrange
            var mockRebateDataStore = new Mock<IRebateDataStore>();
            var mockProductDataStore = new Mock<IProductDataStore>();

            mockRebateDataStore.Setup(m => m.GetRebate(It.IsAny<string>()))
                .Returns(new Rebate
                {
                    Identifier = "R1",
                    Incentive = IncentiveType.FixedCashAmount,
                    Amount = 1.0m
                });

            mockProductDataStore.Setup(m => m.GetProduct(It.IsAny<string>()))
                .Returns(new Product
                {
                    Identifier = "P1",
                    SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
                    Price = 1.0m,
                    Uom = "U1"
                });

            var service = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

            // Act
            var result = service.Calculate(new CalculateRebateRequest
            {
                RebateIdentifier = "R1",
                ProductIdentifier = "P1",
                Volume = 1,
            });

            // Assert
            mockRebateDataStore.Verify(m => m.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Once);
        }

        #endregion
    }
}

