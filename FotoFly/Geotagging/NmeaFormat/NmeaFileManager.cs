// <copyright file="NmeaFileManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-09-27</date>
// <summary>Class that reads and writes Nmea Files</summary>
namespace Fotofly.NmeaFormat
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Threading;

    using Fotofly.Geotagging;

    public static class NmeaFileManager
    {
        public static GpsFile Read(string fileName)
        {
            GpsFile gpsFile = new GpsFile();
            gpsFile.Tracks.Add(new GpsTrack());
            gpsFile.Tracks.First().Segments.Add(new GpsTrackSegment());

            List<List<string>> nmeaFile = NmeaFileManager.ReadFile(fileName);

            // Save the current date
            DateTime currentDate;

            // Look for first GPGGA record, which contains the current date
            List<string> gprmcRecord = nmeaFile.Where(x => x[0].StartsWith("$GPRMC")).FirstOrDefault();

            if (gprmcRecord == null)
            {
                throw new Exception("Unable to find a valid GPRMC in the Nmea file");
            }
            else
            {
                currentDate = NmeaFileManager.ParseGPRMC(gprmcRecord);
            }

            foreach (List<string> nmeaLine in nmeaFile)
            {
                // See what type of data it is
                switch (nmeaLine[0])
                {
                    case "$GPRMC":
                        // Date
                        currentDate = NmeaFileManager.ParseGPRMC(nmeaLine);
                        break;

                    case "$GPGGA":
                        // Gps Coordinate
                        GpsTrackPoint trackPoint = NmeaFileManager.ParseGPGGA(nmeaLine, currentDate);

                        if (trackPoint.IsValidCoordinate)
                        {
                            gpsFile.Tracks.Last().Segments.Last().Points.Add(trackPoint);
                        }
                        break;

                    default:
                        break;
                }
            }

            return gpsFile;
        }

        public static DateTime ParseGPRMC(List<string> sentence)
        {
            // $GPRMC,150353.1,A,4739.466643,N,12207.287734,W,3.4,54.6,240910,,,A*45
            // Get Date
            int days = Convert.ToInt32(sentence[9].Substring(0, 2));
            int months = Convert.ToInt32(sentence[9].Substring(2, 2));
            int years = Convert.ToInt32("20" + sentence[9].Substring(4, 2));

            return new DateTime(years, months, days);
        }

        public static GpsTrackPoint ParseGPGGA(List<string> sentence, DateTime currentDate)
        {
            // Spec from http://aprs.gids.nl/nmea/
            // Example $GPGGA,150353.1,4739.466643,N,12207.287734,W,1,09,1.1,9.4,M,-16.0,M,,*67

            GpsTrackPoint gpsTrackPoint = new GpsTrackPoint();
            gpsTrackPoint.Source = "Nmea File";

            // Get the Date and Time
            if (sentence[1] != "")
            {
                // && sentence[9] != "")

                // Get hours, minutes, seconds and milliseconds
                int utcHours = Convert.ToInt32(sentence[1].Substring(0, 2));
                int utcMinutes = Convert.ToInt32(sentence[1].Substring(2, 2));
                int utcSeconds = Convert.ToInt32(sentence[1].Substring(4, 2));
                int utcMilliseconds = 0;

                // Get milliseconds if it is available
                if (sentence[1].Length > 7)
                {
                    utcMilliseconds = Convert.ToInt32(float.Parse(sentence[1].Substring(6), new CultureInfo("en-US")) * 1000);
                }

                // Save the time
                gpsTrackPoint.Time = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, utcHours, utcMinutes, utcSeconds, utcMilliseconds);
            }
            
            // Get Latitude and Longitude
            if (sentence[2] != "" && sentence[3] != "" && sentence[4] != "" && sentence[5] != "")
            {
                // Get Latitude
                double latHours = Math.Floor(Convert.ToDouble(sentence[2]) / 100);
                double latMinutes = Convert.ToDouble(sentence[2]) - (latHours * 100);

                if (sentence[3] == "N")
                {
                    gpsTrackPoint.Latitude = new GpsCoordinate(GpsCoordinate.LatitudeRef.North, latHours, latMinutes);
                }
                else if (sentence[3] == "S")
                {
                    gpsTrackPoint.Latitude = new GpsCoordinate(GpsCoordinate.LatitudeRef.South, latHours, latMinutes);
                }

                // Get Longitude
                double lonHours = Math.Floor(Convert.ToDouble(sentence[4]) / 100);
                double lonMinutes = Convert.ToDouble(sentence[4]) - (lonHours * 100);

                if (sentence[5] == "E")
                {
                    gpsTrackPoint.Longitude = new GpsCoordinate(GpsCoordinate.LongitudeRef.East, lonHours, lonMinutes);
                }
                else if (sentence[5] == "W")
                {
                    gpsTrackPoint.Longitude = new GpsCoordinate(GpsCoordinate.LongitudeRef.West, lonHours, lonMinutes);
                }
            }

            // Get Latitude and Longitude
            if (sentence[9] != "" && sentence[10] != "")
            {
                // Get Altitude
                double altitude = Convert.ToDouble(sentence[9]);

                gpsTrackPoint.Altitude = altitude;
            }

            if (gpsTrackPoint.IsValidCoordinate)
            {
                return gpsTrackPoint;
            }
            else
            {
                return new GpsTrackPoint();
            }
        }

        private static List<List<string>> ReadFile(string fileName)
        {
            List<List<string>> returnValue = new List<List<string>>();

            if (!File.Exists(fileName))
            {
                return returnValue;
            }

            // Open the file
            using (StreamReader nmeaFile = File.OpenText(fileName))
            {
                string line;

                // Continue reading until the file is empty
                while ((line = nmeaFile.ReadLine()) != null)
                {
                    // Split into words
                    List<string> words = new List<string>(line.Split(','));

                    // Save
                    returnValue.Add(words);
                }
            }

            return returnValue;
        }
    }
}
