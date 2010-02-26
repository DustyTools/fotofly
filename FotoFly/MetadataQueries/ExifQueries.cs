// <copyright file="ExifQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>ExifQueries</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ExifQueries
    {
        public static readonly MetdataQuery<Int32, int> Padding = new MetdataQuery<Int32, int>("/app1/ifd/exif/PaddingSchema:Padding");

        // *********************************************************************************** //
        //       Image Tags
        // *********************************************************************************** //

        // Image Description (270, 0x010E)
        // Shown as Title in Windows File Properties
        public static readonly MetdataQuery<string, string> Description = new MetdataQuery<string, string>("/app1/ifd/exif:{uint=270}");

        // Ascii. The manufacturer of the recording equipment. This is the manufacturer of
        // the DSC, scanner, video digitizer or other equipment that generated the image.
        // When the field is left blank, it is treated as unknown. 
        public static readonly MetdataQuery<string, string> Camera = new MetdataQuery<string, string>("/app1/ifd/exif:{uint=271}");

        // Ascii. The model name or model number of the equipment. This is the model name
        // or number of the DSC, scanner, video digitizer or other equipment that generated
        // the image. When the field is left blank, it is treated as unknown. 
        public static readonly MetdataQuery<string, string> CameraModel = new MetdataQuery<string, string>("/app1/ifd/exif:{uint=272}");

        // Short
        public static readonly MetdataQuery<string[], TagList> Orientation = new MetdataQuery<string[], TagList>("/app1/ifd/exif:{uint=274}");

        // Rational. The number of pixels per <ResolutionUnit> in the <ImageWidth> direction.
        // When the image resolution is unknown, 72 [dpi] is designated. 
        public static readonly MetdataQuery<Int64, double> HorizontalResolution = new MetdataQuery<Int64, double>("/app1/ifd/exif:{uint=282}");

        // Rational. The number of pixels per <ResolutionUnit> in the <ImageLength> direction.
        // The same value as <XResolution> is designated. 
        public static readonly MetdataQuery<Int64, double> VerticalResolution = new MetdataQuery<Int64, double>("/app1/ifd/exif:{uint=283}");

        // Short. The unit for measuring <XResolution> and <YResolution>. The same unit is used
        // for both <XResolution> and <YResolution>. If the image resolution is unknown, 2 (inches)
        // is designated. 
        public static readonly MetdataQuery<Int16, int> ResolutionUnit = new MetdataQuery<Int16, int>("/app1/ifd/exif:{uint=296}");

        // Ascii. This tag records the name and version of the software or firmware of the camera
        // or image input device used to generate the image. The detailed format is not specified,
        // When the field is left blank, it is treated as unknown. 
        public static readonly MetdataQuery<string[], string> CreationSoftware = new MetdataQuery<string[], string>("/app1/ifd/exif:{uint=305}");

        // Ascii. The date and time of image creation. In Exif standard, it is the date and time the
        // file was changed. 
        public static readonly MetdataQuery<string, string> DateModified = new MetdataQuery<string, string>("/app1/ifd/exif/subifd:{uint=306}");

        // Ascii. SubSecond
        public static readonly MetdataQuery<string, string> DateModifiedSubSec = new MetdataQuery<string, string>("/app1/ifd/exif/subifd:{uint=37520}");

        // Artist (315, 0x013B)
        public static readonly MetdataQuery<string, string> Artist = new MetdataQuery<string, string>("/app1/ifd/exif/subifd:{uint=315}");

        // *********************************************************************************** //
        //       Photo Tags
        // *********************************************************************************** //

        // Copyright (33432, 0x8298) 
        public static readonly MetdataQuery<string, string> Copyright = new MetdataQuery<string, string>("/app1/ifd/subifd:{uint=33432}");

        // Rational. Exposure time, given in seconds (sec). 
        public static readonly MetdataQuery<Int64, string> ShutterSpeed = new MetdataQuery<Int64, string>("/app1/ifd/exif/subifd:{uint=33434}");

        // Rational. The F number. 
        public static readonly MetdataQuery<Int64, string> Aperture = new MetdataQuery<Int64, string>("/app1/ifd/exif/subifd:{uint=33437}");

        // Short. The class of the program used by the camera to set exposure when the picture is taken. 
        public static readonly MetdataQuery<Int16, string> ExposureProgram = new MetdataQuery<Int16, string>("/app1/ifd/exif/subifd:{uint=34850}");

        // Short. Indicates the ISO Speed and ISO Latitude of the camera or input device as specified
        // in ISO 12232. 
        public static readonly MetdataQuery<UInt16, string> IsoSpeedRating = new MetdataQuery<UInt16, string>("/app1/ifd/exif/subifd:{uint=34855}");

        // Ascii. The date and time when the original image data was generated. For a digital still camera
        // the date and time the picture was taken are recorded. 
        public static readonly MetdataQuery<string, string> DateTaken = new MetdataQuery<string, string>("/app1/ifd/exif/subifd:{uint=36867}");

        // Ascii. SubSecond
        public static readonly MetdataQuery<string, string> DateTakenSubSec = new MetdataQuery<string, string>("/app1/ifd/exif/subifd:{uint=37521}");

        // Ascii. The date and time when the image was stored as digital data. 
        public static readonly MetdataQuery<string, string> DateDigitized = new MetdataQuery<string, string>("/app1/ifd/exif/subifd:{uint=36868}");

        // Ascii. SubSecond
        public static readonly MetdataQuery<string, string> DateDigitizedSubSec = new MetdataQuery<string, string>("/app1/ifd/exif/subifd:{uint=37522}");

        // SRational. The exposure bias. The units is the APEX value. Ordinarily it is given
        // in the range of -99.99 to 99.99. 
        public static readonly MetdataQuery<Int64, SRational> ExposureBias = new MetdataQuery<Int64, SRational>("/app1/ifd/exif/subifd:{uint=37380}");

        // Short
        public static readonly MetdataQuery<Int16, string> MeteringMode = new MetdataQuery<Int16, string>("/app1/ifd/exif/subifd:{uint=37383}");

        // Short. The kind of light source. 
        public static readonly MetdataQuery<Int16, string> LightSource = new MetdataQuery<Int16, string>("/app1/ifd/exif/subifd:{uint=37384}");

        // Short. This tag is recorded when an image is taken using a strobe light (flash)
        public static readonly MetdataQuery<Int16, string> FlashFired = new MetdataQuery<Int16, string>("/app1/ifd/exif/subifd:{uint=37385}");

        // Rational The actual focal length of the lens, in mm. Conversion is not made to
        // the focal length of a 35 mm film camera. 
        public static readonly MetdataQuery<Int64, string> FocalLenght = new MetdataQuery<Int64, string>("/app1/ifd/exif/subifd:{uint=37386}");

        // Short The color space information tag is always recorded as the color space specifier.
        // Normally sRGB is used to define the color space based on the PC monitor conditions and
        // environment. If a color space other than sRGB is used, Uncalibrated is set. Image data
        // recorded as Uncalibrated can be treated as sRGB when it is converted to FlashPix. 
        public static readonly MetdataQuery<Int16, string> ColorRepresentation = new MetdataQuery<Int16, string>("/app1/ifd/exif/subifd:{uint=40961}");

        // Short
        public static readonly MetdataQuery<Int16, string> ExposureMode = new MetdataQuery<Int16, string>("/app1/ifd/exif/subifd:{uint=41986}");

        // Short
        public static readonly MetdataQuery<Int16, string> WhiteBalance = new MetdataQuery<Int16, string>("/app1/ifd/exif/subifd:{uint=41987}");

        // Rational. This tag indicates the digital zoom ratio when the image was shot. If the
        // numerator of the recorded value is 0, this indicates that digital zoom was not used. 
        public static readonly MetdataQuery<Int64, string> DigitalZoomRatio = new MetdataQuery<Int64, string>("/app1/ifd/exif/subifd:{uint=41988}");

        // Short
        public static readonly MetdataQuery<Int16, string> SubjectDistanceRange = new MetdataQuery<Int16, string>("/app1/ifd/exif/subifd:{uint=41996}");

        // *********************************************************************************** //
        //       Iop Tags
        // *********************************************************************************** //

        // Ascii. RelatedImageFileFormat
        public static readonly MetdataQuery<string, string> ImageFileFormat = new MetdataQuery<string, string>("/app1/ifd/exif:{uint=4096}");

        // Long. Image width
        public static readonly MetdataQuery<Int32, string> ImageWidth = new MetdataQuery<Int32, string>("/app1/ifd/exif:{uint=4097}");

        // Long. Image height
        public static readonly MetdataQuery<Int32, string> ImageHeight = new MetdataQuery<Int32, string>("/app1/ifd/exif:{uint=4098}");

        // *********************************************************************************** //
        //       Iop Tags
        // *********************************************************************************** //

        // Rational. Horizontal Thumbnail Resolution
        public static readonly MetdataQuery<Int64, string> ThumbnailHorizontalResolution = new MetdataQuery<Int64, string>("/App1/{uint=1}/{uint=282}");

        // Rational. Vertical Thumbnail Resolution
        public static readonly MetdataQuery<Int64, string> ThumbnailVerticalResolution = new MetdataQuery<Int64, string>("/App1/{uint=1}/{uint=283}");
    }
}
