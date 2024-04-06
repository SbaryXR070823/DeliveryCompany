using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return totalPrice;
        }
    }
}
