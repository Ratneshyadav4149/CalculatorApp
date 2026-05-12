using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp
{
    public class Calculator
    {
        private readonly IInventoryRepo _inventoryRepo;

        public Calculator(IInventoryRepo inventoryRepo)
        {
            _inventoryRepo = inventoryRepo;
        }

        public double GrossTotal(double price, int quantity)
            => price * quantity;

        private const double VatRate = 1.2;

        public double NetTotal(double price, int quantity)
            => GrossTotal(price, quantity) * VatRate;

        public double BulkBuyDiscount(int quantity)
        {
            if (quantity < 100)
                return 1;

            if (quantity < 1000)
                return 0.90;

            return 0.80;
        }

        public bool IsStockRunningLow(int productId)
        {
            var currentStock = _inventoryRepo.GetStock(productId);

            return currentStock < 10;
        }

        public double StockRunningLowMultiplier(int productId)
        {
            if (IsStockRunningLow(productId))
            {
                return 1.05;
            }

            return 1;
        }

        public double FinalTotal(
            int productId,
            double price,
            int quantity,
            bool calculateWithVat)
        {
            var initialTotal = calculateWithVat
                ? NetTotal(price, quantity)
                : GrossTotal(price, quantity);

            return initialTotal
                   * StockRunningLowMultiplier(productId)
                   * BulkBuyDiscount(quantity);
        }

        public bool IsStockAvailable(int productId, int quantity)
        {
            var currentStock = _inventoryRepo.GetStock(productId);

            return currentStock >= quantity;
        }
    }
}
