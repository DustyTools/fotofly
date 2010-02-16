// <copyright file="XmpExifProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>XmpExifProvider Class</summary>
namespace Fotofly.MetadataProviders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Media.Imaging;

    using Fotofly.BitmapMetadataTools;
    using Fotofly.MetadataQueries;

    public class XmpExifProvider : BaseProvider
    {
        public XmpExifProvider(BitmapMetadata bitmapMetadata)
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
                gpsPosition.Latitude = this.GpsLatitude.Clone() as GpsCoordinate;
                gpsPosition.Longitude = this.GpsLongitude.Clone() as GpsCoordinate;
                gpsPosition.Altitude = this.GpsAltitude;
                gpsPosition.Source = this.GpsProcessingMethod;
                gpsPosition.Dimension = this.GpsMeasureMode;
                gpsPosition.SatelliteTime = this.GpsDateTimeStamp;

                return gpsPosition;
            }

            set
            {
                if (value.IsValidCoordinate)
                {
                    this.GpsAltitude = value.Altitude;
                    this.GpsLatitude = value.Latitude.Clone() as GpsCoordinate;
                    this.GpsLongitude = value.Longitude.Clone() as GpsCoordinate;
                    this.GpsDateTimeStamp = value.SatelliteTime;
                    this.GpsMeasureMode = value.Dimension;
                    this.GpsProcessingMethod = value.Source;
                    this.GpsVersionID = "2200";
                }
                else
                {
                    // Clear all properties
                    this.GpsAltitude = double.NaN;
                    this.GpsLatitude = new GpsCoordinate();
                    this.GpsLongitude = new GpsCoordinate();
                    this.GpsDateTimeStamp = new DateTime();
                    this.GpsProcessingMethod = string.Empty;
                    this.GpsVersionID = string.Empty;
                    this.GpsMeasureMode = GpsPosition.Dimensions.NotSpecified;
                }
            }
        }

        /// <summary>
        /// Gps Altitude
        /// </summary>
        public double GpsAltitude
        {
            get
            {
                string gpsRef = this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsAltitudeRef.Query);
                URational urational = this.BitmapMetadata.GetQuery<URational>(XmpExifQueries.GpsAltitude.Query);

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
                    this.BitmapMetadata.SetQuery(XmpExifQueries.GpsAltitudeRef.Query, string.Empty);
                    this.BitmapMetadata.SetQuery(XmpExifQueries.GpsAltitude.Query, string.Empty);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(XmpExifQueries.GpsAltitudeRef.Query, value > 0 ? 0 : 1);
                    this.BitmapMetadata.SetQuery(XmpExifQueries.GpsAltitude.Query, Math.Abs(value));
                }
            }
        }

        /// <summary>
        /// Gps Latitude
        /// </summary>
        public GpsCoordinate GpsLatitude
        {
            get
            {
                // Format
                // “DDD,MM,SSk” or “DDD,MM.mmk”
                //  51,55,84N or 51,55.6784N
                string gpsLatitudeString = this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsLatitude.Query);

                // Do basic validation, 3 is minimum length
                if (!string.IsNullOrEmpty(gpsLatitudeString) && gpsLatitudeString.Length > 3)
                {
                    // Strip off the last character as the reference
                    string latitudeRefString = gpsLatitudeString[gpsLatitudeString.Length - 1].ToString();

                    // Check the reference is valid
                    if (latitudeRefString == "N" || latitudeRefString == "S")
                    {
                        bool isRefPositive = latitudeRefString == "N" ? true : false;

                        // Stripe off the reference
                        gpsLatitudeString = gpsLatitudeString.Replace(latitudeRefString, string.Empty);
                        
                        // Split the string
                        string[] splitString = gpsLatitudeString.Split(',');

                        if (splitString.Length == 2)
                        {
                            double degrees;
                            double minutes;

                            if (Double.TryParse(splitString[0], out degrees) && Double.TryParse(splitString[1], out minutes))
                            {
                                return new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, isRefPositive, degrees, minutes);
                            }
                        }
                        else if (splitString.Length == 3)
                        {
                            double degrees;
                            double minutes;
                            double seconds;

                            if (Double.TryParse(splitString[0], out degrees) && Double.TryParse(splitString[1], out minutes) && Double.TryParse(splitString[2], out seconds))
                            {
                                return new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, isRefPositive, degrees, minutes, seconds);
                            }
                        }
                    }
                }

                return new GpsCoordinate();
            }

            set
            {
                if (value.IsValidCoordinate)
                {
                    string latitudeAsString = string.Format("{0},{1},{2}{3}", value.Degrees, value.Minutes, value.Seconds, value.Ref);

                    this.BitmapMetadata.SetQuery(XmpExifQueries.GpsLatitude.Query, latitudeAsString);
                }
                else
                {
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsLatitude.Query);
                }
            }
        }

        /// <summary>
        /// Gps Longitude
        /// </summary>
        public GpsCoordinate GpsLongitude
        {
            get
            {
                string gpsLongitudeString = this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsLongitude.Query);

                // Do basic validation, 3 is minimum length
                if (!string.IsNullOrEmpty(gpsLongitudeString) && gpsLongitudeString.Length > 3)
                {
                    // Strip off the last character as the reference
                    string longitudeRefString = gpsLongitudeString[gpsLongitudeString.Length - 1].ToString();

                    // Check the reference is valid
                    if (longitudeRefString == "E" || longitudeRefString == "W")
                    {
                        bool isRefPositive = longitudeRefString == "E" ? true : false;

                        // Stripe off the reference
                        gpsLongitudeString = gpsLongitudeString.Replace(longitudeRefString, string.Empty);

                        // Split the string
                        string[] splitString = gpsLongitudeString.Split(',');

                        if (splitString.Length == 2)
                        {
                            double degrees;
                            double minutes;

                            if (Double.TryParse(splitString[0], out degrees) && Double.TryParse(splitString[1], out minutes))
                            {
                                return new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, isRefPositive, degrees, minutes);
                            }
                        }
                        else if (splitString.Length == 3)
                        {
                            double degrees;
                            double minutes;
                            double seconds;

                            if (Double.TryParse(splitString[0], out degrees) && Double.TryParse(splitString[1], out minutes) && Double.TryParse(splitString[2], out seconds))
                            {
                                return new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, isRefPositive, degrees, minutes, seconds);
                            }
                        }
                    }
                }

                return new GpsCoordinate();
            }

            set
            {
                if (value.IsValidCoordinate)
                {
                    string longitudeAsString = string.Format("{0},{1},{2}{3}", value.Degrees, value.Minutes, value.Seconds, value.Ref);

                    this.BitmapMetadata.SetQuery(XmpExifQueries.GpsLongitude.Query, longitudeAsString);
                }
                else
                {
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsLongitude.Query);
                }
            }
        }

        /// <summary>
        /// Gps Time stamp
        /// </summary>
        public DateTime GpsDateTimeStamp
        {
            get
            {
                // Format 2010-02-01T22:02:34+01:00
                DateTime gpsDateTimeStamp = this.BitmapMetadata.GetQuery<DateTime>(XmpExifQueries.GpsDateTimeStamp.Query);
                
                return gpsDateTimeStamp;
            }

            set
            {
                if (value != null && value != new DateTime())
                {
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsDateTimeStamp.Query);
                }
                else
                {
                    string gpsDateTimeStamp = value.ToString("z");

                    this.BitmapMetadata.SetQuery(XmpExifQueries.GpsDateTimeStamp.Query, gpsDateTimeStamp);
                }
            }
        }

        /// <summary>
        /// Gps Version ID
        /// </summary>
        public string GpsVersionID
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsVersionID.Query);
            }

            set
            {
                this.BitmapMetadata.SetQuery(XmpExifQueries.GpsVersionID.Query, value);
            }
        }

        /// <summary>
        /// Gps Processing Mode
        /// </summary>
        public string GpsProcessingMethod
        {
            get
            {
                if (string.IsNullOrEmpty(this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsProcessingMethod.Query)))
                {
                    return null;
                }
                else
                {
                    return this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsProcessingMethod.Query);
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value) || value == "None")
                {
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsProcessingMethod.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(XmpExifQueries.GpsProcessingMethod.Query, value);
                }
            }
        }

        /// <summary>
        /// Gps Measure Mode
        /// </summary>
        public GpsPosition.Dimensions GpsMeasureMode
        {
            get
            {
                if (this.BitmapMetadata.ContainsQuery(XmpExifQueries.GpsMeasureMode.Query))
                {
                    string measureMode = this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsMeasureMode.Query);

                    if (!string.IsNullOrEmpty(measureMode) && measureMode == "2")
                    {
                        return GpsPosition.Dimensions.TwoDimensional;
                    }
                    else if (!string.IsNullOrEmpty(measureMode) && measureMode == "3")
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
                        this.BitmapMetadata.SetQuery(XmpExifQueries.GpsMeasureMode.Query, string.Empty);
                        break;

                    case GpsPosition.Dimensions.ThreeDimensional:
                        this.BitmapMetadata.SetQuery(XmpExifQueries.GpsMeasureMode.Query, 3);
                        break;

                    case GpsPosition.Dimensions.TwoDimensional:
                        this.BitmapMetadata.SetQuery(XmpExifQueries.GpsMeasureMode.Query, 2);
                        break;
                }
            }
        }
    }
}