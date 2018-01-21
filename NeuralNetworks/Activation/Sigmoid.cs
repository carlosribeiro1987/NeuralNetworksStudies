using NeuralNetworks.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Activation {
    [Serializable]
    public class Sigmoid : ActivationFunction {

        public double ActivationFunction(double input) {
            return 1.0 / (1 + BoundNumbers.Exp(-1.0 * input));
        }

        public double DerivativeFunction(double input) {
            return input * (1.0 - input);
        }
    }
}
