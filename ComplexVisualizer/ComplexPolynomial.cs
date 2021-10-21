using System;
using System.Numerics;

namespace ComplexVisualizer
{
    class ComplexPolynomial
    {
        private Complex[] coefficients;

        public int Degree { get; private set; }

        public ComplexPolynomial(Complex[] coefficients)
        {
            if (coefficients == null || coefficients.Length == 0) {
                throw new ArgumentNullException(nameof(coefficients));
            }

            this.coefficients = coefficients;
            Degree = coefficients.Length - 1;
        }

        public ComplexPolynomial Derivative()
        {
            if (Degree == 0)
            {
                throw new InvalidOperationException("Cannot get a derivative of a constant");
            }

            var coefficients = new Complex[Degree];

            for (int i = 0; i < Degree; i++)
            {
                coefficients[i] = this.coefficients[i] * (Degree - i);
            }

            return new ComplexPolynomial(coefficients);
        }

        public Complex Calculate(Complex z)
        {
            var result = new Complex();

            for (int i = 0; i < Degree; i++)
            {
                result += coefficients[i] * Complex.Pow(z, Degree - i);
            }

            // Add constant
            result += coefficients[Degree];

            return result;
        }
    }
}
