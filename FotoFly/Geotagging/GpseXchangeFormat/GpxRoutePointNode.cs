// <copyright file="GpxRoutePointNode.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-24</date>
// <summary>Class that represents a Gpx Route Point Node</summary>
namespace Fotofly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
   
    public class GpxRoutePointNode
    {
        [XmlElement("src")]
        public string Source
        {
            get;
            set;
        }

        [XmlElement("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("cmt")]
        public string Comment
        {
            get;
            set;
        }

        [XmlElement("desc")]
        public string Description
        {
            get;
            set;
        }
        
        [XmlElement("sym")]
        public string Symbol
        {
            get;
            set;
        }

        [XmlIgnore]
        public DateTime Time
        {
            get
            {
                if (string.IsNullOrEmpty(this.XmlTime))
                {
                    return new DateTime();
                }
                else
                {
                    // Read the XML, remove the Z to force reading the time as local
                    DateTime returnValue = DateTime.Parse(this.XmlTime.Replace("Z", string.Empty));

                    returnValue = DateTime.SpecifyKind(returnValue, DateTimeKind.Local);

                    return returnValue;
                }
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

        [XmlAttribute("lat")]
        public double XmlLat
        {
            get;
            set;
        }

        [XmlAttribute("lon")]
        public double XmlLon
        {
            get;
            set;
        }

        [XmlIgnore]
        public GpsPosition Position
        {
            get
            {
                return new GpsPosition(this.XmlLat, this.XmlLon);
            }
            set
            {
                if (value.IsValidCoordinate)
                {
                    this.XmlLat = value.Latitude.Numeric;
                    this.XmlLon = value.Longitude.Numeric;
                }
                else
                {
                    this.XmlLat = double.NaN;
                    this.XmlLon = double.NaN;
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
