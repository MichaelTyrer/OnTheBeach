namespace UnitTests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using OnTheBeach;

    [TestClass]
    public class CheckoutTests
    {
        private int priceA = 50;
        private int priceB = 30;
        private int priceC = 20;
        private int priceD = 15;

        private Mock<IStockControl> mockStockControl;
        private MockRepository repository;

        [TestInitialize]
        public void TestStartup()
        {
            repository = new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

            mockStockControl = repository.Create<IStockControl>();
        }

        [TestMethod]
        public void CheckoutGetTotal_SingleItem_Test()
        {
            // arrange            
            string skuA = "A";
            int expected = priceA;
            StockKeepingUnit itemA = new StockKeepingUnit
            {
                Name = "A",
                UnitPrice = priceA,
                HasMultibuyOffer = false
            };

            mockStockControl.Setup(s => s.GetStockControlUnit(skuA)).Returns(itemA);

            ICheckout checkout = new Checkout(mockStockControl.Object);
            checkout.Scan(skuA);

            // act
            var actual = checkout.GetTotal();

            // assert
            Assert.AreEqual(expected, actual);

            repository.VerifyAll();
        }

        [TestMethod]
        public void CheckoutGetTotal_MultipleItems_Test()
        {
            // arrange
            int expected = priceA + priceB + priceC;

            StockKeepingUnit itemA = new StockKeepingUnit
            {
                Name = "A",
                UnitPrice = priceA,
                HasMultibuyOffer = false
            };

            StockKeepingUnit itemB = new StockKeepingUnit
            {
                Name = "B",
                UnitPrice = priceB,
                HasMultibuyOffer = false
            };

            StockKeepingUnit itemC = new StockKeepingUnit
            {
                Name = "C",
                UnitPrice = priceC,
                HasMultibuyOffer = false
            };

            mockStockControl.Setup(s => s.GetStockControlUnit("A")).Returns(itemA);
            mockStockControl.Setup(s => s.GetStockControlUnit("B")).Returns(itemB);
            mockStockControl.Setup(s => s.GetStockControlUnit("C")).Returns(itemC);

            ICheckout checkout = new Checkout(mockStockControl.Object);
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("C");

            // act
            var actual = checkout.GetTotal();

            // assert
            Assert.AreEqual(expected, actual);

            repository.VerifyAll();
        }

        [TestMethod]
        public void CheckoutGetTotal_MultipleRepeatedItemsWithoutDiscounts_Test()
        {
            int expected = priceA * 2 + priceB + priceC * 2;

            StockKeepingUnit itemA = new StockKeepingUnit
            {
                Name = "A",
                UnitPrice = priceA,
                HasMultibuyOffer = false
            };

            StockKeepingUnit itemB = new StockKeepingUnit
            {
                Name = "B",
                UnitPrice = priceB,
                HasMultibuyOffer = false
            };

            StockKeepingUnit itemC = new StockKeepingUnit
            {
                Name = "C",
                UnitPrice = priceC,
                HasMultibuyOffer = false
            };

            mockStockControl.Setup(s => s.GetStockControlUnit("A")).Returns(itemA);
            mockStockControl.Setup(s => s.GetStockControlUnit("B")).Returns(itemB);
            mockStockControl.Setup(s => s.GetStockControlUnit("C")).Returns(itemC);

            ICheckout checkout = new Checkout(mockStockControl.Object);
            checkout.Scan("A");
            checkout.Scan("C");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("C");

            // act
            var actual = checkout.GetTotal();

            // assert
            Assert.AreEqual(expected, actual);

            repository.VerifyAll();
        }

        [TestMethod]
        public void CheckoutGetTotal_MultipleItemsWithDiscounts1_Test()
        {
            int expected = priceA * 2 + priceB + priceC * 2;

            StockKeepingUnit itemA = new StockKeepingUnit
            {
                Name = "A",
                UnitPrice = priceA,
                HasMultibuyOffer = true,
                MultibuyPrice = 130,
                MultibuyUnitsRequired = 3
            };

            StockKeepingUnit itemB = new StockKeepingUnit
            {
                Name = "B",
                UnitPrice = priceB,
                HasMultibuyOffer = true,
                MultibuyPrice = 45,
                MultibuyUnitsRequired = 2
            };

            StockKeepingUnit itemC = new StockKeepingUnit
            {
                Name = "C",
                UnitPrice = priceC,
                HasMultibuyOffer = false
            };

            mockStockControl.Setup(s => s.GetStockControlUnit("A")).Returns(itemA);
            mockStockControl.Setup(s => s.GetStockControlUnit("B")).Returns(itemB);
            mockStockControl.Setup(s => s.GetStockControlUnit("C")).Returns(itemC);

            ICheckout checkout = new Checkout(mockStockControl.Object);
            checkout.Scan("A");
            checkout.Scan("C");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("C");

            // act
            var actual = checkout.GetTotal();

            // assert
            Assert.AreEqual(expected, actual);

            repository.VerifyAll();
        }

        [TestMethod]
        public void CheckoutGetTotal_MultipleItemsWithDiscounts2_Test()
        {
            int expected = 130 + priceB + priceC * 2;

            StockKeepingUnit itemA = new StockKeepingUnit
            {
                Name = "A",
                UnitPrice = priceA,
                HasMultibuyOffer = true,
                MultibuyPrice = 130,
                MultibuyUnitsRequired = 3
            };

            StockKeepingUnit itemB = new StockKeepingUnit
            {
                Name = "B",
                UnitPrice = priceB,
                HasMultibuyOffer = true,
                MultibuyPrice = 45,
                MultibuyUnitsRequired = 2
            };

            StockKeepingUnit itemC = new StockKeepingUnit
            {
                Name = "C",
                UnitPrice = priceC,
                HasMultibuyOffer = false
            };

            mockStockControl.Setup(s => s.GetStockControlUnit("A")).Returns(itemA);
            mockStockControl.Setup(s => s.GetStockControlUnit("B")).Returns(itemB);
            mockStockControl.Setup(s => s.GetStockControlUnit("C")).Returns(itemC);

            ICheckout checkout = new Checkout(mockStockControl.Object);
            checkout.Scan("A");
            checkout.Scan("C");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("C");
            checkout.Scan("A");

            // act
            var actual = checkout.GetTotal();

            // assert
            Assert.AreEqual(expected, actual);

            repository.VerifyAll();
        }

        [TestMethod]
        public void CheckoutGetTotal_MultipleItemsWithDiscounts3_Test()
        {
            int expected = 130 + priceA + 45 + priceC * 2;

            StockKeepingUnit itemA = new StockKeepingUnit
            {
                Name = "A",
                UnitPrice = priceA,
                HasMultibuyOffer = true,
                MultibuyPrice = 130,
                MultibuyUnitsRequired = 3
            };

            StockKeepingUnit itemB = new StockKeepingUnit
            {
                Name = "B",
                UnitPrice = priceB,
                HasMultibuyOffer = true,
                MultibuyPrice = 45,
                MultibuyUnitsRequired = 2
            };

            StockKeepingUnit itemC = new StockKeepingUnit
            {
                Name = "C",
                UnitPrice = priceC,
                HasMultibuyOffer = false
            };

            mockStockControl.Setup(s => s.GetStockControlUnit("A")).Returns(itemA);
            mockStockControl.Setup(s => s.GetStockControlUnit("B")).Returns(itemB);
            mockStockControl.Setup(s => s.GetStockControlUnit("C")).Returns(itemC);

            ICheckout checkout = new Checkout(mockStockControl.Object);
            checkout.Scan("A");
            checkout.Scan("C");
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("C");
            checkout.Scan("A");
            checkout.Scan("A");

            // act
            var actual = checkout.GetTotal();

            // assert
            Assert.AreEqual(expected, actual);

            repository.VerifyAll();
        }
    }
}
