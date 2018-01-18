using NeuralNetworks.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.MatrixUtils {
    public class MatrixMath {

        public static Matrix Add(Matrix a, Matrix b) {
            if ((a.Rows != b.Rows) || (a.Columns != b.Columns))
                throw new MatrixError("To add the matrixes they must have the same number of rows and columns.");
            double[,] result = new double[a.Rows, a.Columns];
            for(int row = 0; row < a.Rows; row++) {
                for(int col = 0; col < a.Columns; col++) {
                    result[row, col] = a[row, col] + b[row, col];
                }
            }
            return new Matrix(result);
        }


    }
}



/*
        #region CONSTRUCTORS
        #endregion CONSTRUCTORS

        #region FUNCTIONS
        #endregion FUNCTIONS

        #region PROPERTIES
        #endregion PROPERTIES
     
*/
