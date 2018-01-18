using NeuralNetworks.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.MatrixUtils {
    public class Matrix {
        private double[,] matrix;
        #region CONSTRUCTORS

        public Matrix(bool[,] source) {
            matrix = new double[source.GetUpperBound(0) + 1, source.GetUpperBound(1) + 1];
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    if (source[row, col])
                        this[row, col] = 1;
                    else
                        this[row, col] = -1;
                }
            }
        }

        public Matrix(double[,] source) {
            matrix = new double[source.GetUpperBound(0) + 1, source.GetUpperBound(1) + 1];
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    this[row, col] = source[row, col];
                }
            }
        }

        public Matrix(int rows, int cols) {
            matrix = new double[rows, cols];
        }

        #endregion CONSTRUCTORS
        //-----------------------------------------------------
        #region FUNCTIONS

        public static Matrix CreateRowMatrix(double[] input) {
            double[,] temp = new double[1, input.Length];
            for (int i = 0; i < input.Length; i++) {
                temp[0, i] = input[i];
            }
            return new Matrix(temp);
        }

        public static Matrix CreateColumnMatrix(double[] input) {
            double[,] temp = new double[input.Length, 1];
            for (int i = 0; i < input.Length; i++) {
                temp[i, 0] = input[i];
            }
            return new Matrix(temp);
        }

        public void Add(int row, int col, double value) {
            Validate(row, col);
            this[row, col] += value;
        }

        public void Clear() {
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    this[row, col] = 0;
                }
            }
        }

        public Matrix Clone() {
            return new Matrix(matrix);
        }

        public bool Equals(Matrix other, int precision = 10) {
            if (precision < 0)
                throw new MatrixError("Precision can't be a negative number.");
            double test = Math.Pow(10.0, precision);
            if ((double.IsInfinity(test)) || (double.IsNaN(test)))
                throw new MatrixError(string.Format("Precision of {0} decimal places is not supported", precision));
            precision = (int)Math.Pow(10.0, precision);
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    if ((long)(this[row, col] * precision) != (long)(other[row, col] * precision))
                        return false;
                }
            }
            return true;
        }

        public int FromPackedArray(double[] array, int index) {
            for(int row = 0; row < Rows; row++) {
                for(int col = 0; col < Columns; col++) {
                    matrix[row, col] = array[index++];
                }
            }
            return index;
        }

        public double[] ToPackedArray() {
            double[] result = new double[Rows * Columns];
            int index = 0;
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    result[index++] = matrix[row, col];
                }
            }
            return result;
        }

        public Matrix GetRow(int row) {
            if (row > Rows) 
                throw new MatrixError(string.Format("The row [{0}] does not exist.", row));
            double[,] newMatrix = new double[1, Columns];
            for (int col = 0; col < Columns; col++) {
                newMatrix[0, col] = matrix[row, col];
            }
            return new Matrix(newMatrix);

        }

        public Matrix GetColumn(int column) {
            if (column > Columns) {
                throw new MatrixError(string.Format("The column [{0}] does not exist.", column));
            }
            double[,] newMatrix = new double[Rows, 1];
            for (int row = 0; row < Rows; row++) {
                newMatrix[row, 0] = matrix[row, column];
            }
            return new Matrix(newMatrix);
        }

        public bool IsVector() {
            return (Rows == 1) || (Columns == 1);
        }

        public bool IsZero() {
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    if (matrix[row, col] != 0) {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Randomize(int min, int max) {
            Random rand = new Random();
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    matrix[row, col] = (rand.NextDouble() * (max - min)) + min;
                }
            }
        }

        public double Sum() {
            double result = 0;
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    result += matrix[row, col];
                }
            }
            return result;
        }

        public void Validate(int row, int col) {
            if ((row > Rows) || (row < 0))
                throw new MatrixError(string.Format("The row [{0}] is out of range: {1}.", row, Rows));

            if ((col > Columns) || (col < 0))
                throw new MatrixError(string.Format("The column [{0}] is out of range: {1}.", col, Columns));
        }

        #endregion FUNCTIONS
        //-----------------------------------------------------
        #region PROPERTIES

        public double this[int row, int col] {
            get {
                Validate(row, col);
                return matrix[row, col];
            }
            set {
                Validate(row, col);
                if ((double.IsInfinity(value)) || (double.IsNaN(value))) {
                    throw new MatrixError(string.Format("Trying to assign invalid number to the matrix: [{0}]", value));
                }
                matrix[row, col] = value;
            }
        }

        public int Rows {
            get { return matrix.GetUpperBound(0) + 1; }
        }
        public int Columns {
            get { return matrix.GetUpperBound(1) + 1; }
        }
        public int Size {
            get { return Rows * Columns; }
        }

        #endregion PROPERTIES

    }
}


