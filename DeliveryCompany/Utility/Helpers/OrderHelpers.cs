using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models;

namespace Utility.Helpers
{
    public static  class OrderHelpers
    {
        public static double CalculatePrice(int weight, int width, int length, int height)
        {
            double basePrice = 5.0;
            double weightPrice = weight * 0.1;
            double volumePrice = width * length * height * 0.00001;

            double totalPrice = basePrice + weightPrice + volumePrice;

            if(totalPrice < 0)
            {
                return 0;
            }
            return totalPrice;
        }

        public static bool CheckPackage(PackageCheck package)
        {
            return package.MaxHeight > package.Height &&
                        package.MaxWeight > package.Weight &&
                        package.MaxWidth > package.Width &&
                        package.MaxLength > package.Length;
        }

        public static List<KeyValuePair<int, int>> GetOrderedDeliveriesByNumberOfOrders(Dictionary<int, int> deliveries)
        {
            List<KeyValuePair<int, int>> sortedList = deliveries.ToList();
            sortedList.Sort((x, y) => x.Value.CompareTo(y.Value));
            return sortedList;
        }
       
    }
}
