namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class URational
    {
        private uint numerator;
        private uint denominator;

        public URational(double numeric)
        {
        }

        public URational(ulong data)
        {
            this.numerator = (uint)(data & 0xFFFFFFFFL);
            this.denominator = (uint)((data & 0xFFFFFFFF00000000L) >> 32);
        }

        public double ToDouble()
        {
            return Math.Round(Convert.ToDouble(this.numerator) / Convert.ToDouble(this.denominator), 3);
        }

        public double ToDouble(int decimalPlaces)
        {
            return Math.Round(Convert.ToDouble(this.numerator) / Convert.ToDouble(this.denominator), decimalPlaces);
        }

        public byte[] ToByteArray()
        {
            return null;
        }

        public override string ToString()
        {
            return this.numerator.ToString() + "/" + this.denominator.ToString();
        }
    }
}
