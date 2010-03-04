// <copyright file="JpgMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>JpgMetadata</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// Class representing Photo Metadata
    /// </summary>
    [XmlRoot("PhotoMetadata", Namespace = "http://www.tassography.com/fotofly")]
    public class PhotoMetadata : IFileMetadata
    {
        public PhotoMetadata()
        {
            this.MicrosoftRegionInfo = new MicrosoftImageRegionInfo();
            this.GpsPositionOfLocationShown = new GpsPosition();
            this.GpsPositionOfLocationCreated = new GpsPosition();
            this.Authors = new PeopleList();
            this.Tags = new TagList();
            this.AddressOfLocationCreated = new Address();
            this.AddressOfLocationShown = new Address();
        }

        /// <summary>
        /// Aperture
        /// </summary>
        [XmlElementAttribute]
        public Aperture Aperture
        {
            get;
            set;
        }

        /// <summary>
        /// Software used to last modify the photo
        /// </summary>
        [XmlElementAttribute]
        public string CreationSoftware
        {
            get;
            set;
        }

        /// <summary>
        /// Shutter Speed
        /// </summary>
        [XmlElementAttribute]
        public ShutterSpeed ShutterSpeed
        {
            get;
            set;
        }

        /// <summary>
        /// Exposure Bias
        /// </summary>
        [XmlElementAttribute]
        public ExposureBias ExposureBias
        {
            get;
            set;
        }

        /// <summary>
        /// Focal Length
        /// </summary>
        [XmlElementAttribute]
        public string FocalLength
        {
            get;
            set;
        }

        /// <summary>
        /// Copyright owner of the photo
        /// </summary>
        [XmlElementAttribute]
        public string Copyright
        {
            get;
            set;
        }

        /// <summary>
        /// Camera Model, normally includes camera Manufacturer
        /// </summary>
        [XmlElementAttribute]
        public string CameraModel
        {
            get;
            set;
        }

        /// <summary>
        /// Camera Manufacturer
        /// </summary>
        [XmlElementAttribute]
        public string CameraManufacturer
        {
            get;
            set;
        }

        /// <summary>
        /// Metering Mode
        /// </summary>
        [XmlElementAttribute]
        public MetadataEnums.MeteringModes MeteringMode
        {
            get;
            set;
        }

        /// <summary>
        /// Microsoft Region Info extension which provides data on regions in the photo
        /// </summary>
        [XmlElementAttribute]
        public MicrosoftImageRegionInfo MicrosoftRegionInfo
        {
            get;
            set;
        }

        /// <summary>
        /// List of Tags, sometimes known as Keywords
        /// </summary>
        [XmlArray]
        public TagList Tags
        {
            get;
            set;
        }

        /// <summary>
        /// Gps Position of the Location where the photo was crated
        /// </summary>
        [XmlElementAttribute]
        public GpsPosition GpsPositionOfLocationCreated
        {
            get;
            set;
        }

        /// <summary>
        /// Gps Position of the Location shown in the Photo
        /// </summary>
        [XmlElementAttribute]
        public GpsPosition GpsPositionOfLocationShown
        {
            get;
            set;
        }

        /// <summary>
        /// DateModified, as stored in Metadata (not the same as file last modified)
        /// </summary>
        [XmlElementAttribute]
        public DateTime DateModified
        {
            get;
            set;
        }

        /// <summary>
        /// DateAquired, Microsoft Windows7 Property field
        /// </summary>
        [XmlElementAttribute]
        public DateTime DateAquired
        {
            get;
            set;
        }

        /// <summary>
        /// DateTaken, recorded by the camera when the photo is taken
        /// </summary>
        [XmlElementAttribute]
        public DateTime DateTaken
        {
            get;
            set;
        }

        /// <summary>
        /// ISO Speed rating 
        /// </summary>
        [XmlElementAttribute]
        public IsoSpeed IsoSpeed
        {
            get;
            set;
        }

        /// <summary>
        /// Image Width measured in Pixels
        /// </summary>
        [XmlElementAttribute]
        public int ImageWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Image Height measured in Pixels
        /// </summary>
        [XmlElementAttribute]
        public int ImageHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Rating (Ranging -1.0 to 5.0)
        /// </summary>
        [XmlElementAttribute]
        public Rating Rating
        {
            get;
            set;
        }

        /// <summary>
        /// List of Authors, also known as Photographer
        /// </summary>
        [XmlArray]
        public PeopleList Authors
        {
            get;
            set;
        }

        /// <summary>
        /// Description Also Known as Title (in Windows), User Comment, Caption, Abstract or Description
        /// </summary>
        [XmlElementAttribute]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Comment, not stored in XMP, IPTC or Exif
        /// </summary>
        [XmlElementAttribute]
        public string Comment
        {
            get;
            set;
        }

        /// <summary>
        /// Subject, not stored in XMP, IPTC or Exif
        /// </summary>
        [XmlElementAttribute]
        public string Subject
        {
            get;
            set;
        }

        /// <summary>
        /// DateDigitized, recorded by the camera when the photo is taken
        /// </summary>
        [XmlElementAttribute]
        public DateTime DateDigitised
        {
            get;
            set;
        }

        /// <summary>
        /// List of People stored in Region (Readonly)
        /// </summary>
        [XmlElementAttribute]
        public PeopleList People
        {
            get
            {
                PeopleList people = new PeopleList();

                foreach (MicrosoftImageRegion region in this.MicrosoftRegionInfo.Regions)
                {
                    people.Add(region.PersonDisplayName);
                }

                return people;
            }
        }

        /// <summary>
        /// Orientation of the Image (Readonly)
        /// </summary>
        [XmlElementAttribute]
        public MetadataEnums.ImageOrientations Orientation
        {
            get
            {
                // Work out Orientation
                if (this.ImageHeight > this.ImageWidth)
                {
                    return MetadataEnums.ImageOrientations.Portrait;
                }
                else
                {
                    return MetadataEnums.ImageOrientations.Landscape;
                }
            }
        }

        /// <summary>
        /// Vertical Resolution of main photo
        /// </summary>
        [XmlElementAttribute]
        public int VerticalResolution
        {
            get;
            set;
        }

        /// <summary>
        /// Horizontal Resolution of main photo
        /// </summary>
        [XmlElementAttribute]
        public int HorizontalResolution
        {
            get;
            set;
        }

        /// <summary>
        /// DigitalZoomRatio
        /// </summary>
        [XmlElementAttribute]
        public double? DigitalZoomRatio
        {
            get;
            set;
        }

        [XmlElementAttribute]
        public DateTime DateUtc { get; set; }

        [XmlElementAttribute]
        public double? UtcOffset { get; set; }

        [XmlElementAttribute]
        public DateTime DateLastFotoflySave { get; set; }

        [XmlElementAttribute]
        public DateTime AddressOfGpsLookupDate { get; set; }

        [XmlElementAttribute]
        public DateTime OriginalCameraDate { get; set; }

        [XmlElementAttribute]
        public string OriginalCameraFilename { get; set; }

        [XmlElementAttribute]
        public Address AddressOfLocationCreated { get; set; }

        [XmlElementAttribute]
        public Address AddressOfLocationShown { get; set; }

        [XmlElementAttribute]
        public string AddressOfGpsSource { get; set; }

        [XmlIgnore]
        public bool IsUtcOffsetSet
        {
            get
            {
                return this.UtcOffset != null;
            }
        }

        [XmlIgnore]
        public bool IsUtcDateSet
        {
            get
            {
                return this.DateUtc != new DateTime();
            }
        }

        [XmlIgnore]
        public bool IsOriginalCameraDateSet
        {
            get
            {
                return this.OriginalCameraDate != new DateTime();
            }
        }

        public bool IsUtcOffsetCorrect(DateTime dateTaken)
        {
            if (this.UtcOffset == null || this.DateUtc == null)
            {
                return false;
            }
            else
            {
                double utcOffsetInMins = this.UtcOffset.Value * 60;
                double dateGapInMins = new TimeSpan(dateTaken.Ticks - this.DateUtc.Ticks).TotalMinutes;

                return utcOffsetInMins == dateGapInMins;
            }
        }
    }
}
