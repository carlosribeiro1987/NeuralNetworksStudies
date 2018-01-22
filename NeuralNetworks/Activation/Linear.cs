using NeuralNetworks.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Activation {

    /// <summary>
    /// The Linear layer is really not an activation function at all.  
    /// The input is simply passed on, unmodified, to the output.
    /// This activation function is primarily theoretical and of little actual
    /// use.  Usually an activation function that scales between 0 and 1 or
    /// -1 and 1 should be used.
    /// </summary>
    [Serializable]
    public class Linear : ActivationFunction{

        /// <summary>
        /// A activation function for a neural network.  The input is simply passed on, unmodified, to the output.
        /// </summary>
        /// <param name="d">The input to the function.</param>
        /// <returns>The ouput from the function.</returns>
        public double ActivationFunction(double input) {
            return input;
        }

        /// <summary>
        ///There is no derivative function for the linear activation function, so this method throws an error.
        /// </summary>
        /// <param name="d">The input to the function.</param>
        /// <returns>The ouput from the function.</returns>
        public double DerivativeFunction(double input) {
            throw new NeuralNetworkError("Can't use the linear activation function where a derivative is required.");
        }
    }
}
