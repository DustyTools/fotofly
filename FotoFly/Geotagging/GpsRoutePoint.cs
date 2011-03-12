// <copyright file="GpsRoutePoint.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-25</date>
// <summary>Class for a Gps Route Point</summary>
namespace Fotofly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Fotofly;

    [XmlRootAttribute("GpsRoutePoint", Namespace = "http://www.tassography.com/fotofly")]
    public class GpsRoutePoint : GpsPosition
    {
        public GpsRoutePoint()
        {
        }

        public GpsRoutePoint(GpsPosition gpsPosition) : base (gpsPosition)
        {
        }

        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Comment
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Description
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Symbol
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}