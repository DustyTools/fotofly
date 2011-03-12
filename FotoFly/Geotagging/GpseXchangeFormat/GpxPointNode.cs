// <copyright file="GpxPointNode.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that represents a Gpx Point Node</summary>
namespace Fotofly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
   
    public class GpxPointNode
    {
        [XmlElement("ele")]
        public double Ele
        {
            get;
            set;
        }

        [XmlAttribute("lat")]
        public double Lat
        {
            get;
            set;
        }

        [XmlAttribute("lon")]
        public double Lon
        {
            get;
            set;
        }

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

        public override string ToString()
        {
            return this.Time.ToString() + "  " + this.Lat + "  " + this.Lon;
        }
    }
}
