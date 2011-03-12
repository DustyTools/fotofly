// <copyright file="GpsTrackPoint.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-25</date>
// <summary>Class that a Gps Track Point</summary>
namespace Fotofly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Fotofly;

    [XmlRootAttribute("GpsTrackPoint", Namespace = "http://www.tassography.com/fotofly")]
    public class GpsTrackPoint : GpsPosition
    {
    }
}
