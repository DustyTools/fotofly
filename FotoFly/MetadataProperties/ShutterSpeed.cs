// <copyright file="ShutterSpeed.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-25</date>
// <summary>ShutterSpeed</summary>
namespace Fotofly
{
    using System;
    using System.Text;

    public class ShutterSpeed
    {
        private TimeSpan timespan;

        public ShutterSpeed(string shutterSpeed)
        {
            // Expected format is {numerator}/{denominator}
            string[] splitString = shutterSpeed.Split('/');

            if (splitString.Length == 2)
            {
                URational urational = new URational(Convert.ToInt32(splitString[0]), Convert.ToInt32(splitString[1]));

                int seconds = (int)Math.Floor(urational.ToDouble());
                int milliseconds = (int)((urational.ToDouble() - seconds) * 1000);

                this.timespan = new TimeSpan(0, 0, 0, urational.ToInt(), milliseconds);
            }
            else
            {
                throw new ArgumentException("Shutterspeed was not of expected format:" + shutterSpeed);
            }
        }

        public ShutterSpeed(int numerator, int denominator)
        {
            URational urational = new URational(numerator, denominator);

            int seconds = (int)Math.Floor(urational.ToDouble());
            int milliseconds = (int)((urational.ToDouble() - seconds) * 1000);

            this.timespan = new TimeSpan(0, 0, 0, urational.ToInt(), milliseconds);
        }

        public ShutterSpeed(URational urational)
        {
            int seconds = (int)Math.Floor(urational.ToDouble());
            int milliseconds = (int)((urational.ToDouble() - seconds) * 1000);

            this.timespan = new TimeSpan(0, 0, 0, urational.ToInt(), milliseconds);
        }

        public TimeSpan Timespan
        {
          get
          {
              return this.timespan;
          }
        }

        public override bool Equals(object obj)
        {
            if (obj is ShutterSpeed)
            {
                if ((obj as ShutterSpeed).Timespan.Ticks == this.Timespan.Ticks)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            if (this.timespan.Ticks > 0)
            {
                if (this.timespan.Seconds > 1)
                {
                    return this.timespan.Seconds + " sec.";
                }
                else
                {
                    return "1/" + (this.timespan.Milliseconds * 1000) + " sec.";
                }
            }

            return string.Empty;
        }
    }
}