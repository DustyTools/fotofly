// <copyright file="GpsPositionUtils.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-10</date>
// <summary>GpsPositionUtils</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GpsPositionUtils
    {
        public static void FindBounds(List<GpsPosition> gpsPlaces, out GpsPosition topLeft, out GpsPosition bottomRight)
        {
            if (gpsPlaces == null || gpsPlaces.Count == 0)
            {
                throw new Exception("gpsPlaces is NULL or Empty");
            }

            // Calculate Top Left
            topLeft = new GpsPosition();
            topLeft.Latitude = new GpsCoordinate();
            topLeft.Latitude.Numeric = gpsPlaces.Max(x => x.Latitude.Numeric);
            topLeft.Longitude = new GpsCoordinate();
            topLeft.Longitude.Numeric = gpsPlaces.Max(x => x.Longitude.Numeric);

            // Calculate Bottom Right
            bottomRight = new GpsPosition();
            bottomRight.Latitude = new GpsCoordinate();
            bottomRight.Latitude.Numeric = gpsPlaces.Min(x => x.Latitude.Numeric);
            bottomRight.Longitude = new GpsCoordinate();
            bottomRight.Longitude.Numeric = gpsPlaces.Min(x => x.Longitude.Numeric);
        }

        public static GpsPosition FindCenter(List<GpsPosition> gpsPlaces)
        {
            if (gpsPlaces == null || gpsPlaces.Count == 0)
            {
                throw new Exception("gpsPlaces is NULL or Empty");
            }

            // Calculate Center
            GpsPosition center = new GpsPosition();
            center.Latitude = new GpsCoordinate();
            center.Latitude.Numeric = gpsPlaces.Average(x => x.Latitude.Numeric);
            center.Longitude = new GpsCoordinate();
            center.Longitude.Numeric = gpsPlaces.Average(x => x.Longitude.Numeric);

            return center;
        }
    }
}
