// <copyright file="XmpExifQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>XmpExifQueries</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class XmpExifQueries
    {
        // EXIF Schema for EXIF-specific Properties
        // - The schema name is http://ns.adobe.com/exif/1.0/
        // - The preferred schema namespace prefix is exif

        // exif:GPSVersionID Text Internal GPS tag 0, 0x00. A decimal encoding of each of
        // the four EXIF bytes with period separators. The current value is “2.0.0.0”.
        public static readonly MetdataQuery<string, string> GpsVersionID = new MetdataQuery<string, string>("/xmp/exif:GPSVersionID");
        
        // GPS 1-4 are Location Created

        // GPS Latitude & GPSLongitude
        // * A Text value in the form “DDD,MM,SSk” or “DDD,MM.mmk”, where:
        // – DDD is a number of degrees
        // – MM is a number of minutes
        // – SS is a number of seconds
        // – mm is a fraction of minutes
        // – k is a single character N, S, E, or W indicating a direction (north, south, east, west)
        // Leading zeros are not necesary for the for DDD, MM, and SS values. The DDD,MM.mmk form
        // should be used when any of the native EXIF component rational values has a denonimator
        // other than 1. There can be any number of fractional digits.

        // exif:GPSLatitude GPSCoordinate Internal GPS tag 2, 0x02 (position) and 1, 0x01 (North/South). Indicates latitude.
        public static readonly MetdataQuery<string, string> GpsLatitude = new MetdataQuery<string, string>("/xmp/exif:GPSLatitude");
        
        // exif:GPSLongitude GPSCoordinate Internal GPS tag 4, 0x04 (position) and 3, 0x03 (East/West). Indicates longitude.
        public static readonly MetdataQuery<string, string> GpsLongitude = new MetdataQuery<string, string>("/xmp/exif:GPSLongitude");
        
        // exif:GPSAltitudeRef Closed Choice of Integer Internal GPS tag 5, 0x5. Indicates whether the altitude is
        // above or below sea level: 0 = Above sea level 1 = Below sea level
        public static readonly MetdataQuery<string, int> GpsAltitudeRef = new MetdataQuery<string, int>("/xmp/exif:GPSAltitudeRef");

        // exif:GPSAltitude Rational Internal GPS tag 6, 0x06. Indicates altitude in meters.
        public static readonly MetdataQuery<string, int> GpsAltitude = new MetdataQuery<string, int>("/xmp/exif:GPSAltitude");

        // exif:GPSTimeStamp Date Internal GPS tag 29 (date), 0x1D, and, and GPS tag 7
        // (time), 0x07. Time stamp of GPS data, in Coordinated Universal Time. NOTE: The GPSDateStamp tag is new in EXIF
        // 2.2. The GPS timestamp in EXIF 2.1 does not include a date. If not present, the date component for the XMP should
        // be taken from exif:DateTimeOriginal, or if that is also lacking from exif:DateTimeDigitized. If no date is
        // available, do not write exif:GPSTimeStamp to XMP.
        public static readonly MetdataQuery<string, DateTime> GpsDateTimeStamp = new MetdataQuery<string, DateTime>("/xmp/exif:GPSTimeStamp");

        // exif:GPSSatellites Text Internal GPS tag 8, 0x08. Satellite information, format is unspecified.
        public static readonly MetdataQuery<string, string> GpsSatellites = new MetdataQuery<string, string>("/xmp/exif:GPSSatellites");

        // exif:GPSStatus Closed Choice of Text Internal GPS tag 9, 0x09. Status of GPS receiver at
        // image creation time: A = measurement in progress V = measurement is interoperability
        public static readonly MetdataQuery<string, string> GpsStatus = new MetdataQuery<string, string>("/xmp/exif:GPSStatus");

        // exif:GPSMeasureMode Text Internal GPS tag 10, 0x0A. GPS measurement mode,
        // Text type: 2 = two-dimensional measurement 3 = three-dimensional measurement
        public static readonly MetdataQuery<string, GpsPosition.Dimensions> GpsMeasureMode = new MetdataQuery<string, GpsPosition.Dimensions>("/xmp/exif:GPSMeasureMode");

        // exif:GPSDOP Rational Internal GPS tag 11, 0x0B. Degree of precision for GPS data.
        public static readonly MetdataQuery<string, string> GpsDOP = new MetdataQuery<string, string>("/xmp/exif:GPSDOP");
        
        // exif:GPSSpeedRef Closed Choice of Text Internal GPS tag 12, 0x0C. Units used to speed
        // measurement: K = kilometers per hour M = miles per hour N = knots
        public static readonly MetdataQuery<string, string> GpsSpeedRef = new MetdataQuery<string, string>("/xmp/exif:GPSSpeedRef");

        // exif:GPSSpeed Rational Internal GPS tag 13, 0x0D. Speed of GPS receiver movement.
        public static readonly MetdataQuery<string, string> GpsSpeed = new MetdataQuery<string, string>("/xmp/exif:GPSSpeed");
        
        // exif:GPSTrackRef Closed Choice of Text Internal GPS tag 14, 0x0E. Reference for movement
        // direction: T = true direction M = magnetic direction
        public static readonly MetdataQuery<string, string> GpsTrackRef = new MetdataQuery<string, string>("/xmp/exif:GPSTrackRef");
        
        // exif:GPSTrack Rational Internal GPS tag 15, 0x0F. Direction of GPS movement, values range from 0 to 359.99.
        public static readonly MetdataQuery<string, string> GpsTrack = new MetdataQuery<string, string>("/xmp/exif:GPSTrack");
        
        // exif:GPSImgDirectionRef Closed Choice of Text Internal GPS tag 16, 0x10. Reference for movement
        // direction: T = true direction M = magnetic direction
        public static readonly MetdataQuery<string, string> GpsImgDirectionRef = new MetdataQuery<string, string>("/xmp/exif:GPSImgDirectionRef");

        // exif:GPSImgDirection Rational Internal GPS tag 17, 0x11. Direction of image when
        // captured, values range from 0 to 359.99.
        public static readonly MetdataQuery<string, string> GpsImgDirection = new MetdataQuery<string, string>("/xmp/exif:GPSImgDirection");
        
        // exif:GPSMapDatum Text Internal GPS tag 18, 0x12. Geodetic survey data.
        public static readonly MetdataQuery<string, string> GpsMapDatum = new MetdataQuery<string, string>("/xmp/exif:GPSMapDatum");

        // GPS 19-22 are Location Show

        // exif:GPSDestLatitudeRef 19, 0x13 (North/South). Indicates destination latitude.
        public static readonly MetdataQuery<string, string> GpsDestLatitudeRef = new MetdataQuery<string, string>("/xmp/exif:GPSDestLatitudeRef");

        // exif:GPSDestLatitude GPSCoordinate Internal GPS tag 20. Indicates destination latitude.
        public static readonly MetdataQuery<string, string> GpsDestLatitude = new MetdataQuery<string, string>("/xmp/exif:GPSDestLatitude");

        // exif:GPSDestLongitudeRef 21, 0x15 (East/West). Indicates destination longitude.
        public static readonly MetdataQuery<string, string> GpsDestLongitudeRef = new MetdataQuery<string, string>("/xmp/exif:GPSDestLongitudeRef");

        // exif:GPSDestLongitude GPSCoordinate Internal GPS tag 22, 0x16 (position). Indicates destination longitude.
        public static readonly MetdataQuery<string, string> GpsDestLongitude = new MetdataQuery<string, string>("/xmp/exif:GPSDestLongitude");
        
        // exif:GPSDestBearingRef Closed Choice of Text Internal GPS tag 23, 0x17. Reference for movement
        // direction: T = true direction M = magnetic direction
        public static readonly MetdataQuery<string, string> GpsDestBearingRef = new MetdataQuery<string, string>("/xmp/exif:GPSDestBearingRef");

        // exif:GPSDestBearing Rational Internal GPS tag 24, 0x18. Destination bearing, values from 0 to 359.99.
        public static readonly MetdataQuery<string, string> GpsDestBearing = new MetdataQuery<string, string>("/xmp/exif:GPSDestBearing");
        
        // exif:GPSDestDistanceRef Closed Choice of Text Internal GPS tag 25, 0x19. Units used for speed
        // measurement: K = kilometers M = miles N = knots
        public static readonly MetdataQuery<string, string> GpsDestDistanceRef = new MetdataQuery<string, string>("/xmp/exif:GPSDestDistanceRef");

        // exif:GPSDestDistance Rational Internal GPS tag 26, 0x1A. Distance to destination.
        public static readonly MetdataQuery<string, string> GpsDestDistance = new MetdataQuery<string, string>("/xmp/exif:GPSDestDistance");
        
        // exif:GPSProcessingMethod Text Internal GPS tag 27, 0x1B. A character string recording
        // the name of the method used for location finding.
        public static readonly MetdataQuery<string, string> GpsProcessingMethod = new MetdataQuery<string, string>("/xmp/exif:GPSProcessingMethod");

        // exif:GPSAreaInformation Text Internal GPS tag 28, 0x1C. A character string recording the name of the GPS area.
        public static readonly MetdataQuery<string, string> GpsAreaInformation = new MetdataQuery<string, string>("/xmp/exif:GPSAreaInformation");
        
        // exif:GPSDifferential Closed choice of Integer Internal GPS tag 30, 0x1E. Indicates whether differential
        // correction is applied to the GPS receiver: 0 = Without correction 1 = Correction applied
        public static readonly MetdataQuery<string, string> GpsDifferential = new MetdataQuery<string, string>("/xmp/exif:GPSDifferential");
    
        // exif:ExifVersion Closed Choice of Text
        // Internal EXIF tag 36864, 0x9000. EXIF version number.
        public static readonly MetdataQuery<string, string> ExifVersion = new MetdataQuery<string, string>("/xmp/exif:ExifVersion");
        
        // exif:FlashpixVersion Closed Choice of Text
        // Internal EXIF tag 40960, 0xA000. Version of FlashPix.
        public static readonly MetdataQuery<string, string> FlashpixVersion = new MetdataQuery<string, string>("/xmp/exif:FlashpixVersion");

        // exif:ColorSpace Closed Choice of Integer
        // Internal EXIF tag 40961, 0xA001. Color space information:
        // 1 = sRGB, 65535 = uncalibrated
        public static readonly MetdataQuery<string, string> ColorSpace = new MetdataQuery<string, string>("/xmp/exif:ColorSpace");

        // exif:ComponentsConfiguration Closed Choice of seq Integer
        // Internal EXIF tag 37121, 0x9101. Configuration of
        // components in data: 4 5 6 0 (if RGB compressed data), 1 2 3 0 (other cases).
        // 0 = does not exist, 
        // 1 = Y
        // 2 = Cb
        // 3 = Cr
        // 4 = R
        // 5 = G
        // 6 = B
        public static readonly MetdataQuery<string, string> ComponentsConfiguration = new MetdataQuery<string, string>("/xmp/exif:ComponentsConfiguration");

        // exif:CompressedBitsPerPixel Rational Internal EXIF tag 37122, 0x9102. Compression mode
        // used for a compressed image is indicated in unit bits per pixel.
        public static readonly MetdataQuery<string, string> CompressedBitsPerPixel = new MetdataQuery<string, string>("/xmp/exif:CompressedBitsPerPixel");
        
        // exif:PixelXDimension Integer Internal EXIF tag 40962, 0xA002. Valid image width, in pixels.
        public static readonly MetdataQuery<string, string> PixelXDimension = new MetdataQuery<string, string>("/xmp/exif:PixelXDimension");
        
        // exif:PixelYDimension Integer Internal EXIF tag 40963, 0xA003. Valid image height, in pixels.
        public static readonly MetdataQuery<string, string> PixelYDimension = new MetdataQuery<string, string>("/xmp/exif:PixelYDimension");
        
        // exif:UserComment Lang Alt External EXIF tag 37510, 0x9286. Comments from user.
        public static readonly MetdataQuery<string, string> UserComment = new MetdataQuery<string, string>("/xmp/exif:UserComment");
        
        // exif:RelatedSoundFile Text Internal EXIF tag 40964, 0xA004. An “8.3” file name for the related sound file.
        public static readonly MetdataQuery<string, string> RelatedSoundFile = new MetdataQuery<string, string>("/xmp/exif:RelatedSoundFile");
        
        // exif:DateTimeOriginal Date Internal EXIF tags 36867, 0x9003 (primary) and 37521, 0x9291 (subseconds).
        // Date and time when original image was generated, in ISO 8601 format.
        // Includes the EXIF SubSecTimeOriginal data.}
        public static readonly MetdataQuery<string, string> DateTimeOriginal = new MetdataQuery<string, string>("/xmp/exif:DateTimeOriginal");

        // exif:DateTimeDigitized Date Internal EXIF tag 36868, 0x9004 (primary) and 37522, 0x9292 (subseconds).
        // Date and time when image was stored as digital data, can be the same as DateTimeOriginal if originally
        // stored in digital form. Stored in ISO 8601 format. Includes the EXIF SubSecTimeDigitized data.
        public static readonly MetdataQuery<string, string> DateTimeDigitized = new MetdataQuery<string, string>("/xmp/exif:DateTimeDigitized");

        // exif:ExposureTime Rational Internal EXIF tag 33434, 0x829A. Exposure time in seconds.
        public static readonly MetdataQuery<string, string> ExposureTime = new MetdataQuery<string, string>("/xmp/exif:ExposureTime");
        
        // exif:FNumber Rational Internal EXIF tag 33437, 0x829D. F number.
        public static readonly MetdataQuery<string, string> FNumber = new MetdataQuery<string, string>("/xmp/exif:FNumber");
        
        // exif:ExposureProgram Closed Choice of Integer Internal EXIF tag 34850, 0x8822. Class of program used
        // for exposure:
        // 0 = not defined
        // 1 = Manual
        // 2 = Normal program
        // 3 = Aperture priority
        // 4 = Shutter priority
        // 5 = Creative program
        // 6 = Action program
        // 7 = Portrait mode
        // 8 = Landscape mode
        public static readonly MetdataQuery<string, string> ExposureProgram = new MetdataQuery<string, string>("/xmp/exif:ExposureProgram");
        
        // exif:SpectralSensitivity Text Internal EXIF tag 34852, 0x8824. Spectral sensitivity of each channel.
        public static readonly MetdataQuery<string, string> SpectralSensitivity = new MetdataQuery<string, string>("/xmp/exif:SpectralSensitivity");

        // exif:ISOSpeed seq Integer Internal EXIF tag 34855, 0x8827. ISO Speed and ISO
        // Latitude of the input device as specified in ISO 12232.
        public static readonly MetdataQuery<string, string> IsoSpeed = new MetdataQuery<string, string>("/xmp/exif:ISOSpeed");
        
        // exif:OECF OECF/SFR Internal EXIF tag 34856, 0x8828. Opto-Electoric
        // Conversion Function as specified in ISO 14524.
        public static readonly MetdataQuery<string, string> OptoElectoricConversionFunction = new MetdataQuery<string, string>("/xmp/exif:OptoElectoricConversionFunction");
        
        // exif:ShutterSpeedValue Rational Internal EXIF tag 37377, 0x9201. Shutter speed, unit is
        // APEX. See Annex C of the EXIF specification.
        // public static readonly MetdataQuery<string, string> ShutterSpeedValue = new MetdataQuery<string, string>("/xmp/exif:ShutterSpeedValue");
        
        // exif:ApertureValue Rational Internal EXIF tag 37378, 0x9202. Lens aperture, unit is APEX.
        // public static readonly MetdataQuery<string, string> ApertureValue = new MetdataQuery<string, string>("/xmp/exif:ApertureValue");
        
        // exif:BrightnessValue Rational Internal EXIF tag 37379, 0x9203. Brightness, unit is APEX.
        public static readonly MetdataQuery<string, string> BrightnessValue = new MetdataQuery<string, string>("/xmp/exif:BrightnessValue");
        
        // exif:ExposureBiasValue Rational Internal EXIF tag 37380, 0x9204. Exposure bias, unit is APEX.
        public static readonly MetdataQuery<string, ExposureBias> ExposureBias = new MetdataQuery<string, ExposureBias>("/xmp/exif:ExposureBiasValue");
        
        // exif:MaxApertureValue Rational Internal EXIF tag 37381, 0x9205. Smallest F number of lens, in APEX.
        public static readonly MetdataQuery<string, string> MaxApertureValue = new MetdataQuery<string, string>("/xmp/exif:MaxApertureValue");

        // exif:SubjectDistance Rational Internal EXIF tag 37382, 0x9206. Distance to subject, in meters.
        public static readonly MetdataQuery<string, string> SubjectDistance = new MetdataQuery<string, string>("/xmp/exif:SubjectDistance");
        
        // exif:MeteringMode Closed Choice of Integer Internal EXIF tag 37383, 0x9207. Metering mode:
        // 0 = unknown
        // 1 = Average
        // 2 = CenterWeightedAverage
        // 3 = Spot
        // 4 = MultiSpot
        // 5 = Pattern
        // 6 = Partial
        // 255 = other
        public static readonly MetdataQuery<string, string> MeteringMode = new MetdataQuery<string, string>("/xmp/exif:MeteringMode");

        // exif:LightSource Closed Choice of Integer
        // Internal EXIF tag 37384, 0x9208. EXIF tag , 0x. Light source:
        // 0 = unknown
        // 1 = Daylight
        // 2 = Fluorescent
        // 3 = Tungsten
        // 4 = Flash
        // 9 = Fine weather
        // 10 = Cloudy weather
        // 11 = Shade
        // 12 = Daylight fluorescent (D 5700 – 7100K)
        // 13 = Day white fluorescent (N 4600 – 5400K)
        // 14 = Cool white fluorescent (W 3900 – 4500K)
        // 15 = White fluorescent (WW 3200 – 3700K)
        // 17 = Standard light A
        // 18 = Standard light B
        // 19 = Standard light C
        // 20 = D55
        // 21 = D65
        // 22 = D75
        // 23 = D50
        // 24 = ISO studio tungsten
        // 255 = other
        public static readonly MetdataQuery<string, string> LightSource = new MetdataQuery<string, string>("/xmp/exif:LightSource");
        
        // exif:Flash Flash Internal EXIF tag 37385, 0x9209. Strobe light (flash) source data.
        public static readonly MetdataQuery<string, string> Flash = new MetdataQuery<string, string>("/xmp/exif:Flash");
        
        // exif:FocalLength Rational Internal EXIF tag 37386, 0x920A. Focal length of the lens, in millimeters.
        public static readonly MetdataQuery<string, string> FocalLength = new MetdataQuery<string, string>("/xmp/exif:FocalLength");
        
        // exif:SubjectArea seq Integer Internal EXIF tag 37396, 0x9214. The location and area
        // of the main subject in the overall scene.
        public static readonly MetdataQuery<string, string> SubjectArea = new MetdataQuery<string, string>("/xmp/exif:SubjectArea");
        
        // exif:FlashEnergy Rational Internal EXIF tag 41483, 0xA20B. Strobe energy during image capture.
        public static readonly MetdataQuery<string, string> FlashEnergy = new MetdataQuery<string, string>("/xmp/exif:FlashEnergy");
        
        // exif: SpatialFrequencyResponse OECF/SFR Internal EXIF tag 41484, 0xA20C. Input device spatial
        // frequency table and SFR values as specified in ISO 12233.
        public static readonly MetdataQuery<string, string> SpatialFrequencyResponse = new MetdataQuery<string, string>("/xmp/exif:SpatialFrequencyResponse");
        
        // exif:FocalPlaneXResolution Rational Internal EXIF tag 41486, 0xA20E. Horizontal focal
        // resolution, measured pixels per unit.
        public static readonly MetdataQuery<string, string> FocalPlaneXResolution = new MetdataQuery<string, string>("/xmp/exif:FocalPlaneXResolution");
        
        // exif:FocalPlaneYResolution Rational Internal EXIF tag 41487, 0xA20F. Vertical focal
        // resolution, measured in pixels per unit.
        public static readonly MetdataQuery<string, string> FocalPlaneYResolution = new MetdataQuery<string, string>("/xmp/exif:FocalPlaneYResolution");
        
        // exif:FocalPlaneResolutionUnit Closed Choice of Integer
        // Internal EXIF tag 41488, 0xA210. Unit used for FocalPlaneXResolution and
        // FocalPlaneYResolution. 2 = inches 3 = centimeters
        public static readonly MetdataQuery<string, string> FocalPlaneResolutionUnit = new MetdataQuery<string, string>("/xmp/exif:FocalPlaneResolutionUnit");
        
        // exif:SubjectLocation seq Integer Internal EXIF tag 41492, 0xA214. Location of the main
        // subject of the scene. The first value is the horizontal pixel and the second value is the
        // vertical pixel at which the main subject appears.
        public static readonly MetdataQuery<string, string> SubjectLocation = new MetdataQuery<string, string>("/xmp/exif:SubjectLocation");
        
        // exif:ExposureIndex Rational Internal EXIF tag 41493, 0xA215. Exposure index of input device.
        public static readonly MetdataQuery<string, string> ExposureIndex = new MetdataQuery<string, string>("/xmp/exif:ExposureIndex");

        // exif:SensingMethod  Closed Choice of Integer Internal EXIF tag 41495, 0xA217. Image sensor type on input device:
        // 1 = Not defined
        // 2 = One-chip color area sensor
        // 3 = Two-chip color area sensor
        // 4 = Three-chip color area sensor
        // 5 = Color sequential area sensor
        // 7 = Trilinear sensor
        // 8 = Color sequential linear sensor
        public static readonly MetdataQuery<string, string> SensingMethod = new MetdataQuery<string, string>("/xmp/exif:SensingMethod");
        
        // exif:FileSource Closed Choice of Integer
        // Internal EXIF tag 41728, 0xA300. Indicates image source: 3 (DSC) is the only choice.
        public static readonly MetdataQuery<string, string> FileSource = new MetdataQuery<string, string>("/xmp/exif:FileSource");
        
        // exif:SceneType Closed Choice of Integer
        // Internal EXIF tag 41729, 0xA301. Indicates the type of
        // scene: 1 (directly photographed image) is the only choice.
        public static readonly MetdataQuery<string, string> SceneType = new MetdataQuery<string, string>("/xmp/exif:SceneType");
        
        // exif:CFAPattern CFAPattern Internal EXIF tag 41730, 0xA302. Color filter array
        // geometric pattern of the image sense.
        public static readonly MetdataQuery<string, string> ColorFilterArrayPattern = new MetdataQuery<string, string>("/xmp/exif:ColorFilterArrayPattern");
        
        // exif:CustomRendered Closed Choice of Integer Internal EXIF tag 41985, 0xA401. Indicates the use of
        // special processing on image data: 0 = Normal process 1 = Custom process
        public static readonly MetdataQuery<string, string> CustomRendered = new MetdataQuery<string, string>("/xmp/exif:CustomRendered");
        
        // exif:ExposureMode Closed Choice of Integer Internal EXIF tag 41986, 0xA402. Indicates the
        // exposure mode set when the image was shot:
        // 0 = Auto exposure
        // 1 = Manual exposure
        // 2 = Auto bracket
        public static readonly MetdataQuery<string, string> ExposureMode = new MetdataQuery<string, string>("/xmp/exif:ExposureMode");
        
        // exif:WhiteBalance Closed Choice of Integer Internal EXIF tag 41987, 0xA403. Indicates the white
        // balance mode set when the image was shot:
        // 0 = Auto white balance
        // 1 = Manual white balance
        public static readonly MetdataQuery<string, string> WhiteBalance = new MetdataQuery<string, string>("/xmp/exif:WhiteBalance");
        
        // exif:DigitalZoomRatio Rational Internal EXIF tag 41988, 0xA404. Indicates the digital
        // zoom ratio when the image was shot.
        public static readonly MetdataQuery<string, string> DigitalZoomRatio = new MetdataQuery<string, string>("/xmp/exif:DigitalZoomRatio");
        
        // exif:FocalLengthIn35mmFilm Integer Internal EXIF tag 41989, 0xA405. Indicates the
        // equivalent focal length assuming a 35mm film camera, in mm. A value of 0 means the focal
        // length is unknown. Note that this tag differs from the FocalLength tag.
        public static readonly MetdataQuery<string, string> FocalLengthIn35mmFilm = new MetdataQuery<string, string>("/xmp/exif:FocalLengthIn35mmFilm");
        
        // exif:SceneCaptureType Closed Choice of Integer
        // Internal EXIF tag 41990, 0xA406. Indicates the type of scene that was shot:
        // 0 = Standard
        // 1 = Landscape
        // 2 = Portrait
        // 3 = Night scene
        public static readonly MetdataQuery<string, string> SceneCaptureType = new MetdataQuery<string, string>("/xmp/exif:SceneCaptureType");
        
        // exif:GainControl Closed Choice of Integer Internal EXIF tag 41991, 0xA407. Indicates the degree
        // of overall image gain adjustment:
        // 0 = None
        // 1 = Low gain up
        // 2 = High gain up
        // 3 = Low gain down
        // 4 = High gain down
        public static readonly MetdataQuery<string, string> GainControl = new MetdataQuery<string, string>("/xmp/exif:GainControl");

        // exif:Contrast Closed Choice of Integer Internal EXIF tag 41992, 0xA408. Indicates the
        // direction of contrast processing applied by the camera:
        // 0 = Normal
        // 1 = Soft
        // 2 = Hard
        public static readonly MetdataQuery<string, string> Contrast = new MetdataQuery<string, string>("/xmp/exif:Contrast");
        
        // exif:Saturation Closed Choice of Integer Internal EXIF tag 41993, 0xA409. Indicates the
        // direction of saturation processing applied by the camera:
        // 0 = Normal
        // 1 = Low saturation
        // 2 = High saturation
        public static readonly MetdataQuery<string, string> Saturation = new MetdataQuery<string, string>("/xmp/exif:Saturation");
        
        // exif:Sharpness Closed Choice of Integer Internal EXIF tag 41994, 0xA40A. Indicates the
        // direction of sharpness processing applied by the camera:
        // 0 = Normal
        // 1 = Soft
        // 2 = Hard
        public static readonly MetdataQuery<string, string> Sharpness = new MetdataQuery<string, string>("/xmp/exif:Sharpness");
        
        // exif:DeviceSettingDescription DeviceSettings Internal EXIF tag 41995, 0xA40B. Indicates information
        // on the picture-taking conditions of a particular camera model.
        public static readonly MetdataQuery<string, string> DeviceSettingDescription = new MetdataQuery<string, string>("/xmp/exif:DeviceSettingDescription");
        
        // exif:SubjectDistanceRange Closed Choice of Integer Internal EXIF tag 41996, 0xA40C.
        // Indicates the distance to the subject:
        // 0 = Unknown
        // 1 = Macro
        // 2 = Close view
        // 3 = Distant view
        public static readonly MetdataQuery<string, string> SubjectDistanceRange = new MetdataQuery<string, string>("/xmp/exif:SubjectDistanceRange");

        // exif:ImageUniqueID Text Internal EXIF tag 42016, 0xA420. An identifier assigned
        // uniquely to each image. It is recorded as a 32 character ASCII string, equivalent to
        // hexadecimal notation and 128-bit fixed length.
        public static readonly MetdataQuery<string, string> ImageUniqueID = new MetdataQuery<string, string>("/xmp/exif:ImageUniqueID");
    }
}
