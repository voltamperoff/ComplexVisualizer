using System;
using System.Numerics;

namespace ComplexVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 1920;
            int height = 1080;
            double ratio = (double)width / (double)height;

            var center = new Complex(0.0, 0.0);
            var radius = 2.5;
            
            var r = new Area(center, radius, width, height);

            var coefficients = new Complex[]
            {
                new Complex(2.0, -0.1),
                new Complex(1.5, -1.5),
                new Complex(1.0, -3.0),
                new Complex(1.2, -1.3),
            };

            var polynomial = new Polynomial(coefficients);
            var derivative = polynomial.Derivative();

            Complex Newton(Complex z)
            {
                for (int i = 0; i < 50; i++)
                {
                    z -= polynomial.Calculate(z) / derivative.Calculate(z);
                }

                return z;
            }

            r.Calculate(Newton);

            string format = "yy-MM-dd-HH-mm-ss";
            r.SaveImage($"D:\\test-{DateTime.Now.ToString(format)}.png");
        }
    }
}
