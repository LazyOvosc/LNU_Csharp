using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Homework1
{
    internal class Program
    {

        static void PrintMatrix(int[,] matr)
        {
            double length = Math.Sqrt(matr.Length);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.Write(matr[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            Console.Write("Enter size of array: ");
            int size = int.Parse(Console.ReadLine());

            int[,] matrix = new int[size,size];

            Random rand = new Random();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = rand.Next(-100, 100);
                }
            }

            //Second Part
            int[,] secondMatrix = new int[size, size];
            int x = 0, y = 0;
            for (int k = 0; k < size; k++)
            {
                for (int l = 0; l < size; l++)
                {
                    if (matrix[k, l] > 0)
                    {
                        secondMatrix[y, x] = matrix[k, l];
                        x++;
                        if (x == size)
                        {
                            y++;
                            x = 0;
                        }
                    }
                }
            }
            for (int k = 0; k < size; k++)
            {
                for (int l = 0; l < size; l++)
                {
                    if (matrix[k, l] < 0)
                    {
                        secondMatrix[y, x] = matrix[k, l];
                        x++;
                        if (x == size)
                        {
                            y++;
                            x = 0;
                        }
                    }
                }
            }

            
            PrintMatrix(matrix);
            Console.WriteLine();
            PrintMatrix(secondMatrix);
           
            Console.ReadKey();
        }

        
    }
}
