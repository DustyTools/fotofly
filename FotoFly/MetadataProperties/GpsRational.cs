// <copyright file="GpsRational.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>GpsRational</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GpsRational
    {
        public GpsRational(double hours, double minutes, double seconds)
        {
            this.Hours = new URational(hours, 5);
            this.Minutes = new URational(minutes, 5);
            this.Seconds = new URational(seconds, 5);
        }

        public GpsRational(UInt64[] data)
        {
            this.Hours = new URational(data[0]);
            this.Minutes = new URational(data[1]);
            this.Seconds = new URational(data[2]);

            ////// Check if Seconds are part of Minutes
            ////if (this.Minutes.ToDouble() != Math.Floor(this.Minutes.ToDouble()))
            ////{
            ////    double minutes = Math.Floor(this.Minutes.ToDouble());
            ////    double seconds = (this.Minutes.ToDouble() - minutes) * 60;

            ////    this.Minutes = new Rational(minutes, 5);
            ////    this.Seconds = new Rational(seconds, 5);
            ////}
        }

        public URational Hours
        {
            get;
            set;
        }

        public URational Minutes
        {
            get;
            set;
        }

        public URational Seconds
        {
            get;
            set;
        }

        public ulong[] ToUlongArray(bool secondsInMinutes)
        {
            List<ulong> returnValue = new List<ulong>();

            if (secondsInMinutes)
            {
                returnValue.Add(this.Hours.ToUInt64());
                returnValue.Add(new URational(this.Minutes.ToDouble() + (this.Seconds.ToDouble() / 60), 5).ToUInt64());
                returnValue.Add(new URational(0, 5).ToUInt64());
            }
            else
            {
                returnValue.Add(this.Hours.ToUInt64());
                returnValue.Add(this.Minutes.ToUInt64());
                returnValue.Add(this.Seconds.ToUInt64());
            }

            return returnValue.ToArray();
        }

        public double ToDouble()
        {
            double numeric = this.Hours.ToDouble() + (this.Minutes.ToDouble() / 60) + (this.Seconds.ToDouble() / 3600);

            return Math.Round(numeric, 4);
        }

        public override string ToString()
        {
            return this.Hours.ToDouble() + " " + this.Minutes.ToDouble() + " " + this.Seconds.ToDouble() + " ";
        }

        public string ToUnformattedString()
        {
            return this.Hours.ToUnformattedString() + " " + this.Minutes.ToUnformattedString() + " " + this.Seconds.ToUnformattedString();
        }
    }
}
