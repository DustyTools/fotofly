// <copyright file="GpsPosition.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>GpsPosition Class</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [XmlRootAttribute("GpsPosition")]
    public class GpsPosition : ICloneable
    {
        private const double Rad = Math.PI / 180;
        private double altitude;
        private GpsCoordinate latitude;
        private GpsCoordinate longitude;
        private DateTime satelliteTime;

        public enum Accuracies : int
        {
            Unknown = 0,
            Country = 1,
            Region = 2,
            SubRegion = 3,
            City = 4,
            PostalCode = 5,
            Street = 6,
            Intersection = 7,
            Address = 8,
            Premise = 9
        }

        public GpsPosition()
        {
            this.ResetCoordinates();
        }

        public GpsPosition(double latitude, double longitude)
        {
            this.ResetCoordinates();

            this.Dimension = Dimensions.TwoDimensional;

            this.Latitude.Numeric = latitude;
            this.Longitude.Numeric = longitude;
            this.Altitude = double.NaN;
            this.Source = string.Empty;
        }

        public GpsPosition(GpsCoordinate latitude, GpsCoordinate longitude, double altitude)
        {
            this.ResetCoordinates();

            this.Dimension = Dimensions.ThreeDimensional;

            this.Latitude.Numeric = latitude.Numeric;
            this.Longitude.Numeric = longitude.Numeric;

            this.Altitude = altitude;
            this.Source = string.Empty;
        }

        public enum Dimensions
        {
            NotSpecified,
            TwoDimensional = 2,
            ThreeDimensional = 3
        }
        
        [XmlAttribute]
        public int Accuracy
        {
            get;
            set;
        }

        [XmlAttribute]
        public double Altitude
        {
            get
            {
                return Math.Round(this.altitude, 3);
            }

            set
            {
                if (double.IsNaN(value))
                {
                    this.altitude = double.NaN;
                    this.Dimension = Dimensions.TwoDimensional;
                }
                else
                {
                    this.altitude = Math.Round(value, 3);
                    this.Dimension = Dimensions.ThreeDimensional;
                }
            }
        }

        [XmlIgnore]
        public string DegreesMinutesSecondsAltitude
        {
            get
            {
                string returnValue = this.Latitude.DegreesMinutesSeconds + " " + this.Longitude.DegreesMinutesSeconds;

                if (this.Dimension == Dimensions.ThreeDimensional && !double.IsNaN(this.Altitude))
                {
                    returnValue += " " + this.Altitude + "m";
                }

                if (string.IsNullOrEmpty(returnValue.Replace(" ", string.Empty)))
                {
                    return string.Empty;
                }
                else
                {
                    return returnValue;
                }
            }
        }

        [XmlIgnore]
        public Dimensions Dimension
        {
            get;
            set;
        }

        [XmlAttribute]
        public bool IsValidCoordinate
        {
            get
            {
                if (this.Latitude.IsValidCoordinate && this.Longitude.IsValidCoordinate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                // Swallow this, done done for serialisation
            }
        }

        [XmlElement]
        public GpsCoordinate Latitude
        {
            get
            {
                if (this.latitude == null)
                {
                    this.latitude = new GpsCoordinate();
                    this.latitude.LatOrLon = GpsCoordinate.LatOrLons.Latitude;
                }

                return this.latitude;
            }

            set
            {
                this.latitude = value;
                this.latitude.LatOrLon = GpsCoordinate.LatOrLons.Latitude;
            }
        }

        [XmlElement]
        public GpsCoordinate Longitude
        {
            get
            {
                if (this.longitude == null)
                {
                    this.longitude = new GpsCoordinate();
                    this.longitude.LatOrLon = GpsCoordinate.LatOrLons.Longitude;
                }

                return this.longitude;
            }

            set
            {
                this.longitude = value;
                this.longitude.LatOrLon = GpsCoordinate.LatOrLons.Longitude;
            }
        }

        [XmlAttribute]
        public string Source
        {
            get;
            set;
        }

        [XmlAttribute]
        public DateTime SatelliteTime
        {
            get
            {
                if (this.satelliteTime == null)
                {
                    this.satelliteTime = new DateTime();
                }

                return this.satelliteTime;
            }

            set
            {
                this.satelliteTime = value;
            }
        }

        /// <summary>
        /// Compares two GpsPositions and returns the distance between the two points in meters.
        /// </summary>
        /// <param name="other">A GpsPosition</param>
        /// <returns>Distance in Meters</returns>
        public double Distance(GpsPosition other)
        {
            if (!this.IsValidCoordinate)
            {
                throw new Exception("1st Point is Invalid");
            }
            else if (!other.IsValidCoordinate)
            {
                throw new Exception("2nd Point is Invalid");
            }
            else
            {
                /*
                    The Haversine formula according to Dr. Math.
                    http://mathforum.org/library/drmath/view/51879.html
                        
                    dlon = lon2 - lon1
                    dlat = lat2 - lat1
                    a = (sin(dlat/2))^2 + cos(lat1) * cos(lat2) * (sin(dlon/2))^2
                    c = 2 * atan2(sqrt(a), sqrt(1-a)) 
                    d = R * c
                        
                    Where
                        * dlon is the change in longitude
                        * dlat is the change in latitude
                        * c is the great circle distance in Radians.
                        * R is the radius of a spherical Earth.
                        * The locations of the two points in 
                            spherical coordinates (longitude and 
                            latitude) are lon1,lat1 and lon2, lat2.
                */
                double distance = Double.MinValue;

                double lat1InRad = this.Latitude.Numeric * (Math.PI / 180.0);
                double long1InRad = this.Longitude.Numeric * (Math.PI / 180.0);
                double lat2InRad = other.Latitude.Numeric * (Math.PI / 180.0);
                double long2InRad = other.Longitude.Numeric * (Math.PI / 180.0);

                double longitude = long2InRad - long1InRad;
                double latitude = lat2InRad - lat1InRad;

                // Intermediate result a.
                double a = Math.Pow(Math.Sin(latitude / 2.0), 2.0) +
                           (Math.Cos(lat1InRad) * Math.Cos(lat2InRad) *
                           Math.Pow(Math.Sin(longitude / 2.0), 2.0));

                // Intermediate result c (great circle distance in Radians).
                double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

                // Distance.
                // const Double kEarthRadiusMiles = 3956.0;
                const double EarthRadiusKms = 6376.5;

                // Convert to Meters
                distance = EarthRadiusKms * c * 1000;

                return distance;
            }
        }

        public void ResetCoordinates()
        {
            this.Accuracy = 0;
            this.Latitude.Numeric = double.NaN;
            this.Longitude.Numeric = double.NaN;
            this.Dimension = Dimensions.NotSpecified;
            this.Source = string.Empty;
            this.altitude = double.NaN;
            this.satelliteTime = new DateTime();
        }

        public string AccuracyAsString(GpsPosition.Accuracies accuracy)
        {
            Dictionary<int, string> accuracies = new Dictionary<int,string>();
            accuracies.Add(0, "Unknown");
            accuracies.Add(1, "Country");
            accuracies.Add(2, "Region"); // state, province, prefecture, etc.
            accuracies.Add(3, "Sub-Region"); // county, municipality, etc.
            accuracies.Add(4, "City"); // city, village
            accuracies.Add(5, "Postal code"); // zip code
            accuracies.Add(6, "Street");
            accuracies.Add(7, "Intersection");
            accuracies.Add(8, "Address");
            accuracies.Add(9, "Premise"); // building name, property name, shopping center, etc.

            return accuracies[(int)accuracy];
        }

        public override bool Equals(object unknownObject)
        {
            if (unknownObject is GpsPosition)
            {
                GpsPosition otherPosition = (GpsPosition)unknownObject;

                if (otherPosition.DegreesMinutesSecondsAltitude == this.DegreesMinutesSecondsAltitude
                    && otherPosition.SatelliteTime.Ticks == this.SatelliteTime.Ticks
                    && otherPosition.Source == this.Source)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return this.satelliteTime.ToString() + " - " + this.DegreesMinutesSecondsAltitude;
        }

        public object Clone()
        {
            GpsPosition gpsPosition = new GpsPosition();
            gpsPosition.Latitude = this.Latitude.Clone() as GpsCoordinate;
            gpsPosition.Longitude = this.Longitude.Clone() as GpsCoordinate;
            gpsPosition.Altitude = this.Altitude;
            gpsPosition.Source = this.Source;
            gpsPosition.SatelliteTime = this.SatelliteTime;
            gpsPosition.Accuracy = this.Accuracy;

            return gpsPosition;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
