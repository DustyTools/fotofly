// <copyright file="GpsRoute.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-25</date>
// <summary>Class for a Gps Route</summary>
namespace Fotofly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Fotofly;

    [XmlRootAttribute("GpsRoute", Namespace = "http://www.tassography.com/fotofly")]
    public class GpsRoute
    {
        private List<GpsRoutePoint> points;
        
        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("Point")]
        public List<GpsRoutePoint> Points
        {
            get
            {
                if (this.points == null)
                {
                    this.points = new List<GpsRoutePoint>();
                }

                return this.points;
            }
            set
            {
                this.points = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}