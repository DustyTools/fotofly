// <copyright file="GpsFile.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-25</date>
// <summary>Class for a Gps File</summary>
namespace Fotofly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Fotofly;

    [XmlRootAttribute("GpsFile", Namespace = "http://www.tassography.com/fotofly")]
    public class GpsFile
    {
        private List<GpsRoute> routes;
        private List<GpsTrack> tracks;
        private List<GpsWaypoint> waypoints;
        private GpsTrackSummary summary;

        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        [XmlArray("Routes")]
        [XmlArrayItem("Route")]
        public List<GpsRoute> Routes
        {
            get
            {
                if (this.routes == null)
                {
                    this.routes = new List<GpsRoute>();
                }

                return this.routes;
            }
            set
            {
                this.routes = value;
            }
        }

        [XmlArray("Tracks")]
        [XmlArrayItem("Track")]
        public List<GpsTrack> Tracks
        {
            get
            {
                if (this.tracks == null)
                {
                    this.tracks = new List<GpsTrack>();
                }

                return this.tracks;
            }
            set
            {
                this.tracks = value;
            }
        }

        [XmlArray("Waypoints")]
        [XmlArrayItem("Waypoint")]
        public List<GpsWaypoint> Waypoints
        {
            get
            {
                if (this.waypoints == null)
                {
                    this.waypoints = new List<GpsWaypoint>();
                }

                return this.waypoints;
            }
            set
            {
                this.waypoints = value;
            }
        }

        [XmlElement]
        public GpsTrackSummary TrackSummary
        {
            get
            {
                if (this.summary == null)
                {
                    this.summary = new GpsTrackSummary();
                }

                return this.summary;
            }
            set
            {
                this.summary = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}