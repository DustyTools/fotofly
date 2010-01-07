// <copyright file="WpfMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>WpfMetadata Class</summary>
namespace FotoFly.WpfTools
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using FotoFly.MetadataQueries;

    public class WpfMetadata : IPhotoMetadata, IDisposable
    {
        public WpfMetadata()
        {
        }

        public WpfMetadata(BitmapMetadata bitmapMetadata)
        {
            this.BitmapMetadata = bitmapMetadata;
        }

        public BitmapMetadata BitmapMetadata
        {
            get;
            set;
        }

        /// <summary>
        /// Aperture
        /// </summary>
        public string Aperture
        {
            get
            {
                URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.Aperture.Query);

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

        /// <summary>
        /// List of Authors, also known as Photographer
        /// </summary>
        public PeopleList Authors
        {
            get
            {
                ReadOnlyCollection<string> readOnlyCollectionString = this.BitmapMetadata.Author;

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
                this.BitmapMetadata.Author = new ReadOnlyCollection<string>(value);
            }
        }

        /// <summary>
        /// Comment, also known as Description
        /// </summary>
        public string Comment
        {
            get
            {
                string formattedString = String.Empty;

                try
                {
                    formattedString = this.BitmapMetadata.Comment;
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
                    this.BitmapMetadata.Comment = string.Empty;
                }
                else
                {
                    this.BitmapMetadata.Comment = value;
                }
            }
        }

        /// <summary>
        /// Camera Manufacturer
        /// </summary>
        public string CameraManufacturer
        {
            get
            {
                string formattedString = String.Empty;

                try
                {
                    formattedString = this.BitmapMetadata.CameraManufacturer;
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
                    this.BitmapMetadata.CameraManufacturer = string.Empty;
                }
                else
                {
                    this.BitmapMetadata.CameraManufacturer = value;
                }
            }
        }

        /// <summary>
        /// Camera Model, normally includes camera Manufacturer
        /// </summary>
        public string CameraModel
        {
            get
            {
                string cameraModel = this.BitmapMetadata.CameraModel;

                if (string.IsNullOrEmpty(cameraModel))
                {
                    object cameraModelArray = this.BitmapMetadata.GetQuery<object>(ExifQueries.CameraModel.Query);

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
                    this.BitmapMetadata.CameraModel = string.Empty;
                }
                else
                {
                    this.BitmapMetadata.CameraModel = value;
                }
            }
        }

        /// <summary>
        /// Copyright owner of the photo
        /// </summary>
        public string Copyright
        {
            get
            {
                string formattedString = string.Empty;

                try
                {
                    formattedString = this.BitmapMetadata.Copyright;
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
                    this.BitmapMetadata.Copyright = string.Empty;
                }
                else
                {
                    this.BitmapMetadata.Copyright = value;
                }
            }
        }

        /// <summary>
        /// Software used to last modify the photo
        /// </summary>
        public string CreationSoftware
        {
            get
            {
                return this.BitmapMetadata.ApplicationName;
            }

            set
            {
                this.BitmapMetadata.ApplicationName = value;
            }
        }

        /// <summary>
        /// DateDigitized, recorded by the camera when the photo is taken
        /// </summary>
        public DateTime DateDigitised
        {
            get
            {
                ExifDateTime exifDateTime = this.BitmapMetadata.GetQuery<ExifDateTime>(ExifQueries.DateDigitized.Query);

                if (exifDateTime == null)
                {
                    return new DateTime();
                }
                else
                {
                    return exifDateTime.ToDateTime();
                }
            }

            set
            {
                this.BitmapMetadata.SetQuery(ExifQueries.DateDigitized.Query, new ExifDateTime(value).ToExifString());
            }
        }

        /// <summary>
        /// DateAquired, Microsoft Windows 7 Property
        /// </summary>
        public DateTime DateAquired
        {
            get
            {
                DateTime dateAquired = this.BitmapMetadata.GetQuery<DateTime>(XmpMicrosoftQueries.DateAcquired.Query);

                if (dateAquired == null)
                {
                    return new DateTime();
                }
                else
                {
                    return dateAquired;
                }
            }

            set
            {
                string dateAquired = value.ToString("yyyy-MM-ddTHH:mm:ss");

                this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.DateAcquired.Query, dateAquired);
            }
        }

        /// <summary>
        /// DateTaken, recorded by the camera when the photo is taken
        /// </summary>
        public DateTime DateTaken
        {
            get
            {
                DateTime datetimeTaken = Convert.ToDateTime(this.BitmapMetadata.DateTaken);

                if (datetimeTaken == null)
                {
                    return new DateTime();
                }
                else
                {
                    return datetimeTaken;
                }
            }

            set
            {
                // Write to tempBitmap count
                // Use sortable string to avoid US date format issue with Month\Date
                this.BitmapMetadata.DateTaken = value.ToString("s");

                // Use specific format for EXIF data, 2008:12:01 13:14:10
                this.BitmapMetadata.SetQuery(ExifQueries.DateTaken.Query, value.ToString("yyyy:MM:dd HH:mm:ss"));
            }
        }

        /// <summary>
        /// DigitalZoomRatio
        /// </summary>
        public double DigitalZoomRatio
        {
            get
            {
                URational rational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.DigitalZoomRatio.Query);

                if (rational == null || rational.Numerator == 0)
                {
                    return 0;
                }
                else
                {
                    return rational.ToDouble();
                }
            }
        }

        /// <summary>
        /// Exposure Bias
        /// </summary>
        public string ExposureBias
        {
            get
            {
                SRational rational = this.BitmapMetadata.GetQuery<SRational>(ExifQueries.ExposureBias.Query);

                if (rational != null && rational.ToInt() != 0)
                {
                    if (rational.ToInt() > 0)
                    {
                        return "+" + Math.Round(rational.ToDouble(), 1) + " step";
                    }
                    else
                    {
                        return Math.Round(rational.ToDouble(), 1) + " step";
                    }
                }

                return "0 step";
            }
        }

        /// <summary>
        /// Focal Length
        /// </summary>
        public string FocalLength
        {
            get
            {
                URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.FocalLenght.Query);

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

        /// <summary>
        /// ISO Speed rating 
        /// </summary>
        public string Iso
        {
            get
            {
                if (this.BitmapMetadata.IsQueryOfType(ExifQueries.IsoSpeedRating.Query, ExifQueries.IsoSpeedRating.BitmapMetadataType))
                {
                    UInt16? iso = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.IsoSpeedRating.Query);

                    if (iso == null)
                    {
                        return string.Empty;
                    }
                    else if (iso.Value > 0 && iso.Value < 10000)
                    {
                        return "ISO-" + iso.Value.ToString();
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Horizontal Resolution
        /// </summary>
        public int HorizontalResolution
        {
            get
            {
                URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.HorizontalResolution.Query);

                if (urational == null || urational.Numerator == 0)
                {
                    return 0;
                }
                else
                {
                    return urational.ToInt();
                }
            }
        }

        /// <summary>
        /// List of Tags, sometimes known as Keywords
        /// </summary>
        public TagList Tags
        {
            get
            {
                return new TagList(this.BitmapMetadata.Keywords);
            }

            set
            {
                this.BitmapMetadata.Keywords = new ReadOnlyCollection<string>(value.ToReadOnlyCollection());
            }
        }

        /// <summary>
        /// Address as stored in IPTC fields
        /// </summary>
        public Address IptcAddress
        {
            get
            {
                Address address = new Address();
                address.Country = this.IptcCountry;
                address.Region = this.IptcRegion;
                address.City = this.IptcCity;
                address.AddressLine = this.IptcSubLocation;

                return address;
            }

            set
            {
                this.IptcCountry = value.Country;
                this.IptcRegion = value.Region;
                this.IptcCity = value.City;
                this.IptcSubLocation = value.AddressLine;
            }
        }

        /// <summary>
        /// Image Height measured in Pixels
        /// </summary>
        public int ImageHeight
        {
            get
            {
                int? imageHeight = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.ImageHeight.Query);

                if (imageHeight == null)
                {
                    return 0;
                }
                else
                {
                    return imageHeight.Value;
                }
            }
        }

        /// <summary>
        /// Image Width measured in Pixels
        /// </summary>
        public int ImageWidth
        {
            get
            {
                int? imageWidth = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.ImageWidth.Query);

                if (imageWidth == null)
                {
                    return 0;
                }
                else
                {
                    return imageWidth.Value;
                }
            }
        }

        /// <summary>
        /// Iptc City
        /// </summary>
        public string IptcCity
        {
            get
            {
                string returnValue = this.BitmapMetadata.GetQuery<string>(IptcQueries.City.Query);

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
                    this.BitmapMetadata.SetQuery(IptcQueries.City.Query, string.Empty);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(IptcQueries.City.Query, value);
                }
            }
        }

        /// <summary>
        /// Iptc County
        /// </summary>
        public string IptcCountry
        {
            get
            {
                string returnValue = this.BitmapMetadata.GetQuery<string>(IptcQueries.Country.Query);

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
                    this.BitmapMetadata.SetQuery(IptcQueries.Country.Query, string.Empty);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(IptcQueries.Country.Query, value);
                }
            }
        }

        /// <summary>
        /// Iptc Region, also used for State, County or Province
        /// </summary>
        public string IptcRegion
        {
            get
            {
                string returnValue = this.BitmapMetadata.GetQuery<string>(IptcQueries.Region.Query);

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
                    this.BitmapMetadata.SetQuery(IptcQueries.Region.Query, string.Empty);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(IptcQueries.Region.Query, value);
                }
            }
        }

        /// <summary>
        /// Iptc Sublocation
        /// </summary>
        public string IptcSubLocation
        {
            get
            {
                string returnValue = this.BitmapMetadata.GetQuery<string>(IptcQueries.SubLocation.Query);

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
                    this.BitmapMetadata.SetQuery(IptcQueries.SubLocation.Query, string.Empty);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(IptcQueries.SubLocation.Query, value);
                }
            }
        }

        /// <summary>
        /// Shutter Speed
        /// </summary>
        public string ShutterSpeed
        {
            get
            {
                try
                {
                    URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.ShutterSpeed.Query);

                    if (urational != null)
                    {
                        double exposureTime = urational.ToDouble();

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

        /// <summary>
        /// Rating (Ranging 0-5)
        /// </summary>
        public int Rating
        {
            get
            {
                int rating = this.BitmapMetadata.Rating;

                return rating;
            }

            set
            {
                if (value >= 0 || value <= 5)
                {
                    this.BitmapMetadata.Rating = value;
                }
                else
                {
                    throw new Exception("Invalid Rating");
                }
            }
        }

        /// <summary>
        /// Subject, not often used by software, Title should be used in most cases
        /// </summary>
        public string Subject
        {
            get
            {
                if (string.IsNullOrEmpty(this.BitmapMetadata.Subject))
                {
                    return string.Empty;
                }
                else
                {
                    return this.BitmapMetadata.Subject;
                }
            }

            set
            {
                this.BitmapMetadata.Subject = value;
            }
        }

        /// <summary>
        /// Vertical Resolution of Thumbnail
        /// </summary>
        public int ThumbnailVerticalResolution
        {
            get
            {
                URational rational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.ThumbnailVerticalResolution.Query);

                if (rational == null || rational.Numerator == 0)
                {
                    return 0;
                }
                else
                {
                    return rational.ToInt();
                }
            }
        }

        /// <summary>
        /// Horizontal Resolution of Thumbnail
        /// </summary>
        public int ThumbnailHorizontalResolution
        {
            get
            {
                URational rational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.ThumbnailHorizontalResolution.Query);

                if (rational == null || rational.Numerator == 0)
                {
                    return 0;
                }
                else
                {
                    return rational.ToInt();
                }
            }
        }

        /// <summary>
        /// Title, sometimes knows as Caption and Subject (there is also another attribute for Subject)
        /// </summary>
        public string Title
        {
            get
            {
                string formattedString = this.BitmapMetadata.Title;

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
                this.BitmapMetadata.Title = value;
            }
        }

        /// <summary>
        /// Microsoft Region Info extension which provides data on regions in the photo
        /// </summary>
        public XmpRegionInfo RegionInfo
        {
            get
            {
                XmpRegionInfo regionInfo = new XmpRegionInfo();

                // Read Each Region
                BitmapMetadata regionsMetadata = this.BitmapMetadata.GetQuery<BitmapMetadata>(XmpMicrosoftQueries.Regions.Query);

                if (regionsMetadata != null)
                {
                    foreach (string regionQuery in regionsMetadata)
                    {
                        string regionFullQuery = XmpMicrosoftQueries.Regions.Query + regionQuery;

                        BitmapMetadata regionMetadata = this.BitmapMetadata.GetQuery<BitmapMetadata>(regionFullQuery);

                        if (regionMetadata != null)
                        {
                            XmpRegion newRegion = new XmpRegion();

                            if (regionMetadata.ContainsQuery(XmpMicrosoftQueries.RegionRectangle.Query))
                            {
                                newRegion.RectangleString = regionMetadata.GetQuery(XmpMicrosoftQueries.RegionRectangle.Query).ToString();
                            }

                            if (regionMetadata.ContainsQuery(XmpMicrosoftQueries.RegionPersonDisplayName.Query))
                            {
                                newRegion.PersonDisplayName = regionMetadata.GetQuery(XmpMicrosoftQueries.RegionPersonDisplayName.Query).ToString();
                            }

                            if (regionMetadata.ContainsQuery(XmpMicrosoftQueries.RegionPersonEmailDigest.Query))
                            {
                                newRegion.PersonEmailDigest = regionMetadata.GetQuery(XmpMicrosoftQueries.RegionPersonEmailDigest.Query).ToString();
                            }

                            if (regionMetadata.ContainsQuery(XmpMicrosoftQueries.RegionPersonLiveIdCID.Query))
                            {
                                newRegion.PersonLiveIdCID = regionMetadata.GetQuery(XmpMicrosoftQueries.RegionPersonLiveIdCID.Query).ToString();
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
                    if (!this.BitmapMetadata.ContainsQuery(XmpMicrosoftQueries.RegionInfo.Query))
                    {
                        this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.RegionInfo.Query, new BitmapMetadata(XmpCoreQueries.StructBlock));
                    }

                    // Check for Regions Bag, if none exists, create it
                    if (!this.BitmapMetadata.ContainsQuery(XmpMicrosoftQueries.Regions.Query))
                    {
                        this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.Regions.Query, new BitmapMetadata(XmpCoreQueries.BagBlock));
                    }

                    // If Region count has changed, clear our existing regions and create empty regions
                    if (value.Regions.Count != this.RegionInfo.Regions.Count)
                    {
                        // Delete any 'extra' regions
                        for (int i = value.Regions.Count; i < this.RegionInfo.Regions.Count; i++)
                        {
                            // Build query for current region
                            string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, XmpMicrosoftQueries.Region.Query, i.ToString());

                            this.BitmapMetadata.RemoveQuery(currentRegionQuery);
                        }

                        // Add any additional regions
                        for (int i = this.RegionInfo.Regions.Count; i < value.Regions.Count; i++)
                        {
                            // Build query for current region
                            string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, XmpMicrosoftQueries.Region.Query, i.ToString());

                            this.BitmapMetadata.SetQuery(currentRegionQuery, new BitmapMetadata(XmpCoreQueries.StructBlock));
                        }
                    }

                    int currentRegion = 0;

                    // Loop through each Region
                    foreach (XmpRegion xmpRegion in value.Regions)
                    {
                        // Build query for current region
                        string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, XmpMicrosoftQueries.Region.Query, currentRegion);

                        // Update or clear PersonDisplayName
                        if (string.IsNullOrEmpty(xmpRegion.PersonDisplayName))
                        {
                            this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonDisplayName.Query);
                        }
                        else
                        {
                            this.BitmapMetadata.SetQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonDisplayName.Query, xmpRegion.PersonDisplayName);
                        }

                        // Update or clear PersonEmailDigest
                        if (string.IsNullOrEmpty(xmpRegion.PersonEmailDigest))
                        {
                            this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonEmailDigest.Query);
                        }
                        else
                        {
                            this.BitmapMetadata.SetQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonEmailDigest.Query, xmpRegion.PersonEmailDigest);
                        }

                        // Update or clear PersonLiveIdCID
                        if (string.IsNullOrEmpty(xmpRegion.PersonLiveIdCID))
                        {
                            this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonLiveIdCID.Query);
                        }
                        else
                        {
                            this.BitmapMetadata.SetQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonLiveIdCID.Query, xmpRegion.PersonLiveIdCID);
                        }

                        // Update or clear RectangleString
                        if (string.IsNullOrEmpty(xmpRegion.RectangleString))
                        {
                            this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpMicrosoftQueries.RegionRectangle.Query);
                        }
                        else
                        {
                            this.BitmapMetadata.SetQuery(currentRegionQuery + XmpMicrosoftQueries.RegionRectangle.Query, xmpRegion.RectangleString);
                        }

                        currentRegion++;
                    }
                }
                else
                {
                    // Delete existing RegionInfos, deletes all child data
                    if (this.BitmapMetadata.ContainsQuery(XmpMicrosoftQueries.RegionInfo.Query))
                    {
                        this.BitmapMetadata.RemoveQuery(XmpMicrosoftQueries.RegionInfo.Query);
                    }
                }
            }
        }

        /// <summary>
        /// Vertical Resolution of Main Photo
        /// </summary>
        public int VerticalResolution
        {
            get
            {
                URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.VerticalResolution.Query);

                if (urational == null || urational.Numerator == 0)
                {
                    return 0;
                }
                else
                {
                    return urational.ToInt();
                }
            }
        }

        /// <summary>
        /// ExposureMode
        /// </summary>
        public PhotoMetadataEnums.ExposureModes ExposureMode
        {
            get
            {
                UInt16? exposureMode = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.ExposureMode.Query);

                if (exposureMode == null)
                {
                    return PhotoMetadataEnums.ExposureModes.AutoExposure;
                }
                else
                {
                    return (PhotoMetadataEnums.ExposureModes)exposureMode.Value;
                }
            }
        }

        /// <summary>
        /// SubjectDistanceRange
        /// </summary>
        public PhotoMetadataEnums.SubjectDistanceRanges SubjectDistanceRange
        {
            get
            {
                uint? subjectDistanceRange = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.SubjectDistanceRange.Query);

                if (subjectDistanceRange == null)
                {
                    return PhotoMetadataEnums.SubjectDistanceRanges.Unknown;
                }
                else
                {
                    return (PhotoMetadataEnums.SubjectDistanceRanges)subjectDistanceRange.Value;
                }
            }
        }

        /// <summary>
        /// Metering Mode
        /// </summary>
        public PhotoMetadataEnums.MeteringModes MeteringMode
        {
            get
            {
                UInt16? meteringMode = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.MeteringMode.Query);

                if (meteringMode == null)
                {
                    return PhotoMetadataEnums.MeteringModes.Unknown;
                }
                else
                {
                    return (PhotoMetadataEnums.MeteringModes)meteringMode.Value;
                }
            }
        }

        /// <summary>
        /// Orientation
        /// </summary>
        public PhotoMetadataEnums.Orientations Orientation
        {
            get
            {
                UInt16? orientation = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.Orientation.Query);

                if (orientation == null)
                {
                    return PhotoMetadataEnums.Orientations.Unknown;
                }
                else
                {
                    return (PhotoMetadataEnums.Orientations)orientation.Value;
                }
            }
        }

        /// <summary>
        /// WhiteBalance
        /// </summary>
        public PhotoMetadataEnums.WhiteBalances WhiteBalance
        {
            get
            {
                UInt16? whiteBalance = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.WhiteBalance.Query);

                if (whiteBalance == null)
                {
                    return PhotoMetadataEnums.WhiteBalances.AutoWhiteBalance;
                }
                else
                {
                    return (PhotoMetadataEnums.WhiteBalances)whiteBalance.Value;
                }
            }
        }

        /// <summary>
        /// WhiteBalanceMode
        /// </summary>
        public PhotoMetadataEnums.LightSources LightSource
        {
            get
            {
                uint? lightSource = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.LightSource.Query);

                if (lightSource == null)
                {
                    return PhotoMetadataEnums.LightSources.Unknown;
                }
                else
                {
                    return (PhotoMetadataEnums.LightSources)lightSource.Value;
                }
            }
        }

        /// <summary>
        /// ColorRepresentation
        /// </summary>
        public PhotoMetadataEnums.ColorRepresentations ColorRepresentation
        {
            get
            {
                UInt16? colour = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.ColorRepresentation.Query);

                if (colour == null || colour.Value != 1)
                {
                    return PhotoMetadataEnums.ColorRepresentations.Unknown;
                }
                else
                {
                    return PhotoMetadataEnums.ColorRepresentations.sRGB;
                }
            }
        }

        /// <summary>
        /// ExposureMode
        /// </summary>
        public PhotoMetadataEnums.ExposurePrograms ExposureProgram
        {
            get
            {
                uint? exposureProgram = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.ExposureProgram.Query);

                if (exposureProgram == null)
                {
                    return PhotoMetadataEnums.ExposurePrograms.Unknown;
                }
                else
                {
                    return (PhotoMetadataEnums.ExposurePrograms)exposureProgram.Value;
                }
            }
        }

        #region Flash Code that's not complete
        /// <summary>
        /// FlashStatus
        /// </summary>
        ////public PhotoMetadataEnums.FlashStatus Flash
        ////{
        ////    get
        ////    {
        ////        uint? unknownObject = this.BitmapMetadata.GetQueryAsUint(ExifQueries.FlashFired);

        ////        if (unknownObject == null)
        ////        {
        ////            return PhotoMetadataEnums.FlashStatus.NoFlash;
        ////        }
        ////        else
        ////        {
        ////            // Convert to Array
        ////            char[] flashBytes = Convert.ToString(unknownObject.Value, 2).PadLeft(8, '0').ToCharArray();

        ////            // Reverse the order
        ////            Array.Reverse(flashBytes);

        ////            if (flashBytes[0] == '1' && flashBytes[6] == '0')
        ////            {
        ////                return PhotoMetadataEnums.FlashStatus.Flash;
        ////            }
        ////            else if (flashBytes[0] == '1' && flashBytes[6] == '1')
        ////            {
        ////                return PhotoMetadataEnums.FlashStatus.FlashWithRedEyeReduction;
        ////            }
        ////            else
        ////            {
        ////                return PhotoMetadataEnums.FlashStatus.NoFlash;
        ////            }
        ////        }
        ////    }
        ////}
        #endregion

        /// <summary>
        /// GpsPosition 
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
                    if (value.Dimension == GpsPosition.Dimensions.ThreeDimensional)
                    {
                        this.GpsAltitude = value.Altitude;
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(GpsQueries.AltitudeRef.Query, string.Empty);
                        this.BitmapMetadata.SetQuery(GpsQueries.Altitude.Query, string.Empty);
                    }

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
        public GpsCoordinate GpsLatitude
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
        public GpsCoordinate GpsLongitude
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
        public GpsPosition.Dimensions GpsMeasureMode
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
        public DateTime GpsDateTimeStamp
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
        public string GpsVersionID
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
        public string GpsProcessingMethod
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

        public void Dispose()
        {
            this.Dispose(true);

            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Force Garbage ObjCollection
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
