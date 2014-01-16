// <copyright file="GpsCoordinates.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>GpsCoordinate Class</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
	using System.Globalization;

    [XmlRootAttribute("GpsCoordinate", Namespace = "http://www.tassography.com/fotofly")]
    public class GpsCoordinate : ICloneable
    {
        public static readonly int MaxLatitude = 90;
        public static readonly int MinLatitude = -90;
        public static readonly int MaxLongitude = 180;
        public static readonly int MinLongitude = -180;

        private LatOrLons latOrLon;

        public GpsCoordinate()
        {
            this.Numeric = double.NaN;
            this.latOrLon = LatOrLons.NotSpecified;
        }

        public GpsCoordinate(GpsCoordinate.LatOrLons latOrLon)
        {
            this.Numeric = double.NaN;
            this.latOrLon = latOrLon;
        }

        public GpsCoordinate(GpsCoordinate.LatOrLons latOrLon, double numeric)
        {
            this.Numeric = numeric;
            this.latOrLon = latOrLon;
        }

        public GpsCoordinate(GpsCoordinate.LatitudeRef latitudeRef, double numeric)
        {
            if (Math.Abs(numeric) != numeric)
            {
                throw new Exception("Numeric should only be positive, use reference to specific direction");
            }

            this.latOrLon = LatOrLons.Latitude;
            this.Numeric = latitudeRef == LatitudeRef.North ? numeric : -numeric;
        }

        public GpsCoordinate(GpsCoordinate.LongitudeRef longitudeRef, double numeric)
        {
            if (Math.Abs(numeric) != numeric)
            {
                throw new Exception("Numeric should only be positive, use reference to specific direction");
            }

            this.latOrLon = LatOrLons.Longitude;
            this.Numeric = longitudeRef == LongitudeRef.East ? numeric : -numeric;
        }

        public GpsCoordinate(GpsCoordinate.LatitudeRef latitudeRef, double degrees, double minutes)
        {
            this.latOrLon = LatOrLons.Latitude;
            this.SetCoordinate(degrees, minutes, 0);

            this.Numeric = latitudeRef == LatitudeRef.North ? this.Numeric : -this.Numeric;
        }

        public GpsCoordinate(GpsCoordinate.LongitudeRef longitudeRef, double degrees, double minutes)
        {
            this.latOrLon = LatOrLons.Longitude;
            this.SetCoordinate(degrees, minutes, 0);

            this.Numeric = longitudeRef == LongitudeRef.East ? this.Numeric : -this.Numeric;
        }

        public GpsCoordinate(GpsCoordinate.LatitudeRef latitudeRef, double degrees, double minutes, double seconds)
        {
            this.latOrLon = LatOrLons.Latitude;

            int roundTo = 0;
            this.SetCoordinate(Math.Round(degrees, roundTo), Math.Round(minutes, roundTo), Math.Round(seconds, roundTo));

            this.Numeric = latitudeRef == LatitudeRef.North ? this.Numeric : -this.Numeric;
        }

        public GpsCoordinate(GpsCoordinate.LongitudeRef longitudeRef, double degrees, double minutes, double seconds)
        {
            this.latOrLon = LatOrLons.Longitude;
            this.SetCoordinate(degrees, minutes, seconds);

            this.Numeric = longitudeRef == LongitudeRef.East ? this.Numeric : -this.Numeric;
        }

        public enum LatOrLons
        {
            NotSpecified,
            Latitude,
            Longitude
        }

        public enum LatitudeRef
        {
            NotSpecified,
            North,
            South
        }

        public enum LongitudeRef
        {
            NotSpecified,
            East,
            West
        }

        [XmlAttribute]
        public double Numeric
        {
            get;
            set;
        }

        [XmlIgnore]
        public string DegreesMinutesSeconds
        {
            get
            {
                // Formats the string in the format E 106° 49' 32.9"
                if (this.IsValidCoordinate)
                {
                    double degrees;
                    double minutes;
                    double seconds;

                    this.CalculateDegreesMinutesSeconds(out degrees, out minutes, out seconds);

                    StringBuilder dms = new StringBuilder();
                    dms.Append(this.Ref);
                    dms.Append(" ");
                    dms.Append(this.AddPaddingZeros(degrees, 3, 0));
                    dms.Append("° ");
                    dms.Append(this.AddPaddingZeros(minutes, 2, 0));
                    dms.Append("' ");
                    dms.Append(this.AddPaddingZeros(seconds, 2, 2));
                    dms.Append('"');

                    return dms.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        [XmlIgnore]
        public bool IsValidCoordinate
        {
            get
            {
                if (double.IsNaN(this.Numeric))
                {
                    // Not a valid coordinate
                    return false;
                }
                else if (this.LatOrLon == LatOrLons.Latitude && (this.Numeric > GpsCoordinate.MaxLatitude || this.Numeric < GpsCoordinate.MinLatitude))
                {
                    // Invalid Latitude
                    return false;
                }
                else if (this.LatOrLon == LatOrLons.Longitude && (this.Numeric > GpsCoordinate.MaxLongitude || this.Numeric < GpsCoordinate.MinLongitude))
                {
                    // Invalid Longitude
                    return false;
                }

                // Valid Coordinate
                return true;
            }
        }

        [XmlIgnore]
        public LatOrLons LatOrLon
        {
            get
            {
                return this.latOrLon;
            }
        }

        [XmlIgnore]
        public char Ref
        {
            // ASCII 'E' indicates east longitude, and 'W' is west longitude
            // ASCII 'N' indicates north latitude, and 'S' is south latitude
            get
            {
                if (this.LatOrLon == LatOrLons.Latitude && Math.Abs(this.Numeric) == this.Numeric)
                {
                    return 'N';
                }
                else if (this.LatOrLon == LatOrLons.Latitude && Math.Abs(this.Numeric) != this.Numeric)
                {
                    return 'S';
                }
                else if (this.LatOrLon == LatOrLons.Longitude && Math.Abs(this.Numeric) == this.Numeric)
                {
                    return 'E';
                }
                else
                {
                    return 'W';
                }
            }

            set
            {
                value = value.ToString().ToUpper().ToCharArray()[0];

                if (value == 'N' || value == 'E')
                {
                    this.Numeric = Math.Abs(this.Numeric);
                }
                else if (value == 'S' || value == 'W')
                {
                    this.Numeric = -Math.Abs(this.Numeric);
                }
                else
                {
                    throw new Exception("Invalid Reference");
                }
            }
        }

        [XmlIgnore]
        public int Degrees
        {
            get
            {
                double degrees;
                double minutes;
                double seconds;

                this.CalculateDegreesMinutesSeconds(out degrees, out minutes, out seconds);

                return (int)degrees;
            }
        }

        [XmlIgnore]
        public int Minutes
        {
            get
            {
                double degrees;
                double minutes;
                double seconds;

                this.CalculateDegreesMinutesSeconds(out degrees, out minutes, out seconds);

                return (int)minutes;
            }
        }

        [XmlIgnore]
        public double Seconds
        {
            get
            {
                double degrees;
                double minutes;
                double seconds;

                this.CalculateDegreesMinutesSeconds(out degrees, out minutes, out seconds);

                return seconds;
            }
        }

        public override bool Equals(object unknownObject)
        {
            if (unknownObject is GpsCoordinate)
            {
                GpsCoordinate otherCoor = unknownObject as GpsCoordinate;

                return otherCoor.Degrees == this.Degrees && otherCoor.Minutes == this.Minutes && otherCoor.Seconds == this.Seconds;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return this.Numeric.ToString(NumberFormatInfo.InvariantInfo);
        }

        public object Clone()
        {
            return new GpsCoordinate(this.LatOrLon, this.Numeric);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private void SetCoordinate(double degrees, double minutes, double seconds)
        {
            if (Math.Abs(degrees) != degrees || Math.Abs(minutes) != minutes || Math.Abs(seconds) != seconds)
            {
                throw new Exception(@"Degrees\Minutes\Seconds should only be positive, use reference to specific direction");
            }
            else if (double.IsNaN(degrees) || double.IsNaN(minutes) || double.IsNaN(seconds))
            {
                this.Numeric = double.NaN;
            }
            else
            {
                // Example 32° 56' 26.57" is 32 + (56/60) + (26.57 / 3600) = 32.940713888
				this.Numeric = Math.Round(degrees + (Convert.ToDouble(minutes, NumberFormatInfo.InvariantInfo) / 60) + (Convert.ToDouble(seconds, NumberFormatInfo.InvariantInfo) / 3600), 4);

                // If this isn't valid reset the value
                if (!this.IsValidCoordinate)
                {
                    this.Numeric = double.NaN;
                }
            }
        }

        private void CalculateDegreesMinutesSeconds(out double degrees, out double minutes, out double seconds)
        {
            if (!this.IsValidCoordinate)
            {
                degrees = double.NaN;
                minutes = double.NaN;
                seconds = double.NaN;
            }
            else
            {
                double remainder = Math.Abs(this.Numeric);

                // Calculate Degrees, everything to the left of the decimal
                degrees = (int)Math.Floor(remainder);

                // Calculate the Minutes, remove the Degrees, then divide by 60 and round down
                remainder = remainder - degrees;

                minutes = (int)Math.Floor(remainder * 60);

                // Calculate the Seconds, remove the Minutes
                seconds = Math.Round(Convert.ToDouble((remainder * 3600) - (minutes * 60)));

                // Fix rounding issues
                // If Seconds exceeds 60
                if (seconds >= 60)
                {
                    minutes = minutes + 1;
                    seconds = seconds - 60;
                }

                // If Minutes exceeds 60
                if (minutes >= 60)
                {
                    degrees = degrees + 1;
                    minutes = minutes - 60;
                }
            }
        }

        private string AddPaddingZeros(double number, int leading, int trailing)
        {
            StringBuilder returnValue = new StringBuilder();

            string input = Math.Abs(number).ToString(NumberFormatInfo.InvariantInfo);

            string[] inputArray = input.Split('.');

            leading = leading - inputArray[0].Length;

            while (leading > 0)
            {
                returnValue.Append("0");
                leading--;
            }

            returnValue.Append(inputArray[0]);

            if (trailing > 0)
            {
                returnValue.Append(".");

                if (inputArray.Length == 2)
                {
                    returnValue.Append(inputArray[1]);
                    trailing = trailing - inputArray[1].Length;
                }

                while (trailing > 0)
                {
                    returnValue.Append("0");
                    trailing--;
                }
            }

            return returnValue.ToString();
        }
    }
}
