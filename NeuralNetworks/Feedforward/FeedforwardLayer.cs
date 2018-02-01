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

        /// <summary>
        /// Construct the layer with the specified threshold function.
        /// </summary>
        /// <param name="thresholdFunction">The threshold function to use.</param>
        /// <param name="neurons">The number of neurons in the layer.</param>
        public FeedforwardLayer(ActivationFunction thresholdFunction, int neurons) {
            _fire = new double[neurons];
            _activationFunction = thresholdFunction;
        }

        /// <summary>
        /// Construct the layer with Sigmoid threshold function.
        /// </summary>
        /// <param name="neurons">The number of neurons in the layer.</param>
        public FeedforwardLayer(int neurons) : this(new Sigmoid(), neurons) {
        }

        #endregion CONSTRUCTORS

        #region FUNCTIONS

        /// <summary>
        /// Take a simple double array and turns it into a matrix that can be used to calculate
        /// the results of the input array. Also takes into account the threshold.
        /// </summary>
        /// <param name="patern">The input pattern.</param>
        /// <returns>The input matrix.</returns>
        private Matrix CreateInputMatrix(double[] patern) {
            Matrix result = new Matrix(1, patern.Length + 1);
            for(int i  = 0; i < patern.Length; i++) {
                result[0, i] = patern[i];
            }
            // Add a "fake" first column to the input so that the threshold is always multiplied by 1, resulting in it just being added.
            result[0, patern.Length] = 1;
            return result;
        }

        /// <summary>
        /// Set the output value for the specified neuron.
        /// </summary>
        /// <param name="index">The specified neuron.</param>
        /// <param name="value">The output value for the neuron.</param>
        public void SetFire(int index, double value) {
            _fire[index] = value;
        }

        /// <summary>
        /// Get the output from the specified neuron.
        /// </summary>
        /// <param name="index">The specified neuron.</param>
        /// <returns>The output from the specified neuron.</returns>
        public double GetFire(int index) {
            return _fire[index];
        }

        /// <summary>
        /// Clone the structure of this layer, but do not copy any matrix data.
        /// </summary>
        /// <returns>The cloned layer.</returns>
        public FeedforwardLayer CloneStructure() {
            return new FeedforwardLayer(_activationFunction, NeuronCount);
        }

        
        /// <summary>
        /// Compute the output for this layer given the input pattern.
        /// The output is also stored in the fire instance variable.
        /// </summary>
        /// <param name="pattern">The input pattern</param>
        /// <returns>The output from this layer.</returns>
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

        /// <summary>
        /// Prune one of the neurons from this layer. 
        /// Remove all entries in this weight matrix and other layers.
        /// </summary>
        /// <param name="neuron">The neuron to prune. Zero specifies the first neuron.</param>
        public void Prune(int neuron) { 
            if (_matrix != null) //Delete a row on this matris
                LayerMatrix = MatrixMath.DeleteRow(_matrix, neuron);
            FeedforwardLayer previous = PreviousLayer;
            if (previous.LayerMatrix != null) //Delete a column on the previous
                previous.LayerMatrix = MatrixMath.DeleteColumn(previous.LayerMatrix, neuron);
        }

        /// <summary>
        /// Produce a string representation of this layer.
        /// </summary>
        /// <returns>The string form of this layer.</returns>
        public override string ToString() {
            return string.Format("[FeedforwardLayer - NeuronCount = {0}]", NeuronCount);
        }

        /// <summary>
        /// Determine if this layer has a matrix.
        /// </summary>
        /// <returns>True if this layer has a matrix.</returns>
        public bool HasMatrix() {
            return _matrix != null;
        }

        /// <summary>
        /// Determine if this is an input layer.
        /// </summary>
        /// <returns>True if this is an input layer.</returns>
        public bool IsInputLayer() {
            return _previous == null;
        }

        /// <summary>
        /// Determine if this is a hidden layer.
        /// </summary>
        /// <returns>True if this is a hidden layer.</returns>
        public bool IsHiddenLayer() {
            return (_next != null) && (_previous != null);
        }

        /// <summary>
        /// Determine if this is an output layer.
        /// </summary>
        /// <returns>True if this is an output layer.</returns>
        public bool IsOutputLayer() {
            return _next == null;
        }

        /// <summary>
        /// Reset the weight matrix and threshold values to random numbers between -1 and 1.
        /// </summary>
        public void Reset() {
            if (_matrix != null)
                _matrix.Randomize(-1, 1);
        }

        #endregion FUNCTIONS

        #region PROPERTIES

        /// <summary>
        /// The weight and threshold matrix for the layer.
        /// </summary>
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

        /// <summary>
        /// The number of neurons in the layer.
        /// </summary>
        public int NeuronCount {
            get { return _fire.Length; }
        }

        /// <summary>
        /// The outputs for the layer.
        /// </summary>
        public double[] Fire {
            get { return _fire; }
        }

        /// <summary>
        /// The previous layer in this neural network.
        /// </summary>
        public FeedforwardLayer PreviousLayer {
            get { return _previous; }
            set { _previous = value; }
        }

        /// <summary>
        /// The next layer in this neural network.
        /// </summary>
        public FeedforwardLayer NextLayer {
            get { return _next; }
            set {
                _next = value;
                _matrix = new Matrix(NeuronCount + 1, _next.NeuronCount);
            }
        }

        /// <summary>
        /// The activation function for this layer.
        /// </summary>
        public ActivationFunction LayerActivationFunction {
            get { return _activationFunction; }
            set { _activationFunction = value; }
        }

        /// <summary>
        /// The size the weight matrix.
        /// </summary>
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
