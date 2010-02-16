// <copyright file="GpsProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>GpsProvider Class</summary>
namespace Fotofly.MetadataProviders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using System.Windows.Media.Imaging;

    using Fotofly.BitmapMetadataTools;
    using Fotofly.MetadataQueries;

    public class GpsProvider : BaseProvider
    {
        public GpsProvider(BitmapMetadata bitmapMetadata)
            : base(bitmapMetadata)
        {}

        /// <summary>
        /// GpsPosition, encapculates Latitude, Longitude, Altitude, Source, Dimension and SatelliteTime
        /// </summary>
        public GpsPosition GpsPosition
        {
            get
            {
                GpsPosition gpsPosition = new GpsPosition();
                gpsPosition.Latitude = this.Latitude.Clone() as GpsCoordinate;
                gpsPosition.Longitude = this.Longitude.Clone() as GpsCoordinate;
                gpsPosition.Altitude = this.Altitude;
                gpsPosition.Source = this.ProcessingMethod;
                gpsPosition.Dimension = this.MeasureMode;
                gpsPosition.SatelliteTime = this.DateTimeStamp;

                return gpsPosition;
            }

            set
            {
                if (value.IsValidCoordinate)
                {
                    if (value.Dimension == GpsPosition.Dimensions.ThreeDimensional)
                    {
                        this.Altitude = value.Altitude;
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.AltitudeRef.Query, string.Empty);
                        this.BitmapMetadata.SetQuery(GpsQueries.Altitude.Query, string.Empty);
                    }

                    this.Latitude = value.Latitude.Clone() as GpsCoordinate;
                    this.Longitude = value.Longitude.Clone() as GpsCoordinate;
                    this.DateTimeStamp = value.SatelliteTime;
                    this.MeasureMode = value.Dimension;
                    this.ProcessingMethod = value.Source;
                    this.VersionID = "2200";
                }
                else
                {
                    // Clear all properties
                    this.Altitude = double.NaN;
                    this.Latitude = new GpsCoordinate();
                    this.Longitude = new GpsCoordinate();
                    this.DateTimeStamp = new DateTime();
                    this.ProcessingMethod = string.Empty;
                    this.VersionID = string.Empty;
                    this.MeasureMode = GpsPosition.Dimensions.NotSpecified;
                }
            }
        }

        /// <summary>
        /// Gps Altitude
        /// </summary>
        public double Altitude
        {
            // Altitude is expressed as one RATIONAL count. The reference unit is meters
            // 0 = Above sea level, 1 = Below sea level 
            get
            {
                string gpsRef = this.BitmapMetadata.GetQuery<string>(GpsQueries.AltitudeRef.Query);
                URational urational = this.BitmapMetadata.GetQuery<URational>(GpsQueries.Altitude.Query);

                if (String.IsNullOrEmpty(gpsRef) || urational == null || double.IsNaN(urational.ToDouble()))
                {
                    return double.NaN;
                }
                else if (gpsRef == "1")
                {
                    return -urational.ToDouble(3);
                }
                else
                {
                    return urational.ToDouble(3);
                }
            }

            set
            {
                // Check to see if the values have changed
                if (double.IsNaN(value))
                {
                    // Blank out existing values
                    this.BitmapMetadata.SetQuery(GpsQueries.AltitudeRef.Query, string.Empty);
                    this.BitmapMetadata.SetQuery(GpsQueries.Altitude.Query, string.Empty);
                }
                else
                {
                    // Keep three decimal place of precision
                    URational urational = new URational(value, 3);

                    string altRef;

                    if (value > 0)
                    {
                        altRef = "0";
                    }
                    else
                    {
                        altRef = "1";
                    }

                    this.BitmapMetadata.SetQuery(GpsQueries.AltitudeRef.Query, altRef);
                    this.BitmapMetadata.SetQuery(GpsQueries.Altitude.Query, urational.ToUInt64());
                }
            }
        }

        /// <summary>
        /// Gps Latitude
        /// </summary>
        public GpsCoordinate Latitude
        {
            get
            {
                string gpsRef = this.BitmapMetadata.GetQuery<string>(GpsQueries.LatitudeRef.Query);

                URationalTriplet gpsRational = this.BitmapMetadata.GetQuery<URationalTriplet>(GpsQueries.Latitude.Query);

                if (gpsRef == null)
                {
                    return new GpsCoordinate();
                }
                else if (gpsRef == "N" && gpsRational != null)
                {
                    return new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, true, gpsRational.ToDouble());
                }
                else if (gpsRef == "S" && gpsRational != null)
                {
                    return new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, false, gpsRational.ToDouble());
                }
                else
                {
                    return new GpsCoordinate();
                }
            }

            set
            {
                if (value.IsValidCoordinate)
                {
                    this.BitmapMetadata.SetQuery(GpsQueries.LatitudeRef.Query, value.Ref);

                    URationalTriplet gpsRational = new URationalTriplet(value.Degrees, value.Minutes, value.Seconds);

                    this.BitmapMetadata.SetQuery(GpsQueries.Latitude.Query, gpsRational.ToUlongArray(true));
                }
                else
                {
                    this.BitmapMetadata.SetQuery(GpsQueries.Latitude.Query, string.Empty);
                    this.BitmapMetadata.SetQuery(GpsQueries.LatitudeRef.Query, string.Empty);
                }
            }
        }

        /// <summary>
        /// Gps Longitude
        /// </summary>
        public GpsCoordinate Longitude
        {
            get
            {
                string gpsRef = this.BitmapMetadata.GetQuery<string>(GpsQueries.LongitudeRef.Query);

                URationalTriplet gpsRational = this.BitmapMetadata.GetQuery<URationalTriplet>(GpsQueries.Longitude.Query);

                if (gpsRef == null)
                {
                    return new GpsCoordinate();
                }
                else if (gpsRef == "E" && gpsRational != null)
                {
                    return new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, true, gpsRational.ToDouble());
                }
                else if (gpsRef == "W" && gpsRational != null)
                {
                    return new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, false, gpsRational.ToDouble());
                }
                else
                {
                    return new GpsCoordinate();
                }
            }

            set
            {
                if (value.IsValidCoordinate)
                {
                    this.BitmapMetadata.SetQuery(GpsQueries.LongitudeRef.Query, value.Ref);

                    URationalTriplet gpsRational = new URationalTriplet(value.Degrees, value.Minutes, value.Seconds);

                    this.BitmapMetadata.SetQuery(GpsQueries.Longitude.Query, gpsRational.ToUlongArray(true));
                }
                else
                {
                    this.BitmapMetadata.SetQuery(GpsQueries.Longitude.Query, string.Empty);
                    this.BitmapMetadata.SetQuery(GpsQueries.LongitudeRef.Query, string.Empty);
                }
            }
        }

        /// <summary>
        /// Gps Measure Mode
        /// </summary>
        public GpsPosition.Dimensions MeasureMode
        {
            get
            {
                if (this.BitmapMetadata.IsQueryOfType(GpsQueries.MeasureMode.Query, GpsQueries.MeasureMode.BitmapMetadataType))
                {
                    int? returnValue = this.BitmapMetadata.GetQuery<int?>(GpsQueries.MeasureMode.Query);

                    if (returnValue != null && returnValue.Value == 2)
                    {
                        return GpsPosition.Dimensions.TwoDimensional;
                    }
                    else if (returnValue != null && returnValue.Value == 3)
                    {
                        return GpsPosition.Dimensions.ThreeDimensional;
                    }
                }

                return GpsPosition.Dimensions.NotSpecified;
            }

            set
            {
                switch (value)
                {
                    default:
                    case GpsPosition.Dimensions.NotSpecified:
                        this.BitmapMetadata.SetQuery(GpsQueries.MeasureMode.Query, string.Empty);
                        break;

                    case GpsPosition.Dimensions.ThreeDimensional:
                        this.BitmapMetadata.SetQuery(GpsQueries.MeasureMode.Query, 3);
                        break;

                    case GpsPosition.Dimensions.TwoDimensional:
                        this.BitmapMetadata.SetQuery(GpsQueries.MeasureMode.Query, 2);
                        break;
                }
            }
        }

        /// <summary>
        /// Gps Time stamp
        /// </summary>
        public DateTime DateTimeStamp
        {
            get
            {
                try
                {
                    URationalTriplet time = this.BitmapMetadata.GetQuery<URationalTriplet>(GpsQueries.TimeStamp.Query);
                    string date = this.BitmapMetadata.GetQuery<string>(GpsQueries.DateStamp.Query);

                    if (time != null && !string.IsNullOrEmpty(date))
                    {
                        string[] dateParts = date.Split(':');

                        return new DateTime(Convert.ToInt32(dateParts[0]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]), time.A.ToInt(), time.B.ToInt(), time.C.ToInt());
                    }
                }
                catch
                {
                }

                return new DateTime();
            }

            set
            {
                if (value != new DateTime())
                {
                    URationalTriplet time = new URationalTriplet(value.Hour, value.Minute, value.Second);
                    string date = value.ToString("yyyy:MM:dd");

                    this.BitmapMetadata.SetQuery(GpsQueries.DateStamp.Query, date);
                    this.BitmapMetadata.SetQuery(GpsQueries.TimeStamp.Query, time.ToUlongArray(false));
                }
                else
                {
                    this.BitmapMetadata.SetQuery(GpsQueries.DateStamp.Query, string.Empty);
                    this.BitmapMetadata.SetQuery(GpsQueries.TimeStamp.Query, string.Empty);
                }
            }
        }

        /// <summary>
        /// Gps Version ID
        /// </summary>
        public string VersionID
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(GpsQueries.VersionID.Query);
            }

            set
            {
                this.BitmapMetadata.SetQuery(GpsQueries.VersionID.Query, value);
            }
        }

        /// <summary>
        /// Gps Processing Mode
        /// </summary>
        public string ProcessingMethod
        {
            get
            {
                if (string.IsNullOrEmpty(this.BitmapMetadata.GetQuery<string>(GpsQueries.ProcessingMethod.Query)))
                {
                    return null;
                }
                else
                {
                    return this.BitmapMetadata.GetQuery<string>(GpsQueries.ProcessingMethod.Query);
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value) || value == "None")
                {
                    this.BitmapMetadata.RemoveQuery(GpsQueries.ProcessingMethod.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(GpsQueries.ProcessingMethod.Query, value);
                }
            }
        }
    }
}
