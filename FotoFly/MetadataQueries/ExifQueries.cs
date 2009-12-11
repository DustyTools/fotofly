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
    }
}
