namespace OnTheBeach
{
    using System.Collections.Generic;

    public class Checkout : ICheckout
    {
        private Dictionary<StockKeepingUnit, int> basket;

        private IStockControl stockControl;

        public Checkout(IStockControl stockingControl)
        {
            stockControl = stockingControl;
            basket = new Dictionary<StockKeepingUnit, int>();
        }

        public int GetTotal()
        {
            int total = 0;
            foreach (var item in basket)
            {
                total += GetCostForItems(item.Key, item.Value);
            }

            return total;
        }

        public void Scan(string sku)
        {
            StockKeepingUnit stockControlUnit = stockControl.GetStockControlUnit(sku);

            if (basket.ContainsKey(stockControlUnit))
            {
                basket[stockControlUnit]++;
            }
            else
            {
                basket.Add(stockControlUnit, 1);
            }
        }

        private int GetCostForItems(StockKeepingUnit stockControlUnit, int numberOfUnits)
        {
            if (stockControlUnit.HasMultibuyOffer && numberOfUnits > 1)
            {
                int cost = numberOfUnits / stockControlUnit.MultibuyUnitsRequired * stockControlUnit.MultibuyPrice;
                cost += numberOfUnits % stockControlUnit.MultibuyUnitsRequired * stockControlUnit.UnitPrice;

                return cost;
            }

            return stockControlUnit.UnitPrice * numberOfUnits;
        }
    }
}