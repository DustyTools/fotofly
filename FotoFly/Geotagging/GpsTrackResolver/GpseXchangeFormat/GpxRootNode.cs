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
        private string creator;
        private List<GpxTrackNode> tracks;
        private string version;

        [XmlAttribute("creator")]
        public string Creator
        {
            get
            {
                return this.creator;
            }

            set
            {
                this.creator = value;
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

        [XmlAttribute("version")]
        public string Version
        {
            get
            {
                return this.version;
            }

            set
            {
                this.version = value;
            }
        }
    }
}
