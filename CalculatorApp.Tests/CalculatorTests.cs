using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace CalculatorApp.Tests
{
    public class CalculatorTests
    {
        private readonly Mock<IInventoryRepo> _inventoryRepoMock;
        private readonly Calculator _calculator;

        public CalculatorTests()
        {
            _inventoryRepoMock = new Mock<IInventoryRepo>();

            _calculator = new Calculator(_inventoryRepoMock.Object);
        }

        [Fact]
        public void GrossTotal_ShouldReturnCorrectTotal()
        {
            // Arrange
            double price = 100;
            int quantity = 2;

            // Act
            var result = _calculator.GrossTotal(price, quantity);

            // Assert
            Assert.Equal(200, result);
        }

        [Fact]
        public void NetTotal_ShouldIncludeVat()
        {
            // Arrange
            double price = 100;
            int quantity = 2;

            // Act
            var result = _calculator.NetTotal(price, quantity);

            // Assert
            Assert.Equal(240, result);
        }

        [Fact]
        public void IsStockRunningLow_ShouldReturnTrue_WhenStockLow()
        {
            // Arrange
            _inventoryRepoMock
                .Setup(x => x.GetStock(1))
                .Returns(5);

            // Act
            var result = _calculator.IsStockRunningLow(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsStockAvailable_ShouldReturnTrue_WhenEnoughStock()
        {
            // Arrange
            _inventoryRepoMock
                .Setup(x => x.GetStock(1))
                .Returns(100);

            // Act
            var result = _calculator.IsStockAvailable(1, 50);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FinalTotal_ShouldReturnCorrectAmount()
        {
            // Arrange
            _inventoryRepoMock
                .Setup(x => x.GetStock(1))
                .Returns(5);

            // Act
            var result = _calculator.FinalTotal(
                1,
                100,
                200,
                true);

            // Assert
            Assert.Equal(22680, result);
        }
    }
}
