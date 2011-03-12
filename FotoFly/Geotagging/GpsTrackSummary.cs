// <copyright file="GpsTrackSummary.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-25</date>
// <summary>Class for a Gps Track Summary</summary>
namespace Fotofly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    
    using Fotofly;

    [XmlRootAttribute("GpsTrackSummary", Namespace = "http://www.tassography.com/fotofly")]
    public class GpsTrackSummary
    {
        [XmlElement]
        public double AltitudeAverage
        {
            get;
            set;
        }

        [XmlElement]
        public double AltitudeMaximum
        {
            get;
            set;
        }

        [XmlElement]
        public double AltitudeChanged
        {
            get;
            set;
        }

        [XmlElement]
        public double AltitudeMinimum
        {
            get;
            set;
        }

        [XmlElement]
        public double DistanceEndToEnd
        {
            get;
            set;
        }

        [XmlElement]
        public double DistanceTracked
        {
            get;
            set;
        }

        [XmlElement]
        public double Height
        {
            get;
            set;
        }

        [XmlElement]
        public double Width
        {
            get;
            set;
        }

        [XmlElement]
        public DateTime StartUtc
        {
            get;
            set;
        }

        [XmlElement]
        public DateTime EndUtc
        {
            get;
            set;
        }

        [XmlElement]
        public GpsPosition TopLeft
        {
            get;
            set;
        }

        [XmlElement]
        public GpsPosition BottomRight
        {
            get;
            set;
        }

        [XmlElement]
        public GpsPosition Center
        {
            get;
            set;
        }
    }
}
