using System;
using System.Drawing;
using System.Numerics;

namespace ComplexVisualizer
{
    class ResultsArray
    {
        private readonly Complex[,] results;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public ResultsArray(int width, int height)
        {
            Width = width;
            Height = height;

            results = new Complex[Width, Height];
        }

        private bool isSetUp = false;
        private Complex a;
        private Complex b;
        private Func<Complex, Complex> function;

        public void Setup(Complex a, Complex b, Func<Complex, Complex> function)
        {
            this.a = a;
            this.b = b;
            this.function = function;

            isSetUp = true;
        }

        public void Calculate()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    double real = ((double)x / (double)Width) * (b.Real - a.Real) + a.Real;
                    double imaginary = ((double)y / (double)Height) * (b.Imaginary - a.Imaginary) + a.Imaginary;

                    results[x, y] = function(new Complex(real, imaginary));
                }
            }
        }

        public Color MapComplexToColor(Complex z)
        {
            var phi = z.Phase;
            var arc = 2.0 * Math.PI / 3.0;

            phi = phi >= 0.0 ? phi : phi + 2.0 * Math.PI;

            double dr = 0, dg = 0, db = 0;

            if (phi >= 0.0 * arc && phi < 1.0 * arc)
            {
                dg = (phi - 0.0 * arc) / arc;
                dr = 1.0 - dg;
            }

            if (phi >= 1.0 * arc && phi < 2.0 * arc)
            {
                db = (phi - 1.0 * arc) / arc;
                dg = 1 - db;
            }

            if (phi >= 2.0 * arc && phi < 3.0 * arc)
            {
                dr = (phi - 2.0 * arc) / arc;
                db = 1 - dr;
            }

            int r = Convert.ToInt32(255.0 * dr);
            int g = Convert.ToInt32(255.0 * dg);
            int b = Convert.ToInt32(255.0 * db);

            return Color.FromArgb(r, g, b);
        }

        public void SaveImage(string path)
        {
            var image = new Bitmap(Width, Height);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    image.SetPixel(x, y, MapComplexToColor(results[x, y]));
                }
            }

            image.Save(path);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var r = new ResultsArray(800, 800);
            var x = 3.0;
            var d = new Complex(0.0, 0.0);
            var a = new Complex(-x, x) + d;
            var b = new Complex(x, -x) + d;

            var coefficients = new Complex[]
            {
                new Complex(2.0, -0.1),
                new Complex(1.5, -1.5),
                new Complex(1.0, -3.0),
                new Complex(0.0, -1.3),
            };

            var polynomial = new ComplexPolynomial(coefficients);
            var derivative = polynomial.Derivative();

            Complex Newton(Complex z)
            {
                for (int i = 0; i < 50; i++)
                {
                    z -= polynomial.Calculate(z) / derivative.Calculate(z);
                }

                return z;
            }

            r.Setup(a, b, Newton);

            r.Calculate();

            string format = "yy-MM-dd-HH-mm-ss";
            r.SaveImage($"D:\\test-{DateTime.Now.ToString(format)}.png");
        }
    }
}
