// <copyright file="GpsPosition.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>GpsPosition Class</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using System.ComponentModel;

    [XmlRootAttribute("GpsPosition", Namespace = "http://www.tassography.com/fotofly")]
    public class GpsPosition : ICloneable
    {
        private const double Rad = Math.PI / 180;
        private double altitude;
        private GpsCoordinate latitude;
        private GpsCoordinate longitude;
        private DateTime satelliteTime;

        public GpsPosition()
        {
            this.ResetCoordinates();
        }

        public GpsPosition(GpsPosition gpsPosition)
        {
            this.Altitude = gpsPosition.Altitude;
            this.Latitude = gpsPosition.Latitude.Clone() as GpsCoordinate;
            this.Longitude = gpsPosition.Longitude.Clone() as GpsCoordinate;
            this.Source = gpsPosition.Source;
            this.Time = gpsPosition.Time;
        }

        public GpsPosition(double latitude, double longitude)
        {
            this.ResetCoordinates();

            this.Latitude.Numeric = latitude;
            this.Longitude.Numeric = longitude;
            this.Altitude = double.NaN;
        }

        public GpsPosition(double latitude, double longitude, double altitude)
        {
            this.ResetCoordinates();

            this.Latitude.Numeric = latitude;
            this.Longitude.Numeric = longitude;
            this.Altitude = altitude;
        }

        public GpsPosition(GpsCoordinate latitude, GpsCoordinate longitude, double altitude)
        {
            this.ResetCoordinates();

            this.Latitude.Numeric = latitude.Numeric;
            this.Longitude.Numeric = longitude.Numeric;
            this.Altitude = altitude;
        }

        public enum Dimensions
        {
            NotSpecified,
            OneDimensional = 1,
            TwoDimensional = 2,
            ThreeDimensional = 3
        }

        public enum AltitudeRef
        {
            NotSpecified,
            AboveSeaLevel,
            BelowSeaLevel
        }

        [XmlAttribute("Alt")]
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
                }
                else
                {
                    this.altitude = Math.Round(value, 3);
                }
            }
        }

        [XmlIgnore]
        public Dimensions Dimension
        {
            get
            {
                if (double.IsNaN(this.Altitude) && (!this.Latitude.IsValidCoordinate || !this.Longitude.IsValidCoordinate))
                {
                    // Altitude is not set and Lat & Lon are not correct
                    return Dimensions.NotSpecified;
                }
                else if (!double.IsNaN(this.Altitude) && (!this.Latitude.IsValidCoordinate || !this.Longitude.IsValidCoordinate))
                {
                    // Altitude is correct, Lat & Lon are not
                    return Dimensions.OneDimensional;
                }
                else if (double.IsNaN(this.Altitude) && this.Latitude.IsValidCoordinate && this.Longitude.IsValidCoordinate)
                {
                    // Altitude is not correct, Lat & Lon are correct
                    return Dimensions.TwoDimensional;
                }
                else
                {
                    return Dimensions.ThreeDimensional;
                }
            }

            set
            {
                // Ignoreonly used for Serialization
            }
        }

        [XmlAttribute]
        [DefaultValue(true)]
        public bool IsValidCoordinate
        {
            get
            {
                // Dimension does the root check of what is valid or not
                return this.Dimension != Dimensions.NotSpecified;
            }

            set
            {
                // Swallow this, done done for serialisation
            }
        }

        [XmlAttribute("Lat")]
        public double LatitudeAsDouble
        {
            get
            {
                return this.Latitude.Numeric;
            }

            set
            {
                this.Latitude.Numeric = value;
            }
        }

        [XmlIgnore]
        public GpsCoordinate Latitude
        {
            get
            {
                if (this.latitude == null || double.IsNaN(this.latitude.Numeric))
                {
                    this.latitude = new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude);
                }

                return this.latitude;
            }

            set
            {
                if (double.IsNaN(value.Numeric))
                {
                    this.latitude = new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude);
                }

                this.latitude = new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, value.Numeric);
            }
        }

        [XmlAttribute("Lon")]
        public double LongitudeAsDouble
        {
            get
            {
                return this.Longitude.Numeric;
            }

            set
            {
                this.Longitude.Numeric = value;
            }
        }

        [XmlIgnore]
        public GpsCoordinate Longitude
        {
            get
            {
                if (this.longitude == null || double.IsNaN(this.longitude.Numeric))
                {
                    this.longitude = new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude);
                }

                return this.longitude;
            }

            set
            {
                if (double.IsNaN(value.Numeric))
                {
                    this.longitude = new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude);
                }

                this.longitude = new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, value.Numeric);
            }
        }

        [XmlAttribute]
        [DefaultValue("")]
        public string Source
        {
            get;
            set;
        }

        [XmlAttribute]
        [DefaultValue(typeof(DateTime), "0001-01-01T00:00:00")]
        public DateTime Time
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
            this.Latitude = new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude);
            this.Longitude = new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude);
            this.Source = null;
            this.altitude = double.NaN;
            this.satelliteTime = new DateTime();
        }

        public override bool Equals(object unknownObject)
        {
            if (unknownObject is GpsPosition)
            {
                GpsPosition otherPosition = (GpsPosition)unknownObject;

                if (otherPosition.ToString() == this.ToString()
                    && otherPosition.Time.Ticks == this.Time.Ticks
                    && otherPosition.Source == this.Source)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            // Return the correct format based on the dimensions
            switch (this.Dimension)
            {
                default:
                case Dimensions.NotSpecified:
                    return string.Empty;

                case Dimensions.OneDimensional:
                    return this.Altitude + "m";

                case Dimensions.TwoDimensional:
                    return this.Latitude.DegreesMinutesSeconds + " " + this.Longitude.DegreesMinutesSeconds;

                case Dimensions.ThreeDimensional:
                    return this.Latitude.DegreesMinutesSeconds + " " + this.Longitude.DegreesMinutesSeconds + " " + this.Altitude + "m";
            }
        }

        public object Clone()
        {
            GpsPosition gpsPosition = new GpsPosition();
            gpsPosition.Latitude = this.Latitude.Clone() as GpsCoordinate;
            gpsPosition.Longitude = this.Longitude.Clone() as GpsCoordinate;
            gpsPosition.Altitude = this.Altitude;
            gpsPosition.Source = this.Source;
            gpsPosition.Time = this.Time;

            return gpsPosition;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool ShouldSerializeAltitude()
        {
            return !double.IsNaN(this.Altitude);
        }

        /* Accuracy Code, not currently used
        public string AccuracyAsString(GpsPosition.Accuracies accuracy)
        {
            Dictionary<int, string> accuracies = new Dictionary<int, string>();
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

        public GpsPosition.Accuracies AccuracyOfGps
        {
            get
            {
                string accuracyOfGpsString = this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.AccuracyOfGps.Query);

                if (!string.IsNullOrEmpty(accuracyOfGpsString))
                {
                    int accuracyOfGps;

                    if (Int32.TryParse(accuracyOfGpsString[0].ToString(), out accuracyOfGps) && accuracyOfGps < 9 && accuracyOfGps > 0)
                    {
                        return (GpsPosition.Accuracies)accuracyOfGps;
                    }
                }

                return GpsPosition.Accuracies.Unknown;
            }

            set
            {
                if (this.ValueHasChanged(value, this.AccuracyOfGps))
                {
                    this.CreateFotoflyStruct();

                    if (value == GpsPosition.Accuracies.Unknown)
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.AccuracyOfGps.Query);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.AccuracyOfGps.Query, (int)value + "-" + value.ToString());
                    }
                }
            }
        }
         
                // Set Accuracy
                double distanceToPointA = this.GpsPositionBefore.Distance(this.gpsPositionMiddle);
                double distanceToPointB = this.GpsPositionAfter.Distance(this.gpsPositionMiddle);

                // Work out farthest point because this is the least accurate point
                double distanceToFarthestPoint = distanceToPointA > distanceToPointB ? distanceToPointA : distanceToPointB;

                // Calculate Accuracy
                if (distanceToFarthestPoint < 100)
                {
                    this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.Address;
                }
                else if (distanceToFarthestPoint < 1000)
                {
                    this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.Street;
                }
                else if (distanceToFarthestPoint < 10000)
                {
                    this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.City;
                }
                else if (distanceToFarthestPoint < 50000)
                {
                    this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.Region;
                }
                else
                {
                    this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.Country;
                }
        */
    }
}
