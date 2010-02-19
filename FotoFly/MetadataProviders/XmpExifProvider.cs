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
                URational urational = this.BitmapMetadata.GetQuery<URational>(XmpExifQueries.GpsAltitude.Query);

                if (this.GpsAltitudeRef == GpsPosition.AltitudeRef.NotSpecified || urational == null || double.IsNaN(urational.ToDouble()))
                {
                    return double.NaN;
                }
                else if (this.GpsAltitudeRef == GpsPosition.AltitudeRef.BelowSeaLevel)
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
                if (this.ValueHasChanged(value, this.GpsAltitude))
                {
                    // Check to see if the values have changed
                    if (double.IsNaN(value))
                    {
                        // Blank out existing values
                        this.GpsAltitudeRef = GpsPosition.AltitudeRef.NotSpecified;

                        this.BitmapMetadata.SetQuery(XmpExifQueries.GpsAltitude.Query, string.Empty);
                    }
                    else
                    {
                        this.GpsAltitudeRef = value > 0 ? GpsPosition.AltitudeRef.AboveSeaLevel : GpsPosition.AltitudeRef.BelowSeaLevel;

                        this.BitmapMetadata.SetQuery(XmpExifQueries.GpsAltitude.Query, Math.Abs(value));
                    }
                }
            }
        }

        public GpsPosition.AltitudeRef GpsAltitudeRef
        {
            get
            {
                if (this.BitmapMetadata.ContainsQuery(XmpExifQueries.GpsAltitudeRef.Query))
                {
                    string gpsAltitudeRef = this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsAltitudeRef.Query);

                    if (gpsAltitudeRef == "0")
                    {
                        return GpsPosition.AltitudeRef.AboveSeaLevel;
                    }
                    else if (gpsAltitudeRef == "1")
                    {
                        return GpsPosition.AltitudeRef.BelowSeaLevel;
                    }
                }

                return GpsPosition.AltitudeRef.NotSpecified;
            }

            set
            {
                if (this.ValueHasChanged(value, this.GpsAltitudeRef))
                {
                    if (value == GpsPosition.AltitudeRef.NotSpecified)
                    {
                        this.BitmapMetadata.SetQuery(XmpExifQueries.GpsAltitudeRef.Query, string.Empty);
                    }
                    else if (value == GpsPosition.AltitudeRef.AboveSeaLevel)
                    {
                        this.BitmapMetadata.SetQuery(XmpExifQueries.GpsAltitudeRef.Query, "0");
                    }
                    else if (value == GpsPosition.AltitudeRef.BelowSeaLevel)
                    {
                        this.BitmapMetadata.SetQuery(XmpExifQueries.GpsAltitudeRef.Query, "1");
                    }
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
                GpsCoordinate.LatitudeRef gpsLatitudeRef = this.GpsLatitudeRef;

                // Do basic validation, 3 is minimum length
                if (gpsLatitudeRef != GpsCoordinate.LatitudeRef.NotSpecified && gpsLatitudeString.Length > 3)
                {
                    // Split the string
                    string[] splitString = gpsLatitudeString.Substring(0, gpsLatitudeString.Length - 1).Split(',');

                    if (splitString.Length == 2)
                    {
                        double degrees;
                        double minutes;

                        if (Double.TryParse(splitString[0], out degrees) && Double.TryParse(splitString[1], out minutes))
                        {
                            return new GpsCoordinate(gpsLatitudeRef, degrees, minutes);
                        }
                    }
                    else if (splitString.Length == 3)
                    {
                        double degrees;
                        double minutes;
                        double seconds;

                        if (Double.TryParse(splitString[0], out degrees) && Double.TryParse(splitString[1], out minutes) && Double.TryParse(splitString[2], out seconds))
                        {
                            return new GpsCoordinate(gpsLatitudeRef, degrees, minutes, seconds);
                        }
                    }
                }

                return new GpsCoordinate();
            }

            set
            {
                if (this.ValueHasChanged(value, this.GpsLatitude))
                {
                    if (value.IsValidCoordinate)
                    {
                        string latitudeAsString = string.Format("{0},{1},{2}", value.Degrees, value.Minutes, value.Seconds);

                        this.BitmapMetadata.SetQuery(XmpExifQueries.GpsLatitude.Query, latitudeAsString);

                        this.GpsLatitudeRef = value.Numeric > 0 ? GpsCoordinate.LatitudeRef.North : GpsCoordinate.LatitudeRef.South;
                    }
                    else
                    {
                        this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsLatitude.Query);

                        this.GpsLatitudeRef = GpsCoordinate.LatitudeRef.NotSpecified;
                    }
                }
            }
        }

        public GpsCoordinate.LatitudeRef GpsLatitudeRef
        {
            get
            {
                if (this.BitmapMetadata.ContainsQuery(XmpExifQueries.GpsLatitude.Query))
                {
                    string gpsLatitudeRef = this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsLatitude.Query);

                    if (gpsLatitudeRef.EndsWith("N"))
                    {
                        return GpsCoordinate.LatitudeRef.North;
                    }
                    else if (gpsLatitudeRef.EndsWith("S"))
                    {
                        return GpsCoordinate.LatitudeRef.South;
                    }
                }

                return GpsCoordinate.LatitudeRef.NotSpecified;
            }

            set
            {
                if (this.ValueHasChanged(value, this.GpsLatitudeRef))
                {
                    if (value == GpsCoordinate.LatitudeRef.NotSpecified)
                    {
                        this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsLatitude.Query);
                    }
                    else
                    {
                        string gpsLatitude = this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsLatitude.Query);
                        gpsLatitude.TrimEnd('N');
                        gpsLatitude.TrimEnd('S');

                        if (value == GpsCoordinate.LatitudeRef.North)
                        {
                            this.BitmapMetadata.SetQuery(XmpExifQueries.GpsLatitude.Query, gpsLatitude + "N");
                        }
                        else if (value == GpsCoordinate.LatitudeRef.South)
                        {
                            this.BitmapMetadata.SetQuery(XmpExifQueries.GpsLatitude.Query, gpsLatitude + "S");
                        }
                    }
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
                GpsCoordinate.LongitudeRef gpsLongitudeRef = this.GpsLongitudeRef;

                // Do basic validation, 3 is minimum length
                if (gpsLongitudeRef != GpsCoordinate.LongitudeRef.NotSpecified && !string.IsNullOrEmpty(gpsLongitudeString) && gpsLongitudeString.Length > 3)
                {
                    // Split the string
                    string[] splitString = gpsLongitudeString.Substring(0, gpsLongitudeString.Length - 1).Split(',');

                    if (splitString.Length == 2)
                    {
                        double degrees;
                        double minutes;

                        if (Double.TryParse(splitString[0], out degrees) && Double.TryParse(splitString[1], out minutes))
                        {
                            return new GpsCoordinate(gpsLongitudeRef, degrees, minutes);
                        }
                    }
                    else if (splitString.Length == 3)
                    {
                        double degrees;
                        double minutes;
                        double seconds;

                        if (Double.TryParse(splitString[0], out degrees) && Double.TryParse(splitString[1], out minutes) && Double.TryParse(splitString[2], out seconds))
                        {
                            return new GpsCoordinate(gpsLongitudeRef, degrees, minutes, seconds);
                        }
                    }
                }

                return new GpsCoordinate();
            }

            set
            {
                if (this.ValueHasChanged(value, this.GpsLongitude))
                {
                    if (value.IsValidCoordinate)
                    {
                        string latitudeAsString = string.Format("{0},{1},{2}", value.Degrees, value.Minutes, value.Seconds);

                        this.BitmapMetadata.SetQuery(XmpExifQueries.GpsLongitude.Query, latitudeAsString);

                        this.GpsLongitudeRef = value.Numeric > 0 ? GpsCoordinate.LongitudeRef.East : GpsCoordinate.LongitudeRef.West;
                    }
                    else
                    {
                        this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsLongitude.Query);

                        this.GpsLongitudeRef = GpsCoordinate.LongitudeRef.NotSpecified;
                    }
                }
            }
        }

        public GpsCoordinate.LongitudeRef GpsLongitudeRef
        {
            get
            {
                if (this.BitmapMetadata.ContainsQuery(XmpExifQueries.GpsLongitude.Query))
                {
                    string gpsLongitudeRef = this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsLongitude.Query);

                    if (gpsLongitudeRef.EndsWith("E"))
                    {
                        return GpsCoordinate.LongitudeRef.East;
                    }
                    else if (gpsLongitudeRef.EndsWith("W"))
                    {
                        return GpsCoordinate.LongitudeRef.West;
                    }
                }

                return GpsCoordinate.LongitudeRef.NotSpecified;
            }

            set
            {
                if (this.ValueHasChanged(value, this.GpsLongitudeRef))
                {
                    if (value == GpsCoordinate.LongitudeRef.NotSpecified)
                    {
                        this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsLongitude.Query);
                    }
                    else
                    {
                        string gpsLongitude = this.BitmapMetadata.GetQuery<string>(XmpExifQueries.GpsLongitude.Query);
                        gpsLongitude.TrimEnd('E');
                        gpsLongitude.TrimEnd('W');

                        if (value == GpsCoordinate.LongitudeRef.West)
                        {
                            this.BitmapMetadata.SetQuery(XmpExifQueries.GpsLongitude.Query, gpsLongitude + "W");
                        }
                        else if (value == GpsCoordinate.LongitudeRef.East)
                        {
                            this.BitmapMetadata.SetQuery(XmpExifQueries.GpsLongitude.Query, gpsLongitude + "E");
                        }
                    }
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
                if (this.ValueHasChanged(value, this.GpsDateTimeStamp))
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
                if (this.ValueHasChanged(value, this.GpsVersionID))
                {
                    this.BitmapMetadata.SetQuery(XmpExifQueries.GpsVersionID.Query, value);
                }
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
                if (this.ValueHasChanged(value, this.GpsProcessingMethod))
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
                if (this.ValueHasChanged(value, this.GpsMeasureMode))
                {
                    switch (value)
                    {
                        default:
                        case GpsPosition.Dimensions.NotSpecified:
                            this.BitmapMetadata.SetQuery(XmpExifQueries.GpsMeasureMode.Query, string.Empty);
                            break;

                        case GpsPosition.Dimensions.ThreeDimensional:
                            this.BitmapMetadata.SetQuery(XmpExifQueries.GpsMeasureMode.Query, "3");
                            break;

                        case GpsPosition.Dimensions.TwoDimensional:
                            this.BitmapMetadata.SetQuery(XmpExifQueries.GpsMeasureMode.Query, "2");
                            break;
                    }
                }
            }
        }
    }
}