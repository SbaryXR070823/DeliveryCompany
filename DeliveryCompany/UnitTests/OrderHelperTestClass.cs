using Utility.Helpers;

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
    }
}