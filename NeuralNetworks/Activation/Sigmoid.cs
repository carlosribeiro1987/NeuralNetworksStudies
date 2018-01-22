using NeuralNetworks.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Activation {

    /// <summary>
    /// The sigmoid activation function takes on a sigmoidal shape.  
    /// Only positive numbers are generated. 
    /// </summary>
    [Serializable]
    public class Sigmoid : ActivationFunction {

        /// <summary>
        /// A activation function for a neural network.
        /// </summary>
        /// <param name="d">The input to the function.</param>
        /// <returns>The ouput from the function.</returns>
        public double ActivationFunction(double input) {
            return 1.0 / (1 + BoundNumbers.Exp(-1.0 * input));
        }

        /// <summary>
        /// Performs the derivative function of the activation function function on the input.
        /// </summary>
        /// <param name="d">The input to the function.</param>
        /// <returns>The ouput from the function.</returns>
        public double DerivativeFunction(double input) {
            return input * (1.0 - input);
        }
    }
}
