using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Utils {
    public class ErrorCalculation {
        private double _globalError;
        private int _trainingSize;

        public double RootMeanSquare() {
            return Math.Sqrt(_globalError / _trainingSize);
        }

        public void UpdateError(double[] actual, double[] ideal) {
            for(int i = 0; i < actual.Length; i++) {
                double delta = ideal[i] - actual[i];
                _globalError += delta * delta;
            }
            _trainingSize += ideal.Length;
        }

        public void Reset() {
            _globalError = 0;
            _trainingSize = 0;
        }
    }
}
