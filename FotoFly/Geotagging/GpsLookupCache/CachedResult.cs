// <copyright file="CachedResult.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Restult Returned from Gps Cache</summary>
namespace FotoFly.Geotagging.GpsLookupCache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FotoFly;

    public class CachedResult
    {
        public enum ResultTypes
        {
            Manual,
            BingAddressLookup,
            BingGpsLookup,
            GoogleAddressLookup
        }

        public DateTime Date
        {
            get;
            set;
        }

        public Address Address
        {
            get;
            set;
        }

        public ResultTypes ResultType
        {
            get;
            set;
        }

        public GpsPosition GpsPosition
        {
            get;
            set;
        }
    }
}
