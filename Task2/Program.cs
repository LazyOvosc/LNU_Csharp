using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2
{
    internal class Matrix
    {
        private int[,] matrix;
        private int rowLength;
        private int columnLength;

        public Matrix(int[,] matrix)
        {
            rowLength = matrix.GetLength(0);
            columnLength = matrix.GetLength(1);
            this.matrix = new int[rowLength, columnLength];
            for (int i = 0; i < rowLength; i++)
            { 
                for (int j = 0; j < columnLength; j++)
                {
                    this.matrix[i, j] = matrix[i, j];
                }
            }
        }

        public int[,] MatrixData
        {
            get { return matrix; }
        }

        public int RowLength
        {
            get { return rowLength; }
        }

        public int ColumnLength
        {
            get { return columnLength; }
        }

        public int this[int i, int j]
        {
            get { return matrix[i, j]; }
            set { matrix[i, j] = value; }
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.rowLength != b.rowLength || a.columnLength != b.columnLength)
            {
                return false;
            }
            for (int i = 0; i < a.rowLength; i++)
            {
                for (int j = 0; j < a.columnLength; j++)
                {
                    if (a[i, j] != b[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            Matrix other = (Matrix)obj;
            if (this.rowLength != other.rowLength || this.ColumnLength != other.ColumnLength)
            {
                return false;
            }
            for (int i = 0; i < this.rowLength; i++)
            {
                for (int j = 0; j < this.ColumnLength; j++)
                {
                    if (this[i, j] != other[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + this.rowLength.GetHashCode();
            hash = hash * 31 + this.ColumnLength.GetHashCode();
            for (int i = 0; i < this.rowLength; i++)
            {
                for (int j = 0; j < this.ColumnLength; j++)
                {
                    hash = hash * 31 + this[i, j].GetHashCode();
                }
            }
            return hash;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.rowLength != b.rowLength || a.columnLength != b.columnLength)
            {
                throw new ArgumentException("Matrices must be of the same length");
            }
            int[,] newData = new int[a.rowLength, a.columnLength];
            for (int i = 0; i < a.rowLength; i++)
            {
                for (int j = 0; j < a.columnLength; j++)
                {
                    newData[i, j] = a[i, j] + b[i, j];
                }
            }
            return new Matrix(newData);
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.rowLength != b.rowLength || a.columnLength != b.columnLength)
            {
                throw new ArgumentException("Matrices must be of the same length");
            }
            int[,] newData = new int[a.rowLength, a.columnLength];
            for (int i = 0; i < a.rowLength; i++)
            {
                for (int j = 0; j < a.columnLength; j++)
                {
                    newData[i, j] = a[i, j] - b[i, j];
                }
            }
            return new Matrix(newData);
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.columnLength != b.rowLength)
            {
                throw new ArgumentException("Matrix dimensions are not compatible for multiplication");
            }
            int[,] newData = new int[a.rowLength, b.columnLength];
            for (int i = 0; i < a.rowLength; i++)
            {
                for (int j = 0; j < b.columnLength; j++)
                {
                    for (int k = 0; k < a.columnLength; k++)
                    {
                        newData[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return new Matrix(newData);
        }

        public static Matrix operator *(Matrix a, int scalar)
        {
            int[,] newData = new int[a.rowLength, a.columnLength];
            for (int i = 0; i < a.rowLength; i++)
            {
                for (int j = 0; j < a.columnLength; j++)
                {
                    newData[i, j] = a[i, j] * scalar;
                }
            }
            return new Matrix(newData);
        }

        public static Matrix operator *(int scalar, Matrix a)
        {
            return a * scalar;
        }

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            if (matrix.ColumnLength != vector.Length)
            {
                throw new ArgumentException("Matrix columns count must match vector length");
            }
            
            int rows = matrix.RowLength;
            int cols = matrix.ColumnLength;
            int length = vector.Length;

            int[] result = new int[rows];

            for (int i = 0; i < rows; i++)
            {
                int sum = 0;
                for (int j = 0; j < cols; j++)
                {
                    sum += matrix[i, j] * vector[j];
                }
                result[i] = sum;
            }

            return new Vector(result);
        }
    }

    internal class Vector
    {
        private int length;
        private int[] vector;

        public Vector(int[] vector)
        {
            length = vector.Length;
            this.vector = new int[length];
            for (int i = 0; i < length; i++)
            {
                this.vector[i] = vector[i];
            }
        }

        public int Length
        {
            get { return length; }
        }

        public int[] VectorData
        {
            get { return vector; }
        }

        public int this[int i]
        {
            get { return vector[i]; }
            set { vector[i] = value; }
        }

        public static bool operator ==(Vector a, Vector b)
        {
            if (a.length != b.length)
            {
                return false;
            }
            for (int i = 0; i < a.length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Vector other = (Vector)obj;
            if (length != other.length)
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                if (this[i] != other[i])
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < length; i++)
            {
                hash = hash * 31 + this[i].GetHashCode();
            }
            return hash;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            if (a.length != b.length)
            {
                throw new ArgumentException("Vectors must have the same length.");
            }
            int[] result = new int[a.length];
            for (int i = 0; i < a.length; i++)
            {
                result[i] = a[i] + b[i];
            }
            return new Vector(result);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a.length != b.length)
            {
                throw new ArgumentException("Vectors must have the same length.");
            }
            int[] result = new int[a.length];
            for (int i = 0; i < a.length; i++)
            {
                result[i] = a[i] - b[i];
            }
            return new Vector(result);
        }

        public static Vector operator *(Vector a, int b)
        {
            int[] result = new int[a.length];
            for (int i = 0; i < a.length; i++)
            {
                result[i] = a[i] * b;
            }
            return new Vector(result);
        }

        public static Vector operator *(int a, Vector b)
        {
            return b * a;
        }

        public static Vector operator *(Vector vector, Matrix matrix)
        {
            if (vector.Length != matrix.ColumnLength)
                throw new ArgumentException("The size of the vector must match the number of columns in the matrix.");

            int rows = matrix.RowLength;
            int columns = matrix.ColumnLength;

            int size = Math.Max(rows, columns);
            int[] result = new int[size];

            for (int i = 0; i < rows; i++)
            {
                int dotProduct = 0;
                for (int j = 0; j < columns; j++)
                {
                    dotProduct += matrix[i, j] * vector[j];
                }
                result[i] = dotProduct;
            }

            return new Vector(result);
        }
    }

    internal class Program
    {
        static int[,] MatrixInput()
        {
            Console.Write("Enter the number of rows");
            int rows = int.Parse(Console.ReadLine());
            Console.Write("Enter the number of columns");
            int columns = int.Parse(Console.ReadLine());
            int[,] matrix = new int[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.WriteLine($"Enter the element at position {i},{j}");
                    matrix[i, j] = int.Parse(Console.ReadLine());
                }
            }
            return matrix;
        }

        static int[] VectorInput()
        {
            Console.Write("Enter length of vector: ");
            int length = int.Parse(Console.ReadLine());
            int[] vector = new int[length];
            for (int i = 0; i < length; i++)
            {
                Console.WriteLine($"Enter element at position {i}");
                vector[i] = int.Parse(Console.ReadLine());
            }
            return vector;
        }

        static int ScalarInput()
        {
            Console.Write("Enter a scalar: ");
            int scalar = int.Parse(Console.ReadLine());
            return scalar;
        }
        
        static void Print(Matrix matrix)
        {
            for (int i = 0; i < matrix.RowLength; i++)
            {
                for (int j = 0; j < matrix.ColumnLength; j++)
                {
                    Console.Write(matrix[i,j].ToString() +  ' ');
                }
                Console.WriteLine();
            }
        }

        static void Print(Vector vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.Write(vector[i].ToString() + ' ');
            }
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            int[,] tempMatrix1 = { { 1, 1 }, { 1, 1 }, { 1, 1 } };
            int[,] tempMatrix2 = { { 1, 1 }, { 1, 1 }, { 1, 1 } };
            int[,] tempMatrix3 = { { 1, 1 }, { 1, 1 } };
            int[] tempVector1 = { 1, 2, 3 };
            int[] tempVector2 = { 4, 5, 6 };
            int[] tempVector3 = { 7, 8 };
            int scalar = ScalarInput();
            Matrix matrix1 = new Matrix(tempMatrix1);
            Matrix matrix2 = new Matrix(tempMatrix2);
            Matrix matrix3 = new Matrix(tempMatrix3);
            Vector vector1 = new Vector(tempVector1);
            Vector vector2 = new Vector(tempVector2);
            Vector vector3 = new Vector(tempVector3);
            
            Console.WriteLine("Matrix + Matrix:");
            Print(matrix1 + matrix2);
            Console.WriteLine();

            Console.WriteLine("Matrix - Matrix:");
            Print(matrix1 - matrix2);
            Console.WriteLine();

            Console.WriteLine("Matrix * Matrix:");
            Print(matrix1 * matrix2);
            Console.WriteLine();

            Console.WriteLine("Matrix * Number:");
            Print(matrix1 * 2);
            Console.WriteLine();

            Console.WriteLine("Number * Matrix:");
            Print(2 * matrix1);
            Console.WriteLine();

            Console.WriteLine("Matrix * Vector:");
            Print(matrix1 * vector3);
            Console.WriteLine();
            
            Console.WriteLine("Vector * Matrix:");
            Print(vector3 * matrix1);
            Console.WriteLine();

            Console.WriteLine("Vector + Vector:");
            Print(vector1 + vector2);
            Console.WriteLine();

            Console.WriteLine("Vector - Vector:");
            Print(vector1 - vector2);
            Console.WriteLine();

            Console.WriteLine("Vector * Number:");
            Print(vector1 * 2);
            Console.WriteLine();

            Console.WriteLine("Number * Vector:");
            Print(2 * vector1);
            Console.WriteLine();

            Console.WriteLine("Matrix == Matrix:");
            Console.WriteLine(matrix1 == matrix2);
            Console.WriteLine(matrix1 == matrix3);
            Console.WriteLine();

            Console.WriteLine("Matrix != Matrix:");
            Console.WriteLine(matrix1 != matrix2);
            Console.WriteLine(matrix1 != matrix3);
            Console.WriteLine();

            Console.WriteLine("Matrix Equals:");
            Console.WriteLine(matrix1.Equals(matrix2));
            Console.WriteLine(matrix1.Equals(matrix3));
            Console.WriteLine();

            Console.WriteLine("Matrix GetHashCode:");
            Console.WriteLine(matrix1.GetHashCode());
            Console.WriteLine(matrix2.GetHashCode());
            Console.WriteLine(matrix3.GetHashCode());
            Console.WriteLine();

            Console.WriteLine("Vector == Vector:");
            Console.WriteLine(vector1 == vector2);
            Console.WriteLine();

            Console.WriteLine("Vector != Vector:");
            Console.WriteLine(vector1 != vector2);
            Console.WriteLine();

            Console.WriteLine("Vector Equals:");
            Console.WriteLine(vector1.Equals(vector2));
            Console.WriteLine();

            Console.WriteLine("Vector GetHashCode:");
            Console.WriteLine(vector1.GetHashCode());
            Console.WriteLine(vector2.GetHashCode());
            Console.ReadKey();

        }
    }
}
