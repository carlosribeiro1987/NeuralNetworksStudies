using NeuralNetworks.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Activation {

    /// <summary>
    /// The hyperbolic tangent activation function takes the curved shape of the hyperbolic tangent.  
    /// This activation function produces both positive and negative output.  
    /// </summary>
    [Serializable]
    public class HiperbolicTangent : ActivationFunction {

        /// <summary>
        /// A activation function for a neural network.
        /// </summary>
        /// <param name="d">The input to the function.</param>
        /// <returns>The ouput from the function.</returns>
        public double ActivationFunction(double input) {
            //return (BoundNumbers.Exp(input * 2.0) - 1) / (BoundNumbers.Exp(input * 2.0 + 1));
            
            //Correction suggested by https://github.com/felipetavares
            return (BoundNumbers.Exp(input) - BoundNumbers.Exp(-input)) / (BoundNumbers.Exp(input) + BoundNumbers.Exp(-input)); 
        }

        /// <summary>
        /// Performs the derivative function of the activation function function on the input.
        /// </summary>
        /// <param name="d">The input to the function.</param>
        /// <returns>The ouput from the function.</returns>
        public double DerivativeFunction(double input) {
            return (1.0 - Math.Pow(ActivationFunction(input), 2.0));
        }
    }
}
