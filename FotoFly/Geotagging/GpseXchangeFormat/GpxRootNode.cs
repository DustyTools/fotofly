// <copyright file="GpxRootNode.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that represents a Gpx Root Node</summary>
namespace Fotofly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    // [XmlRootAttribute("gpx")]
    // <gpx xmlns="http://www.topografix.com/GPX/1/1"
    // creator=""
    // version="1.1"
    // xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    // xsi:schemaLocation="http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd">
    [XmlRoot("gpx", Namespace = "http://www.topografix.com/GPX/1/1")]
    public class GpxRootNode
    {
        private List<GpxTrackNode> tracks;
        private List<GpxWaypointNode> waypoints;
        private List<GpxRouteNode> routes;
        private GpxMetadataNode metadata;

        [XmlAttribute("version")]
        public string Version
        {
            get;
            set;
        }

        [XmlAttribute("creator")]
        public string Creator
        {
            get;
            set;
        }

        [XmlAttribute("author")]
        public string Author
        {
            get;
            set;
        }

        [XmlElement("metadata")]
        public GpxMetadataNode Metadata
        {
            get
            {
                if (this.metadata == null)
                {
                    this.metadata = new GpxMetadataNode();
                }

                return this.metadata;
            }

            set
            {
                this.metadata = value;
            }
        }

        [XmlElement("wpt")]
        public List<GpxWaypointNode> Waypoints
        {
            get
            {
                if (this.waypoints == null)
                {
                    this.waypoints = new List<GpxWaypointNode>();
                }

                return this.waypoints;
            }

            set
            {
                this.waypoints = value;
            }
        }

        [XmlElement("trk")]
        public List<GpxTrackNode> Tracks
        {
            get
            {
                if (this.tracks == null)
                {
                    this.tracks = new List<GpxTrackNode>();
                }

                return this.tracks;
            }

            set
            {
                this.tracks = value;
            }
        }

        [XmlElement("rte")]
        public List<GpxRouteNode> Routes
        {
            get
            {
                if (this.routes == null)
                {
                    this.routes = new List<GpxRouteNode>();
                }

                return this.routes;
            }

            set
            {
                this.routes = value;
            }
        }
    }
}
