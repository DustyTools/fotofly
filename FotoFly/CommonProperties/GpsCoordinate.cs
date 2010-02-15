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

    public class GpsCoordinate : ICloneable
    {
        public static readonly int MaxLatitude = 90;
        public static readonly int MinLatitude = -90;
        public static readonly int MaxLongitude = 180;
        public static readonly int MinLongitude = -180;

        private double degrees;
        private double seconds;
        private double minutes;

        private bool isRefPositive;
        private LatOrLons latOrLon;

        public GpsCoordinate()
        {
            this.degrees = double.NaN;
            this.seconds = double.NaN;
            this.minutes = double.NaN;
        }

        public GpsCoordinate(LatOrLons coorType, double numeric)
        {
            this.LatOrLon = coorType;
            this.Numeric = numeric;
        }

        public GpsCoordinate(LatOrLons coorType, bool isRefPositive, double numeric)
        {
            this.LatOrLon = coorType;
            this.Numeric = numeric;
            this.isRefPositive = isRefPositive;
        }

        public GpsCoordinate(LatOrLons coorType, bool isRefPositive, double degrees, double minutes, double seconds)
        {
            this.LatOrLon = coorType;
            this.isRefPositive = isRefPositive;
            this.Degrees = Math.Round(degrees, 0);
            this.Minutes = Math.Round(minutes, 0);
            this.Seconds = Math.Round(seconds, 0);
        }

        public enum LatOrLons
        {
            NotSpecified,
            Latitude,
            Longitude
        }

        [XmlAttribute]
        public double Numeric
        {
            get
            {
                if (double.IsNaN(this.Degrees) || double.IsNaN(this.Minutes) || double.IsNaN(this.Seconds))
                {
                    return double.NaN;
                }
                else
                {
                    // Example 32° 56' 26.57" is 32 + (56/60) + (26.57 / 3600) = 32.940713888
                    double numeric = Math.Round(this.Degrees + (Convert.ToDouble(this.Minutes) / 60) + (Convert.ToDouble(this.Seconds) / 3600), 4);

                    if (!this.IsValidNumeric(numeric, this.LatOrLon))
                    {
                        return double.NaN;
                    }
                    else if (this.isRefPositive)
                    {
                        return numeric;
                    }
                    else
                    {
                        return -numeric;
                    }
                }
            }

            set
            {
                if (double.IsNaN(value) || !this.IsValidNumeric(value, this.LatOrLon))
                {
                    // Set internals so we don't loop
                    this.Degrees = double.NaN;
                    this.Minutes = double.NaN;
                    this.Seconds = double.NaN;
                }
                else
                {
                    if (value > 0)
                    {
                        this.isRefPositive = true;
                    }
                    else
                    {
                        this.isRefPositive = false;
                    }

                    double remainder = Math.Abs(value);

                    // Calculate Degrees, everything to the left of the decimal
                    this.Degrees = (int)Math.Floor(remainder);

                    // Calculate the Minutes, remove the Degrees, then divide by 60 and round down
                    remainder = remainder - this.Degrees;

                    this.Minutes = (int)Math.Floor(remainder * 60);

                    // Calculate the Seconds, remove the Minutes
                    this.Seconds = Math.Round(Convert.ToDouble((remainder * 3600) - (this.Minutes * 60)));

                    // Fix rounding issues
                    if (this.Seconds >= 60)
                    {
                        this.Minutes = this.Minutes + 1;
                        this.Seconds = this.Seconds - 60;
                    }

                    if (this.Minutes >= 60)
                    {
                        this.Degrees = this.Degrees + 1;
                        this.Minutes = this.Minutes - 60;
                    }
                }
            }
        }

        [XmlIgnore]
        public string DegreesMinutesSeconds
        {
            get
            {
                // Formats the string in the format E 106° 49' 32.9"
                if (this.IsValidCoordinate)
                {
                    StringBuilder dms = new StringBuilder();
                    dms.Append(this.Ref);
                    dms.Append(" ");
                    dms.Append(this.AddPaddingZeros(this.degrees, 3, 0));
                    dms.Append("° ");
                    dms.Append(this.AddPaddingZeros(this.minutes, 2, 0));
                    dms.Append("' ");
                    dms.Append(this.AddPaddingZeros(this.seconds, 2, 2));
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
                return this.IsValidNumeric(this.Numeric, this.LatOrLon);
            }
        }

        [XmlIgnore]
        public LatOrLons LatOrLon
        {
            get
            {
                return this.latOrLon;
            }

            set
            {
                this.latOrLon = value;

                if (!this.IsValidNumeric(this.Numeric, value))
                {
                    this.Numeric = double.NaN;
                }
            }
        }

        [XmlIgnore]
        public string Ref
        {
            // ASCII 'E' indicates east longitude, and 'W' is west longitude
            // ASCII 'N' indicates north latitude, and 'S' is south latitude
            get
            {
                if (this.LatOrLon == LatOrLons.Latitude && this.isRefPositive)
                {
                    return "N";
                }
                else if (this.LatOrLon == LatOrLons.Latitude && !this.isRefPositive)
                {
                    return "S";
                }
                else if (this.LatOrLon == LatOrLons.Longitude && this.isRefPositive)
                {
                    return "E";
                }
                else
                {
                    return "W";
                }
            }

            set
            {
                // Take the incoming count and work out if the numberic should be positive or negative
                value = value.ToUpper();

                if (String.IsNullOrEmpty(value))
                {
                    this.isRefPositive = true;
                }
                else if (value == "N" || value == "E")
                {
                    this.isRefPositive = true;
                }
                else if (value == "S" || value == "W")
                {
                    this.isRefPositive = false;
                }
                else
                {
                    throw new Exception("Invalid Reference");
                }
            }
        }

        [XmlIgnore]
        public double Degrees
        {
            get { return this.degrees; }
            set { this.degrees = Math.Round(value, 0); }
        }

        [XmlIgnore]
        public double Minutes
        {
            get { return this.minutes; }
            set { this.minutes = Math.Round(value, 0); }
        }

        [XmlIgnore]
        public double Seconds
        {
            get { return this.seconds; }
            set { this.seconds = Math.Round(value, 0); }
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
            return this.Numeric.ToString();
        }

        public object Clone()
        {
            return new GpsCoordinate(this.LatOrLon, this.isRefPositive, this.Degrees, this.Minutes, this.Seconds);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private string AddPaddingZeros(double number, int leading, int trailing)
        {
            StringBuilder returnValue = new StringBuilder();

            string input = Math.Abs(number).ToString();

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

        private bool IsValidNumeric(double numeric, LatOrLons latOrLon)
        {
            if (latOrLon == LatOrLons.Latitude && (numeric > GpsCoordinate.MaxLatitude || numeric < GpsCoordinate.MinLatitude))
            {
                return false;
            }
            else if (this.LatOrLon == LatOrLons.Longitude && (numeric > GpsCoordinate.MaxLongitude || numeric < GpsCoordinate.MinLongitude))
            {
                return false;
            }
            else if (double.IsNaN(numeric))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
