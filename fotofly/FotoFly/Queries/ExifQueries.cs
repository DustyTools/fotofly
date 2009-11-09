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
        public static readonly string Padding = "/app1/ifd/exif/PaddingSchema:Padding";

        // *********************************************************************************** //
        //       Image Tags
        // *********************************************************************************** //

        // Ascii. The manufacturer of the recording equipment. This is the manufacturer of
        // the DSC, scanner, video digitizer or other equipment that generated the image.
        // When the field is left blank, it is treated as unknown. 
        public static readonly string Camera = "/app1/ifd/exif:{uint=271}";

        // Ascii. The model name or model number of the equipment. This is the model name
        // or number of the DSC, scanner, video digitizer or other equipment that generated
        // the image. When the field is left blank, it is treated as unknown. 
        public static readonly string CameraModel = "/app1/ifd/exif:{uint=272}";

        // Short
        public static readonly string Orientation = "/app1/ifd/exif:{uint=274}";

        // Rational. The number of pixels per <ResolutionUnit> in the <ImageWidth> direction.
        // When the image resolution is unknown, 72 [dpi] is designated. 
        public static readonly string HorizontalResolution = "/app1/ifd/exif:{uint=282}";

        // Rational. The number of pixels per <ResolutionUnit> in the <ImageLength> direction.
        // The same value as <XResolution> is designated. 
        public static readonly string VerticalResolution = "/app1/ifd/exif:{uint=283}";

        // Short. The unit for measuring <XResolution> and <YResolution>. The same unit is used
        // for both <XResolution> and <YResolution>. If the image resolution is unknown, 2 (inches)
        // is designated. 
        public static readonly string ResolutionUnit = "/app1/ifd/exif:{uint=296}";

        // Ascii. This tag records the name and version of the software or firmware of the camera
        // or image input device used to generate the image. The detailed format is not specified,
        // When the field is left blank, it is treated as unknown. 
        public static readonly string CreationSoftware = "/app1/ifd/exif:{uint=305}";

        // Ascii. The date and time of image creation. In Exif standard, it is the date and time the
        // file was changed. 
        public static readonly string DateTaken = "/app1/ifd/exif/subifd:{uint=306}";

        // *********************************************************************************** //
        //       Photo Tags
        // *********************************************************************************** //

        // Rational. Exposure time, given in seconds (sec). 
        public static readonly string ShutterSpeed = "/app1/ifd/exif/subifd:{uint=33434}";

        // Rational. The F number. 
        public static readonly string Aperture = "/app1/ifd/exif/subifd:{uint=33437}";

        // Short. The class of the program used by the camera to set exposure when the picture is taken. 
        public static readonly string ExposureProgram = "/app1/ifd/exif/subifd:{uint=34850}";

        // Short. Indicates the ISO Speed and ISO Latitude of the camera or input device as specified
        // in ISO 12232. 
        public static readonly string IsoSpeedRating = "/app1/ifd/exif/subifd:{uint=34855}";

        // Ascii. The date and time when the original image data was generated. For a digital still camera
        // the date and time the picture was taken are recorded. 
        public static readonly string DateTakenOther = "/app1/ifd/exif/subifd:{uint=36867}";

        // Ascii. The date and time when the image was stored as digital data. 
        public static readonly string DateDigitized = "/app1/ifd/exif/subifd:{uint=36868}";

        // SRational. The exposure bias. The units is the APEX value. Ordinarily it is given
        // in the range of -99.99 to 99.99. 
        public static readonly string ExposureBias = "/app1/ifd/exif/subifd:{uint=37380}";

        // Short
        public static readonly string MeteringMode = "/app1/ifd/exif/subifd:{uint=37383}";

        // Short. The kind of light source. 
        public static readonly string LightSource = "/app1/ifd/exif/subifd:{uint=37384}";

        // Short. This tag is recorded when an image is taken using a strobe light (flash)
        public static readonly string FlashFired = "/app1/ifd/exif/subifd:{uint=37385}";

        // Rational The actual focal length of the lens, in mm. Conversion is not made to
        // the focal length of a 35 mm film camera. 
        public static readonly string FocalLenght = "/app1/ifd/exif/subifd:{uint=37386}";

        // Short The color space information tag is always recorded as the color space specifier.
        // Normally sRGB is used to define the color space based on the PC monitor conditions and
        // environment. If a color space other than sRGB is used, Uncalibrated is set. Image data
        // recorded as Uncalibrated can be treated as sRGB when it is converted to FlashPix. 
        public static readonly string ColorRepresentation = "/app1/ifd/exif/subifd:{uint=40961}";

        // Short
        public static readonly string ExposureMode = "/app1/ifd/exif/subifd:{uint=41986}";

        // Short
        public static readonly string WhiteBalance = "/app1/ifd/exif/subifd:{uint=41987}";

        // Rational. This tag indicates the digital zoom ratio when the image was shot. If the
        // numerator of the recorded value is 0, this indicates that digital zoom was not used. 
        public static readonly string DigitalZoomRatio = "/app1/ifd/exif/subifd:{uint=41988}";

        // Short
        public static readonly string SubjectDistanceRange = "/app1/ifd/exif/subifd:{uint=41996}";

        // *********************************************************************************** //
        //       Iop Tags
        // *********************************************************************************** //

        // Ascii. RelatedImageFileFormat
        public static readonly string ImageFileFormat = "/app1/ifd/exif:{uint=4096}";

        // Long. Image width
        public static readonly string ImageWidth = "/app1/ifd/exif:{uint=4097}";

        // Long. Image height
        public static readonly string ImageHeight = "/app1/ifd/exif:{uint=4098}";

        // *********************************************************************************** //
        //       Iop Tags
        // *********************************************************************************** //

        // Rational. Horizontal Thumbnail Resolution
        public static readonly string ThumbnailHorizontalResolution = "/App1/{uint=1}/{uint=282}";

        // Rational. Vertical Thumbnail Resolution
        public static readonly string ThumbnailVerticalResolution = "/App1/{uint=1}/{uint=283}";

        // *********************************************************************************** //
        //       GPSInfo Tags
        // *********************************************************************************** //

        // Byte sequence 2, 2, 0, 0 to indicate version 2.2
        public static readonly string GpsVersionID = "/app1/ifd/Gps/subifd:{uint=0}";

        // ASCII count 'N' indicates north latitude, and 'S' is south latitude
        public static readonly string GpsLatitudeRef = "/app1/ifd/Gps/subifd:{uint=1}";

        // The latitude is expressed as three RATIONAL values giving the degrees, minutes,
        // and seconds, respectively. When degrees, minutes and seconds are expressed, the
        // format is dd/1,mm/1,ss/1. When degrees and minutes are used and, for example,
        // fractions of minutes are given up to two decimal places, the format is dd/1,mmmm/100,0/1.
        public static readonly string GpsLatitude = "/app1/ifd/Gps/subifd:{uint=2}";

        // ASCII 'E' indicates east longitude, and 'W' is west longitude
        public static readonly string GpsLongitudeRef = "/app1/ifd/Gps/subifd:{uint=3}";

        // The longitude is expressed as three RATIONAL values giving the degrees, minutes,
        // and seconds, respectively. When degrees, minutes and seconds are expressed, the
        // format is dd/1,mm/1,ss/1. When degrees and minutes are used and, for example,
        // fractions of minutes are given up to two decimal places, the format is dd/1,mmmm/100,0/1.
        public static readonly string GpsLongitude = "/app1/ifd/Gps/subifd:{uint=4}";

        // 0 = Above sea level, 1 = Below sea level 
        public static readonly string GpsAltitudeRef = "/app1/ifd/Gps/subifd:{uint=5}";

        // Altitude is expressed as one RATIONAL value. The reference unit is meters.
        public static readonly string GpsAltitude = "/app1/ifd/Gps/subifd:{uint=6}";

        // Indicates the time as UTC (Coordinated Universal Time). <TimeStamp> is expressed as
        // three RATIONAL values giving the hour, minute, and second (atomic clock).
        public static readonly string GpsTimeStamp = "/app1/ifd/Gps/subifd:{uint=7}";

        // Indicates the GPS satellites used for measurements. This tag can be used to describe
        // the number of satellites, their ID number, angle of elevation, azimuth, SNR and other
        // information in ASCII notation. 
        public static readonly string GpsSatellites = "/app1/ifd/Gps/subifd:{uint=8}";

        // Indicates the status of the GPS receiver when the image is recorded. "A" means measurement
        // is in progress, and "V" means the measurement is Interoperability. Stored as Ascii.
        public static readonly string GpsStatus = "/app1/ifd/Gps/subifd:{uint=9}";
        
        // Indicates the Gps measurement mode. '2' means two-dimensional measurement and '3' means
        // three-dimensional
        public static readonly string GpsMeasureMode = "/app1/ifd/Gps/subifd:{uint=10}";

        // Indicates the GPS DOP (data degree of precision). An HDOP value is written during
        // two-dimensional measurement, and PDOP during three-dimensional measurement.
        public static readonly string GpsDOP = "/app1/ifd/Gps/subifd:{uint=11}";

        // Indicates the unit used to express the GPS receiver speed of movement. "K" "M" and
        // "N" represents kilometers per hour, miles per hour, and knots.
        public static readonly string GpsSpeedRef = "/app1/ifd/Gps/subifd:{uint=12}";

        // Rational Indicates the speed of GPS receiver movement. 
        public static readonly string GpsSpeed = "/app1/ifd/Gps/subifd:{uint=13}";

        // Ascii Indicates the reference for giving the direction of GPS receiver movement.
        // "T" denotes true direction and "M" is magnetic direction. 
        public static readonly string GpsTrackRef = "/app1/ifd/Gps/subifd:{uint=14}";
        
        // Rational Indicates the direction of GPS receiver movement. The range of values
        // is from 0.00 to 359.99. 
        public static readonly string GpsTrack = "/app1/ifd/Gps/subifd:{uint=15}";

        // Ascii Indicates the reference for giving the direction of the image when it is captured.
        // "T" denotes true direction and "M" is magnetic direction. 
        public static readonly string GpsImgDirectionRef = "/app1/ifd/Gps/subifd:{uint=16}";

        // Rational Indicates the direction of the image when it was captured. The range of
        // values is from 0.00 to 359.99. 
        public static readonly string GpsImgDirection = "/app1/ifd/Gps/subifd:{uint=17}";

        // Ascii Indicates the geodetic survey data used by the GPS receiver. If the survey data
        // is restricted to Japan, the value of this tag is "TOKYO" or "WGS-84". 
        public static readonly string GpsMapDatum = "/app1/ifd/Gps/subifd:{uint=18}";

        // Ascii Indicates whether the latitude of the destination point is north or south latitude.
        // The ASCII value "N" indicates north latitude, and "S" is south latitude. 
        public static readonly string GpsDestLatitudeRef = "/app1/ifd/Gps/subifd:{uint=19}";

        // Rational Indicates the latitude of the destination point. The latitude is expressed as
        // three RATIONAL values giving the degrees, minutes, and seconds, respectively. If latitude
        // is expressed as degrees, minutes and seconds, a typical format would be dd/1,mm/1,ss/1.
        // When degrees and minutes are used and, for example, fractions of minutes are given up
        // to two decimal places, the format would be dd/1,mmmm/100,0/1. 
        public static readonly string GpsDestLatitude = "/app1/ifd/Gps/subifd:{uint=20}";

        // Ascii Indicates whether the longitude of the destination point is east or west longitude.
        // ASCII "E" indicates east longitude, and "W" is west longitude. 
        public static readonly string GpsDestLongitudeRef = "/app1/ifd/Gps/subifd:{uint=21}";

        // Rational Indicates the longitude of the destination point. The longitude is expressed as
        // three RATIONAL values giving the degrees, minutes, and seconds, respectively. If longitude
        // is expressed as degrees, minutes and seconds, a typical format would be ddd/1,mm/1,ss/1.
        // When degrees and minutes are used and, for example, fractions of minutes are given up to
        // two decimal places, the format would be ddd/1,mmmm/100,0/1. 
        public static readonly string GpsDestLongitude = "/app1/ifd/Gps/subifd:{uint=22}";

        // Ascii Indicates the reference used for giving the bearing to the destination point.
        // "T" denotes true direction and "M" is magnetic direction. 
        public static readonly string GpsDestBearingRef = "/app1/ifd/Gps/subifd:{uint=23}";

        // Rational Indicates the bearing to the destination point. The range of values is from 0.00 to 359.99. 
        public static readonly string GpsDestBearing = "/app1/ifd/Gps/subifd:{uint=24}";

        // Ascii. Indicates the unit used to express the distance to the destination point. "K", "M" and
        // "N" represent kilometers, miles and knots. 
        public static readonly string GpsDestDistanceRef = "/app1/ifd/Gps/subifd:{uint=25}";

        // Rational. Indicates the distance to the destination point. 
        public static readonly string GpsDestDistance = "/app1/ifd/Gps/subifd:{uint=26}";

        // Undefined. A character string recording the name of the method used for location finding. The
        // first byte indicates the character code used, and this is followed by the name of the method. 
        public static readonly string GpsProcessingMethod = "/app1/ifd/Gps/subifd:{uint=27}";

        // Undefined. A character string recording the name of the GPS area. The first byte indicates
        // the character code used, and this is followed by the name of the GPS area. 
        public static readonly string GpsAreaInformation = "/app1/ifd/Gps/subifd:{uint=28}";

        // Ascii. A character string recording date and time information relative to UTC
        // The format is "YYYY:MM:DD.". 
        public static readonly string GpsDateStamp = "/app1/ifd/Gps/subifd:{uint=29}";

        // Short. Indicates whether differential correction is applied to the GPS receiver. 
        public static readonly string GpsDifferential = "/app1/ifd/Gps/subifd:{uint=30}";
    }
}
