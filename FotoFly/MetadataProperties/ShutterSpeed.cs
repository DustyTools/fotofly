// <copyright file="ShutterSpeed.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-25</date>
// <summary>ShutterSpeed</summary>
namespace Fotofly
{
    using System;
    using System.Text;
    using System.Xml.Serialization;

    [XmlRootAttribute("ShutterSpeed", Namespace = "http://www.tassography.com/fotofly")]
    public class ShutterSpeed
    {
        private readonly int millisecondsInSecond = 1000;

        public ShutterSpeed()
        {
            this.Seconds = double.NaN;
        }

        public ShutterSpeed(string shutterSpeed)
        {
            // Expected format is {numerator}/{denominator}
            string[] splitString = shutterSpeed.Split('/');

            if (splitString.Length == 2)
            {
                URational urational = new URational(Convert.ToInt32(splitString[0]), Convert.ToInt32(splitString[1]));

                this.Seconds = urational.ToDouble();
            }
            else
            {
                throw new ArgumentException("Shutterspeed was not of expected format:" + shutterSpeed);
            }
        }

        public ShutterSpeed(double seconds)
        {
            this.Seconds = seconds;
        }

        public ShutterSpeed(URational urational)
        {
            // Use 6 decimal places
            this.Seconds = urational.ToDouble(6);
        }

        [XmlAttribute]
        public double Seconds
        {
            get;
            set;
        }

        public bool IsValid
        {
            get
            {
                return !double.IsNaN(this.Seconds);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ShutterSpeed)
            {
                if ((obj as ShutterSpeed).ToString() == this.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            string formattedString = string.Empty;

            if (this.Seconds > 1)
            {
                formattedString = this.Seconds.ToString();
            }
            else
            {
                // Convert Decimal to Integer
                formattedString = Math.Round(1 / this.Seconds, 0).ToString();

                formattedString = "1/" + formattedString;
            }

            if (formattedString != String.Empty)
            {
                formattedString += " sec.";
            }

            return formattedString;
        }
    }
}