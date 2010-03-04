// <copyright file="GpsQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-10</date>
// <summary>Metadata queries for GPS metadata</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class GpsQueries
    {
        // Byte sequence 2, 2, 0, 0 to indicate version 2.2
        public static readonly MetdataQuery<byte[], string> VersionID = new MetdataQuery<byte[], string>("/app1/ifd/Gps/subifd:{uint=0}");

        // ASCII count 'N' indicates north latitude, and 'S' is south latitude
        public static readonly MetdataQuery<string, string> LatitudeRef = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=1}");

        // The latitude is expressed as three RATIONAL values giving the degrees, minutes,
        // and seconds, respectively. When degrees, minutes and seconds are expressed, the
        // format is dd/1,mm/1,ss/1. When degrees and minutes are used and, for example,
        // fractions of minutes are given up to two decimal places, the format is dd/1,mmmm/100,0/1.
        public static readonly MetdataQuery<UInt64[], GpsCoordinate> Latitude = new MetdataQuery<UInt64[], GpsCoordinate>("/app1/ifd/Gps/subifd:{uint=2}");

        // ASCII 'E' indicates east longitude, and 'W' is west longitude
        public static readonly MetdataQuery<string, string> LongitudeRef = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=3}");

        // The longitude is expressed as three RATIONAL values giving the degrees, minutes,
        // and seconds, respectively. When degrees, minutes and seconds are expressed, the
        // format is dd/1,mm/1,ss/1. When degrees and minutes are used and, for example,
        // fractions of minutes are given up to two decimal places, the format is dd/1,mmmm/100,0/1.
        public static readonly MetdataQuery<UInt64[], GpsCoordinate> Longitude = new MetdataQuery<UInt64[], GpsCoordinate>("/app1/ifd/Gps/subifd:{uint=4}");

        // 0 = Above sea level, 1 = Below sea level 
        public static readonly MetdataQuery<string, string> AltitudeRef = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=5}");

        // Altitude is expressed as one RATIONAL value. The reference unit is meters.
        public static readonly MetdataQuery<Int64, double> Altitude = new MetdataQuery<Int64, double>("/app1/ifd/Gps/subifd:{uint=6}");

        // Indicates the time as UTC (Coordinated Universal Time). <TimeStamp> is expressed as
        // three RATIONAL values giving the hour, minute, and second (atomic clock).
        public static readonly MetdataQuery<UInt64[], DateTime> TimeStamp = new MetdataQuery<UInt64[], DateTime>("/app1/ifd/Gps/subifd:{uint=7}");

        // Indicates the GPS satellites used for measurements. This tag can be used to describe
        // the number of satellites, their ID number, angle of elevation, azimuth, SNR and other
        // information in ASCII notation. 
        public static readonly MetdataQuery<string, string> Satellites = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=8}");

        // Indicates the status of the GPS receiver when the image is recorded. "A" means measurement
        // is in progress, and "V" means the measurement is Interoperability. Stored as Ascii.
        public static readonly MetdataQuery<string, string> Status = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=9}");
        
        // Indicates the Gps measurement mode. '2' means two-dimensional measurement and '3' means
        // three-dimensional, '1' means one-dimensional (Altitude only)
        public static readonly MetdataQuery<Int32, GpsPosition.Dimensions> MeasureMode = new MetdataQuery<Int32, GpsPosition.Dimensions>("/app1/ifd/Gps/subifd:{uint=10}");

        // Indicates the GPS DOP (data degree of precision). An HDOP value is written during
        // two-dimensional measurement, and PDOP during three-dimensional measurement.
        public static readonly MetdataQuery<string, string> DOP = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=11}");

        // Indicates the unit used to express the GPS receiver speed of movement. "K" "M" and
        // "N" represents kilometers per hour, miles per hour, and knots.
        public static readonly MetdataQuery<string, string> SpeedRef = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=12}");

        // Rational Indicates the speed of GPS receiver movement. 
        public static readonly MetdataQuery<Int16, double> Speed = new MetdataQuery<Int16, double>("/app1/ifd/Gps/subifd:{uint=13}");

        // Ascii Indicates the reference for giving the direction of GPS receiver movement.
        // "T" denotes true direction and "M" is magnetic direction. 
        public static readonly MetdataQuery<string, string> TrackRef = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=14}");
        
        // Rational Indicates the direction of GPS receiver movement. The range of values
        // is from 0.00 to 359.99. 
        public static readonly MetdataQuery<Int16, double> Track = new MetdataQuery<Int16, double>("/app1/ifd/Gps/subifd:{uint=15}");

        // Ascii Indicates the reference for giving the direction of the image when it is captured.
        // "T" denotes true direction and "M" is magnetic direction. 
        public static readonly MetdataQuery<string, string> ImgDirectionRef = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=16}");

        // Rational Indicates the direction of the image when it was captured. The range of
        // values is from 0.00 to 359.99. 
        public static readonly MetdataQuery<Int16, double> ImgDirection = new MetdataQuery<Int16, double>("/app1/ifd/Gps/subifd:{uint=17}");

        // Ascii Indicates the geodetic survey data used by the GPS receiver. If the survey data
        // is restricted to Japan, the value of this tag is "TOKYO" or "WGS-84". 
        public static readonly MetdataQuery<string, string> MapDatum = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=18}");

        // Ascii Indicates whether the latitude of the destination point is north or south latitude.
        // The ASCII value "N" indicates north latitude, and "S" is south latitude. 
        public static readonly MetdataQuery<string, string> DestLatitudeRef = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=19}");

        // Rational Indicates the latitude of the destination point. The latitude is expressed as
        // three RATIONAL values giving the degrees, minutes, and seconds, respectively. If latitude
        // is expressed as degrees, minutes and seconds, a typical format would be dd/1,mm/1,ss/1.
        // When degrees and minutes are used and, for example, fractions of minutes are given up
        // to two decimal places, the format would be dd/1,mmmm/100,0/1. 
        public static readonly MetdataQuery<UInt64[], GpsCoordinate> DestLatitude = new MetdataQuery<UInt64[], GpsCoordinate>("/app1/ifd/Gps/subifd:{uint=20}");

        // Ascii Indicates whether the longitude of the destination point is east or west longitude.
        // ASCII "E" indicates east longitude, and "W" is west longitude. 
        public static readonly MetdataQuery<string, string> DestLongitudeRef = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=21}");

        // Rational Indicates the longitude of the destination point. The longitude is expressed as
        // three RATIONAL values giving the degrees, minutes, and seconds, respectively. If longitude
        // is expressed as degrees, minutes and seconds, a typical format would be ddd/1,mm/1,ss/1.
        // When degrees and minutes are used and, for example, fractions of minutes are given up to
        // two decimal places, the format would be ddd/1,mmmm/100,0/1. 
        public static readonly MetdataQuery<UInt64[], GpsCoordinate> DestLongitude = new MetdataQuery<UInt64[], GpsCoordinate>("/app1/ifd/Gps/subifd:{uint=22}");

        // Ascii Indicates the reference used for giving the bearing to the destination point.
        // "T" denotes true direction and "M" is magnetic direction. 
        public static readonly MetdataQuery<string, string> DestBearingRef = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=23}");

        // Rational Indicates the bearing to the destination point. The range of values is from 0.00 to 359.99. 
        public static readonly MetdataQuery<Int16, double> DestBearing = new MetdataQuery<Int16, double>("/app1/ifd/Gps/subifd:{uint=24}");

        // Ascii. Indicates the unit used to express the distance to the destination point. "K", "M" and
        // "N" represent kilometers, miles and knots. 
        public static readonly MetdataQuery<string, string> DestDistanceRef = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=25}");

        // Rational. Indicates the distance to the destination point. 
        public static readonly MetdataQuery<Int64, double> DestDistance = new MetdataQuery<Int64, double>("/app1/ifd/Gps/subifd:{uint=26}");

        // Undefined. A character string recording the name of the method used for location finding. The
        // first byte indicates the character code used, and this is followed by the name of the method. 
        public static readonly MetdataQuery<string, string> ProcessingMethod = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=27}");

        // Undefined. A character string recording the name of the GPS area. The first byte indicates
        // the character code used, and this is followed by the name of the GPS area. 
        public static readonly MetdataQuery<string, string> AreaInformation = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=28}");

        // Ascii. A character string recording date and time information relative to UTC
        // The format is "YYYY:MM:DD.". 
        public static readonly MetdataQuery<string, string> DateStamp = new MetdataQuery<string, string>("/app1/ifd/Gps/subifd:{uint=29}");

        // Short. Indicates whether differential correction is applied to the GPS receiver. 
        public static readonly MetdataQuery<Int16, int> Differential = new MetdataQuery<Int16, int>("/app1/ifd/Gps/subifd:{uint=30}");
    }
}
