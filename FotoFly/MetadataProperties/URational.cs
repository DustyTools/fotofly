// <copyright file="URational.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>URational</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class URational
    {
        private int numerator;
        private int denominator;

        public URational(UInt64 data)
        {
            this.numerator = (int)(data & 0xFFFFFFFFL);
            this.denominator = (int)((data & 0xFFFFFFFF00000000L) >> 32);
        }

        public URational(double numerator, int accuracy)
        {
            accuracy = (int)Math.Pow(10, accuracy);

            this.numerator = Convert.ToInt32(Math.Abs(numerator * accuracy));
            this.denominator = accuracy;
        }

        public double ToDouble()
        {
            return Math.Round(Convert.ToDouble(this.numerator) / Convert.ToDouble(this.denominator), 3);
        }

        public double ToDouble(int decimalPlaces)
        {
            return Math.Round(Convert.ToDouble(this.numerator) / Convert.ToDouble(this.denominator), decimalPlaces);
        }

        /// <summary>
        /// Returns the Rational as a Ulong, typically used to write back to exif metadata
        /// </summary>
        /// <returns>Ulong</returns>
        public ulong ToUInt64()
        {
            return ((ulong)this.numerator) | (((ulong)this.denominator) << 32);
        }

        /// <summary>
        /// Returns the Rational as an Integer
        /// </summary>
        /// <returns>Int</returns>
        public int ToInt()
        {
            return Convert.ToInt32(Math.Round(Convert.ToDouble(this.numerator) / Convert.ToDouble(this.denominator)));
        }

        public byte[] ToByteArray()
        {
            return null;
        }

        public override string ToString()
        {
            return this.numerator.ToString() + "/" + this.denominator.ToString();
        }

        public string ToUnformattedString()
        {
            return this.numerator.ToString() + " / " + this.denominator.ToString() + " (" + this.ToDouble() + ")";
        }
    }
}
