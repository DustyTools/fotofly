// <copyright file="ExifQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>ExifQueries</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ExifQueries
    {
        public static readonly MetdataQuery Padding = new MetdataQuery("/app1/ifd/exif/PaddingSchema:Padding", typeof(Int32));

        // *********************************************************************************** //
        //       Image Tags
        // *********************************************************************************** //

        // Ascii. The manufacturer of the recording equipment. This is the manufacturer of
        // the DSC, scanner, video digitizer or other equipment that generated the image.
        // When the field is left blank, it is treated as unknown. 
        public static readonly MetdataQuery Camera = new MetdataQuery("/app1/ifd/exif:{uint=271}", typeof(string));

        // Ascii. The model name or model number of the equipment. This is the model name
        // or number of the DSC, scanner, video digitizer or other equipment that generated
        // the image. When the field is left blank, it is treated as unknown. 
        public static readonly MetdataQuery CameraModel = new MetdataQuery("/app1/ifd/exif:{uint=272}", typeof(string));

        // Short
        public static readonly MetdataQuery Orientation = new MetdataQuery("/app1/ifd/exif:{uint=274}", typeof(Int16));

        // Rational. The number of pixels per <ResolutionUnit> in the <ImageWidth> direction.
        // When the image resolution is unknown, 72 [dpi] is designated. 
        public static readonly MetdataQuery HorizontalResolution = new MetdataQuery("/app1/ifd/exif:{uint=282}", typeof(Int64));

        // Rational. The number of pixels per <ResolutionUnit> in the <ImageLength> direction.
        // The same value as <XResolution> is designated. 
        public static readonly MetdataQuery VerticalResolution = new MetdataQuery("/app1/ifd/exif:{uint=283}", typeof(Int64));

        // Short. The unit for measuring <XResolution> and <YResolution>. The same unit is used
        // for both <XResolution> and <YResolution>. If the image resolution is unknown, 2 (inches)
        // is designated. 
        public static readonly MetdataQuery ResolutionUnit = new MetdataQuery("/app1/ifd/exif:{uint=296}", typeof(Int16));

        // Ascii. This tag records the name and version of the software or firmware of the camera
        // or image input device used to generate the image. The detailed format is not specified,
        // When the field is left blank, it is treated as unknown. 
        public static readonly MetdataQuery CreationSoftware = new MetdataQuery("/app1/ifd/exif:{uint=305}", typeof(string[]));

        // Ascii. The date and time of image creation. In Exif standard, it is the date and time the
        // file was changed. 
        public static readonly MetdataQuery DateTaken = new MetdataQuery("/app1/ifd/exif/subifd:{uint=306}", typeof(string));

        // *********************************************************************************** //
        //       Photo Tags
        // *********************************************************************************** //

        // Rational. Exposure time, given in seconds (sec). 
        public static readonly MetdataQuery ShutterSpeed = new MetdataQuery("/app1/ifd/exif/subifd:{uint=33434}", typeof(Int64));

        // Rational. The F number. 
        public static readonly MetdataQuery Aperture = new MetdataQuery("/app1/ifd/exif/subifd:{uint=33437}", typeof(Int64));

        // Short. The class of the program used by the camera to set exposure when the picture is taken. 
        public static readonly MetdataQuery ExposureProgram = new MetdataQuery("/app1/ifd/exif/subifd:{uint=34850}", typeof(Int16));

        // Short. Indicates the ISO Speed and ISO Latitude of the camera or input device as specified
        // in ISO 12232. 
        public static readonly MetdataQuery IsoSpeedRating = new MetdataQuery("/app1/ifd/exif/subifd:{uint=34855}", typeof(UInt16));

        // Ascii. The date and time when the original image data was generated. For a digital still camera
        // the date and time the picture was taken are recorded. 
        public static readonly MetdataQuery DateTakenOther = new MetdataQuery("/app1/ifd/exif/subifd:{uint=36867}", typeof(string));

        // Ascii. The date and time when the image was stored as digital data. 
        public static readonly MetdataQuery DateDigitized = new MetdataQuery("/app1/ifd/exif/subifd:{uint=36868}", typeof(string));

        // SRational. The exposure bias. The units is the APEX value. Ordinarily it is given
        // in the range of -99.99 to 99.99. 
        public static readonly MetdataQuery ExposureBias = new MetdataQuery("/app1/ifd/exif/subifd:{uint=37380}", typeof(Int64));

        // Short
        public static readonly MetdataQuery MeteringMode = new MetdataQuery("/app1/ifd/exif/subifd:{uint=37383}", typeof(Int16));

        // Short. The kind of light source. 
        public static readonly MetdataQuery LightSource = new MetdataQuery("/app1/ifd/exif/subifd:{uint=37384}", typeof(Int16));

        // Short. This tag is recorded when an image is taken using a strobe light (flash)
        public static readonly MetdataQuery FlashFired = new MetdataQuery("/app1/ifd/exif/subifd:{uint=37385}", typeof(Int16));

        // Rational The actual focal length of the lens, in mm. Conversion is not made to
        // the focal length of a 35 mm film camera. 
        public static readonly MetdataQuery FocalLenght = new MetdataQuery("/app1/ifd/exif/subifd:{uint=37386}", typeof(Int64));

        // Short The color space information tag is always recorded as the color space specifier.
        // Normally sRGB is used to define the color space based on the PC monitor conditions and
        // environment. If a color space other than sRGB is used, Uncalibrated is set. Image data
        // recorded as Uncalibrated can be treated as sRGB when it is converted to FlashPix. 
        public static readonly MetdataQuery ColorRepresentation = new MetdataQuery("/app1/ifd/exif/subifd:{uint=40961}", typeof(Int16));

        // Short
        public static readonly MetdataQuery ExposureMode = new MetdataQuery("/app1/ifd/exif/subifd:{uint=41986}", typeof(Int16));

        // Short
        public static readonly MetdataQuery WhiteBalance = new MetdataQuery("/app1/ifd/exif/subifd:{uint=41987}", typeof(Int16));

        // Rational. This tag indicates the digital zoom ratio when the image was shot. If the
        // numerator of the recorded value is 0, this indicates that digital zoom was not used. 
        public static readonly MetdataQuery DigitalZoomRatio = new MetdataQuery("/app1/ifd/exif/subifd:{uint=41988}", typeof(Int64));

        // Short
        public static readonly MetdataQuery SubjectDistanceRange = new MetdataQuery("/app1/ifd/exif/subifd:{uint=41996}", typeof(Int16));

        // *********************************************************************************** //
        //       Iop Tags
        // *********************************************************************************** //

        // Ascii. RelatedImageFileFormat
        public static readonly MetdataQuery ImageFileFormat = new MetdataQuery("/app1/ifd/exif:{uint=4096}", typeof(string));

        // Long. Image width
        public static readonly MetdataQuery ImageWidth = new MetdataQuery("/app1/ifd/exif:{uint=4097}", typeof(Int32));

        // Long. Image height
        public static readonly MetdataQuery ImageHeight = new MetdataQuery("/app1/ifd/exif:{uint=4098}", typeof(Int32));

        // *********************************************************************************** //
        //       Iop Tags
        // *********************************************************************************** //

        // Rational. Horizontal Thumbnail Resolution
        public static readonly MetdataQuery ThumbnailHorizontalResolution = new MetdataQuery("/App1/{uint=1}/{uint=282}", typeof(Int64));

        // Rational. Vertical Thumbnail Resolution
        public static readonly MetdataQuery ThumbnailVerticalResolution = new MetdataQuery("/App1/{uint=1}/{uint=283}", typeof(Int64));

        // *********************************************************************************** //
        //       GPSInfo Tags
        // *********************************************************************************** //

        // Byte sequence 2, 2, 0, 0 to indicate version 2.2
        public static readonly MetdataQuery GpsVersionID = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=0}", typeof(byte[]));

        // ASCII count 'N' indicates north latitude, and 'S' is south latitude
        public static readonly MetdataQuery GpsLatitudeRef = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=1}", typeof(string));

        // The latitude is expressed as three RATIONAL values giving the degrees, minutes,
        // and seconds, respectively. When degrees, minutes and seconds are expressed, the
        // format is dd/1,mm/1,ss/1. When degrees and minutes are used and, for example,
        // fractions of minutes are given up to two decimal places, the format is dd/1,mmmm/100,0/1.
        public static readonly MetdataQuery GpsLatitude = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=2}", typeof(UInt64[]));

        // ASCII 'E' indicates east longitude, and 'W' is west longitude
        public static readonly MetdataQuery GpsLongitudeRef = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=3}", typeof(string));

        // The longitude is expressed as three RATIONAL values giving the degrees, minutes,
        // and seconds, respectively. When degrees, minutes and seconds are expressed, the
        // format is dd/1,mm/1,ss/1. When degrees and minutes are used and, for example,
        // fractions of minutes are given up to two decimal places, the format is dd/1,mmmm/100,0/1.
        public static readonly MetdataQuery GpsLongitude = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=4}", typeof(UInt64[]));

        // 0 = Above sea level, 1 = Below sea level 
        public static readonly MetdataQuery GpsAltitudeRef = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=5}", typeof(string));

        // Altitude is expressed as one RATIONAL value. The reference unit is meters.
        public static readonly MetdataQuery GpsAltitude = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=6}", typeof(Int64));

        // Indicates the time as UTC (Coordinated Universal Time). <TimeStamp> is expressed as
        // three RATIONAL values giving the hour, minute, and second (atomic clock).
        public static readonly MetdataQuery GpsTimeStamp = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=7}", typeof(UInt64[]));

        // Indicates the GPS satellites used for measurements. This tag can be used to describe
        // the number of satellites, their ID number, angle of elevation, azimuth, SNR and other
        // information in ASCII notation. 
        public static readonly MetdataQuery GpsSatellites = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=8}", typeof(string));

        // Indicates the status of the GPS receiver when the image is recorded. "A" means measurement
        // is in progress, and "V" means the measurement is Interoperability. Stored as Ascii.
        public static readonly MetdataQuery GpsStatus = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=9}", typeof(string));
        
        // Indicates the Gps measurement mode. '2' means two-dimensional measurement and '3' means
        // three-dimensional
        public static readonly MetdataQuery GpsMeasureMode = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=10}", typeof(Int32));

        // Indicates the GPS DOP (data degree of precision). An HDOP value is written during
        // two-dimensional measurement, and PDOP during three-dimensional measurement.
        public static readonly MetdataQuery GpsDOP = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=11}", typeof(string));

        // Indicates the unit used to express the GPS receiver speed of movement. "K" "M" and
        // "N" represents kilometers per hour, miles per hour, and knots.
        public static readonly MetdataQuery GpsSpeedRef = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=12}", typeof(string));

        // Rational Indicates the speed of GPS receiver movement. 
        public static readonly MetdataQuery GpsSpeed = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=13}", typeof(Int16));

        // Ascii Indicates the reference for giving the direction of GPS receiver movement.
        // "T" denotes true direction and "M" is magnetic direction. 
        public static readonly MetdataQuery GpsTrackRef = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=14}", typeof(string));
        
        // Rational Indicates the direction of GPS receiver movement. The range of values
        // is from 0.00 to 359.99. 
        public static readonly MetdataQuery GpsTrack = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=15}", typeof(Int16));

        // Ascii Indicates the reference for giving the direction of the image when it is captured.
        // "T" denotes true direction and "M" is magnetic direction. 
        public static readonly MetdataQuery GpsImgDirectionRef = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=16}", typeof(string));

        // Rational Indicates the direction of the image when it was captured. The range of
        // values is from 0.00 to 359.99. 
        public static readonly MetdataQuery GpsImgDirection = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=17}", typeof(Int16));

        // Ascii Indicates the geodetic survey data used by the GPS receiver. If the survey data
        // is restricted to Japan, the value of this tag is "TOKYO" or "WGS-84". 
        public static readonly MetdataQuery GpsMapDatum = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=18}", typeof(string));

        // Ascii Indicates whether the latitude of the destination point is north or south latitude.
        // The ASCII value "N" indicates north latitude, and "S" is south latitude. 
        public static readonly MetdataQuery GpsDestLatitudeRef = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=19}", typeof(string));

        // Rational Indicates the latitude of the destination point. The latitude is expressed as
        // three RATIONAL values giving the degrees, minutes, and seconds, respectively. If latitude
        // is expressed as degrees, minutes and seconds, a typical format would be dd/1,mm/1,ss/1.
        // When degrees and minutes are used and, for example, fractions of minutes are given up
        // to two decimal places, the format would be dd/1,mmmm/100,0/1. 
        public static readonly MetdataQuery GpsDestLatitude = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=20}", typeof(UInt64[]));

        // Ascii Indicates whether the longitude of the destination point is east or west longitude.
        // ASCII "E" indicates east longitude, and "W" is west longitude. 
        public static readonly MetdataQuery GpsDestLongitudeRef = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=21}", typeof(string));

        // Rational Indicates the longitude of the destination point. The longitude is expressed as
        // three RATIONAL values giving the degrees, minutes, and seconds, respectively. If longitude
        // is expressed as degrees, minutes and seconds, a typical format would be ddd/1,mm/1,ss/1.
        // When degrees and minutes are used and, for example, fractions of minutes are given up to
        // two decimal places, the format would be ddd/1,mmmm/100,0/1. 
        public static readonly MetdataQuery GpsDestLongitude = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=22}", typeof(UInt64[]));

        // Ascii Indicates the reference used for giving the bearing to the destination point.
        // "T" denotes true direction and "M" is magnetic direction. 
        public static readonly MetdataQuery GpsDestBearingRef = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=23}", typeof(string));

        // Rational Indicates the bearing to the destination point. The range of values is from 0.00 to 359.99. 
        public static readonly MetdataQuery GpsDestBearing = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=24}", typeof(Int16));

        // Ascii. Indicates the unit used to express the distance to the destination point. "K", "M" and
        // "N" represent kilometers, miles and knots. 
        public static readonly MetdataQuery GpsDestDistanceRef = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=25}", typeof(string));

        // Rational. Indicates the distance to the destination point. 
        public static readonly MetdataQuery GpsDestDistance = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=26}", typeof(Int64));

        // Undefined. A character string recording the name of the method used for location finding. The
        // first byte indicates the character code used, and this is followed by the name of the method. 
        public static readonly MetdataQuery GpsProcessingMethod = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=27}", typeof(string));

        // Undefined. A character string recording the name of the GPS area. The first byte indicates
        // the character code used, and this is followed by the name of the GPS area. 
        public static readonly MetdataQuery GpsAreaInformation = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=28}", typeof(string));

        // Ascii. A character string recording date and time information relative to UTC
        // The format is "YYYY:MM:DD.". 
        public static readonly MetdataQuery GpsDateStamp = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=29}", typeof(string));

        // Short. Indicates whether differential correction is applied to the GPS receiver. 
        public static readonly MetdataQuery GpsDifferential = new MetdataQuery("/app1/ifd/Gps/subifd:{uint=30}", typeof(Int16));
    }
}
