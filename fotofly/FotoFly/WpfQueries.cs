namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class WpfQueries
    {
        public static readonly uint PaddingAmount = 5120;

        public static readonly string ExifAperture = "/app1/ifd/exif/subifd:{uint=33437}";
        public static readonly string ExifColorRepresentation = "/app1/ifd/exif/subifd:{uint=40961}";
        public static readonly string ExifCreationSoftware = "/app1/ifd/exif:{uint=305}";
        public static readonly string ExifCameraModel = "/app1/ifd/exif:{uint=272}";
        public static readonly string ExifCamera = "/app1/ifd/exif:{uint=271}";

        // TODO Mostly not correct 0x9203
        public static readonly string ExifBrightness = "/app1/ifd/exif:{uint=33435}";
        
        // Standard Exif Stuff
        public static readonly string ExifDateTaken = "/app1/ifd/exif/subifd:{uint=306}";
        public static readonly string ExifDateTakenOther = "/app1/ifd/exif/subifd:{uint=36867}";
        public static readonly string ExifDateDigitised = "/app1/ifd/exif/subifd:{uint=36868}";

        public static readonly string ExifDigitalZoomRatio = "/app1/ifd/exif/subifd:{uint=41988}";
        public static readonly string ExifExposureBias = "/app1/ifd/exif/subifd:{uint=37380}";
        public static readonly string ExifExposureMode = "/app1/ifd/exif/subifd:{uint=34850}";
        public static readonly string ExifFlashFired = "/app1/ifd/exif/subifd:{uint=37385}";
        public static readonly string ExifFocalLenght = "/app1/ifd/exif/subifd:{uint=37386}";
        public static readonly string ExifHeight = "/app1/ifd/exif/subifd:{uint=40963}";
        public static readonly string ExifHorizontalResolution = "/app1/ifd/exif:{uint=282}";
        public static readonly string ExifIso = "/app1/ifd/exif/subifd:{uint=34855}";
        public static readonly string ExifPadding = "/app1/ifd/exif/PaddingSchema:Padding";
        public static readonly string ExifShutterSpeed = "/app1/ifd/exif/subifd:{uint=33434}";
        public static readonly string ExifVerticalResolution = "/app1/ifd/exif:{uint=283}";
        public static readonly string ExifWhiteBalanceMode = "/app1/ifd/exif/subifd:{uint=37384}";
        public static readonly string ExifWidth = "/app1/ifd/exif/subifd:{uint=40962}";

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

        // Padding
        public static readonly string IfdPadding = "/app1/ifd/PaddingSchema:Padding";

        // Place Fields
        public static readonly string IptcCity = "/app13/irb/8bimiptc/iptc/City";
        public static readonly string IptcCountry = @"/app13/irb/8bimiptc/iptc/Country\/Primary Location Name";
        public static readonly string IptcRegion = @"/app13/irb/8bimiptc/iptc/Province\/State";
        public static readonly string IptcSubLocation = "/app13/irb/8bimiptc/iptc/Sub-location";

        // IPTC Fields
        // See http://www.ap.org/apserver/userguide/codes.htm
        public static readonly string IptcCopyright = @"/ifd/iptc/copyright notice";
        public static readonly string IptcWriterEditor = @"/ifd/iptc/writer\/editor";

        // XMP Types
        public static readonly string XmpStruct = "xmpstruct";
        public static readonly string XmpBag = "xmpbag";

        // Padding used when adding properties to the File
        public static readonly string XmpPadding = "/xmp/PaddingSchema:Padding";

        // XMP People
        public static readonly string XmpPeople = "/xmp/mediapro:People";

        // "RegionInfo" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/" (LPWSTR)]
        // "Regions" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/RegionInfo#" (LPWSTR)]
        // "Rectangle" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        // "PersonDisplayName" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        // "PersonEmailDigest" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        // MPRI:DateRegionsValid
        public static readonly string MicrosoftRegionInfo = @"/xmp/MP:RegionInfo";
        public static readonly string MicrosoftRegionsLastUpdate = @"/xmp/MP:RegionInfo/MPRI:DateRegionsValid";

        public static readonly string MicrosoftRegions = @"/xmp/MP:RegionInfo/MPRI:Regions";
        public static readonly string MicrosoftRegion = @"/xmp/MP:RegionInfo/MPRI:Regions/{{ulong={0}}}";

        public static readonly string MicrosoftPersonDisplayName = @"/MPReg:PersonDisplayName";
        public static readonly string MicrosoftRectangle = @"/MPReg:Rectangle";
        public static readonly string MicrosoftPersonEmailDigest = @"/MPReg:PersonEmailDigest";
        public static readonly string MicrosoftPersonLiveIdCID = @"/MPReg:PersonLiveIdCID";
    }
}
