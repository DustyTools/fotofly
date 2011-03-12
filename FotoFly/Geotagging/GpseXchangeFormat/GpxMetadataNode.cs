// <copyright file="GpxMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-24</date>
// <summary>Class that represents a Gpx Metadata Node</summary>
namespace Fotofly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
   
    public class GpxMetadataNode
    {
        private GpxMetadataLink link;
        private GpxMetadataBounds bounds;

        [XmlIgnore]
        public DateTime Time
        {
            get
            {
                // Read the XML, remove the Z to force reading the time as local
                DateTime returnValue = DateTime.Parse(this.XmlTime.Replace("Z", string.Empty));

                returnValue = DateTime.SpecifyKind(returnValue, DateTimeKind.Local);

                return returnValue;
            }

            set
            {
                this.XmlTime = value.ToString("s") + "Z";
            }
        }

        [XmlElement("time")]
        public string XmlTime
        {
            get;
            set;
        }

        [XmlElement("link")]
        public GpxMetadataLink Link
        {
            get
            {
                if (this.link == null)
                {
                    this.link = new GpxMetadataLink();
                }

                return this.link;
            }
            set
            {
                this.link = value;
            }
        }

        [XmlElement("bounds")]
        public GpxMetadataBounds Bounds 
        {
            get
            {
                if (this.bounds == null)
                {
                    this.bounds = new GpxMetadataBounds();
                }

                return this.bounds;
            }
            set
            {
                this.bounds = value;
            }
        }
    }
}
