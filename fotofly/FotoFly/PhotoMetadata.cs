// <copyright file="JpgMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>JpgMetadata</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class PhotoMetadata : IPhotoMetadata
    {
        private XmpRegionInfo regionInfo;
        private GpsPosition gpsPosition;
        private PeopleList authors;
        private TagList tags;
        private Address iptcAddress;

        public PhotoMetadata()
        {
        }

        [XmlAttribute]
        public string Aperture
        {
            get;
            set;
        }

        [XmlAttribute]
        public string ShutterSpeed
        {
            get;
            set;
        }

        [XmlIgnore]
        public string ShutterSpeedAndAperture
        {
            get
            {
                return this.ShutterSpeed + " " + this.Aperture;
            }
        }

        [XmlAttribute]
        public string ExposureBias
        {
            get;
            set;
        }

        [XmlAttribute]
        public string FocalLength
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Copyright
        {
            get;
            set;
        }

        [XmlAttribute]
        public string CameraModel
        {
            get;
            set;
        }

        [XmlAttribute]
        public string CameraManufacturer
        {
            get;
            set;
        }

        [XmlElementAttribute]
        public XmpRegionInfo RegionInfo
        {
            get
            {
                if (this.regionInfo == null)
                {
                    this.regionInfo = new XmpRegionInfo();
                }

                return this.regionInfo;
            }

            set
            {
                this.regionInfo = value;
            }
        }

        [XmlArray]
        public TagList Tags
        {
            get
            {
                if (this.tags == null)
                {
                    this.tags = new TagList();
                }

                return this.tags;
            }

            set
            {
                this.tags = value;
            }
        }

        [XmlElementAttribute]
        public Address IptcAddress
        {
            get
            {
                if (this.iptcAddress == null)
                {
                    this.iptcAddress = new Address();
                }

                return this.iptcAddress;
            }

            set
            {
                this.iptcAddress = value;
            }
        }

        [XmlElementAttribute]
        public GpsPosition GpsPosition
        {
            get
            {
                if (this.gpsPosition == null)
                {
                    this.gpsPosition = new GpsPosition();
                }

                return this.gpsPosition;
            }

            set
            {
                this.gpsPosition = value;
            }
        }

        [XmlAttribute]
        public DateTime DateTaken
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Iso
        {
            get;
            set;
        }

        [XmlAttribute("Width")]
        public int ImageWidth
        {
            get;
            set;
        }

        [XmlAttribute("Height")]
        public int ImageHeight
        {
            get;
            set;
        }

        [XmlAttribute]
        public int Rating
        {
            get;
            set;
        }

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

        public PeopleList Authors
        {
            get
            {
                if (this.authors == null)
                {
                    this.authors = new PeopleList();
                }

                return this.authors;
            }

            set
            {
                this.authors = value;
            }
        }

        [XmlAttribute]
        public string Comment
        {
            get;
            set;
        }

        [XmlIgnore]
        public DateTime DateDigitised
        {
            get;
            set;
        }

        [XmlIgnore]
        public PeopleList People
        {
            get
            {
                PeopleList people = new PeopleList();

                foreach (XmpRegion region in this.RegionInfo.Regions)
                {
                    people.Add(region.PersonDisplayName);
                }

                return people;
            }
        }

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

        [XmlIgnore]
        public int VerticalResolution
        {
            get;
            set;
        }

        [XmlIgnore]
        public int HorizontalResolution
        {
            get;
            set;
        }

        [XmlAttribute]
        public double DigitalZoomRatio
        {
            get;
            set;
        }
    }
}
