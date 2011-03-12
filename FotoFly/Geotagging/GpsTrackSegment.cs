// <copyright file="GpsTrackSegment.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-25</date>
// <summary>Class that a Gps Track Segment</summary>
namespace Fotofly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Fotofly;

    [XmlRootAttribute("GpsTrackSegment", Namespace = "http://www.tassography.com/fotofly")]
    public class GpsTrackSegment
    {
        private List<GpsTrackPoint> points;

        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("Point")]
        public List<GpsTrackPoint> Points
        {
            get
            {
                if (this.points == null)
                {
                    this.points = new List<GpsTrackPoint>();
                }

                return this.points;
            }
            set
            {
                this.points = value;
            }
        }
    }
}
