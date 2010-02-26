// <copyright file="AbstractRational.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-17</date>
// <summary>AbstractRational</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class AbstractRational
    {
        public AbstractRational()
        {
        }

        /// <summary>
        /// Creates an Exif Rational
        /// </summary>
        /// <param name="numerator">The value you want to store in the Rational</param>
        /// <param name="accuracy">The number of decimal places of accuracy</param>
        public AbstractRational(double numerator, int accuracy)
        {
            accuracy = (int)Math.Pow(10, accuracy);

            this.Numerator = Convert.ToInt32(Math.Abs(numerator * accuracy));
            this.Denominator = accuracy;
        }

        /// <summary>
        /// Creates an Exif Rational
        /// </summary>
        /// <param name="numerator">The numerator</param>
        /// <param name="denonimator">The denominator</param>
        public AbstractRational(int numerator, int denominator)
        {
            this.Numerator = numerator;
            this.Denominator = denominator;
        }

        public int Numerator
        {
            get;
            set;
        }

        public int Denominator
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the Rational as a Double
        /// </summary>
        /// <returns>Double, accurate to four decimal places</returns>
        public double ToDouble()
        {
            return this.ToDouble(4);
        }

        public double ToDouble(int decimalPlaces)
        {
            return Math.Round(Convert.ToDouble(this.Numerator) / Convert.ToDouble(this.Denominator), decimalPlaces);
        }

        /// <summary>
        /// Returns the Rational as an Integer
        /// </summary>
        /// <returns>Int</returns>
        public int ToInt()
        {
            if (this.Numerator != 0 && this.Numerator != 0)
            {
                return Convert.ToInt32(Math.Round(Convert.ToDouble(this.Numerator) / Convert.ToDouble(this.Denominator)));
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns the Rational as a Ulong, typically used to write back to exif metadata
        /// </summary>
        /// <returns>Ulong</returns>
        public ulong ToUInt64()
        {
            return ((ulong)this.Numerator) | (((ulong)this.Denominator) << 32);
        }

        public string ToDoubleString()
        {
            return this.ToDouble().ToString();
        }

        public string ToFractionString()
        {
            return this.Numerator.ToString() + " / " + this.Denominator.ToString() + " (" + this.ToDouble() + ")";
        }

        /// <summary>
        /// Returns the Rational as a string
        /// </summary>
        /// <returns>A string in the format numerator/denominator</returns>
        public new string ToString()
        {
            return this.ToFractionString();
        }
    }
}