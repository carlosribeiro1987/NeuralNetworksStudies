using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Activation {
    public interface ActivationFunction {
        double ActivationFunction(double input);
        double DerivativeFunction(double input);
    }
}
