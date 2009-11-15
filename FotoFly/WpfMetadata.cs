// <copyright file="WpfMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>WpfMetadata Class</summary>
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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class WpfMetadata : IPhotoMetadata, IDisposable
    {
        private bool disposed = false;

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
                URational urational = this.BitmapMetadata.GetQueryAsURational(ExifQueries.Aperture);

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
                    object cameraModelArray = this.BitmapMetadata.GetQueryAsObject(ExifQueries.CameraModel);

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
                return this.BitmapMetadata.GetQueryAsString(ExifQueries.CreationSoftware);
            }

            set
            {
                this.BitmapMetadata.SetQuery(ExifQueries.CreationSoftware, value);
            }
        }

        /// <summary>
        /// DateDigitized, recorded by the camera when the photo is taken
        /// </summary>
        public DateTime DateDigitised
        {
            get
            {
                DateTime? newDateTime = this.BitmapMetadata.GetQueryAsExifDateTime(ExifQueries.DateDigitized);

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

                this.BitmapMetadata.SetQuery(ExifQueries.DateDigitized, exifDate);
            }
        }

        /// <summary>
        /// DateTaken, recorded by the camera when the photo is taken
        /// </summary>
        public DateTime DateTaken
        {
            get
            {
                DateTime? datetimeTaken = Convert.ToDateTime(this.BitmapMetadata.DateTaken);

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
                this.BitmapMetadata.DateTaken = value.ToString("s");

                // Use specific format for EXIF data, 2008:12:01 13:14:10
                this.BitmapMetadata.SetQuery(ExifQueries.DateTaken, value.ToString("yyyy:MM:dd HH:mm:ss"));
            }
        }

        /// <summary>
        /// DigitalZoomRatio
        /// </summary>
        public double DigitalZoomRatio
        {
            get
            {
                Rational rational = this.BitmapMetadata.GetQueryAsRational(ExifQueries.DigitalZoomRatio);

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
                try
                {
                    // TODO: This doesn't deal determine positive or negative values
                    Rational rational = this.BitmapMetadata.GetQueryAsRational(ExifQueries.ExposureBias);

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

        /// <summary>
        /// Focal Length
        /// </summary>
        public string FocalLength
        {
            get
            {
                URational urational = this.BitmapMetadata.GetQueryAsURational(ExifQueries.FocalLenght);

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
                uint? iso = this.BitmapMetadata.GetQueryAsUint(ExifQueries.IsoSpeedRating);

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
        }

        /// <summary>
        /// Horizontal Resolution
        /// </summary>
        public int HorizontalResolution
        {
            get
            {
                Rational rational = this.BitmapMetadata.GetQueryAsRational(ExifQueries.HorizontalResolution);

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
                int? imageHeight = this.BitmapMetadata.GetQueryAsInt(ExifQueries.ImageHeight);

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
                int? imageWidth = this.BitmapMetadata.GetQueryAsInt(ExifQueries.ImageWidth);

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
                string returnValue = this.BitmapMetadata.GetQueryAsString(IptcQueries.City);

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
                    this.BitmapMetadata.SetQuery(IptcQueries.City, string.Empty);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(IptcQueries.City, value);
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
                string returnValue = this.BitmapMetadata.GetQueryAsString(IptcQueries.Country);

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
                    this.BitmapMetadata.SetQuery(IptcQueries.Country, string.Empty);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(IptcQueries.Country, value);
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
                string returnValue = this.BitmapMetadata.GetQueryAsString(IptcQueries.Region);

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
                    this.BitmapMetadata.SetQuery(IptcQueries.Region, string.Empty);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(IptcQueries.Region, value);
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
                string returnValue = this.BitmapMetadata.GetQueryAsString(IptcQueries.SubLocation);

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
                    this.BitmapMetadata.SetQuery(IptcQueries.SubLocation, string.Empty);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(IptcQueries.SubLocation, value);
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
                    Rational rational = this.BitmapMetadata.GetQueryAsRational(ExifQueries.ShutterSpeed);

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
                Rational rational = this.BitmapMetadata.GetQueryAsRational(ExifQueries.ThumbnailVerticalResolution);

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
                Rational rational = this.BitmapMetadata.GetQueryAsRational(ExifQueries.ThumbnailHorizontalResolution);

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

                // Read Last Update Date
                if (this.BitmapMetadata.GetQueryAsDateTime(XmpQueries.MicrosoftRegionsLastUpdate) != null)
                {
                    regionInfo.LastUpdate = this.BitmapMetadata.GetQueryAsDateTime(XmpQueries.MicrosoftRegionsLastUpdate).Value;
                }

                // Read Each Region
                BitmapMetadata regionsMetadata = this.BitmapMetadata.GetQueryAsBitmapMetadata(XmpQueries.MicrosoftRegions);

                if (regionsMetadata != null)
                {
                    foreach (string regionQuery in regionsMetadata)
                    {
                        string regionFullQuery = XmpQueries.MicrosoftRegions + regionQuery;

                        BitmapMetadata regionMetadata = this.BitmapMetadata.GetQueryAsBitmapMetadata(regionFullQuery);

                        if (regionMetadata != null)
                        {
                            XmpRegion newRegion = new XmpRegion();

                            if (regionMetadata.ContainsQuery(XmpQueries.MicrosoftRectangle))
                            {
                                newRegion.RectangleString = regionMetadata.GetQuery(XmpQueries.MicrosoftRectangle).ToString();
                            }

                            if (regionMetadata.ContainsQuery(XmpQueries.MicrosoftPersonDisplayName))
                            {
                                newRegion.PersonDisplayName = regionMetadata.GetQuery(XmpQueries.MicrosoftPersonDisplayName).ToString();
                            }

                            if (regionMetadata.ContainsQuery(XmpQueries.MicrosoftPersonEmailDigest))
                            {
                                newRegion.PersonEmailDigest = regionMetadata.GetQuery(XmpQueries.MicrosoftPersonEmailDigest).ToString();
                            }

                            if (regionMetadata.ContainsQuery(XmpQueries.MicrosoftPersonLiveIdCID))
                            {
                                newRegion.PersonLiveIdCID = regionMetadata.GetQuery(XmpQueries.MicrosoftPersonLiveIdCID).ToString();
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
                    if (!this.BitmapMetadata.ContainsQuery(XmpQueries.MicrosoftRegionInfo))
                    {
                        this.BitmapMetadata.SetQuery(XmpQueries.MicrosoftRegionInfo, new BitmapMetadata(XmpQueries.StructBlock));
                    }

                    // Set LastUpdate if it's been set
                    if (value.LastUpdate != new DateTime())
                    {
                        this.BitmapMetadata.SetQuery(XmpQueries.MicrosoftRegionsLastUpdate, value.LastUpdate.ToUniversalTime());
                    }

                    // Check for Regions Bag, if none exists, create it
                    if (!this.BitmapMetadata.ContainsQuery(XmpQueries.MicrosoftRegions))
                    {
                        this.BitmapMetadata.SetQuery(XmpQueries.MicrosoftRegions, new BitmapMetadata(XmpQueries.BagBlock));
                    }

                    // If Region count has changed, clear our existing regions and create empty regions
                    if (value.Regions.Count != this.RegionInfo.Regions.Count)
                    {
                        // Delete any 'extra' regions
                        for (int i = value.Regions.Count; i < this.RegionInfo.Regions.Count; i++)
                        {
                            // Build query for current region
                            string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, XmpQueries.MicrosoftRegion, i.ToString());

                            this.BitmapMetadata.RemoveQuery(currentRegionQuery);
                        }

                        // Add any additional regions
                        for (int i = this.RegionInfo.Regions.Count; i < value.Regions.Count; i++)
                        {
                            // Build query for current region
                            string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, XmpQueries.MicrosoftRegion, i.ToString());

                            this.BitmapMetadata.SetQuery(currentRegionQuery, new BitmapMetadata(XmpQueries.StructBlock));
                        }
                    }

                    int currentRegion = 0;

                    // Loop through each Region
                    foreach (XmpRegion xmpRegion in value.Regions)
                    {
                        // Build query for current region
                        string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, XmpQueries.MicrosoftRegion, currentRegion);

                        // Update or clear PersonDisplayName
                        if (string.IsNullOrEmpty(xmpRegion.PersonDisplayName))
                        {
                            this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpQueries.MicrosoftPersonDisplayName);
                        }
                        else
                        {
                            this.BitmapMetadata.SetQuery(currentRegionQuery + XmpQueries.MicrosoftPersonDisplayName, xmpRegion.PersonDisplayName);
                        }

                        // Update or clear PersonEmailDigest
                        if (string.IsNullOrEmpty(xmpRegion.PersonEmailDigest))
                        {
                            this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpQueries.MicrosoftPersonEmailDigest);
                        }
                        else
                        {
                            this.BitmapMetadata.SetQuery(currentRegionQuery + XmpQueries.MicrosoftPersonEmailDigest, xmpRegion.PersonEmailDigest);
                        }

                        // Update or clear PersonLiveIdCID
                        if (string.IsNullOrEmpty(xmpRegion.PersonLiveIdCID))
                        {
                            this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpQueries.MicrosoftPersonLiveIdCID);
                        }
                        else
                        {
                            this.BitmapMetadata.SetQuery(currentRegionQuery + XmpQueries.MicrosoftPersonLiveIdCID, xmpRegion.PersonLiveIdCID);
                        }

                        // Update or clear RectangleString
                        if (string.IsNullOrEmpty(xmpRegion.RectangleString))
                        {
                            this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpQueries.MicrosoftRectangle);
                        }
                        else
                        {
                            this.BitmapMetadata.SetQuery(currentRegionQuery + XmpQueries.MicrosoftRectangle, xmpRegion.RectangleString);
                        }

                        currentRegion++;
                    }
                }
                else
                {
                    // Delete existing RegionInfos, deletes all child data
                    if (this.BitmapMetadata.ContainsQuery(XmpQueries.MicrosoftRegionInfo))
                    {
                        this.BitmapMetadata.RemoveQuery(XmpQueries.MicrosoftRegionInfo);
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
                Rational rational = this.BitmapMetadata.GetQueryAsRational(ExifQueries.VerticalResolution);

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
        /// ExposureMode
        /// </summary>
        public PhotoMetadataEnums.ExposureModes ExposureMode
        {
            get
            {
                uint? exposureMode = this.BitmapMetadata.GetQueryAsUint(ExifQueries.ExposureMode);

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
                uint? subjectDistanceRange = this.BitmapMetadata.GetQueryAsUint(ExifQueries.SubjectDistanceRange);

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
                uint? meteringMode = this.BitmapMetadata.GetQueryAsUint(ExifQueries.MeteringMode);

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
                uint? orientation = this.BitmapMetadata.GetQueryAsUint(ExifQueries.Orientation);

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
                uint? whiteBalance = this.BitmapMetadata.GetQueryAsUint(ExifQueries.WhiteBalance);

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
                uint? lightSource = this.BitmapMetadata.GetQueryAsUint(ExifQueries.LightSource);

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
                uint? colour = this.BitmapMetadata.GetQueryAsUint(ExifQueries.ColorRepresentation);

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
                uint? exposureProgram = this.BitmapMetadata.GetQueryAsUint(ExifQueries.ExposureProgram);

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
                        this.BitmapMetadata.SetQuery(ExifQueries.GpsAltitudeRef, string.Empty);
                        this.BitmapMetadata.SetQuery(ExifQueries.GpsAltitude, string.Empty);
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
                string gpsRef = this.BitmapMetadata.GetQueryAsString(ExifQueries.GpsAltitudeRef);
                URational urational = this.BitmapMetadata.GetQueryAsURational(ExifQueries.GpsAltitude);

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
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsAltitudeRef, string.Empty);
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsAltitude, string.Empty);
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

                    this.BitmapMetadata.SetQuery(ExifQueries.GpsAltitudeRef, altRef);
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsAltitude, rational.ToUlong());
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
                string gpsRef = this.BitmapMetadata.GetQueryAsString(ExifQueries.GpsLatitudeRef).ToUpper();
                GpsRational gpsRational = this.BitmapMetadata.GetQueryAsGpsRational(ExifQueries.GpsLatitude);

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
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsLatitudeRef, value.Ref);

                    GpsRational gpsRational = new GpsRational(value.Degrees, value.Minutes, value.Seconds);

                    this.BitmapMetadata.SetQuery(ExifQueries.GpsLatitude, gpsRational.ToUlongArray(true));
                }
                else
                {
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsLatitude, string.Empty);
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsLatitudeRef, string.Empty);
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
                string gpsRef = this.BitmapMetadata.GetQueryAsString(ExifQueries.GpsLongitudeRef).ToUpper();
                GpsRational gpsRational = this.BitmapMetadata.GetQueryAsGpsRational(ExifQueries.GpsLongitude);

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
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsLongitudeRef, value.Ref);

                    GpsRational gpsRational = new GpsRational(value.Degrees, value.Minutes, value.Seconds);

                    this.BitmapMetadata.SetQuery(ExifQueries.GpsLongitude, gpsRational.ToUlongArray(true));
                }
                else
                {
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsLongitude, string.Empty);
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsLongitudeRef, string.Empty);
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
                string returnValue = this.BitmapMetadata.GetQueryAsString(ExifQueries.GpsMeasureMode);

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
                        this.BitmapMetadata.SetQuery(ExifQueries.GpsMeasureMode, string.Empty);
                        break;

                    case GpsPosition.Dimensions.ThreeDimensional:
                        this.BitmapMetadata.SetQuery(ExifQueries.GpsMeasureMode, 3);
                        break;

                    case GpsPosition.Dimensions.TwoDimensional:
                        this.BitmapMetadata.SetQuery(ExifQueries.GpsMeasureMode, 2);
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
                    GpsRational time = this.BitmapMetadata.GetQueryAsGpsRational(ExifQueries.GpsTimeStamp);
                    string date = this.BitmapMetadata.GetQueryAsString(ExifQueries.GpsDateStamp);

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

                    this.BitmapMetadata.SetQuery(ExifQueries.GpsDateStamp, date);
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsTimeStamp, time.ToUlongArray(false));
                }
                else
                {
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsDateStamp, string.Empty);
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsTimeStamp, string.Empty);
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
                return this.BitmapMetadata.GetQueryAsString(ExifQueries.GpsVersionID);
            }

            set
            {
                this.BitmapMetadata.SetQuery(ExifQueries.GpsVersionID, value);
            }
        }

        /// <summary>
        /// Gps Processing Mode
        /// </summary>
        public string GpsProcessingMethod
        {
            get
            {
                if (string.IsNullOrEmpty(this.BitmapMetadata.GetQueryAsString(ExifQueries.GpsProcessingMethod)))
                {
                    return "None";
                }
                else
                {
                    return this.BitmapMetadata.GetQueryAsString(ExifQueries.GpsProcessingMethod);
                }
            }

            set
            {
                if (value == "None")
                {
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsProcessingMethod, string.Empty);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(ExifQueries.GpsProcessingMethod, value);
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

            this.disposed = true;
        }

        public Dictionary<string, object> MetadataDictionary()
        {
            return this.GenerateMetadataDictionary(this.BitmapMetadata, string.Empty);
        }

        private Dictionary<string, object> GenerateMetadataDictionary(BitmapMetadata metadata)
        {
            return this.GenerateMetadataDictionary(metadata, string.Empty);
        }

        private Dictionary<string, object> GenerateMetadataDictionary(BitmapMetadata metadata, string fullQuery)
        {
            Dictionary<string, object> returnValue = new Dictionary<string, object>();

            if (metadata != null)
            {
                foreach (string query in metadata)
                {
                    string tempQuery = fullQuery + query;

                    try
                    {
                        // query string here is relative to the previous metadata reader.
                        object unknownObject = metadata.GetQuery(query);

                        BitmapMetadata moreMetadata = unknownObject as BitmapMetadata;

                        if (moreMetadata != null)
                        {
                            // Add all sub values
                            foreach (KeyValuePair<string, object> keyValuePair in this.GenerateMetadataDictionary(moreMetadata, tempQuery))
                            {
                                returnValue.Add(keyValuePair.Key, keyValuePair.Value);
                            }
                        }
                        else
                        {
                            returnValue.Add(tempQuery, unknownObject);
                        }
                    }
                    catch
                    {
                        // Swallow it
                    }
                }
            }

            return returnValue;
        }
    }
}
