namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ExifQueries
    {
        public static readonly string Padding = "/app1/ifd/exif/PaddingSchema:Padding";

        public static readonly string Aperture = "/app1/ifd/exif/subifd:{uint=33437}";
        public static readonly string ColorRepresentation = "/app1/ifd/exif/subifd:{uint=40961}";
        public static readonly string CreationSoftware = "/app1/ifd/exif:{uint=305}";
        public static readonly string CameraModel = "/app1/ifd/exif:{uint=272}";
        public static readonly string Camera = "/app1/ifd/exif:{uint=271}";

        // TODO Mostly not correct 0x9203
        public static readonly string Brightness = "/app1/ifd/exif:{uint=33435}";

        // Standard Exif Stuff
        public static readonly string DateTaken = "/app1/ifd/exif/subifd:{uint=306}";
        public static readonly string DateTakenOther = "/app1/ifd/exif/subifd:{uint=36867}";
        public static readonly string DateDigitised = "/app1/ifd/exif/subifd:{uint=36868}";

        public static readonly string DigitalZoomRatio = "/app1/ifd/exif/subifd:{uint=41988}";
        public static readonly string ExposureBias = "/app1/ifd/exif/subifd:{uint=37380}";
        public static readonly string ExposureMode = "/app1/ifd/exif/subifd:{uint=34850}";
        public static readonly string FlashFired = "/app1/ifd/exif/subifd:{uint=37385}";
        public static readonly string FocalLenght = "/app1/ifd/exif/subifd:{uint=37386}";
        public static readonly string Height = "/app1/ifd/exif/subifd:{uint=40963}";
        public static readonly string HorizontalResolution = "/app1/ifd/exif:{uint=282}";
        public static readonly string Iso = "/app1/ifd/exif/subifd:{uint=34855}";
        public static readonly string ShutterSpeed = "/app1/ifd/exif/subifd:{uint=33434}";
        public static readonly string VerticalResolution = "/app1/ifd/exif:{uint=283}";
        public static readonly string WhiteBalanceMode = "/app1/ifd/exif/subifd:{uint=37384}";
        public static readonly string Width = "/app1/ifd/exif/subifd:{uint=40962}";

        // GPS Altitude
        public static readonly string GpsAltitude = "/app1/ifd/Gps/subifd:{uint=6}";

        // 0 = Above sea level, 1 = Below sea level 
        public static readonly string GpsAltitudeRef = "/app1/ifd/Gps/subifd:{uint=5}";

        // ASCII count 'N' indicates north latitude, and 'S' is south latitude
        public static readonly string GpsLatitudeRef = "/app1/ifd/Gps/subifd:{uint=1}";
        public static readonly string GpsLatitude = "/app1/ifd/Gps/subifd:{uint=2}";

        // ASCII 'E' indicates east longitude, and 'W' is west longitude
        public static readonly string GpsLongitudeRef = "/app1/ifd/Gps/subifd:{uint=3}";
        public static readonly string GpsLongitude = "/app1/ifd/Gps/subifd:{uint=4}";

        // Indicates the Gps measurement mode. '2' means two-dimensional measurement and '3' means three-dimensional
        public static readonly string GpsMeasureMode = "/app1/ifd/Gps/subifd:{uint=10}";

        // A character string recording the name of the method used for place finding.
        public static readonly string GpsProcessingMethod = "/app1/ifd/Gps/subifd:{uint=27}";

        // Byte sequence 2, 2, 0, 0 to indicate version 2.2
        public static readonly string GpsVersionID = "/app1/ifd/Gps/subifd:{uint=0}";

        // 0x0007 GPSTimeStamp rational64u[3] 
        public static readonly string GpsTimeStamp = "/app1/ifd/Gps/subifd:{uint=7}";

        public static readonly string GpsDateStamp = "/app1/ifd/Gps/subifd:{uint=29}";

    }
}
