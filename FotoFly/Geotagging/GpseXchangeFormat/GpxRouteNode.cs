// <copyright file="GpxRouteNode.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-24</date>
// <summary>Class that represents a Gpx Route Node</summary>
namespace Fotofly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
   
    public class GpxRouteNode
    {
        /*
          <rte>
            <name>12/20 ARK to 12/23 Ngorongoro Wildlife Lodge</name>
            <rtept lat="-3.375198" lon="36.631262">
              <time>2010-08-25T02:42:24Z</time>
              <name>12/20 ARK</name>
              <sym>Airport</sym>
            </rtept>
          </rte>
        */

        private List<GpxRoutePointNode> routePoints;

        [XmlElement("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("rtept")]
        public List<GpxRoutePointNode> RoutePoints
        {
            get
            {
                if (this.routePoints == null)
                {
                    this.routePoints = new List<GpxRoutePointNode>();
                }

                return this.routePoints;
            }
            set
            {
                this.routePoints = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
