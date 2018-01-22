using NeuralNetworks.Feedforward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.MatrixUtils {
    public class MatrixCODEC {
        public static void ArrayToNetwork(double[] array, FeedforwardNetwork network) {
            int index = 0;
            foreach(FeedforwardLayer layer in network.Layers) {
                if (layer.NextLayer != null)
                    index = layer.LayerMatrix.FromPackedArray(array, index);
            }
        }

        public static double[] NetworkToArray(FeedforwardNetwork network) {
            int size = 0;
            foreach(FeedforwardLayer layer in network.Layers) {
                if (layer.HasMatrix())
                    size += layer.MatrixSize;
            }
            double[] result = new double[size];
            int index = 0;
            foreach(FeedforwardLayer layer in network.Layers) {
                if(layer.NextLayer != null) {
                    double[] matrix = layer.LayerMatrix.ToPackedArray();
                    for(int i = 0; i < matrix.Length; i++) {
                        result[index++] = matrix[i];
                    }
                }
            }
            return result;
        }
    }
}
