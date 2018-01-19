using NeuralNetworks.Exception;
using System;

namespace NeuralNetworks.MatrixUtils {
    public class Matrix {
        private double[,] matrix;
        #region CONSTRUCTORS

        /// <summary>
        /// Create the matrix from a 2D boolean array.
        /// Translate true to 1 and false to -1.
        /// </summary>
        /// <param name="source">The 2D source array.</param>
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

        /// <summary>
        /// Create the matrix from a 2D double array.
        /// </summary>
        /// <param name="source">The 2D source array.</param>
        public Matrix(double[,] source) {
            matrix = new double[source.GetUpperBound(0) + 1, source.GetUpperBound(1) + 1];
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    this[row, col] = source[row, col];
                }
            }
        }

        /// <summary>
        /// Create a matrix with the specified diensions. All elements start with zero.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of columns.</param>
        public Matrix(int rows, int cols) {
            matrix = new double[rows, cols];
        }

        #endregion CONSTRUCTORS
        //-----------------------------------------------------
        #region FUNCTIONS
        
        /// <summary>
        /// Create a single row matrix.
        /// </summary>
        /// <param name="input">The array with the elements of the matrix.</param>
        /// <returns>A single row matrix.</returns>
        public static Matrix CreateRowMatrix(double[] input) {
            double[,] temp = new double[1, input.Length];
            for (int i = 0; i < input.Length; i++) {
                temp[0, i] = input[i];
            }
            return new Matrix(temp);
        }

        /// <summary>
        /// Create a single column matrix.
        /// </summary>
        /// <param name="input">The array with the elements of the matrix.</param>
        /// <returns>A single column matrix.</returns>
        public static Matrix CreateColumnMatrix(double[] input) {
            double[,] temp = new double[input.Length, 1];
            for (int i = 0; i < input.Length; i++) {
                temp[i, 0] = input[i];
            }
            return new Matrix(temp);
        }

        /// <summary>
        /// Add the specified value to the specified element of the matrix.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="col">The column index.</param>
        /// <param name="value">The value to add.</param>
        public void Add(int row, int col, double value) {
            Validate(row, col);
            this[row, col] += value;
        }

        /// <summary>
        /// Clear the matrix. All elements will be zero.
        /// </summary>
        public void Clear() {
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    this[row, col] = 0;
                }
            }
        }

        /// <summary>
        /// Clone the matrix.
        /// </summary>
        /// <returns>A cloned copy of the matrix.</returns>
        public Matrix Clone() {
            return new Matrix(matrix);
        }

        /// <summary>
        /// Compare this matrix to another.
        /// </summary>
        /// <param name="other">The matrix to compare.</param>
        /// <param name="precision">The number of decimal places for the precision. Default is 10 decimal places.</param>
        /// <returns>True if the matrices are equal.</returns>
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

        /// <summary>
        /// Take the elements of the matrix from a packed array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index to start reading.</param>
        /// <returns>The new index after the matrix is read.</returns>
        public int FromPackedArray(double[] array, int index) {
            for(int row = 0; row < Rows; row++) {
                for(int col = 0; col < Columns; col++) {
                    matrix[row, col] = array[index++];
                }
            }
            return index;
        }

        /// <summary>
        /// Convert the matrix to a packed array.
        /// </summary>
        /// <returns>The packed array.</returns>
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

        /// <summary>
        /// Get the specified row from the matrix.
        /// </summary>
        /// <param name="row">The row to get.</param>
        /// <returns>The row.</returns>
        public Matrix GetRow(int row) {
            if (row > Rows) 
                throw new MatrixError(string.Format("The row [{0}] does not exist.", row));
            double[,] newMatrix = new double[1, Columns];
            for (int col = 0; col < Columns; col++) {
                newMatrix[0, col] = matrix[row, col];
            }
            return new Matrix(newMatrix);

        }

        /// <summary>
        /// Get the specified column from the matrix.
        /// </summary>
        /// <param name="column">The column to get.</param>
        /// <returns>The column.</returns>
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

        /// <summary>
        /// Determine if the matrix is a vector.
        /// </summary>
        /// <returns>True if the matrix is a vector.</returns>
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

        /// <summary>
        /// Fill the matrix with random values in the specified range.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public void Randomize(int min, int max) {
            Random rand = new Random();
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    matrix[row, col] = (rand.NextDouble() * (max - min)) + min;
                }
            }
        }

        /// <summary>
        /// Get the sum of all elements in the matrix.
        /// </summary>
        /// <returns>The sum of the elements in the matrix.</returns>
        public double Sum() {
            double result = 0;
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Columns; col++) {
                    result += matrix[row, col];
                }
            }
            return result;
        }

        /// <summary>
        /// Validete if the specified element is in the range of the matrix.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="col"> the column index.</param>
        public void Validate(int row, int col) {
            if ((row > Rows) || (row < 0))
                throw new MatrixError(string.Format("The row [{0}] is out of range: {1}.", row, Rows));

            if ((col > Columns) || (col < 0))
                throw new MatrixError(string.Format("The column [{0}] is out of range: {1}.", col, Columns));
        }

        #endregion FUNCTIONS
        //-----------------------------------------------------
        #region PROPERTIES
        
        /// <summary>
        /// Allows access to the elements of the matrix.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="col">The column index.</param>
        /// <returns>The element at the specified position.</returns>
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

        /// <summary>
        /// The number of rows in the matrix.
        /// </summary>
        public int Rows {
            get { return matrix.GetUpperBound(0) + 1; }
        }
        /// <summary>
        /// The number of columns in the matrix.
        /// </summary>
        public int Columns {
            get { return matrix.GetUpperBound(1) + 1; }
        }

        /// <summary>
        /// The number of elements in the matrix.
        /// </summary>
        public int Size {
            get { return Rows * Columns; }
        }

        #endregion PROPERTIES

    }
}


