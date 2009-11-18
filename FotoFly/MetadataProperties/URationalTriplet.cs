// <copyright file="URationalTriplet.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>URationalTriplet</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class URationalTriplet
    {
        public URationalTriplet(double a, double b, double c)
        {
            this.A = new URational(a, 5);
            this.B = new URational(b, 5);
            this.C = new URational(c, 5);
        }

        public URationalTriplet(UInt64[] data)
        {
            this.A = new URational(data[0]);
            this.B = new URational(data[1]);
            this.C = new URational(data[2]);

            ////// Check if Seconds are part of Minutes
            ////if (this.Minutes.ToDouble() != Math.Floor(this.Minutes.ToDouble()))
            ////{
            ////    double minutes = Math.Floor(this.Minutes.ToDouble());
            ////    double seconds = (this.Minutes.ToDouble() - minutes) * 60;

            ////    this.Minutes = new Rational(minutes, 5);
            ////    this.Seconds = new Rational(seconds, 5);
            ////}
        }

        public URational A
        {
            get;
            set;
        }

        public URational B
        {
            get;
            set;
        }

        public URational C
        {
            get;
            set;
        }

        public ulong[] ToUlongArray(bool cAsPartOfB)
        {
            List<ulong> returnValue = new List<ulong>();

            if (cAsPartOfB)
            {
                returnValue.Add(this.A.ToUInt64());
                returnValue.Add(new URational(this.B.ToDouble() + (this.C.ToDouble() / 60), 5).ToUInt64());
                returnValue.Add(new URational(0, 5).ToUInt64());
            }
            else
            {
                returnValue.Add(this.A.ToUInt64());
                returnValue.Add(this.B.ToUInt64());
                returnValue.Add(this.C.ToUInt64());
            }

            return returnValue.ToArray();
        }

        public double ToDouble()
        {
            double numeric = this.A.ToDouble() + (this.B.ToDouble() / 60) + (this.C.ToDouble() / 3600);

            return Math.Round(numeric, 4);
        }

        public override string ToString()
        {
            return this.ToDoubleString();
        }

        public string ToDoubleString()
        {
            return this.A.ToDouble() + " " + this.B.ToDouble() + " " + this.C.ToDouble() + " ";
        }

        public string ToFractionString()
        {
            return this.A.ToFractionString() + " " + this.B.ToFractionString() + " " + this.C.ToFractionString();
        }
    }
}
