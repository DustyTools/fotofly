// <copyright file="Rational.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>Rational</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Rational
    {
        private int numerator;
        private int denominator;

        /// <summary>
        /// Creates an Exif Rational
        /// </summary>
        /// <param name="numerator">The value you want to store in the Rational</param>
        /// <param name="accuracy">The number of decimal places of accuracy</param>
        public Rational(double numerator, int accuracy)
        {
            accuracy = (int)Math.Pow(10, accuracy);

            this.numerator = Convert.ToInt32(Math.Abs(numerator * accuracy));
            this.denominator = accuracy;
        }

        /// <summary>
        /// Creates an Exif Rational
        /// </summary>
        /// <param name="data">A ulong typically read from exif metadata</param>
        public Rational(Int64 data)
        {
            this.numerator = (int)(data & 0xFFFFFFFFL);
            this.denominator = (int)(((ulong)data & 0xFFFFFFFF00000000L) >> 32);
        }

        public int Numerator
        {
            get { return this.numerator; }
        }

        public int Denominator
        {
            get { return this.denominator; }
        }

        /// <summary>
        /// Returns the Rational as a Ulong, typically used to write back to exif metadata
        /// </summary>
        /// <returns>Ulong</returns>
        public Int64 ToInt64()
        {
            return (Int64)((Int64)this.numerator) | (((Int64)this.denominator) << 32);
        }

        /// <summary>
        /// Returns the Rational as an Integer
        /// </summary>
        /// <returns>Int</returns>
        public int ToInt()
        {
            return Convert.ToInt32(Math.Round(Convert.ToDouble(this.numerator) / Convert.ToDouble(this.denominator)));
        }

        /// <summary>
        /// Returns the Rational as a Double
        /// </summary>
        /// <returns>Double, accurate to four decimal places</returns>
        public double ToDouble()
        {
            return Math.Round(Convert.ToDouble(this.numerator) / Convert.ToDouble(this.denominator), 4);
        }

        /// <summary>
        /// Returns the Rational as a string
        /// </summary>
        /// <returns>A string in the format numerator/denominator</returns>
        public new string ToString()
        {
            return this.numerator.ToString() + "/" + this.denominator.ToString();
        }

        public string ToUnformattedString()
        {
            return this.numerator.ToString() + " / " + this.denominator.ToString() + " (" + this.ToDouble() + ")";
        }
    }
}
