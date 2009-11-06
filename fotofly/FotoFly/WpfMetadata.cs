namespace FotoFly
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Globalization;

    public class WpfMetadata : IImageMetadata
    {
        protected WpfBitmapMetadataExtender bitmapMetadataExtender;

        public BitmapMetadata BitmapMetadata
        {
            get
            {
                return this.bitmapMetadataExtender.BitmapMetadata;
            }
        }

        public WpfMetadata()
        {
            this.bitmapMetadataExtender = new WpfBitmapMetadataExtender(new BitmapMetadata("jpg"));
        }

        public WpfMetadata(BitmapMetadata bitmapMetadata)
        {
            this.bitmapMetadataExtender = new WpfBitmapMetadataExtender(bitmapMetadata);
        }

        public string Aperture
        {
            get
            {
                URational urational = this.bitmapMetadataExtender.QueryMetadataForURational(WpfQueries.ExifAperture);

                if (urational != null)
                {
                    return "f/" + urational.ToDouble().ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        // / <summary>
        // / Author (also known as Photographer)
        // / </summary>
        public PeopleList Authors
        {
            get
            {
                ReadOnlyCollection<string> readOnlyCollectionString = this.bitmapMetadataExtender.BitmapMetadata.Author;

                if (readOnlyCollectionString == null || readOnlyCollectionString.Count == 0)
                {
                    return new PeopleList();
                }
                else
                {
                    return new PeopleList(readOnlyCollectionString);
                }
            }

            set
            {
                this.bitmapMetadataExtender.BitmapMetadata.Author = new ReadOnlyCollection<string>(value);
            }
        }

        public double Brightness
        {
            get
            {
                try
                {
                    Rational rational = this.bitmapMetadataExtender.QueryMetadataForRational(WpfQueries.ExifBrightness);

                    if (rational != null)
                    {
                        return Math.Round(rational.ToDouble(), 4);
                    }
                    else
                    {
                        return double.NaN;
                    }
                }
                catch
                {
                    return double.NaN;
                }
            }
        }

        // / <summary>
        // / Comment (also know as description)
        // / </summary>
        public string Comment
        {
            get
            {
                string formattedString = String.Empty;

                try
                {
                    formattedString = this.bitmapMetadataExtender.BitmapMetadata.Comment;
                }
                catch
                {
                    formattedString = String.Empty;
                }

                return formattedString;
            }

            set
            {
                if (value == null)
                {
                    this.bitmapMetadataExtender.BitmapMetadata.Comment = string.Empty;
                }
                else
                {
                    this.bitmapMetadataExtender.BitmapMetadata.Comment = value;
                }
            }
        }

        public string CameraManufacturer
        {
            get
            {
                string formattedString = String.Empty;

                try
                {
                    formattedString = this.bitmapMetadataExtender.BitmapMetadata.CameraManufacturer;
                }
                catch
                {
                    formattedString = String.Empty;
                }

                return formattedString;
            }

            set
            {
                if (value == null)
                {
                    this.bitmapMetadataExtender.BitmapMetadata.CameraManufacturer = string.Empty;
                }
                else
                {
                    this.bitmapMetadataExtender.BitmapMetadata.CameraManufacturer = value;
                }
            }
        }

        public string CameraModel
        {
            get
            {
                string cameraModel = this.bitmapMetadataExtender.BitmapMetadata.CameraModel;

                if (string.IsNullOrEmpty(cameraModel))
                {
                    object cameraModelArray = this.bitmapMetadataExtender.QueryMetadataForObject(WpfQueries.ExifCameraModel);

                    if (cameraModelArray is string[])
                    {
                        cameraModel = (cameraModelArray as string[])[0];
                    }
                }

                return cameraModel;
            }

            set
            {
                if (value == null)
                {
                    this.bitmapMetadataExtender.BitmapMetadata.CameraModel = string.Empty;
                }
                else
                {
                    this.bitmapMetadataExtender.BitmapMetadata.CameraModel = value;
                }
            }
        }

        public string Copyright
        {
            get
            {
                string formattedString = string.Empty;

                try
                {
                    formattedString = this.bitmapMetadataExtender.BitmapMetadata.Copyright;
                }
                catch
                {
                    return String.Empty;
                }

                return formattedString;
            }

            set
            {
                if (value == null)
                {
                    this.bitmapMetadataExtender.BitmapMetadata.Copyright = string.Empty;
                }
                else
                {
                    this.bitmapMetadataExtender.BitmapMetadata.Copyright = value;
                }
            }
        }

        public string CreationSoftware
        {
            get
            {
                return this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.ExifCreationSoftware);
            }

            set
            {
                this.bitmapMetadataExtender.SetProperty(WpfQueries.ExifCreationSoftware, value);
            }
        }

        public DateTime DateDigitised
        {
            get
            {
                DateTime? newDateTime = this.bitmapMetadataExtender.QueryMetadataForExifDateTime(WpfQueries.ExifDateDigitised);

                if (newDateTime == null)
                {
                    return new DateTime();
                }
                else
                {
                    return newDateTime.Value;
                }
            }

            set
            {
                // Convert to sortable date and convert to EXIF format
                // 2008:01:01 10:00:00
                string exifDate = value.ToString("s");

                exifDate = exifDate.Replace("-", ":");
                exifDate = exifDate.Replace("T", " ");

                this.bitmapMetadataExtender.SetProperty(WpfQueries.ExifDateDigitised, exifDate);
            }
        }

        public DateTime DateTaken
        {
            get
            {
                DateTime? datetimeTaken = Convert.ToDateTime(this.bitmapMetadataExtender.BitmapMetadata.DateTaken);

                if (datetimeTaken == null)
                {
                    return new DateTime();
                }
                else
                {
                    return datetimeTaken.Value;
                }
            }

            set
            {
                // Write to tempBitmap count
                // Use sortable string to avoid US date format issue with Month\Date
                this.bitmapMetadataExtender.BitmapMetadata.DateTaken = value.ToString("s");

                // Use specific format for EXIF data, 2008:12:01 13:14:10
                this.bitmapMetadataExtender.SetProperty(WpfQueries.ExifDateTaken, value.ToString("yyyy:MM:dd HH:mm:ss"));
            }
        }

        public string ExposureBias
        {
            get
            {
                try
                {
                    Rational rational = this.bitmapMetadataExtender.QueryMetadataForRational(WpfQueries.ExifExposureBias);

                    if (rational != null)
                    {
                        return Math.Round(rational.ToDouble(), 2) + " eV";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public string FocalLength
        {
            get
            {
                URational urational = this.bitmapMetadataExtender.QueryMetadataForURational(WpfQueries.ExifFocalLenght);

                if (urational != null)
                {
                    return urational.ToDouble() + " mm";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public double GpsAltitude
        {
            // Altitude is expressed as one RATIONAL count. The reference unit is meters
            // 0 = Above sea level, 1 = Below sea level 
            get
            {
                string gpsRef = this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.GpsAltitudeRef);
                URational urational = this.bitmapMetadataExtender.QueryMetadataForURational(WpfQueries.GpsAltitude);

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
                    this.bitmapMetadataExtender.ClearProperty(WpfQueries.GpsAltitudeRef);
                    this.bitmapMetadataExtender.ClearProperty(WpfQueries.GpsAltitude);
                }
                else
                {
                    // Keep three decimal place of precision
                    Rational rational = new Rational(value, 3);

                    string altRef;

                    if (value > 0)
                    {
                        altRef = "0";
                    }
                    else
                    {
                        altRef = "1";
                    }

                    this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsAltitudeRef, altRef);
                    this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsAltitude, rational.ToUlong());
                }
            }
        }

        public GpsCoordinate GpsLatitude
        {
            get
            {
                string gpsRef = this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.GpsLatitudeRef).ToUpper();
                GpsRational gpsRational = this.bitmapMetadataExtender.QueryMetadataForGpsRational(WpfQueries.GpsLatitude);

                if (gpsRef == "N" && gpsRational != null)
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
                    this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsLatitudeRef, value.Ref);

                    GpsRational gpsRational = new GpsRational(value.Degrees, value.Minutes, value.Seconds);

                    this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsLatitude, gpsRational.ToUlongArray(true));
                }
                else
                {
                    this.bitmapMetadataExtender.RemoveProperty(WpfQueries.GpsLatitude);
                    this.bitmapMetadataExtender.RemoveProperty(WpfQueries.GpsLatitudeRef);
                }
            }
        }

        public GpsCoordinate GpsLongitude
        {
            get
            {
                string gpsRef = this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.GpsLongitudeRef).ToUpper();
                GpsRational gpsRational = this.bitmapMetadataExtender.QueryMetadataForGpsRational(WpfQueries.GpsLongitude);

                if (gpsRef == "E" && gpsRational != null)
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
                    this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsLongitudeRef, value.Ref);

                    GpsRational gpsRational = new GpsRational(value.Degrees, value.Minutes, value.Seconds);

                    this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsLongitude, gpsRational.ToUlongArray(true));
                }
                else
                {
                    this.bitmapMetadataExtender.RemoveProperty(WpfQueries.GpsLongitude);
                    this.bitmapMetadataExtender.RemoveProperty(WpfQueries.GpsLongitudeRef);
                }
            }
        }

        public GpsPosition.Dimensions GpsMeasureMode
        {
            get
            {
                string returnValue = this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.GpsMeasureMode);

                if (string.IsNullOrEmpty(returnValue))
                {
                    return GpsPosition.Dimensions.NotSpecified;
                }
                else if (returnValue == "2")
                {
                    return GpsPosition.Dimensions.TwoDimensional;
                }
                else if (returnValue == "3")
                {
                    return GpsPosition.Dimensions.ThreeDimensional;
                }
                else
                {
                    return GpsPosition.Dimensions.NotSpecified;
                }
            }

            set
            {
                switch (value)
                {
                    default:
                    case GpsPosition.Dimensions.NotSpecified:
                        this.bitmapMetadataExtender.RemoveProperty(WpfQueries.GpsMeasureMode);
                        break;

                    case GpsPosition.Dimensions.ThreeDimensional:
                        this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsMeasureMode, 3);
                        break;

                    case GpsPosition.Dimensions.TwoDimensional:
                        this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsMeasureMode, 2);
                        break;
                }
            }
        }

        public DateTime GpsDateTimeStamp
        {
            get
            {
                try
                {
                    GpsRational time = this.bitmapMetadataExtender.QueryMetadataForGpsRational(WpfQueries.GpsTimeStamp);
                    string date = this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.GpsDateStamp);

                    if (time != null && !string.IsNullOrEmpty(date))
                    {
                        string[] dateParts = date.Split(':');

                        return new DateTime(Convert.ToInt32(dateParts[0]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]), time.Hours.ToInt(), time.Minutes.ToInt(), time.Seconds.ToInt());
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
                    GpsRational time = new GpsRational(value.Hour, value.Minute, value.Second);
                    string date = value.ToString("yyyy:MM:dd");

                    this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsDateStamp, date);
                    this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsTimeStamp, time.ToUlongArray(false));
                }
                else
                {
                    this.bitmapMetadataExtender.ClearProperty(WpfQueries.GpsDateStamp);
                    this.bitmapMetadataExtender.ClearProperty(WpfQueries.GpsTimeStamp);
                }
            }
        }

        public string GpsVersionID
        {
            get
            {
                return this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.GpsVersionID);
            }

            set
            {
                this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsVersionID, value);
            }
        }

        public string GpsProcessingMethod
        {
            get
            {
                if (string.IsNullOrEmpty(this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.GpsProcessingMethod)))
                {
                    return "None";
                }
                else
                {
                    return this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.GpsProcessingMethod);
                }
            }

            set
            {
                if (value == "None")
                {
                    this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsProcessingMethod, string.Empty);
                }
                else
                {
                    this.bitmapMetadataExtender.SetProperty(WpfQueries.GpsProcessingMethod, value);
                }
            }
        }

        public string Iso
        {
            get
            {
                uint? iso = this.bitmapMetadataExtender.QueryMetadataForUint(WpfQueries.ExifIso);

                if (iso == null)
                {
                    return string.Empty;
                }
                else if (iso.Value > 0 && iso.Value < 10000)
                {
                    return "ISO-" + iso.Value.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                this.bitmapMetadataExtender.SetProperty(WpfQueries.ExifIso, value.Replace("ISO-", string.Empty));
            }
        }

        public TagList Tags
        {
            get
            {
                return new TagList(this.bitmapMetadataExtender.BitmapMetadata.Keywords);
            }

            set
            {
                this.bitmapMetadataExtender.BitmapMetadata.Keywords = new ReadOnlyCollection<string>(value.ToReadOnlyCollection());
            }
        }

        public Address IptcAddress
        {
            get
            {
                Address address = new Address();
                address.Country = this.PlaceCountry;
                address.Region = this.PlaceRegion;
                address.City = this.PlaceCity;
                address.AddressLine = this.PlaceSubLocation;

                return address;
            }

            set
            {
                this.PlaceCountry = value.Country;
                this.PlaceRegion = value.Region;
                this.PlaceCity = value.City;
                this.PlaceSubLocation = value.AddressLine;
            }
        }

        public string PlaceCity
        {
            get
            {
                string returnValue = this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.IptcCity);

                if (string.IsNullOrEmpty(returnValue))
                {
                    return string.Empty;
                }
                else
                {
                    return returnValue;
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.bitmapMetadataExtender.RemoveProperty(WpfQueries.IptcCity);
                }
                else
                {
                    this.bitmapMetadataExtender.SetProperty(WpfQueries.IptcCity, value);
                }
            }
        }

        public string PlaceCountry
        {
            get
            {
                string returnValue = this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.IptcCountry);

                if (string.IsNullOrEmpty(returnValue))
                {
                    return string.Empty;
                }
                else
                {
                    return returnValue;
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.bitmapMetadataExtender.RemoveProperty(WpfQueries.IptcCountry);
                }
                else
                {
                    this.bitmapMetadataExtender.SetProperty(WpfQueries.IptcCountry, value);
                }
            }
        }

        public string PlaceRegion
        {
            get
            {
                string returnValue = this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.IptcRegion);

                if (string.IsNullOrEmpty(returnValue))
                {
                    return string.Empty;
                }
                else
                {
                    return returnValue;
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.bitmapMetadataExtender.RemoveProperty(WpfQueries.IptcRegion);
                }
                else
                {
                    this.bitmapMetadataExtender.SetProperty(WpfQueries.IptcRegion, value);
                }
            }
        }

        public string PlaceSubLocation
        {
            get
            {
                string returnValue = this.bitmapMetadataExtender.QueryMetadataForString(WpfQueries.IptcSubLocation);

                if (string.IsNullOrEmpty(returnValue))
                {
                    return string.Empty;
                }
                else
                {
                    return returnValue;
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.bitmapMetadataExtender.RemoveProperty(WpfQueries.IptcSubLocation);
                }
                else
                {
                    this.bitmapMetadataExtender.SetProperty(WpfQueries.IptcSubLocation, value);
                }
            }
        }

        public string ShutterSpeed
        {
            get
            {
                try
                {
                    Rational rational = this.bitmapMetadataExtender.QueryMetadataForRational(WpfQueries.ExifShutterSpeed);

                    if (rational != null)
                    {
                        double exposureTime = rational.ToDouble();

                        string formattedString = String.Empty;

                        if (exposureTime > 1)
                        {
                            formattedString = exposureTime.ToString();
                        }
                        else
                        {
                            // Convert Decimal to Integer
                            double newDecimal = System.Math.Round((1 / exposureTime), 0);

                            formattedString = newDecimal.ToString();

                            formattedString = "1/" + formattedString;
                        }

                        if (formattedString != String.Empty)
                        {
                            formattedString += " sec.";
                        }

                        return formattedString;
                    }
                }
                catch
                {
                }

                return String.Empty;
            }
        }

        public int Rating
        {
            get
            {
                int rating = this.bitmapMetadataExtender.BitmapMetadata.Rating;

                return rating;
            }

            set
            {
                if (value >= 0 || value <= 5)
                {
                    this.bitmapMetadataExtender.BitmapMetadata.Rating = value;
                }
                else
                {
                    throw new Exception("Invalid Rating");
                }
            }
        }

        public string Subject
        {
            get
            {
                if (string.IsNullOrEmpty(this.bitmapMetadataExtender.BitmapMetadata.Subject))
                {
                    return string.Empty;
                }
                else
                {
                    return this.bitmapMetadataExtender.BitmapMetadata.Subject;
                }
            }

            set
            {
                this.bitmapMetadataExtender.BitmapMetadata.Subject = value;
            }
        }

        public string Title
        {
            get
            {
                string formattedString = this.bitmapMetadataExtender.BitmapMetadata.Title;

                if (String.IsNullOrEmpty(formattedString))
                {
                    return string.Empty;
                }
                else
                {
                    return formattedString;
                }
            }

            set
            {
                this.bitmapMetadataExtender.BitmapMetadata.Title = value;
            }
        }

        public XmpRegionInfo RegionInfo
        {
            get
            {
                XmpRegionInfo regionInfo = new XmpRegionInfo();

                // Read Last Update Date
                if (this.bitmapMetadataExtender.QueryMetadataForDateTime(WpfQueries.MicrosoftRegionsLastUpdate) != null)
                {
                    regionInfo.LastUpdate = this.bitmapMetadataExtender.QueryMetadataForDateTime(WpfQueries.MicrosoftRegionsLastUpdate).Value;
                }

                // Read Each Region
                BitmapMetadata regionsMetadata = this.bitmapMetadataExtender.QueryMetadataForBitmapMetadata(WpfQueries.MicrosoftRegions);

                if (regionsMetadata != null)
                {
                    foreach (string regionQuery in regionsMetadata)
                    {
                        string regionFullQuery = WpfQueries.MicrosoftRegions + regionQuery;

                        BitmapMetadata regionMetadata = this.bitmapMetadataExtender.QueryMetadataForBitmapMetadata(regionFullQuery);

                        if (regionMetadata != null)
                        {
                            XmpRegion newRegion = new XmpRegion();

                            if (this.bitmapMetadataExtender.BitmapMetadata.ContainsQuery(WpfQueries.MicrosoftRectangle))
                            {
                                newRegion.RectangleString = this.bitmapMetadataExtender.BitmapMetadata.GetQuery(WpfQueries.MicrosoftRectangle).ToString();
                            }

                            if (this.bitmapMetadataExtender.BitmapMetadata.ContainsQuery(WpfQueries.MicrosoftPersonDisplayName))
                            {
                                newRegion.PersonDisplayName = this.bitmapMetadataExtender.BitmapMetadata.GetQuery(WpfQueries.MicrosoftPersonDisplayName).ToString();
                            }

                            if (this.bitmapMetadataExtender.BitmapMetadata.ContainsQuery(WpfQueries.MicrosoftPersonEmailDigest))
                            {
                                newRegion.PersonEmailDigest = this.bitmapMetadataExtender.BitmapMetadata.GetQuery(WpfQueries.MicrosoftPersonEmailDigest).ToString();
                            }

                            if (this.bitmapMetadataExtender.BitmapMetadata.ContainsQuery(WpfQueries.MicrosoftPersonLiveIdCID))
                            {
                                newRegion.PersonLiveIdCID = this.bitmapMetadataExtender.BitmapMetadata.GetQuery(WpfQueries.MicrosoftPersonLiveIdCID).ToString();
                            }

                            regionInfo.Regions.Add(newRegion);
                        }
                    }
                }

                return regionInfo;
            }

            set
            {
                // This method is distructive, it deletes all existing regions
                // In place editing would be complicated because each region doesn't have a unique IDs
                // In theory the new data is based on the existing data so nothing should be lost
                // Except where new properties exist that aren't in the XMPRegion class

                // Delete RegionInfo if there's no Regions
                if (value != null && value.Regions.Count > 0)
                {
                    // Check for RegionInfo Struct, if none exists, create it
                    if (!this.bitmapMetadataExtender.BitmapMetadata.ContainsQuery(WpfQueries.MicrosoftRegionInfo))
                    {
                        this.bitmapMetadataExtender.BitmapMetadata.SetQuery(WpfQueries.MicrosoftRegionInfo, new BitmapMetadata(WpfQueries.XmpStruct));
                    }

                    // Set LastUpdate if it's been set
                    if (value.LastUpdate != new DateTime())
                    {
                        this.bitmapMetadataExtender.BitmapMetadata.SetQuery(WpfQueries.MicrosoftRegionsLastUpdate, value.LastUpdate.ToUniversalTime());
                    }

                    // Check for Regions Bag, if none exists, create it
                    if (!this.bitmapMetadataExtender.BitmapMetadata.ContainsQuery(WpfQueries.MicrosoftRegions))
                    {
                        this.bitmapMetadataExtender.BitmapMetadata.SetQuery(WpfQueries.MicrosoftRegions, new BitmapMetadata(WpfQueries.XmpBag));
                    }

                    // If Region count has changed, clear our existing regions and create empty regions
                    if (value.Regions.Count != this.RegionInfo.Regions.Count)
                    {
                        // Delete any 'extra' regions
                        for (int i = value.Regions.Count; i < this.RegionInfo.Regions.Count; i++)
                        {
                            // Build query for current region
                            string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, WpfQueries.MicrosoftRegion, i.ToString());

                            this.bitmapMetadataExtender.BitmapMetadata.RemoveQuery(currentRegionQuery);
                        }

                        // Add any additional regions
                        for (int i = this.RegionInfo.Regions.Count; i < value.Regions.Count; i++)
                        {
                            // Build query for current region
                            string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, WpfQueries.MicrosoftRegion, i.ToString());

                            this.bitmapMetadataExtender.BitmapMetadata.SetQuery(currentRegionQuery, new BitmapMetadata(WpfQueries.XmpStruct));
                        }
                    }

                    int currentRegion = 0;

                    // Loop through each Region
                    foreach (XmpRegion xmpRegion in value.Regions)
                    {
                        // Build query for current region
                        string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, WpfQueries.MicrosoftRegion, currentRegion);

                        // Update or clear PersonDisplayName
                        if (string.IsNullOrEmpty(xmpRegion.PersonDisplayName))
                        {
                            this.bitmapMetadataExtender.BitmapMetadata.RemoveQuery(currentRegionQuery + WpfQueries.MicrosoftPersonDisplayName);
                        }
                        else
                        {
                            this.bitmapMetadataExtender.BitmapMetadata.SetQuery(currentRegionQuery + WpfQueries.MicrosoftPersonDisplayName, xmpRegion.PersonDisplayName);
                        }

                        // Update or clear PersonEmailDigest
                        if (string.IsNullOrEmpty(xmpRegion.PersonEmailDigest))
                        {
                            this.bitmapMetadataExtender.BitmapMetadata.RemoveQuery(currentRegionQuery + WpfQueries.MicrosoftPersonEmailDigest);
                        }
                        else
                        {
                            this.bitmapMetadataExtender.BitmapMetadata.SetQuery(currentRegionQuery + WpfQueries.MicrosoftPersonEmailDigest, xmpRegion.PersonEmailDigest);
                        }

                        // Update or clear PersonLiveIdCID
                        if (string.IsNullOrEmpty(xmpRegion.PersonLiveIdCID))
                        {
                            this.bitmapMetadataExtender.BitmapMetadata.RemoveQuery(currentRegionQuery + WpfQueries.MicrosoftPersonLiveIdCID);
                        }
                        else
                        {
                            this.bitmapMetadataExtender.BitmapMetadata.SetQuery(currentRegionQuery + WpfQueries.MicrosoftPersonLiveIdCID, xmpRegion.PersonLiveIdCID);
                        }

                        // Update or clear RectangleString
                        if (string.IsNullOrEmpty(xmpRegion.RectangleString))
                        {
                            this.bitmapMetadataExtender.BitmapMetadata.RemoveQuery(currentRegionQuery + WpfQueries.MicrosoftRectangle);
                        }
                        else
                        {
                            this.bitmapMetadataExtender.BitmapMetadata.SetQuery(currentRegionQuery + WpfQueries.MicrosoftRectangle, xmpRegion.RectangleString);
                        }

                        currentRegion++;
                    }
                }
                else
                {
                    // Delete existing RegionInfos, deletes all child data
                    if (this.bitmapMetadataExtender.BitmapMetadata.ContainsQuery(WpfQueries.MicrosoftRegionInfo))
                    {
                        this.bitmapMetadataExtender.BitmapMetadata.RemoveQuery(WpfQueries.MicrosoftRegionInfo);
                    }
                }
            }
        }

        public int Height
        {
            get
            {
                ////return this.bitmapMetadataExtender.BitmapDecoder.Frames[0].PixelHeight;
                return 0;
            }
        }

        public int Width
        {
            get
            {
                ////return this.bitmapMetadataExtender.BitmapDecoder.Frames[0].PixelWidth;
                return 0;
            }
        }
    }
}
