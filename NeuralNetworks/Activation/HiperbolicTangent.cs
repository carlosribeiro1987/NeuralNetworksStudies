using NeuralNetworks.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Activation {
    [Serializable]
    public class HiperbolicTangent : ActivationFunction {

        public double ActivationFunction(double input) {
            return (BoundNumbers.Exp(input * 2.0) - 1) / (BoundNumbers.Exp(input * 2.0 + 1));
        }

        public double DerivativeFunction(double input) {
            return (1.0 - Math.Pow(ActivationFunction(input), 2.0));
        }
    }
}
