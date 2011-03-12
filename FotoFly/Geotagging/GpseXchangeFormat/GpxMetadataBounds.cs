// <copyright file="GpxMetadataBounds.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-24</date>
// <summary>Class that represents a Gpx Metadata Bounds</summary>
namespace Fotofly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
   
    public class GpxMetadataBounds
    {
        [XmlAttribute("maxlat")]
        public double XmlMaxLat
        {
            get;
            set;
        }

        [XmlAttribute("maxlon")]
        public double XmlMaxLon
        {
            get;
            set;
        }

        [XmlAttribute("minlat")]
        public double XmlMinLat
        {
            get;
            set;
        }

        [XmlAttribute("minlon")]
        public double XmlMinLon
        {
            get;
            set;
        }

        [XmlIgnore]
        public GpsPosition MinCoordinate
        {
            get
            {
                return new GpsPosition(this.XmlMinLat, this.XmlMinLon);
            }
            set
            {
                if (value.IsValidCoordinate)
                {
                    this.XmlMinLat = value.Latitude.Numeric;
                    this.XmlMinLon = value.Longitude.Numeric;
                }
                else
                {
                    this.XmlMinLat = double.NaN;
                    this.XmlMinLon = double.NaN;
                }
            }
        }

        [XmlIgnore]
        public GpsPosition MaxCoordinate
        {
            get
            {
                return new GpsPosition(this.XmlMaxLat, this.XmlMaxLon);
            }
            set
            {
                if (value.IsValidCoordinate)
                {
                    this.XmlMaxLat = value.Latitude.Numeric;
                    this.XmlMaxLon = value.Longitude.Numeric;
                }
                else
                {
                    this.XmlMaxLat = double.NaN;
                    this.XmlMaxLon = double.NaN;
                }
            }
        }
    }
}
