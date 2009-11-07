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
            this.Hours = new Rational(hours, 5);
            this.Minutes = new Rational(minutes, 5);
            this.Seconds = new Rational(seconds, 5);
        }

        public GpsRational(ulong[] data)
        {
            this.Hours = new Rational(data[0]);
            this.Minutes = new Rational(data[1]);
            this.Seconds = new Rational(data[2]);

            ////// Check if Seconds are part of Minutes
            ////if (this.Minutes.ToDouble() != Math.Floor(this.Minutes.ToDouble()))
            ////{
            ////    double minutes = Math.Floor(this.Minutes.ToDouble());
            ////    double seconds = (this.Minutes.ToDouble() - minutes) * 60;

            ////    this.Minutes = new Rational(minutes, 5);
            ////    this.Seconds = new Rational(seconds, 5);
            ////}
        }

        public Rational Hours
        {
            get;
            set;
        }

        public Rational Minutes
        {
            get;
            set;
        }

        public Rational Seconds
        {
            get;
            set;
        }

        public ulong[] ToUlongArray(bool secondsInMinutes)
        {
            List<ulong> returnValue = new List<ulong>();

            if (secondsInMinutes)
            {
                returnValue.Add(this.Hours.ToUlong());
                returnValue.Add(new Rational(this.Minutes.ToDouble() + (this.Seconds.ToDouble() / 60), 5).ToUlong());
                returnValue.Add(new Rational(0, 5).ToUlong());
            }
            else
            {
                returnValue.Add(this.Hours.ToUlong());
                returnValue.Add(this.Minutes.ToUlong());
                returnValue.Add(this.Seconds.ToUlong());
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
    }
}
