// <copyright file="GpxPointNode.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that represents a Gpx Point Node</summary>
namespace FotoFly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
   
    public class GpxPointNode
    {
        private double ele;
        private double lat;
        private double lon;
        private string xmlTime;

        [XmlElement("ele")]
        public double Ele
        {
            get
            {
                return this.ele;
            }

            set
            {
                this.ele = value;
            }
        }

        [XmlAttribute("lat")]
        public double Lat
        {
            get
            {
                return this.lat;
            }

            set
            {
                this.lat = value;
            }
        }

        [XmlAttribute("lon")]
        public double Lon
        {
            get
            {
                return this.lon;
            }

            set
            {
                this.lon = value;
            }
        }

        [XmlIgnore]
        public DateTime Time
        {
            get
            {
                // Read the XML, remove the Z to force reading the time as local
                DateTime returnValue = DateTime.Parse(this.xmlTime.Replace("Z", string.Empty));

                returnValue = DateTime.SpecifyKind(returnValue, DateTimeKind.Local);

                return returnValue;
            }

            set
            {
                this.xmlTime = value.ToString("s") + "Z";
            }
        }

        [XmlElement("time")]
        public string XmlTime
        {
            get
            {
                return this.xmlTime;
            }

            set
            {
                this.xmlTime = value;
            }
        }

        public override string ToString()
        {
            return this.Time.ToString() + "  " + this.Lat + "  " + this.Lon;
        }
    }
}
