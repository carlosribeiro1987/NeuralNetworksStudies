using NeuralNetworks.Exception;
using NeuralNetworks.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks.MatrixUtils;
namespace NeuralNetworks.Feedforward {
    [Serializable]
    public class FeedforwardNetwork {
        protected FeedforwardLayer _inputLayer;
        protected FeedforwardLayer _outputLayer;
        protected IList<FeedforwardLayer> _layers = new List<FeedforwardLayer>();


        #region CONSTRUCTOR

        #endregion CONSTRUCTOR

        #region FUNCTIONS

        /// <summary>
        /// Add a layer to this neural network.
        /// </summary>
        /// <param name="layer">The layer to add.</param>
        public void AddLayer(FeedforwardLayer layer) {
            if (_outputLayer != null) {
                layer.PreviousLayer = _outputLayer;
                _outputLayer.NextLayer = layer;
            }
            if (_layers.Count == 0)
                _inputLayer = _outputLayer = layer;
            else
                _outputLayer = layer;
            _layers.Add(layer);
        }

        /// <summary>
        /// Compute the output from a given input to the neural network.
        /// </summary>
        /// <param name="input">The input to provide to the neural network.</param>
        /// <returns>The results from the output neurons.</returns>
        public double[] ComputeOutputs(double[] input) {
            if (input.Length != _inputLayer.NeuronCount)
                throw new NeuralNetworkError(string.Format("Size mismatch: Can't compute outputs for input size of {0}. The size of input layer is {1}.", input.Length, _inputLayer.NeuronCount));
            foreach (FeedforwardLayer layer in _layers) {
                if (layer.IsInputLayer())
                    layer.ComputeOutputs(input);
                else
                    layer.ComputeOutputs(null);
            }
            return _outputLayer.Fire;
        }

        /// <summary>
        /// Calculate the error for this neural network. The error is calculated using Root Mean Square.
        /// </summary>
        /// <param name="input">The input patterns.</param>
        /// <param name="ideal">The output patterns.</param>
        /// <returns>The error percentage.</returns>
        public double CalculateError(double[][] input, double[][] ideal) {
            ErrorCalculation errorCalc = new ErrorCalculation();
            for (int i = 0; i < ideal.Length; i++) {
                ComputeOutputs(input[i]);
                errorCalc.UpdateError(_outputLayer.Fire, ideal[i]);
            }
            return errorCalc.RootMeanSquare();
        }

        /// <summary>
        /// Calculate the total nuber of layers in this neural network.
        /// </summary>
        /// <returns>The total nuber of neurons.</returns>
        public int CalculateNeuronCount() {
            int result = 0;
            foreach (FeedforwardLayer layer in _layers) {
                result += layer.NeuronCount;
            }
            return result;
        }

        /// <summary>
        /// Clone the structure of this neural network.
        /// </summary>
        /// <returns>A cloned copy of the structure of the neural network.</returns>
        public FeedforwardNetwork CloneStructure() {
            FeedforwardNetwork result = new FeedforwardNetwork();
            foreach (FeedforwardLayer layer in _layers) {
                FeedforwardLayer cloned = new FeedforwardLayer(layer.NeuronCount);
                result.AddLayer(cloned);
            }
            return result;
        }

        /// <summary>
        /// Returns a clone of this neural network. Including weight, threshold and structure.
        /// </summary>
        /// <returns>A cloned copy of this neural network.</returns>
        public object Clone() {
            FeedforwardNetwork result = CloneStructure();
            double[] copy = MatrixCODEC.NetworkToArray(this);
            MatrixCODEC.ArrayToNetwork(copy, result);
            return result;
        }

        /// <summary>
        /// Reset the weight matrix and thresholds.
        /// </summary>
        public void Reset() {
            foreach(FeedforwardLayer layer in _layers) {
                layer.Reset();
            }
        }

        /// <summary>
        /// Compare this neural network with another.
        /// To be equal it must have the same structure and matrix values.
        /// </summary>
        /// <param name="other">The other neural network.</param>
        /// <returns>True if the neural networks are equal.</returns>
        public bool Equals(FeedforwardNetwork other) {
            int i = 0;
            foreach (FeedforwardLayer layer in _layers) {
                FeedforwardLayer otherLayer = other.Layers[i++];
                if (layer.NeuronCount != otherLayer.NeuronCount)
                    return false;
                if ((layer.LayerMatrix == null) && (otherLayer.LayerMatrix != null))
                    return false;
                if ((layer.LayerMatrix != null) && (otherLayer.LayerMatrix == null))
                    return false;
                if ((layer.LayerMatrix != null) && (otherLayer.LayerMatrix != null)) {
                    if (!layer.LayerMatrix.Equals(otherLayer.LayerMatrix))
                        return false;
                }
            }
            return true;
        }

        #endregion FUNCTIONS

        #region PROPERTIES

        /// <summary>
        /// The input layer.
        /// </summary>
        public FeedforwardLayer InputLayer {
            get { return _inputLayer; }
        }

        /// <summary>
        /// A list of the hidden layers.
        /// </summary>
        public ICollection<FeedforwardLayer> HiddenLayers {
            get {
                ICollection<FeedforwardLayer> result = new List<FeedforwardLayer>();
                foreach (FeedforwardLayer layer in _layers) {
                    if (layer.IsHiddenLayer())
                        result.Add(layer);
                }
                return result;
            }
        }

        /// <summary>
        /// The output layer.
        /// </summary>
        public FeedforwardLayer OutputLayer {
            get { return _outputLayer; }
        }

        /// <summary>
        /// The layers in this neural network.
        /// </summary>
        public IList<FeedforwardLayer> Layers {
            get { return _layers; }
        }

        /// <summary>
        /// The number of hidden layers in this neural network.
        /// </summary>
        public int HiddenLayersCount {
            get { return _layers.Count - 2; }
        }

        /// <summary>
        /// The size of weight and threshold matrix.
        /// </summary>
        public int MatrixSize {
            get {
                int result = 0;
                foreach (FeedforwardLayer layer in _layers) {
                    result += layer.MatrixSize;
                }
                return result;
            }
        }        

        #endregion PROPERTIES
    }
}
