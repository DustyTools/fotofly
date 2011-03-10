// <copyright file="GpsTrack.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-25</date>
// <summary>Class for a Gps Track</summary>
namespace Fotofly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [XmlRootAttribute("GpsTrack", Namespace = "http://www.tassography.com/fotofly")]
    public class GpsTrack  : GpsTrackSummary
    {
        private List<GpsTrackSegment> segments;
        private GpsTrackSummary summary;

        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("Segment")]
        public List<GpsTrackSegment> Segments
        {
            get
            {
                if (this.segments == null)
                {
                    this.segments = new List<GpsTrackSegment>();
                }

                return this.segments;
            }
            set
            {
                this.segments = value;
            }
        }
    }
}
