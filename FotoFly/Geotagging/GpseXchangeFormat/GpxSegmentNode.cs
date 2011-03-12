// <copyright file="GpxSegmentNode.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that represents a Gpx Segment Node</summary>
namespace Fotofly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    public class GpxSegmentNode
    {
        private List<GpxPointNode> points;

        [XmlElement("trkpt")]
        public List<GpxPointNode> Points
        {
            get
            {
                if (this.points == null)
                {
                    this.points = new List<GpxPointNode>();
                }

                return this.points;
            }

            set
            {
                this.points = value;
            }
        }

        [XmlIgnore]
        public DateTime DateOfFirstPoint
        {
            get
            {
                return this.Points[0].Time;
            }
        }

        [XmlIgnore]
        public DateTime DateOfLastPoint
        {
            get
            {
                return this.Points[this.Points.Count - 1].Time;
            }
        }
    }
}
