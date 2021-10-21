using System;
using System.Drawing;
using System.Numerics;

namespace ComplexVisualizer
{
    class Area
    {
        private readonly Complex[,] values;
        private readonly Complex a;
        private readonly Complex b;

        public int Width { get; private set; }
        public int Height { get; private set; }

        #region Constructors
        public Area(Complex a, Complex b, int width, int height)
        {
            if (a == b)
            {
                throw new ArgumentException("Area on the complex plane cannot be a single point");
            }

            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("Area resolution cannot be zero");
            }

            Width = width;
            Height = height;

            values = new Complex[Width, Height];

            this.a = a;
            this.b = b;
        }

        public Area(Complex center, double radius, int width, int height)
        {
            if (radius == 0.0)
            {
                throw new ArgumentException("Radius cannot be zero");
            }

            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("Area resolution cannot be zero");
            }

            Width = width;
            Height = height;

            values = new Complex[Width, Height];

            double ratio = (double)width / (double)height;

            a = new Complex(-radius * ratio, radius) + center;
            b = new Complex(radius * ratio, -radius) + center;
        }
        #endregion

        public void Calculate(Func<Complex, Complex> function)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    double real = ((double)x / (double)Width) * (b.Real - a.Real) + a.Real;
                    double imaginary = ((double)y / (double)Height) * (b.Imaginary - a.Imaginary) + a.Imaginary;

                    values[x, y] = function(new Complex(real, imaginary));
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
                    image.SetPixel(x, y, MapComplexToColor(values[x, y]));
                }
            }

            image.Save(path);
        }
    }
}
