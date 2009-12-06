// <copyright file="GenericGpsTrack.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that represents a Generic Gps Track</summary>
namespace FotoFly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GpsTrack
    {
        public string Name
        {
            get;
            set;
        }

        public List<GpsPosition> Points
        {
            get;
            set;
        }
    }
}
