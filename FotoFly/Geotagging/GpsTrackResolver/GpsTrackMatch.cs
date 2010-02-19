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
                && this.GpsPositionBefore.SatelliteTime < utcTime
                && this.GpsPositionAfter.SatelliteTime > utcTime)
            {
                // Set common properties
                this.gpsPositionMiddle.Dimension = GpsPosition.Dimensions.ThreeDimensional;
                this.gpsPositionMiddle.Source = this.GpsPositionAfter.Source;
                this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.Premise;

                // Work out the difference between the two points in nano seconds
                long nanoSecondsGap = this.GpsPositionAfter.SatelliteTime.Ticks - this.GpsPositionBefore.SatelliteTime.Ticks;

                // Work out LatLong distance between the two points
                double latDistance = this.GpsPositionAfter.Latitude.Numeric - this.GpsPositionBefore.Latitude.Numeric;
                double longDistance = this.GpsPositionAfter.Longitude.Numeric - this.GpsPositionBefore.Longitude.Numeric;
                double elevDistance = this.GpsPositionAfter.Altitude - this.GpsPositionBefore.Altitude;

                long nanoSecondsPhoto = utcTime.Ticks - this.GpsPositionBefore.SatelliteTime.Ticks;

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
                if (this.GpsPositionBefore.SatelliteTime.Ticks - utcTime.Ticks > utcTime.Ticks - this.GpsPositionAfter.SatelliteTime.Ticks)
                {
                    this.gpsPositionMiddle.SatelliteTime = this.GpsPositionBefore.SatelliteTime;
                }
                else
                {
                    this.gpsPositionMiddle.SatelliteTime = this.GpsPositionAfter.SatelliteTime;
                }

                // Set Accuracy
                double distanceToPointA = this.GpsPositionBefore.Distance(this.gpsPositionMiddle);
                double distanceToPointB = this.GpsPositionAfter.Distance(this.gpsPositionMiddle);

                // Work out farthest point because this is the least accurate point
                double distanceToFarthestPoint = distanceToPointA > distanceToPointB ? distanceToPointA : distanceToPointB;

                // Calculate Accuracy
                if (distanceToFarthestPoint < 100)
                {
                    this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.Address;
                }
                else if (distanceToFarthestPoint < 1000)
                {
                    this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.Street;
                }
                else if (distanceToFarthestPoint < 10000)
                {
                    this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.City;
                }
                else if (distanceToFarthestPoint < 50000)
                {
                    this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.Region;
                }
                else
                {
                    this.gpsPositionMiddle.Accuracy = GpsPosition.Accuracies.Country;
                }
            }
        }
    }
}
