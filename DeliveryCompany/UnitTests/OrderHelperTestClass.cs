using Utility.Helpers;
using Utility.Models;

namespace UnitTests
{
    [TestClass]
    public class OrderHelpersTests
    {
        [TestMethod]
        public void GetOrderedDeliveriesNumberOfOrders()
        {
            Dictionary<int, int> deliveries = new Dictionary<int, int>
            {
                { 1, 5 },
                { 2, 3 },
                { 3, 7 }
            };

            var result = OrderHelpers.GetOrderedDeliveriesByNumberOfOrders(deliveries);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[0].Key);
            Assert.AreEqual(1, result[1].Key);
            Assert.AreEqual(3, result[2].Key);
        }

        [TestMethod]
        public void GetOrderedDeliveriesByNumberOfOrders_ReturnsEmptyListForEmptyDictionary()
        {
            Dictionary<int, int> emptyDictionary = new Dictionary<int, int>();

            var result = OrderHelpers.GetOrderedDeliveriesByNumberOfOrders(emptyDictionary);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CheckPackage_ReturnsTrue_WhenPackageIsValid()
        {
            PackageCheck package = new PackageCheck
            {
                MaxHeight = 10,
                MaxWeight = 20,
                MaxWidth = 30,
                MaxLength = 40,
                Height = 5,
                Weight = 10,
                Width = 20,
                Length = 30
            };

            bool isValid = OrderHelpers.CheckPackage(package);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void CheckPackage_ReturnsFalse_WhenPackageIsInvalid()
        {
            PackageCheck package = new PackageCheck
            {
                MaxHeight = 10,
                MaxWeight = 20,
                MaxWidth = 30,
                MaxLength = 40,
                Height = 15, 
                Weight = 10,
                Width = 20,
                Length = 30
            };

            bool isValid = OrderHelpers.CheckPackage(package);

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void CalculatePrice_ReturnsCorrectPrice_WhenAllParametersArePositive()
        {
            int weight = 10;
            int width = 5;
            int length = 8;
            int height = 3;

            double price = OrderHelpers.CalculatePrice(weight, width, length, height);

            Assert.AreEqual(5.0 + (weight * 0.1) + (width * length * height * 0.00001), price);
        }

        [TestMethod]
        public void CalculatePrice_ReturnsZero_WhenTotalPriceIsNegative()
        {

            int weight = -1000;
            int width = -5;
            int length = -8;
            int height = -3;

            double price = OrderHelpers.CalculatePrice(weight, width, length, height);

            Assert.AreEqual(5.0, price);
        }

        [TestMethod]
        public void CalculatePrice_ReturnsBasePrice_WhenAllParametersAreZero()
        {

            int weight = 0;
            int width = 0;
            int length = 0;
            int height = 0;

            double price = OrderHelpers.CalculatePrice(weight, width, length, height);

            Assert.AreEqual(5.0, price);
        }
    }
}