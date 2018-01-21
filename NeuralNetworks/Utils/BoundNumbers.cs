using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Utils {
    public class BoundNumbers {
        const double MAX_VALUE = 1.0E20;
        const double MIN_VALUE = -1.0E20;

        public static double Bound(double value) {
            if (value < MIN_VALUE)
                return MIN_VALUE;
            else if (value > MAX_VALUE)
                return MAX_VALUE;
            else
                return value;
        }

        public static double Exp(double value) {
            return Bound(Math.Exp(value));
        }
    }
}
