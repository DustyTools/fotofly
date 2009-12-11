// <copyright file="GpsTrackManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that represents a GpsTrackManager</summary>
namespace FotoFly.Geotagging.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    using FotoFly;
    using FotoFly.GpseXchangeFormat;

    public class GpsTrackResolver
    {
        private readonly long defaultToleranceInMinutes = 5;
        private readonly long defaultToleranceInMeters = 500;

        private DateTime earliestUtcDate;
        private DateTime latestUtcDate;

        public GpsTrackResolver()
        {
            this.TrackPoints = new List<GpsPosition>();

            this.ToleranceInMeters = this.defaultToleranceInMeters;
            this.ToleranceInMinutes = this.defaultToleranceInMinutes;
            this.GpsTrackSource = string.Empty;
        }

        public string GpsTrackSource
        {
            get;
            set;
        }

        public long ToleranceInMinutes
        {
            get;
            set;
        }

        public long ToleranceInMeters
        {
            get;
            set;
        }

        public DateTime StartOfTrack
        {
            get
            {
                return this.TrackPoints.First().SatelliteTime;
            }
        }

        public DateTime EndOfTrack
        {
            get
            {
                return this.TrackPoints.Last().SatelliteTime;
            }
        }

        public List<GpsPosition> TrackPoints
        {
            get;
            set;
        }

        public void Add(GpxFile gpxFile)
        {
            if (gpxFile != null && gpxFile.RootNode != null && gpxFile.RootNode.Tracks.Count > 0)
            {
                // Loop through each Track
                foreach (GpxTrackNode gpxTrackNode in gpxFile.RootNode.Tracks)
                {
                    // Loop through each Segment
                    foreach (GpxSegmentNode gpxSegmentNode in gpxTrackNode.Segments)
                    {
                        // Loop through each Point
                        foreach (GpxPointNode pointNode in gpxSegmentNode.Points)
                        {
                            GpsPosition newCoordinate = new GpsPosition();
                            newCoordinate.SatelliteTime = pointNode.Time;
                            newCoordinate.Latitude.Numeric = pointNode.Lat;
                            newCoordinate.Longitude.Numeric = pointNode.Lon;
                            newCoordinate.Altitude = pointNode.Ele;
                            newCoordinate.Dimension = GpsPosition.Dimensions.ThreeDimensional;

                            // Use the tracks data or the parameter override
                            newCoordinate.Source = string.IsNullOrEmpty(this.GpsTrackSource) ? gpxFile.RootNode.Creator : this.GpsTrackSource;

                            // Add the point to the cache
                            this.TrackPoints.Add(newCoordinate);
                        }
                    }
                }

                // ReOptimise the Track
                this.OptimiseTrack();
            }
        }

        /// <summary>
        /// Deletes any points before or after the dates
        /// </summary>
        /// <param name="earliestUtcDate">The earliest possible date of a time to match</param>
        /// <param name="latestUtcDate">The latest possible date of a time to match</param>
        public void TrimTrackPoints(DateTime earliestUtcDate, DateTime latestUtcDate)
        {
            this.earliestUtcDate = earliestUtcDate;
            this.latestUtcDate = latestUtcDate;

            this.OptimiseTrack();
        }

        public GpsPosition FindGpsPosition(DateTime utcTime)
        {
            GpsTrackMatch gpsTrackMatch = new GpsTrackMatch();

            if (this.StartOfTrack < utcTime && this.EndOfTrack > utcTime)
            {
                // Find the closet point before utcTime
                var beforeQuery = from x in this.TrackPoints
                                  where x.SatelliteTime < utcTime
                                  orderby x.SatelliteTime
                                  select x;

                // Find the closest point after utcTime
                var afterQuery = from x in this.TrackPoints
                                 where x.SatelliteTime > utcTime
                                 orderby x.SatelliteTime
                                 select x;

                gpsTrackMatch.GpsPositionBefore = beforeQuery.LastOrDefault();
                gpsTrackMatch.GpsPositionAfter = afterQuery.FirstOrDefault();

                // Check we have a before and after point
                if (gpsTrackMatch.GpsPositionBefore == null || gpsTrackMatch.GpsPositionAfter == null)
                {
                    gpsTrackMatch = new GpsTrackMatch();
                }
                else
                {
                    // Calculate the inbetween point
                    gpsTrackMatch.SetGpsPositionDate(utcTime);

                    // Work out the time before and after
                    long minsBefore = (utcTime.Ticks - gpsTrackMatch.GpsPositionBefore.SatelliteTime.Ticks) / TimeSpan.TicksPerMinute;
                    long minsAfter = (gpsTrackMatch.GpsPositionAfter.SatelliteTime.Ticks - utcTime.Ticks) / TimeSpan.TicksPerMinute;

                    // Save accurancy in Minutes of the closest point
                    gpsTrackMatch.AccuracyInMinutes = minsBefore > minsAfter ? minsAfter : minsBefore;

                    // If the point is close enough, then use it
                    // Else see if the distance is with in tolerance
                    if (gpsTrackMatch.AccuracyInMinutes <= this.ToleranceInMinutes)
                    {
                        Debug.WriteLine(string.Format("Geotagging Time Match, mins before {0} mins after {1}", minsBefore, minsAfter));
                    }
                    else
                    {
                        // Work out distance between the two points
                        double distanceBefore = gpsTrackMatch.GpsPositionBefore.Distance(gpsTrackMatch.GpsPositionMiddle);
                        double distanceAfter = gpsTrackMatch.GpsPositionMiddle.Distance(gpsTrackMatch.GpsPositionAfter);

                        // Record Accuracy in Meters of closest point
                        gpsTrackMatch.AccuracyInMeters = distanceBefore > distanceAfter ? distanceAfter : distanceBefore;

                        // Check it's with in the accuracy
                        if (gpsTrackMatch.AccuracyInMeters < this.defaultToleranceInMeters)
                        {
                            Debug.WriteLine(string.Format("Geotagging Distance Match, mins before {0} mins after {1}", minsBefore, minsAfter));
                        }
                        else
                        {
                            Debug.WriteLine(string.Format("Geotagging Failed, mins before {0} mins after {1} distance {2}", minsBefore, minsAfter, gpsTrackMatch.AccuracyInMeters));

                            gpsTrackMatch = new GpsTrackMatch();
                        }
                    }
                }
            }

            return gpsTrackMatch.GpsPositionMiddle;
        }

        private void OptimiseTrack()
        {
            // Trim the track or just sort it
            if (this.earliestUtcDate != new DateTime() && this.latestUtcDate != new DateTime())
            {
                // Filter out the points that are not needed and sort
                var query = from x in this.TrackPoints
                            where x.SatelliteTime > this.earliestUtcDate
                            && x.SatelliteTime < this.latestUtcDate
                            orderby x.SatelliteTime
                            select x;

                // Save the points list
                this.TrackPoints = query.ToList();
            }
            else
            {
                // Filter out the points that are not needed and sort
                var query = from x in this.TrackPoints
                            orderby x.SatelliteTime
                            select x;

                // Save the points list
                this.TrackPoints = query.ToList();
            }
        }
    }
}
