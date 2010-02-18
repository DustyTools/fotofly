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
    /// Class representing Industry Standard Metadata
    /// </summary>
    public class PhotoMetadata
    {
        public PhotoMetadata()
        {
            this.RegionInfo = new ImageRegionInfo();
            this.GpsPosition = new GpsPosition();
            this.Authors = new PeopleList();
            this.Tags = new TagList();
            this.Address = new Address();
            this.AddressOfGps = new Address();
        }

        /// <summary>
        /// Aperture
        /// </summary>
        [XmlAttribute]
        public string Aperture
        {
            get;
            set;
        }

        /// <summary>
        /// Software used to last modify the photo
        /// </summary>
        [XmlIgnore]
        public string CreationSoftware
        {
            get;
            set;
        }

        /// <summary>
        /// Shutter Speed
        /// </summary>
        [XmlAttribute]
        public string ShutterSpeed
        {
            get;
            set;
        }

        /// <summary>
        /// Shutter Speed and Aperture (Readonly)
        /// </summary>
        [XmlIgnore]
        public string ShutterSpeedAndAperture
        {
            get
            {
                return this.ShutterSpeed + " " + this.Aperture;
            }
        }

        /// <summary>
        /// Exposure Bias
        /// </summary>
        [XmlAttribute]
        public string ExposureBias
        {
            get;
            set;
        }

        /// <summary>
        /// Focal Length
        /// </summary>
        [XmlAttribute]
        public string FocalLength
        {
            get;
            set;
        }

        /// <summary>
        /// Copyright owner of the photo
        /// </summary>
        [XmlAttribute]
        public string Copyright
        {
            get;
            set;
        }

        /// <summary>
        /// Camera Model, normally includes camera Manufacturer
        /// </summary>
        [XmlAttribute]
        public string CameraModel
        {
            get;
            set;
        }

        /// <summary>
        /// Camera Manufacturer
        /// </summary>
        [XmlAttribute]
        public string CameraManufacturer
        {
            get;
            set;
        }

        /// <summary>
        /// Metering Mode
        /// </summary>
        [XmlAttribute]
        public PhotoMetadataEnums.MeteringModes MeteringMode
        {
            get;
            set;
        }

        /// <summary>
        /// Microsoft Region Info extension which provides data on regions in the photo
        /// </summary>
        [XmlElementAttribute]
        public ImageRegionInfo RegionInfo
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
        /// Gps Position
        /// </summary>
        [XmlElementAttribute]
        public GpsPosition GpsPosition
        {
            get;
            set;
        }

        /// <summary>
        /// DateAquired, Microsoft Windows7 Property field
        /// </summary>
        [XmlAttribute]
        public DateTime DateAquired
        {
            get;
            set;
        }

        /// <summary>
        /// DateTaken, recorded by the camera when the photo is taken
        /// </summary>
        [XmlAttribute]
        public DateTime DateTaken
        {
            get;
            set;
        }

        /// <summary>
        /// ISO Speed rating 
        /// </summary>
        [XmlAttribute]
        public string Iso
        {
            get;
            set;
        }

        /// <summary>
        /// Image Width measured in Pixels
        /// </summary>
        [XmlAttribute("Width")]
        public int ImageWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Image Height measured in Pixels
        /// </summary>
        [XmlAttribute("Height")]
        public int ImageHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Rating (Ranging 0-5)
        /// </summary>
        [XmlAttribute]
        public int Rating
        {
            get;
            set;
        }

        /// <summary>
        /// Subject, not often used by software, Title should be used in most cases
        /// </summary>
        [XmlAttribute]
        public string Subject
        {
            get;
            set;
        }

        /// <summary>
        /// Title, sometimes knows as Caption and Subject (there is also another attribute for Subject)
        /// </summary>
        [XmlAttribute]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// List of Authors, also known as Photographer
        /// </summary>
        public PeopleList Authors
        {
            get;
            set;
        }

        /// <summary>
        /// Comment, also known as Description
        /// </summary>
        [XmlAttribute]
        public string Comment
        {
            get;
            set;
        }

        /// <summary>
        /// DateDigitized, recorded by the camera when the photo is taken
        /// </summary>
        [XmlIgnore]
        public DateTime DateDigitised
        {
            get;
            set;
        }

        /// <summary>
        /// List of People stored in Region (Readonly)
        /// </summary>
        [XmlIgnore]
        public PeopleList People
        {
            get
            {
                PeopleList people = new PeopleList();

                foreach (ImageRegion region in this.RegionInfo.Regions)
                {
                    people.Add(region.PersonDisplayName);
                }

                return people;
            }
        }

        /// <summary>
        /// Orientation of the Image (Readonly)
        /// </summary>
        [XmlIgnore]
        public PhotoMetadataEnums.ImageOrientations Orientation
        {
            get
            {
                // Work out Orientation
                if (this.ImageHeight > this.ImageWidth)
                {
                    return PhotoMetadataEnums.ImageOrientations.Portrait;
                }
                else
                {
                    return PhotoMetadataEnums.ImageOrientations.Landscape;
                }
            }
        }

        /// <summary>
        /// Vertical Resolution of main photo
        /// </summary>
        [XmlIgnore]
        public int VerticalResolution
        {
            get;
            set;
        }

        /// <summary>
        /// Horizontal Resolution of main photo
        /// </summary>
        [XmlIgnore]
        public int HorizontalResolution
        {
            get;
            set;
        }

        /// <summary>
        /// DigitalZoomRatio
        /// </summary>
        [XmlAttribute]
        public double DigitalZoomRatio
        {
            get;
            set;
        }

        [XmlAttribute]
        public DateTime UtcDate { get; set; }

        [XmlElement]
        public double? UtcOffset { get; set; }

        [XmlAttribute]
        public DateTime FotoflyLastEditDate { get; set; }

        [XmlAttribute]
        public DateTime AddressOfGpsLookupDate { get; set; }

        [XmlAttribute]
        public DateTime OriginalCameraDate { get; set; }

        [XmlAttribute]
        public string OriginalCameraFilename { get; set; }

        public Address AddressOfGps { get; set; }

        public Address Address { get; set; }

        [XmlAttribute]
        public string AddressOfGpsSource { get; set; }

        [XmlAttribute]
        public GpsPosition.Accuracies AccuracyOfGps { get; set; }

        [XmlIgnore]
        public bool IsUtcOffsetSet
        {
            get
            {
                return this.UtcOffset != null;
            }
        }

        [XmlIgnore]
        public bool IsAddressOfGpsSet
        {
            get
            {
                return this.AddressOfGps.IsValidAddress;
            }
        }

        [XmlIgnore]
        public bool IsUtcDateSet
        {
            get
            {
                return this.UtcDate != new DateTime();
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
            if (this.UtcOffset == null || this.UtcDate == null)
            {
                return false;
            }
            else
            {
                double utcOffsetInMins = this.UtcOffset.Value * 60;
                double dateGapInMins = new TimeSpan(dateTaken.Ticks - this.UtcDate.Ticks).TotalMinutes;

                return utcOffsetInMins == dateGapInMins;
            }
        }
    }
}
