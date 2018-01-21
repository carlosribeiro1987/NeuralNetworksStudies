using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Exception {
    public class NeuralNetworkError : System.Exception {
        public NeuralNetworkError(String str) : base(str) {
        }
    }
}
