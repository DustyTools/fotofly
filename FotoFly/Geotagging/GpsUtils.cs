// <copyright file="GpsUtils.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-25</date>
// <summary>Class for Gps Utils</summary>
namespace Fotofly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Fotofly;
    using Fotofly.GpseXchangeFormat;

    public static class GpsUtils
    {
        public static GpsFile ConvertToFile(GpxFile gpxFile)
        {
            GpsFile gpsFile = new GpsFile();
            gpsFile.Name = gpxFile.Filename.Split('\\').Last();

            gpsFile.Tracks.AddRange(GpsUtils.ConvertToTracks(gpxFile, string.Empty));
            gpsFile.Waypoints.AddRange(GpsUtils.ConvertToWaypoints(gpxFile));
            gpsFile.Routes.AddRange(GpsUtils.ConvertToRoutes(gpxFile));

            gpsFile.TrackSummary = new GpsTrackSummary();

            GpsUtils.CalculateNodeData(gpsFile.Tracks, gpsFile.TrackSummary);

            return gpsFile;
        }

        public static List<GpsWaypoint> ConvertToWaypoints(GpxFile gpxFile)
        {
            List<GpsWaypoint> waypoints = new List<GpsWaypoint>();

            if (gpxFile == null)
            {
                return waypoints;
            }

            foreach (GpxWaypointNode waypoint in gpxFile.RootNode.Waypoints)
            {
                // Add a new Waypoint
                waypoints.Add(new GpsWaypoint(waypoint.Position));
                waypoints.Last().Comment = waypoint.Comment;
                waypoints.Last().Description = waypoint.Description;
                waypoints.Last().Name = waypoint.Name;
                waypoints.Last().Symbol = waypoint.Symbol;
            }

            return waypoints;
        }

        public static List<GpsTrack> ConvertToTracks(GpxFile gpxFile, string sourceName)
        {
            List<GpsTrack> gpsTrack = new List<GpsTrack>();

            if (gpxFile == null)
            {
                return gpsTrack;
            }

            foreach (GpxTrackNode trackNode in gpxFile.RootNode.Tracks)
            {
                // Add a new track
                gpsTrack.Add(new GpsTrack());
                gpsTrack.Last().Name = trackNode.Name;
                gpsTrack.Last().Segments = new List<GpsTrackSegment>();

                foreach (GpxSegmentNode segment in trackNode.Segments)
                {
                    // Add a new Segment
                    gpsTrack.Last().Segments.Add(new GpsTrackSegment());
                    gpsTrack.Last().Segments.Last().Name = trackNode.Name;
                    gpsTrack.Last().Segments.Last().Points = new List<GpsTrackPoint>();

                    foreach (GpxPointNode pointNode in trackNode.Segments.First().Points)
                    {
                        // Add each point
                        gpsTrack.Last().Segments.Last().Points.Add(new GpsTrackPoint());
                        gpsTrack.Last().Segments.Last().Points.Last().Time = pointNode.Time;
                        gpsTrack.Last().Segments.Last().Points.Last().Latitude.Numeric = pointNode.Lat;
                        gpsTrack.Last().Segments.Last().Points.Last().Longitude.Numeric = pointNode.Lon;
                        gpsTrack.Last().Segments.Last().Points.Last().Altitude = pointNode.Ele;
                        gpsTrack.Last().Segments.Last().Points.Last().Dimension = GpsPosition.Dimensions.ThreeDimensional;
                        gpsTrack.Last().Segments.Last().Points.Last().Source = sourceName;
                    }
                }

                GpsUtils.CalculateNodeData(gpsTrack.Last().Segments, gpsTrack.Last());
            }

            return gpsTrack;
        }

        public static List<GpsRoute> ConvertToRoutes(GpxFile gpxFile)
        {
            List<GpsRoute> gpsRoutes = new List<GpsRoute>();

            if (gpxFile == null)
            {
                return gpsRoutes;
            }

            foreach (GpxRouteNode route in gpxFile.RootNode.Routes)
            {
                // Add a new Route
                gpsRoutes.Add(new GpsRoute());
                gpsRoutes.Last().Name = route.Name;

                gpsRoutes.Last().Points = new List<GpsRoutePoint>();

                foreach (GpxRoutePointNode pointNode in route.RoutePoints)
                {
                    // Add a new Route Point
                    gpsRoutes.Last().Points.Add(new GpsRoutePoint(pointNode.Position));
                    gpsRoutes.Last().Points.Last().Comment = pointNode.Comment;
                    gpsRoutes.Last().Points.Last().Description = pointNode.Description;
                    gpsRoutes.Last().Points.Last().Name = pointNode.Name;
                    gpsRoutes.Last().Points.Last().Source = pointNode.Source;
                }
            }

            return gpsRoutes;
        }

        public static void CalculateNodeData(List<GpsTrack> tracks, GpsTrackSummary nodeSummary)
        {
            GpsUtils.CalculateNodeData(tracks.SelectMany(x => x.Segments).ToList(), nodeSummary);
        }

        public static void CalculateNodeData(List<GpsTrackSegment> segments, GpsTrackSummary nodeSummary)
        {
            GpsUtils.CalculateNodeData(segments.SelectMany(x => x.Points).ToList(), nodeSummary);
        }

        public static void CalculateNodeData(List<GpsTrackPoint> points, GpsTrackSummary nodeSummary)
        {
            string sourceName = "Calculated";

            if (points.Count > 0)
            {
                // Calculate Altitude Data
                nodeSummary.AltitudeMaximum = Math.Round(points.Max(p => p.Altitude), 1);
                nodeSummary.AltitudeMinimum = Math.Round(points.Min(p => p.Altitude), 1);
                nodeSummary.AltitudeAverage = Math.Round(points.Average(p => p.Altitude), 1);
                nodeSummary.AltitudeChanged = nodeSummary.AltitudeMaximum - nodeSummary.AltitudeMinimum;

                // Calculate Bounds
                double left = points.Min(p => p.Longitude.Numeric);
                double right = points.Max(p => p.Longitude.Numeric);
                double bottom = points.Min(p => p.Latitude.Numeric);
                double top = points.Max(p => p.Latitude.Numeric);

                nodeSummary.TopLeft = new GpsPosition(top, left);
                nodeSummary.TopLeft.Source = sourceName;
                nodeSummary.BottomRight = new GpsPosition(bottom, right);
                nodeSummary.BottomRight.Source = sourceName;

                // Calculate Center
                nodeSummary.Width = nodeSummary.TopLeft.Latitude.Numeric - nodeSummary.BottomRight.Latitude.Numeric;
                nodeSummary.Height = nodeSummary.BottomRight.Longitude.Numeric - nodeSummary.TopLeft.Longitude.Numeric;

                // Calculate Center
                nodeSummary.Center = new GpsPosition(left + (nodeSummary.Width / 2), bottom + (nodeSummary.Height / 2));
                nodeSummary.Center.Source = sourceName;

                // Calculate Dates
                nodeSummary.StartUtc = points.Min(p => p.Time);
                nodeSummary.EndUtc = points.Max(p => p.Time);

                // Calculate Distances
                for (int i = 1; i < points.Count; i++)
                {
                    nodeSummary.DistanceTracked += points[i].Distance(points[i - 1]);
                }

                nodeSummary.DistanceTracked = Math.Round(nodeSummary.DistanceTracked, 1);
                nodeSummary.DistanceEndToEnd = Math.Round(points.First().Distance(points.Last()), 1);
            }
        }

        public static void CalculateNodeData(List<GpsPosition> points, GpsTrackSummary nodeSummary)
        {
            string sourceName = "Calculated";

            if (points.Count > 0)
            {
                // Calculate Altitude Data
                nodeSummary.AltitudeMaximum = Math.Round(points.Max(p => p.Altitude), 1);
                nodeSummary.AltitudeMinimum = Math.Round(points.Min(p => p.Altitude), 1);
                nodeSummary.AltitudeAverage = Math.Round(points.Average(p => p.Altitude), 1);
                nodeSummary.AltitudeChanged = nodeSummary.AltitudeMaximum - nodeSummary.AltitudeMinimum;

                // Calculate Bounds
                double left = points.Min(p => p.Longitude.Numeric);
                double right = points.Max(p => p.Longitude.Numeric);
                double bottom = points.Min(p => p.Latitude.Numeric);
                double top = points.Max(p => p.Latitude.Numeric);

                nodeSummary.TopLeft = new GpsPosition(top, left);
                nodeSummary.TopLeft.Source = sourceName;
                nodeSummary.BottomRight = new GpsPosition(bottom, right);
                nodeSummary.BottomRight.Source = sourceName;

                nodeSummary.Height = Math.Abs(nodeSummary.TopLeft.Latitude.Numeric - nodeSummary.BottomRight.Latitude.Numeric);
                nodeSummary.Width = Math.Abs(nodeSummary.BottomRight.Longitude.Numeric - nodeSummary.TopLeft.Longitude.Numeric);

                // Calculate Center
                nodeSummary.Center = new GpsPosition(bottom + (nodeSummary.Height / 2), left + (nodeSummary.Width / 2));
                nodeSummary.Center.Source = sourceName;

                // Calculate Dates
                nodeSummary.StartUtc = points.Min(p => p.Time);
                nodeSummary.EndUtc = points.Max(p => p.Time);

                // Calculate Distances
                for (int i = 1; i < points.Count; i++)
                {
                    nodeSummary.DistanceTracked += points[i].Distance(points[i - 1]);
                }

                nodeSummary.DistanceTracked = Math.Round(nodeSummary.DistanceTracked, 1);
                nodeSummary.DistanceEndToEnd = Math.Round(points.First().Distance(points.Last()), 1);
            }
        }
    }
}
