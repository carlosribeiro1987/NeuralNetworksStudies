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

        public double CalculateError(double[][] input, double[][] ideal) {
            ErrorCalculation errorCalc = new ErrorCalculation();
            for (int i = 0; i < ideal.Length; i++) {
                ComputeOutputs(input[i]);
                errorCalc.UpdateError(_outputLayer.Fire, ideal[i]);
            }
            return errorCalc.RootMeanSquare();
        }

        public int CalculateNeuronCount() {
            int result = 0;
            foreach (FeedforwardLayer layer in _layers) {
                result += layer.NeuronCount;
            }
            return result;
        }

        public FeedforwardNetwork CloneStructure() {
            FeedforwardNetwork result = new FeedforwardNetwork();
            foreach (FeedforwardLayer layer in _layers) {
                FeedforwardLayer cloned = new FeedforwardLayer(layer.NeuronCount);
                result.AddLayer(cloned);
            }
            return result;
        }

        public object Clone() {
            FeedforwardNetwork result = CloneStructure();
            double[] copy = MatrixCODEC.NetworkToArray(this);
            MatrixCODEC.ArrayToNetwork(copy, result);
            return result;
        }

        public void Reset() {
            foreach(FeedforwardLayer layer in _layers) {
                layer.Reset();
            }
        }

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

        public FeedforwardLayer InputLayer {
            get { return _inputLayer; }
        }

        public FeedforwardLayer OutputLayer {
            get { return _outputLayer; }
        }

        public IList<FeedforwardLayer> Layers {
            get { return _layers; }
        }

        public int HiddenLayersCount {
            get { return _layers.Count - 2; }
        }

        public int MatrixSize {
            get {
                int result = 0;
                foreach (FeedforwardLayer layer in _layers) {
                    result += layer.MatrixSize;
                }
                return result;
            }
        }

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

        #endregion PROPERTIES
    }
}
