using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Activation {
    public interface ActivationFunction {

        /// <summary>
        /// Activation function for a neural network
        /// </summary>
        /// <param name="input">The input to the function</param>
        /// <returns>The output from the function</returns>
        double ActivationFunction(double input);

        /// <summary>
        /// Performs the derivative function of the activation function on the input
        /// </summary>
        /// <param name="input">The input to the function</param>
        /// <returns>The output from the function</returns>
        double DerivativeFunction(double input);
    }
}
