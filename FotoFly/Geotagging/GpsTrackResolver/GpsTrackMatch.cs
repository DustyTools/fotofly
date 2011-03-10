// <copyright file="GpsTrackManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that represents a GpsTrackManager</summary>
namespace Fotofly.Geotagging.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class GpsTrackMatch
    {
        private double accuracyInMinutes;
        private double accuracyInMeters;

        private GpsPosition gpsPositionMiddle;

        public GpsTrackMatch()
        {
            this.gpsPositionMiddle = new GpsPosition();
            this.GpsPositionAfter = new GpsPosition();
            this.GpsPositionBefore = new GpsPosition();
        }

        public GpsPosition GpsPositionMiddle
        {
            get { return this.gpsPositionMiddle; }
        }

        public GpsPosition GpsPositionBefore
        {
            get;
            set;
        }

        public GpsPosition GpsPositionAfter
        {
            get;
            set;
        }

        public double AccuracyInMinutes
        {
            get { return Math.Round(this.accuracyInMinutes, 0); }
            set { this.accuracyInMinutes = value; }
        }

        public double AccuracyInMeters
        {
            get { return Math.Round(this.accuracyInMeters, 0); }
            set { this.accuracyInMeters = value; }
        }

        public void SetGpsPositionDate(DateTime utcTime)
        {
            this.gpsPositionMiddle = new GpsPosition();

            // Ensure Points are valid and utcTime is inbetween the two points
            if (this.GpsPositionBefore.IsValidCoordinate
                && this.GpsPositionAfter.IsValidCoordinate
                && this.GpsPositionBefore.Time < utcTime
                && this.GpsPositionAfter.Time > utcTime)
            {
                // Set common properties
                this.gpsPositionMiddle.Source = this.GpsPositionAfter.Source;

                // Work out the difference between the two points in nano seconds
                long nanoSecondsGap = this.GpsPositionAfter.Time.Ticks - this.GpsPositionBefore.Time.Ticks;

                // Work out LatLong distance between the two points
                double latDistance = this.GpsPositionAfter.Latitude.Numeric - this.GpsPositionBefore.Latitude.Numeric;
                double longDistance = this.GpsPositionAfter.Longitude.Numeric - this.GpsPositionBefore.Longitude.Numeric;
                double elevDistance = this.GpsPositionAfter.Altitude - this.GpsPositionBefore.Altitude;

                long nanoSecondsPhoto = utcTime.Ticks - this.GpsPositionBefore.Time.Ticks;

                // Work out the new values using the % difference between the two points and add this to the first point
                // Round Lat\Long to 6 decimals and altitude to 3
                if (nanoSecondsGap == 0)
                {
                    this.gpsPositionMiddle.Latitude.Numeric = this.GpsPositionBefore.Latitude.Numeric;
                    this.gpsPositionMiddle.Longitude.Numeric = this.GpsPositionBefore.Longitude.Numeric;
                    this.gpsPositionMiddle.Altitude = this.GpsPositionBefore.Altitude;
                }
                else
                {
                    this.gpsPositionMiddle.Latitude.Numeric = Math.Round((latDistance * nanoSecondsPhoto / nanoSecondsGap) + this.GpsPositionBefore.Latitude.Numeric, 6);
                    this.gpsPositionMiddle.Longitude.Numeric = Math.Round((longDistance * nanoSecondsPhoto / nanoSecondsGap) + this.GpsPositionBefore.Longitude.Numeric, 6);
                    this.gpsPositionMiddle.Altitude = Math.Round((elevDistance * nanoSecondsPhoto / nanoSecondsGap) + this.GpsPositionBefore.Altitude, 3);
                }

                // Set the match time based on the closest satellite
                if (this.GpsPositionBefore.Time.Ticks - utcTime.Ticks > utcTime.Ticks - this.GpsPositionAfter.Time.Ticks)
                {
                    this.gpsPositionMiddle.Time = this.GpsPositionBefore.Time;
                }
                else
                {
                    this.gpsPositionMiddle.Time = this.GpsPositionAfter.Time;
                }
            }
        }
    }
}
