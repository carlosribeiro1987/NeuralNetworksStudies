using NeuralNetworks.Activation;
using NeuralNetworks.Exception;
using NeuralNetworks.MatrixUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Feedforward {
    [Serializable]
    public class FeedforwardLayer {
        private double[] _fire; //Results from the last time the outputs were calculated for the neural network
        public Matrix _matrix; //The weight and threshold matrix
        private FeedforwardLayer _next; //The next layer in the neural network
        private FeedforwardLayer _previous; //The previous layer in the neural network
        private ActivationFunction _activationFunction; //The activation function to use for this layer

        #region CONSTRUCTORS

        public FeedforwardLayer(ActivationFunction thresholdFunction, int neurons) {
            _fire = new double[neurons];
            _activationFunction = thresholdFunction;
        }

        public FeedforwardLayer(int neurons) : this(new Sigmoid(), neurons) {
        }

        #endregion CONSTRUCTORS

        #region FUNCTIONS

        private Matrix CreateInputMatrix(double[] patern) {
            Matrix result = new Matrix(1, patern.Length + 1);
            for(int i  = 0; i < patern.Length; i++) {
                result[0, i] = patern[i];
            }
            // Add a "fake" first column to the input so that the threshold is always multiplied by 1, resulting in it just being added.
            result[0, patern.Length] = 1;
            return result;
        }


        public void SetFire(int index, double value) {
            _fire[index] = value;
        }

        public double GetFire(int index) {
            return _fire[index];
        }

        public FeedforwardLayer CloneStructure() {
            return new FeedforwardLayer(_activationFunction, NeuronCount);
        }

        

        public double[] ComputeOutputs(double[] pattern) {
            if(pattern != null) {
                for(int i = 0; i < NeuronCount; i++) {
                    SetFire(i, pattern[i]);
                }
            }
            Matrix inputMatrix = CreateInputMatrix(_fire);
            for(int i = 0; i < _next.NeuronCount; i++) {
                Matrix col = _matrix.GetColumn(i);
                double sum = MatrixMath.DotProduct(col, inputMatrix);
                _next.SetFire(i, _activationFunction.ActivationFunction(sum));
            }
            return _fire;
        }

        public void Prune(int neuron) { 
            if (_matrix != null) //Delete a row on this matris
                LayerMatrix = MatrixMath.DeleteRow(_matrix, neuron);
            FeedforwardLayer previous = PreviousLayer;
            if (previous.LayerMatrix != null) //Delete a column on the previous
                previous.LayerMatrix = MatrixMath.DeleteColumn(previous.LayerMatrix, neuron);
        }

        public override string ToString() {
            return string.Format("[FeedforwardLayer - NeuronCount = {0}]", NeuronCount);
        }

        public bool HasMatrix() {
            return _matrix != null;
        }

        public bool IsInputLayer() {
            return _previous == null;
        }

        public bool IsHiddenLayer() {
            return (_next != null) && (_previous != null);
        }

        public bool IsOutputLayer() {
            return _next == null;
        }

        public void Reset() {
            if (_matrix != null)
                _matrix.Randomize(-1, 1);
        }

        #endregion FUNCTIONS

        #region PROPERTIES

        public Matrix LayerMatrix {
            get { return _matrix; }
            set {
                if(_matrix.Rows < 2)
                    throw new NeuralNetworkError("Weight matrix includes threshold values, and must have at least two rows.");
                if (_matrix != null)
                    _fire = new double[_matrix.Rows - 1];
                _matrix = value;
            }
        }

        public int NeuronCount {
            get { return _fire.Length; }
        }

        public double[] Fire {
            get { return _fire; }
        }

        public FeedforwardLayer PreviousLayer {
            get { return _previous; }
            set { _previous = value; }
        }

        public FeedforwardLayer NextLayer {
            get { return _next; }
            set {
                _next = value;
                _matrix = new Matrix(NeuronCount + 1, _next.NeuronCount);
            }
        }

        public ActivationFunction LayerActivationFunction {
            get { return _activationFunction; }
            set { _activationFunction = value; }
        }

        public int MatrixSize {
            get {
                if (_matrix == null)
                    return 0;
                else
                    return _matrix.Size;
            }
        }



        #endregion PROPERTIES

    }
}
