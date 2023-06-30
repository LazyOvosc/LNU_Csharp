using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    internal class Point
    {
        private double _x;
        private double _y;

        public Point(double y, double x)
        {
            _x = x;
            _y = y;
        }
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
    }

    internal interface IArea
    {
        double Area();
    }

    internal class Square: IArea
    {
        private double _side;

        public Square(double side)
        {
            _side = side;
        }

        public double Side
        {
            get { return _side; }
            set { _side = value; }
        }

        public double Area()
        {
            return _side * _side;
        }
    }

    internal class Triangle: IArea
    {
        private double _side1;
        private double _side2;
        private double _side3;

        public Triangle(double side1, double side2, double side3)
        {
            _side1 = side1;
            _side2 = side2;
            _side3 = side3;
        }


        public double Area()
        {
            double perimeter_2 = (_side1 + _side2 + _side3) / 2;
            double area = Math.Sqrt(perimeter_2 * (perimeter_2 - _side1) * (perimeter_2 - _side2) * (perimeter_2 - _side3));
            return area;
        }
    }

    internal class Circle : IArea, ICloneable, IComparable<Circle>
    {
        private Point _center;
        private double _radius;

        public Circle(Point center, double radius)
        {
            _center = center;
            _radius = radius;
        }

        public double Area()
        {
            return Math.PI * _radius * _radius;
        }

        public object Clone()
        {
            Point point = new Point(_center.X, _center.Y);
            return new Circle(point, _radius);
        }

        public int CompareTo(Circle other)
        {
            if (other == null)
            {
                return 1;
            }

            return Area().CompareTo(other.Area());
        }

        public override string ToString()
        {
            return $"Circle with radius {_radius:F2} and area {Area():F2}";
        }
    }

    internal class Program
    {
        static public void SumArea(List<IArea> array)
        {
            double sum = 0;
            foreach(IArea figure in array)
            {
                sum += figure.Area();
            }
            Console.WriteLine($"Area of all figures is {sum}");
        }

        static void CircleListSort(List<Circle> circleList)
        {
            circleList.Sort();
            Console.WriteLine("Sorted circles:");
            foreach (Circle circle in circleList)
            {
                Console.WriteLine(circle.ToString());
            }

        }
        static void Main(string[] args)
        {
            List<IArea> figureArray = new List<IArea>();
            figureArray.Add(new Triangle(3, 4, 5));
            figureArray.Add(new Circle(new Point(1, 2), 5));
            figureArray.Add(new Square(4));
            SumArea(figureArray);
            Console.WriteLine();

            List<Circle> circleList = new List<Circle>();
            circleList.Add(new Circle(new Point(1, 2), 5));
            circleList.Add(new Circle(new Point(3, 4), 3));
            circleList.Add(new Circle(new Point(5, 6), 4));

            CircleListSort(circleList);
            Console.WriteLine();

            Circle smallestCircle = (Circle)circleList[0].Clone();
            Console.WriteLine($"Clone of smallest circle: {smallestCircle.ToString()}");
        }
    }
}
