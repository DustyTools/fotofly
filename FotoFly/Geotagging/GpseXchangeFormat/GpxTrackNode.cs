// <copyright file="GpxTrackNode.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that represents a Gpx Track Node</summary>
namespace FotoFly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    public class GpxTrackNode
    {
        private string name;
        private List<GpxSegmentNode> segment;

        [XmlElement("name")]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        [XmlElement("trkseg")]
        public List<GpxSegmentNode> Segments
        {
            get
            {
                if (this.segment == null)
                {
                    this.segment = new List<GpxSegmentNode>();
                }

                return this.segment;
            }

            set
            {
                this.segment = value;
            }
        }

        [XmlIgnore]
        private DateTime DateOfFirstPoint
        {
            get
            {
                return this.Segments[0].Points[0].Time;
            }
        }

        [XmlIgnore]
        private DateTime DateOfLastPoint
        {
            get
            {
                return this.Segments[0].Points[this.Segments[0].Points.Count - 1].Time;
            }
        }
    }
}
