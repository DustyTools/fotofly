// <copyright file="FileMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>FileMetadata Class</summary>
namespace Fotofly.MetadataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;
    using System.Collections.ObjectModel;
    using Fotofly.MetadataQueries;

    public class FileMetadata : IDisposable, IFileMetadata
    {
        private XmpIptcProvider xmpIptcProvider;
        private ExifProvider exifProvider;
        private GpsProvider gpsProvider;
        private IptcProvider iptcProvider;
        private XmpCoreProvider xmpCoreProvider;
        private XmpExifProvider xmpExifProvider;
        private XmpFotoflyProvider xmpFotoflyProvider;
        private XmpMicrosoftProvider xmpMicrosoftProvider;
        private XmpRightsProvider xmpRightsProvider;
        private XmpTiffProvider xmpTiffProvider;
        private XmpXapProvider xmpXapProvider;
        private XmpPhotoshopProvider xmpPhotoshopProvider;
        private BitmapMetadata bitmapMetadata;

        public FileMetadata(BitmapMetadata bitmapMetadata)
        {
            this.exifProvider = new ExifProvider(bitmapMetadata);
            this.gpsProvider = new GpsProvider(bitmapMetadata);
            this.iptcProvider = new IptcProvider(bitmapMetadata);
            this.xmpCoreProvider = new XmpCoreProvider(bitmapMetadata);
            this.xmpExifProvider = new XmpExifProvider(bitmapMetadata);
            this.xmpFotoflyProvider = new XmpFotoflyProvider(bitmapMetadata);
            this.xmpMicrosoftProvider = new XmpMicrosoftProvider(bitmapMetadata);
            this.xmpRightsProvider = new XmpRightsProvider(bitmapMetadata);
            this.xmpIptcProvider = new XmpIptcProvider(bitmapMetadata);
            this.xmpTiffProvider = new XmpTiffProvider(bitmapMetadata);
            this.xmpXapProvider = new XmpXapProvider(bitmapMetadata);
            this.xmpPhotoshopProvider = new XmpPhotoshopProvider(bitmapMetadata);
            this.bitmapMetadata = bitmapMetadata;
        }

        #region Public access to all providers
        /// <summary>
        /// Data stored in Exif IFD
        /// </summary>
        public ExifProvider ExifProvider
        {
            get { return this.exifProvider; }
        }

        /// <summary>
        /// Data stored in Gps IFD
        /// </summary>
        public GpsProvider GpsProvider
        {
            get { return this.gpsProvider; }
        }

        /// <summary>
        /// Data stored in IPTC (International Press Telecommunications Council) IFD
        /// </summary>
        public IptcProvider IptcProvider
        {
            get { return this.iptcProvider; }
        }

        /// <summary>
        /// Data stored in xmp
        /// </summary>
        public XmpCoreProvider XmpCoreProvider
        {
            get { return this.xmpCoreProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Fotofly Schema
        /// </summary>
        public XmpFotoflyProvider XmpFotoflyProvider
        {
            get { return this.xmpFotoflyProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Xap Schema
        /// </summary>
        public XmpXapProvider XmpXapProvider
        {
            get { return this.xmpXapProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Photoshop Schema
        /// </summary>
        public XmpPhotoshopProvider XmpPhotoshopProvider
        {
            get { return this.xmpPhotoshopProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Tiff Schema
        /// </summary>
        public XmpTiffProvider XmpTiffProvider
        {
            get { return this.xmpTiffProvider; }
        }

        /// <summary>
        /// Data stored in xmp using IPTC for XMP Schema
        /// </summary>
        public XmpIptcProvider XmpIptcProvider
        {
            get { return this.xmpIptcProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Microsoft Schema
        /// </summary>
        public XmpMicrosoftProvider XmpMicrosoftProvider
        {
            get { return this.xmpMicrosoftProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Exif for XMP Schema
        /// </summary>
        public XmpExifProvider XmpExifProvider
        {
            get { return this.xmpExifProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Rights Schema
        /// </summary>
        public XmpRightsProvider XmpRightsProvider
        {
            get { return this.xmpRightsProvider; }
        }

        public BitmapMetadata BitmapMetadata
        {
            get { return this.bitmapMetadata; }
        }
        #endregion

        public void Dispose()
        {
            this.Dispose(true);

            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Aperture
        /// </summary>
        public Aperture Aperture
        {
            get
            {
                return this.ReconcileAndReadExifAndXmp<Aperture>(this.ExifProvider.Aperture, this.XmpExifProvider.Aperture);
            }
        }

        /// <summary>
        /// Software used to last modify the photo
        /// </summary>
        public string CreationSoftware
        {
            get
            {
                return this.ExifProvider.CreationSoftware;
            }

            set
            {
                this.ExifProvider.CreationSoftware = value;
            }
        }

        /// <summary>
        /// Shutter Speed
        /// </summary>
        public ShutterSpeed ShutterSpeed
        {
            get
            {
                return this.ReconcileAndReadExifAndXmp<ShutterSpeed>(this.ExifProvider.ShutterSpeed, this.XmpExifProvider.ShutterSpeed);
            }
        }

        /// <summary>
        /// Exposure Bias
        /// </summary>
        public ExposureBias ExposureBias
        {
            get
            {
                return this.ReconcileAndReadExifAndXmp<ExposureBias>(this.ExifProvider.ExposureBias, this.XmpExifProvider.ExposureBias);
            }
        }

        /// <summary>
        /// Focal Length
        /// </summary>
        public string FocalLength
        {
            get
            {
                return this.ReconcileAndReadExifAndXmp<string>(this.ExifProvider.FocalLength, this.XmpExifProvider.FocalLength);
            }
        }

        /// <summary>
        /// Copyright owner of the photo
        /// </summary>
        public string Copyright
        {
            // Exif Copyright (33432, 0x8298) 
            // IPTC CopyrightNotice (IIM 2:116, 0x0274) 
            // XMP (dc:rights). 
            get
            {
                return this.ReconcileAndReadExifXmpAndIptc<string>(this.ExifProvider.Copyright, this.xmpRightsProvider.Copyright, this.IptcProvider.CopyrightNotice);
            }

            set
            {
                this.ExifProvider.Copyright = value;

                if (string.IsNullOrEmpty(this.xmpRightsProvider.Copyright))
                {
                    this.xmpRightsProvider.Copyright = value;
                }

                if (string.IsNullOrEmpty(this.IptcProvider.CopyrightNotice))
                {
                    this.IptcProvider.CopyrightNotice = value;
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
                return this.ExifProvider.CameraModel;
            }

            set
            {
                this.ExifProvider.CameraModel = value;
            }
        }

        /// <summary>
        /// Camera Manufacturer
        /// </summary>
        public string CameraManufacturer
        {
            get
            {
                return this.ExifProvider.CameraManufacturer;
            }

            set
            {
                this.ExifProvider.CameraManufacturer = value;
            }
        }

        /// <summary>
        /// Metering Mode
        /// </summary>
        public MetadataEnums.MeteringModes MeteringMode
        {
            get
            {
                return this.ReconcileAndReadExifAndXmp<MetadataEnums.MeteringModes>(this.ExifProvider.MeteringMode, this.XmpExifProvider.MeteringMode);
            }
        }

        /// <summary>
        /// Microsoft Region Info extension which provides data on regions in the photo
        /// </summary>
        public MicrosoftImageRegionInfo MicrosoftRegionInfo
        {
            get
            {
                return this.xmpMicrosoftProvider.RegionInfo;
            }

            set
            {
                this.xmpMicrosoftProvider.RegionInfo = value;
            }
        }

        /// <summary>
        /// List of Tags, sometimes known as Keywords
        /// </summary>
        public TagList Tags
        {
            // Primary is dc:subject
            // /app13/irb/8bimiptc/iptc/keywords
            // /app1/ifd/{ushort=18247}
            // /app1/ifd/{ushort=40094}
            // Only write to:
            ////<WritePath path="/xmp/&lt;xmpbag&gt;MicrosoftPhoto:LastKeywordXMP" dependent_path="/xmp/&lt;xmpbag&gt;dc:subject" disk_format="unicode" data_format="bag" />
            ////<WritePath path="/xmp/&lt;xmpbag&gt;MicrosoftPhoto:LastKeywordIPTC" dependent_path="/app13/irb/8bimiptc/iptc/keywords" disk_format="unicode" data_format="bag" />
            get
            {
                return this.XmpCoreProvider.Tags;
            }

            set
            {
                this.XmpCoreProvider.Tags = value;
            }
        }

        /// <summary>
        /// Gps Position of the Location where the photo was crated
        /// </summary>
        public GpsPosition GpsPositionOfLocationCreated
        {
            get
            {
                return this.ReconcileAndReadExifAndXmp<GpsPosition>(this.GpsProvider.GpsPositionOfLocationCreated, this.XmpExifProvider.GpsPositionOfLocationCreated);
            }

            set
            {
                this.GpsProvider.GpsPositionOfLocationCreated = value;

                if (this.XmpExifProvider.GpsPositionOfLocationCreated.IsValidCoordinate && this.XmpExifProvider.GpsPositionOfLocationCreated != this.GpsProvider.GpsPositionOfLocationCreated)
                {
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsAltitude.Query);
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsAltitudeRef.Query);
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsLatitude.Query);
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsLongitude.Query);
                }
            }
        }

        /// <summary>
        /// Gps Position of the Location shown in the Photo
        /// </summary>
        public GpsPosition GpsPositionOfLocationShown
        {
            get
            {
                return this.ReconcileAndReadExifAndXmp<GpsPosition>(this.GpsProvider.GpsPositionOfLocationShown, this.XmpExifProvider.GpsPositionOfLocationShown);
            }

            set
            {
                this.GpsProvider.GpsPositionOfLocationShown = value;

                if (this.XmpExifProvider.GpsPositionOfLocationShown.IsValidCoordinate && this.XmpExifProvider.GpsPositionOfLocationShown != this.GpsProvider.GpsPositionOfLocationShown)
                {
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsDestLatitude.Query);
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsDestLatitudeRef.Query);
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsDestLongitude.Query);
                    this.BitmapMetadata.RemoveQuery(XmpExifQueries.GpsDestLongitudeRef.Query);
                }
            }
        }

        /// <summary>
        /// DateModified, as stored in Metadata (not the same as file last modified)
        /// </summary>
        public DateTime DateModified
        {
            ////Modification Date/Time - Modification date of the digital image file
            ////Exif DateTime (306, 0x132) and SubSecTime (37520, 0x9290)
            ////XMP (xmp:ModifyDate)
            get
            {
                return this.ReconcileAndReadExifAndXmp<DateTime>(this.ExifProvider.DateTimeModified, this.XmpCoreProvider.DateTimeModified);
            }

            set
            {
                this.ExifProvider.DateTimeModified = value;

                if (this.XmpCoreProvider.DateTimeModified != new DateTime())
                {
                    this.XmpCoreProvider.DateTimeModified = value;
                }
            }
        }

        /// <summary>
        /// DateAquired, Microsoft Windows7 Property field
        /// </summary>
        public DateTime DateAquired
        {
            get
            {
                return this.xmpMicrosoftProvider.DateAquired;
            }

            set
            {
                this.xmpMicrosoftProvider.DateAquired = value;
            }
        }

        /// <summary>
        /// DateTaken, recorded by the camera when the photo is taken
        /// </summary>
        public DateTime DateTaken
        {
            ////Original Date/Time - Creation date of the intellectual content (e.g. the photograph), rather than the creation date of the content being shown
            ////Exif DateTimeOriginal (36867, 0x9003) and SubSecTimeOriginal (37521, 0x9291)
            ////IPTC DateCreated (IIM 2:55, 0x0237) and TimeCreated (IIM 2:60, 0x023C)
            ////XMP (photoshop:DateCreated)
            get
            {
                return this.ReconcileAndReadExifXmpAndIptc<DateTime>(this.ExifProvider.DateTimeTaken, this.XmpPhotoshopProvider.DateTimeOriginal, this.iptcProvider.DateTimeCreated);
            }

            set
            {
                this.ExifProvider.DateTimeTaken = value;

                if (this.XmpPhotoshopProvider.DateTimeOriginal != new DateTime())
                {
                    this.XmpPhotoshopProvider.DateTimeOriginal = value;
                }

                if (this.iptcProvider.DateTimeCreated != new DateTime())
                {
                    this.iptcProvider.DateTimeCreated = value;
                }
            }
        }

        /// <summary>
        /// ISO Speed rating 
        /// </summary>
        public IsoSpeed IsoSpeed
        {
            get
            {
                return this.ReconcileAndReadExifAndXmp<IsoSpeed>(this.ExifProvider.IsoSpeed, this.XmpExifProvider.IsoSpeed);
            }
        }

        /// <summary>
        /// Image Width measured in Pixels
        /// </summary>
        public int ImageWidth
        {
            get
            {
                return this.exifProvider.ImageWidth;
            }
        }

        /// <summary>
        /// Image Height measured in Pixels
        /// </summary>
        public int ImageHeight
        {
            get
            {
                return this.exifProvider.ImageHeight;
            }
        }

        /// <summary>
        /// Rating (Range -1.0 to 5.0)
        /// </summary>
        public Rating Rating
        {
            // Check Xap, then Microsoft
            get
            {
                return this.ReconcileAndReadXmpAndXmp<Rating>(this.xmpXapProvider.Rating, this.xmpMicrosoftProvider.Rating);
            }

            set
            {
                this.xmpXapProvider.Rating = value;
                
                if (this.xmpMicrosoftProvider.Rating.AsEnum != Rating.Ratings.NoRating)
                {
                    this.xmpMicrosoftProvider.Rating = value;
                }
            }
        }

        /// <summary>
        /// Comment, not stored in XMP, IPTC or Exif
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
                if (this.ValueHasChanged(value, this.Comment))
                {
                    this.BitmapMetadata.Comment = value;
                }
            }
        }

        /// <summary>
        /// Description, Also known as Title (in Windows), User Comment, Caption, Abstract or Description
        /// </summary>
        public string Description
        {
            ////the same:
            // Exif ImageDescription (270, 0x010E)
            // Xmp dc:title
            // Xmp dc:description
            // Iptc caption
            // Use Built in methods
            get
            {
                string formattedString = String.Empty;

                try
                {
                    formattedString = this.BitmapMetadata.Title;
                }
                catch
                {
                    formattedString = String.Empty;
                }

                return formattedString;
            }

            set
            {
                if (this.ValueHasChanged(value, this.Description))
                {
                    this.BitmapMetadata.Title = value;
                }
            }
        }

        /// <summary>
        /// Subject, not stored in XMP, IPTC or Exif
        /// </summary>
        public string Subject
        {
            get
            {
                string formattedString = String.Empty;

                try
                {
                    formattedString = this.BitmapMetadata.Subject;
                }
                catch
                {
                    formattedString = String.Empty;
                }

                return formattedString;
            }

            set
            {
                if (this.ValueHasChanged(value, this.Subject))
                {
                    this.BitmapMetadata.Subject = value;
                }
            }
        }

        /// <summary>
        /// List of Authors, also known as Photographer
        /// </summary>
        public PeopleList Authors
        {
            ////The creator is available in the following properties:
            ////Exif Artist (315, 0x013B)
            ////IPTC By-line (IIM 2:80, 0x0250)
            ////XMP (dc:creator)
            /// Use BitmapMetadata.Author because it implements all the right rules
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
                if (this.ValueHasChanged(value, this.Authors))
                {
                    this.BitmapMetadata.Author = new ReadOnlyCollection<string>(value);
                }
            }
        }

        /// <summary>
        /// DateDigitized, recorded by the camera when the photo is taken
        /// </summary>
        public DateTime DateDigitised
        {
            ////Digitized Date/Time - Creation date of the digital representation
            ////Exif DateTimeDigitized (36868, 0x9004) and SubSecTimeDigitized (37522, 0x9292)
            ////IPTC DigitalCreationDate (IIM 2:62, 0x023E) and DigitalCreationTime (IIM 2:63, 0x023F)
            ////XMP (xmp:CreateDate)
            get
            {
                return this.ReconcileAndReadExifXmpAndIptc<DateTime>(this.ExifProvider.DateDigitised, this.XmpCoreProvider.DateTimeModified, this.iptcProvider.DateTimeDigitised);
            }

            set
            {
                this.ExifProvider.DateDigitised = value;

                if (this.XmpCoreProvider.DateTimeDigitised != new DateTime())
                {
                    this.XmpCoreProvider.DateTimeModified = value;
                }

                if (this.iptcProvider.DateTimeDigitised != new DateTime())
                {
                    this.iptcProvider.DateTimeDigitised = value;
                }
            }
        }

        /// <summary>
        /// Vertical Resolution of main photo
        /// </summary>
        public int VerticalResolution
        {
            get
            {
                return this.ExifProvider.VerticalResolution;
            }
        }

        /// <summary>
        /// Horizontal Resolution of main photo
        /// </summary>
        public int HorizontalResolution
        {
            get
            {
                return this.ExifProvider.HorizontalResolution;
            }
        }

        /// <summary>
        /// DigitalZoomRatio
        /// </summary>
        public double? DigitalZoomRatio
        {
            get
            {
                return this.ReconcileAndReadExifAndXmp<double?>(this.ExifProvider.DigitalZoomRatio, this.XmpExifProvider.DigitalZoomRatio);
            }
        }

        public DateTime DateUtc
        {
            get
            {
                return this.xmpFotoflyProvider.UtcDate;
            }

            set
            {
                this.xmpFotoflyProvider.UtcDate = value;
            }
        }

        public double? UtcOffset
        {
            get
            {
                return this.xmpFotoflyProvider.UtcOffset;
            }

            set
            {
                this.xmpFotoflyProvider.UtcOffset = value;
            }
        }

        public DateTime DateLastFotoflySave
        {
            get
            {
                return this.xmpFotoflyProvider.LastEditDate;
            }

            set
            {
                this.xmpFotoflyProvider.LastEditDate = value;
            }
        }

        public DateTime AddressOfGpsLookupDate
        {
            get
            {
                return this.xmpFotoflyProvider.AddressOfGpsLookupDate;
            }

            set
            {
                this.xmpFotoflyProvider.AddressOfGpsLookupDate = value;
            }
        }

        public DateTime OriginalCameraDate
        {
            get
            {
                return this.xmpFotoflyProvider.OriginalCameraDate;
            }

            set
            {
                this.xmpFotoflyProvider.OriginalCameraDate = value;
            }
        }

        public string OriginalCameraFilename
        {
            get
            {
                return this.xmpFotoflyProvider.OriginalCameraFilename;
            }

            set
            {
                this.xmpFotoflyProvider.OriginalCameraFilename = value;
            }
        }

        public string AddressOfGpsSource
        {
            get
            {
                return this.xmpFotoflyProvider.AddressOfGpsSource;
            }

            set
            {
                this.xmpFotoflyProvider.AddressOfGpsSource = value;
            }
        }

        public Address AddressOfLocationShown
        {
            get
            {
                return this.ReconcileAndReadXmpAndIptc<Address>(this.XmpIptcProvider.AddressOfLocationShown, this.IptcProvider.AddressOfLocationShown);       
            }

            set
            {
                this.XmpIptcProvider.AddressOfLocationShown = value;

                if (this.IptcProvider.AddressOfLocationShown.IsValidAddress)
                {
                    this.IptcProvider.AddressOfLocationShown = value;
                }
            }
        }

        public Address AddressOfLocationCreated
        {
            get
            {
                return this.ReconcileAndReadXmpAndIptc<Address>(this.XmpIptcProvider.AddressOfLocationCreated, this.IptcProvider.AddressOfLocationCreated);
            }

            set
            {
                this.XmpIptcProvider.AddressOfLocationCreated = value;

                if (this.IptcProvider.AddressOfLocationCreated.IsValidAddress)
                {
                    this.IptcProvider.AddressOfLocationCreated = value;
                }
            }
        }

        private T ReconcileAndReadExifAndXmp<T>(T exifValue, T xmpValue)
        {
            if (exifValue != null)
            {
                return exifValue;
            }
            else if (xmpValue != null)
            {
                return xmpValue;
            }

            return default(T);
        }

        private T ReconcileAndReadXmpAndIptc<T>(T xmpValue, T iptcValue)
        {
            if (xmpValue != null)
            {
                return xmpValue;
            }
            else if (iptcValue != null)
            {
                return iptcValue;
            }

            return default(T);
        }

        private T ReconcileAndReadXmpAndXmp<T>(T xmpValuePriOne, T xmpValuePriTwo)
        {
            if (xmpValuePriOne != null)
            {
                return xmpValuePriOne;
            }
            else if (xmpValuePriTwo != null)
            {
                return xmpValuePriTwo;
            }

            return default(T);
        }

        private T ReconcileAndReadExifXmpAndIptc<T>(T exifValue, T xmpValue, T iptcValue)
        {
            if (exifValue != null)
            {
                return exifValue;
            }
            else if (xmpValue != null)
            {
                return xmpValue;
            }
            else if (iptcValue != null)
            {
                return xmpValue;
            }

            return default(T);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Dispose of everything
            this.exifProvider.Dispose();
            this.gpsProvider.Dispose();
            this.iptcProvider.Dispose();
            this.xmpCoreProvider.Dispose();
            this.xmpExifProvider.Dispose();
            this.xmpFotoflyProvider.Dispose();
            this.xmpMicrosoftProvider.Dispose();
            this.xmpRightsProvider.Dispose();

            // Force Garbage ObjCollection
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        protected bool ValueHasChanged(object source, object destination)
        {
            if (source == null && destination == null)
            {
                return false;
            }
            else if (source == null && destination != null)
            {
                return true;
            }
            else if (source != null && destination == null)
            {
                return true;
            }

            return !source.Equals(destination);
        }
    }
}
