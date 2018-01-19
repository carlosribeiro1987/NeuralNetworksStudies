using NeuralNetworks.Exception;
using System;

namespace NeuralNetworks.MatrixUtils {
    public class MatrixMath {

        /// <summary>
        /// Add two matrices. Both matrices must have the same dimensions.
        /// </summary>
        /// <param name="a">The fist matrix to add.</param>
        /// <param name="b">The second matrix to add.</param>
        /// <returns>A new matrix with the results of addition.</returns>
        public static Matrix Add(Matrix a, Matrix b) {
            if ((a.Rows != b.Rows) || (a.Columns != b.Columns))
                throw new MatrixError("To add the matrixes they must have the same number of rows and columns.");
            double[,] result = new double[a.Rows, a.Columns];
            for (int row = 0; row < a.Rows; row++) {
                for (int col = 0; col < a.Columns; col++) {
                    result[row, col] = a[row, col] + b[row, col];
                }
            }
            return new Matrix(result);
        }
        /// <summary>
        /// Subtract one matrix from another. Both matrices must have the same dimensions.
        /// </summary>
        /// <param name="a">The matrix to subtract from.</param>
        /// <param name="b">The matrix that will be subtracted from the first.</param>
        /// <returns>A new matrix with the results of subtraction.</returns>
        public static Matrix Subtract(Matrix a, Matrix b) {
            if ((a.Rows != b.Rows) || (a.Columns != b.Columns))
                throw new MatrixError("To subtract the matrixes they must have the same number of rows and columns.");
            double[,] result = new double[a.Rows, a.Columns];
            for (int row = 0; row < a.Rows; row++) {
                for (int col = 0; col < a.Columns; col++) {
                    result[row, col] = a[row, col] - b[row, col];
                }
            }
            return new Matrix(result);
        }
        /// <summary>
        /// Multiply every elements of a matrix by the specified value.
        /// </summary>
        /// <param name="matrix">The matrix whose elements will be multiplied.</param>
        /// <param name="value">The value to multiply.</param>
        /// <returns>A new matrix with the results of multiplication.</returns>
        public static Matrix Multiply(Matrix matrix, double value) {
            double[,] result = new double[matrix.Rows, matrix.Columns];
            for (int row = 0; row < matrix.Rows; row++) {
                for (int col = 0; col < matrix.Columns; col++) {
                    result[row, col] = matrix[row, col] * value;
                }
            }
            return new Matrix(result);
        }
        /// <summary>
        /// Multiply two matrices. The number of coumns of the first matrix must match the number of rows in the second.
        /// </summary>
        /// <param name="a">The first matrix. The number of columns must match the number of rows in the second.</param>
        /// <param name="b">The second matrix. The number of rows must match the number of columns in the first.</param>
        /// <returns>A new matrix with the results of multiplication.</returns>
        public static Matrix Multiply(Matrix a, Matrix b) {
            if (a.Columns != b.Rows)
                throw new MatrixError("The number of columns on matrix 'a' must match the number of rows on matrix 'b'.");
            double[,] result = new double[a.Rows, b.Columns];
            for (int row = 0; row < a.Rows; row++) {
                for (int col = 0; col < b.Columns; col++) {
                    double value = 0;
                    for (int i = 0; i < a.Columns; i++) {
                        value += a[row, i] * b[i, col];
                    }
                    result[row, col] = value;
                }
            }
            return new Matrix(result);
        }
        /// <summary>
        /// Divide every element of a matrix by the specified value.
        /// </summary>
        /// <param name="matrix">The matrix whose elements will be divided.</param>
        /// <param name="value">The value to divide.</param>
        /// <returns>A new matrix with the results of division.</returns>
        public static Matrix Divide(Matrix matrix, double value) {
            double[,] result = new double[matrix.Rows, matrix.Columns];
            for (int row = 0; row < matrix.Rows; row++) {
                for (int col = 0; col < matrix.Columns; col++) {
                    result[row, col] = matrix[row, col] / value;
                }
            }
            return new Matrix(result);
        }
        /// <summary>
        /// Calculte the dot product of two matrices. Both matrices must be vectors.
        /// </summary>
        /// <param name="a">The first matrix. Must be a vector.</param>
        /// <param name="b">The second matrix. Must be a vector.</param>
        /// <returns>The dot product of the matrices.</returns>
        public static double DotProduct(Matrix a, Matrix b) {
            if (!a.IsVector() || !b.IsVector())
                throw new MatrixError("To take the dot product, both matrices must be vectors.");
            double[] aArray = a.ToPackedArray();
            double[] bArray = b.ToPackedArray();
            if (aArray.Length != bArray.Length)
                throw new MatrixError("To take the dot product, both matrices must be of the same length.");
            double result = 0;
            for (int i = 0; i < aArray.Length; i++) {
                result += aArray[i] * bArray[i];
            }
            return result;
        }

        /// <summary>
        /// Calculate the vector length of of the matrix. The matrix must be a vector.
        /// </summary>
        /// <param name="input">The vector to calculate.</param>
        /// <returns>The vector length.</returns>
        public static double VectorLength(Matrix input) {
            if (!input.IsVector())
                throw new MatrixError("Can only take the vector length of a vector.");
            double[] vector = input.ToPackedArray();
            double temp = 0;
            for (int i = 0; i < vector.Length; i++) {
                temp += Math.Pow(vector[i], 2);
            }
            return Math.Sqrt(temp);
        }

        /// <summary>
        /// Transpose the specified matrix.
        /// </summary>
        /// <param name="input">The matrix to transpose.</param>
        /// <returns>The transposed matrix.</returns>
        public static Matrix Transpose(Matrix input) {
            double[,] output = new double[input.Columns, input.Rows];
            for (int row = 0; row < input.Rows; row++) {
                for (int col = 0; col < input.Columns; col++) {
                    output[col, row] = input[row, col];
                }
            }
            return new Matrix(output);
        }

        /// <summary>
        /// Create an identity matrix. The identity matrix is always square.
        /// </summary>
        /// <param name="size">The size of the identity matrix. Must be greater or equals to 1.</param>
        /// <returns>The identity matrix.</returns>
        public static Matrix Identity(int size) {
            if (size < 1)
                throw new MatrixError("The size of identity matrices can't be less than 1.");
            Matrix result = new Matrix(size, size);
            for (int i = 0; i < size; i++) {
                result[i, i] = 1;
            }
            return result;
        }

        /// <summary>
        /// Copy one matrix to another. Both matrices must have the same dimensions.
        /// </summary>
        /// <param name="source">The matrix to be copied.</param>
        /// <param name="target">The matrix to copy to.</param>
        public static void Copy(Matrix source, Matrix target) {
            if ((source.Rows != target.Rows) || (source.Columns != target.Columns))
                throw new MatrixError("The target matrix must have the same dimensions of the source matrix.");
            for(int row = 0; row < source.Rows; row++) {
                for(int col = 0; col < source.Columns; col++) {
                    target[row, col] = source[row, col];
                }
            }
        }

        /// <summary>
        /// Delete a row from the matrix.
        /// </summary>
        /// <param name="matrix">The matrix</param>
        /// <param name="rowIndex">The row index.</param>
        /// <returns>The matrix without the delected row.</returns>
        public static Matrix DeleteRow(Matrix matrix, int rowIndex) {
            if(rowIndex > matrix.Rows)
                throw new MatrixError(string.Format("Can't delete row [{0}] from matrix. It has only {1} rows.", rowIndex, matrix.Rows));
            double[,] newMatrix = new double[matrix.Rows - 1, matrix.Columns];
            int targetRow = 0;
            for(int row = 0; row < matrix.Rows; row++) {
                if(row != rowIndex) {
                    for (int col = 0; col < matrix.Columns; col++) {
                        newMatrix[targetRow, col] = matrix[row, col];
                    }
                    targetRow++;
                }
            }
            return new Matrix(newMatrix);
        }

        /// <summary>
        /// Delete a column from the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The matrix without the deleted column.</returns>
        public static Matrix DeleteColumn(Matrix matrix, int columnIndex) {
            if(columnIndex > matrix.Columns)
                throw new MatrixError(string.Format("Can't delete column [{0}] from matrix. It has only {1} columns.", columnIndex, matrix.Columns));
            double[,] newMatrix = new double[matrix.Rows, matrix.Columns - 1];
            for(int row = 0; row < matrix.Rows; row++) {
                int targetColumn = 0;
                for(int col = 0; col < matrix.Columns; col++) {
                    if(col != columnIndex) {
                        newMatrix[row, targetColumn] = matrix[row, col];
                        targetColumn++;
                    }
                }
            }
            return new Matrix(newMatrix);
        }
    }
}