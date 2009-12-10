// <copyright file="GeoCacheRecord.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>Record stored in from Gps Cache</summary>
namespace FotoFly.Geotagging.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FotoFly;

    public class GeoCacheRecord
    {
        public GeoCacheRecord()
        {
        }

        public GeoCacheRecord(string hierarchicalName, string latitude, string longitude, string date)
        {
            DateTime satelliteDate;

            if (!DateTime.TryParse(date, out satelliteDate))
            {
                this.Date = new DateTime();
            }

            this.Date = satelliteDate;

            // Create new address
            this.Address = new Address(hierarchicalName.Trim());

            // Create a new GpsPosition
            this.GpsPosition = new GpsPosition(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            this.GpsPosition.SatelliteTime = this.Date;
        }

        public GeoCacheRecord(DateTime date, Address address, GpsPosition gpsPosition)
        {
            this.Date = date;
            this.Address = address;
            this.GpsPosition = gpsPosition;
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

        public GpsPosition GpsPosition
        {
            get;
            set;
        }
    }
}
