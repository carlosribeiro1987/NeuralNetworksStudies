using NeuralNetworks.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Activation {
    [Serializable]
    public class Linear : ActivationFunction{
        public double ActivationFunction(double input) {
            return input;
        }
        public double DerivativeFunction(double input) {
            throw new NeuralNetworkError("Can't use the linear activation function where a derivative is required.");
        }
    }
}
