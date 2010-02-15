// <copyright file="GpsTrackManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that represents a GpsTrackManager</summary>
namespace Fotofly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Fotofly;
    using Fotofly.GpseXchangeFormat;

    public class GpsTrackCache
    {
        public GpsTrackCache()
        {
            this.Tracks = new List<GpsTrack>();
        }

        public List<GpsTrack> Tracks
        {
            get;
            set;
        }

        public void Add(GpxFile gpxFile)
        {
            if (gpxFile != null && gpxFile.RootNode != null && gpxFile.RootNode.Tracks.Count > 0)
            {
                // Create a new track
                GpsTrack gpsTrack = new GpsTrack();
                gpsTrack.Name = gpxFile.Filename;
                gpsTrack.Points = new List<GpsPosition>();

                // Loop through each Track in the File
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
                            newCoordinate.Source = gpxFile.RootNode.Creator;

                            gpsTrack.Points.Add(newCoordinate);
                        }
                    }
                }

                this.Tracks.Add(gpsTrack);
            }
        }
    }
}
