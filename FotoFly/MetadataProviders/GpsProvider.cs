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
        {
        }

        /// <summary>
        /// GpsPosition, encapculates Latitude, Longitude, Altitude, Source, Dimension and SatelliteTime
        /// </summary>
        public GpsPosition GpsPositionOfLocationCreated
        {
            get
            {
                GpsPosition gpsPosition = new GpsPosition();
                gpsPosition.Latitude = this.Latitude.Clone() as GpsCoordinate;
                gpsPosition.Longitude = this.Longitude.Clone() as GpsCoordinate;
                gpsPosition.Altitude = this.Altitude;
                gpsPosition.Source = this.ProcessingMethod;
                gpsPosition.SatelliteTime = this.DateTimeStamp;

                return gpsPosition;
            }

            set
            {
                if (value.IsValidCoordinate)
                {
                    this.Altitude = value.Altitude;
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
                    this.ProcessingMethod = null;
                    this.MeasureMode = GpsPosition.Dimensions.NotSpecified;
                }
            }
        }

        /// <summary>
        /// GpsPosition, encapculates Latitude, Longitude, Altitude, Source, Dimension and SatelliteTime
        /// </summary>
        public GpsPosition GpsPositionOfLocationShown
        {
            get
            {
                GpsPosition gpsPosition = new GpsPosition();
                gpsPosition.Latitude = this.DestLatitude.Clone() as GpsCoordinate;
                gpsPosition.Longitude = this.DestLongitude.Clone() as GpsCoordinate;
                gpsPosition.Altitude = Double.NaN;
                gpsPosition.Source = null;
                gpsPosition.SatelliteTime = new DateTime();

                return gpsPosition;
            }

            set
            {
                if (value.IsValidCoordinate)
                {
                    this.DestLatitude = value.Latitude.Clone() as GpsCoordinate;
                    this.DestLongitude = value.Longitude.Clone() as GpsCoordinate;
                    this.VersionID = "2200";
                }
                else
                {
                    // Clear all properties
                    this.DestLatitude = new GpsCoordinate();
                    this.DestLongitude = new GpsCoordinate();
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
                GpsPosition.AltitudeRef altitudeRef = this.AltitudeRef;
                URational urational = this.BitmapMetadata.GetQuery<URational>(GpsQueries.Altitude.Query);

                if (altitudeRef == GpsPosition.AltitudeRef.NotSpecified || urational == null || double.IsNaN(urational.ToDouble()))
                {
                    return double.NaN;
                }
                else if (altitudeRef == GpsPosition.AltitudeRef.BelowSeaLevel)
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
                if (this.ValueHasChanged(value, this.Altitude))
                {
                    // Check to see if the values have changed
                    if (double.IsNaN(value))
                    {
                        // Blank out existing values
                        this.AltitudeRef = GpsPosition.AltitudeRef.NotSpecified;
                        this.BitmapMetadata.RemoveQuery(GpsQueries.Altitude.Query);
                    }
                    else
                    {
                        // Keep three decimal place of precision
                        URational urational = new URational(value, 3);

                        this.BitmapMetadata.SetQuery(GpsQueries.Altitude.Query, urational.ToUInt64());

                        this.AltitudeRef = value > 0 ? GpsPosition.AltitudeRef.AboveSeaLevel : GpsPosition.AltitudeRef.BelowSeaLevel;
                    }
                }
            }
        }

        public GpsPosition.AltitudeRef AltitudeRef
        {
            get
            {
                // 0 = Above sea level, 1 = Below sea level 
                if (this.BitmapMetadata.ContainsQuery(GpsQueries.AltitudeRef.Query))
                {
                    string gpsLatitudeRef = this.BitmapMetadata.GetQuery<string>(GpsQueries.AltitudeRef.Query);

                    if (gpsLatitudeRef == "1")
                    {
                        return GpsPosition.AltitudeRef.BelowSeaLevel;
                    }
                    else if (gpsLatitudeRef == "0")
                    {
                        return GpsPosition.AltitudeRef.AboveSeaLevel;
                    }
                }

                return GpsPosition.AltitudeRef.NotSpecified;
            }

            set
            {
                if (this.ValueHasChanged(value, this.AltitudeRef))
                {
                    if (value == GpsPosition.AltitudeRef.NotSpecified)
                    {
                        // Blank out existing values
                        this.BitmapMetadata.RemoveQuery(GpsQueries.AltitudeRef.Query);
                    }
                    else if (value == GpsPosition.AltitudeRef.AboveSeaLevel)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.AltitudeRef.Query, "0");
                    }
                    else if (value == GpsPosition.AltitudeRef.BelowSeaLevel)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.AltitudeRef.Query, "1");
                    }
                }
            }
        }

        /// <summary>
        /// Gps Location Created Latitude
        /// </summary>
        public GpsCoordinate Latitude
        {
            get
            {
                GpsCoordinate.LatitudeRef latitudeRef = this.LatitudeRef;

                URationalTriplet gpsRational = this.BitmapMetadata.GetQuery<URationalTriplet>(GpsQueries.Latitude.Query);

                if (latitudeRef != GpsCoordinate.LatitudeRef.NotSpecified && gpsRational != null)
                {
                    return new GpsCoordinate(latitudeRef, gpsRational.ToDouble());
                }

                return new GpsCoordinate();
            }

            set
            {
                if (this.ValueHasChanged(value, this.Latitude))
                {
                    if (value.IsValidCoordinate)
                    {
                        this.LatitudeRef = value.Numeric > 0 ? GpsCoordinate.LatitudeRef.North : GpsCoordinate.LatitudeRef.South;

                        URationalTriplet gpsRational = new URationalTriplet(value.Degrees, value.Minutes, value.Seconds);
                        this.BitmapMetadata.SetQuery(GpsQueries.Latitude.Query, gpsRational.ToUlongArray(true));
                    }
                    else
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.Latitude.Query);

                        this.LatitudeRef = GpsCoordinate.LatitudeRef.NotSpecified;
                    }
                }
            }
        }

        public GpsCoordinate.LatitudeRef LatitudeRef
        {
            get
            {
                if (this.BitmapMetadata.ContainsQuery(GpsQueries.LatitudeRef.Query))
                {
                    string gpsLatitudeRef = this.BitmapMetadata.GetQuery<string>(GpsQueries.LatitudeRef.Query);

                    if (gpsLatitudeRef == "N")
                    {
                        return GpsCoordinate.LatitudeRef.North;
                    }
                    else if (gpsLatitudeRef == "S")
                    {
                        return GpsCoordinate.LatitudeRef.South;
                    }
                }

                return GpsCoordinate.LatitudeRef.NotSpecified;
            }

            set
            {
                if (this.ValueHasChanged(value, this.Latitude))
                {
                    if (value == GpsCoordinate.LatitudeRef.NotSpecified)
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.LatitudeRef.Query);
                    }
                    else if (value == GpsCoordinate.LatitudeRef.North)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.LatitudeRef.Query, "N");
                    }
                    else if (value == GpsCoordinate.LatitudeRef.South)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.LatitudeRef.Query, "S");
                    }
                }
            }
        }

        /// <summary>
        /// Gps Location Created Longitude
        /// </summary>
        public GpsCoordinate Longitude
        {
            get
            {
                GpsCoordinate.LongitudeRef longitudeRef = this.LongitudeRef;

                URationalTriplet gpsRational = this.BitmapMetadata.GetQuery<URationalTriplet>(GpsQueries.Longitude.Query);

                if (longitudeRef != GpsCoordinate.LongitudeRef.NotSpecified && gpsRational != null)
                {
                    return new GpsCoordinate(longitudeRef, gpsRational.ToDouble());
                }

                return new GpsCoordinate();
            }

            set
            {
                if (this.ValueHasChanged(value, this.Longitude))
                {
                    if (value.IsValidCoordinate)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.LongitudeRef.Query, value.Ref);

                        URationalTriplet gpsRational = new URationalTriplet(value.Degrees, value.Minutes, value.Seconds);

                        this.BitmapMetadata.SetQuery(GpsQueries.Longitude.Query, gpsRational.ToUlongArray(true));
                    }
                    else
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.Longitude.Query);
                        this.BitmapMetadata.RemoveQuery(GpsQueries.LongitudeRef.Query);
                    }
                }
            }
        }

        public GpsCoordinate.LongitudeRef LongitudeRef
        {
            get
            {
                if (this.BitmapMetadata.ContainsQuery(GpsQueries.LongitudeRef.Query))
                {
                    string gpsLongitudeRef = this.BitmapMetadata.GetQuery<string>(GpsQueries.LongitudeRef.Query);

                    if (gpsLongitudeRef == "E")
                    {
                        return GpsCoordinate.LongitudeRef.East;
                    }
                    else if (gpsLongitudeRef == "W")
                    {
                        return GpsCoordinate.LongitudeRef.West;
                    }
                }

                return GpsCoordinate.LongitudeRef.NotSpecified;
            }

            set
            {
                if (this.ValueHasChanged(value, this.Longitude))
                {
                    if (value == GpsCoordinate.LongitudeRef.NotSpecified)
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.LongitudeRef.Query);
                    }
                    else if (value == GpsCoordinate.LongitudeRef.West)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.LongitudeRef.Query, "W");
                    }
                    else if (value == GpsCoordinate.LongitudeRef.East)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.LongitudeRef.Query, "E");
                    }
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
                if (this.BitmapMetadata.IsQueryValidAndOfType(GpsQueries.MeasureMode.Query, GpsQueries.MeasureMode.BitmapMetadataType))
                {
                    int returnValue = this.BitmapMetadata.GetQuery<int>(GpsQueries.MeasureMode.Query);

                    if (returnValue == 1)
                    {
                        return GpsPosition.Dimensions.OneDimensional;
                    }
                    else if (returnValue == 2)
                    {
                        return GpsPosition.Dimensions.TwoDimensional;
                    }
                    else if (returnValue == 3)
                    {
                        return GpsPosition.Dimensions.ThreeDimensional;
                    }
                }

                return GpsPosition.Dimensions.NotSpecified;
            }

            set
            {
                if (this.ValueHasChanged(value, this.MeasureMode))
                {
                    switch (value)
                    {
                        default:
                        case GpsPosition.Dimensions.NotSpecified:
                            this.BitmapMetadata.RemoveQuery(GpsQueries.MeasureMode.Query);
                            break;

                        case GpsPosition.Dimensions.OneDimensional:
                            this.BitmapMetadata.SetQuery(GpsQueries.MeasureMode.Query, 1);
                            break;

                        case GpsPosition.Dimensions.TwoDimensional:
                            this.BitmapMetadata.SetQuery(GpsQueries.MeasureMode.Query, 2);
                            break;

                        case GpsPosition.Dimensions.ThreeDimensional:
                            this.BitmapMetadata.SetQuery(GpsQueries.MeasureMode.Query, 3);
                            break;
                    }
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
                return this.DateStamp.AddTicks(this.TimeStamp.Ticks);
            }

            set
            {
                this.DateStamp = value.Date;
                this.TimeStamp = value.TimeOfDay;
            }
        }

        /// <summary>
        /// Gps Date Stamp
        /// </summary>
        public DateTime DateStamp
        {
            get
            {
                try
                {
                    string date = this.BitmapMetadata.GetQuery<string>(GpsQueries.DateStamp.Query);

                    if (!string.IsNullOrEmpty(date))
                    {
                        string[] dateParts = date.Split(':');

                        return new DateTime(Convert.ToInt32(dateParts[0]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]));
                    }
                }
                catch
                {
                }

                return new DateTime();
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateStamp))
                {
                    if (value != new DateTime())
                    {
                        string date = value.ToString("yyyy:MM:dd");

                        this.BitmapMetadata.SetQuery(GpsQueries.DateStamp.Query, date);
                    }
                    else
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.DateStamp.Query);
                    }
                }
            }
        }

        /// <summary>
        /// Gps Time stamp
        /// </summary>
        public TimeSpan TimeStamp
        {
            get
            {
                try
                {
                    URationalTriplet time = this.BitmapMetadata.GetQuery<URationalTriplet>(GpsQueries.TimeStamp.Query);

                    if (time != null)
                    {
                        return new TimeSpan(time.A.ToInt(), time.B.ToInt(), time.C.ToInt());
                    }
                }
                catch
                {
                }

                return new TimeSpan();
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateTimeStamp))
                {
                    if (value != new TimeSpan())
                    {
                        URationalTriplet time = new URationalTriplet(value.Hours, value.Minutes, value.Seconds);

                        this.BitmapMetadata.SetQuery(GpsQueries.TimeStamp.Query, time.ToUlongArray(false));
                    }
                    else
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.TimeStamp.Query);
                    }
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
                if (this.ValueHasChanged(value, this.VersionID))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.VersionID.Query);
                    }
                    else if (value == "2200")
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.VersionID.Query, "2200");
                    }
                    else
                    {
                        throw new Exception("Invalid Gps.VersionID, must be equal to 2200");
                    }
                }
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
                if (this.ValueHasChanged(value, this.ProcessingMethod))
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

        /// <summary>
        /// Gps Location Show Latitude
        /// </summary>
        public GpsCoordinate DestLatitude
        {
            get
            {
                GpsCoordinate.LatitudeRef latitudeRef = this.DestLatitudeRef;

                URationalTriplet gpsRational = this.BitmapMetadata.GetQuery<URationalTriplet>(GpsQueries.DestLatitude.Query);

                if (latitudeRef != GpsCoordinate.LatitudeRef.NotSpecified && gpsRational != null)
                {
                    return new GpsCoordinate(latitudeRef, gpsRational.ToDouble());
                }

                return new GpsCoordinate();
            }

            set
            {
                if (this.ValueHasChanged(value, this.DestLatitude))
                {
                    if (value.IsValidCoordinate)
                    {
                        this.DestLatitudeRef = value.Numeric > 0 ? GpsCoordinate.LatitudeRef.North : GpsCoordinate.LatitudeRef.South;

                        URationalTriplet gpsRational = new URationalTriplet(value.Degrees, value.Minutes, value.Seconds);
                        this.BitmapMetadata.SetQuery(GpsQueries.DestLatitude.Query, gpsRational.ToUlongArray(true));
                    }
                    else
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.DestLatitude.Query);

                        this.DestLatitudeRef = GpsCoordinate.LatitudeRef.NotSpecified;
                    }
                }
            }
        }

        public GpsCoordinate.LatitudeRef DestLatitudeRef
        {
            get
            {
                if (this.BitmapMetadata.ContainsQuery(GpsQueries.DestLatitudeRef.Query))
                {
                    string gpsDestLatitudeRef = this.BitmapMetadata.GetQuery<string>(GpsQueries.DestLatitudeRef.Query);

                    if (gpsDestLatitudeRef == "N")
                    {
                        return GpsCoordinate.LatitudeRef.North;
                    }
                    else if (gpsDestLatitudeRef == "S")
                    {
                        return GpsCoordinate.LatitudeRef.South;
                    }
                }

                return GpsCoordinate.LatitudeRef.NotSpecified;
            }

            set
            {
                if (this.ValueHasChanged(value, this.Latitude))
                {
                    if (value == GpsCoordinate.LatitudeRef.NotSpecified)
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.DestLatitudeRef.Query);
                    }
                    else if (value == GpsCoordinate.LatitudeRef.North)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.DestLatitudeRef.Query, "N");
                    }
                    else if (value == GpsCoordinate.LatitudeRef.South)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.DestLatitudeRef.Query, "S");
                    }
                }
            }
        }

        /// <summary>
        /// /// Gps Location Show Longitude
        /// </summary>
        public GpsCoordinate DestLongitude
        {
            get
            {
                GpsCoordinate.LongitudeRef longitudeRef = this.DestLongitudeRef;

                URationalTriplet gpsRational = this.BitmapMetadata.GetQuery<URationalTriplet>(GpsQueries.DestLongitude.Query);

                if (longitudeRef != GpsCoordinate.LongitudeRef.NotSpecified && gpsRational != null)
                {
                    return new GpsCoordinate(longitudeRef, gpsRational.ToDouble());
                }

                return new GpsCoordinate();
            }

            set
            {
                if (this.ValueHasChanged(value, this.DestLongitude))
                {
                    if (value.IsValidCoordinate)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.DestLongitudeRef.Query, value.Ref);

                        URationalTriplet gpsRational = new URationalTriplet(value.Degrees, value.Minutes, value.Seconds);

                        this.BitmapMetadata.SetQuery(GpsQueries.DestLongitude.Query, gpsRational.ToUlongArray(true));
                    }
                    else
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.DestLongitude.Query);
                        this.BitmapMetadata.RemoveQuery(GpsQueries.DestLongitudeRef.Query);
                    }
                }
            }
        }

        public GpsCoordinate.LongitudeRef DestLongitudeRef
        {
            get
            {
                if (this.BitmapMetadata.ContainsQuery(GpsQueries.DestLongitudeRef.Query))
                {
                    string gpsDestLongitudeRef = this.BitmapMetadata.GetQuery<string>(GpsQueries.DestLongitudeRef.Query);

                    if (gpsDestLongitudeRef == "E")
                    {
                        return GpsCoordinate.LongitudeRef.East;
                    }
                    else if (gpsDestLongitudeRef == "W")
                    {
                        return GpsCoordinate.LongitudeRef.West;
                    }
                }

                return GpsCoordinate.LongitudeRef.NotSpecified;
            }

            set
            {
                if (this.ValueHasChanged(value, this.Longitude))
                {
                    if (value == GpsCoordinate.LongitudeRef.NotSpecified)
                    {
                        this.BitmapMetadata.RemoveQuery(GpsQueries.DestLongitudeRef.Query);
                    }
                    else if (value == GpsCoordinate.LongitudeRef.West)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.DestLongitudeRef.Query, "W");
                    }
                    else if (value == GpsCoordinate.LongitudeRef.East)
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.DestLongitudeRef.Query, "E");
                    }
                }
            }
        }
    }
}
