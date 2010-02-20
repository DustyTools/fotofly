// <copyright file="MetadataEnums.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>MetadataEnums</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public class MetadataEnums
    {
        /// <summary>
        /// Image orientation viewed in terms of rows and columns.
        /// </summary>
        public enum Orientations : uint
        {
            Unknown = 0,

            /// <summary>The 0th row is at the top of the visual image, and the 0th column is the visual left side.</summary>
            TopLeft = 1,
            
            /// <summary>The 0th row is at the visual top of the image, and the 0th column is the visual right side.</summary>
            TopRight = 2,
            
            /// <summary>The 0th row is at the visual bottom of the image, and the 0th column is the visual right side.</summary>
            BottomLeft = 3,
            
            /// <summary>The 0th row is at the visual bottom of the image, and the 0th column is the visual right side.</summary>
            BottomRight = 4,
            
            /// <summary>The 0th row is the visual left side of the image, and the 0th column is the visual top.</summary>
            LeftTop = 5,
            
            /// <summary>The 0th row is the visual right side of the image, and the 0th column is the visual top.</summary>
            RightTop = 6,
            
            /// <summary>The 0th row is the visual right side of the image, and the 0th column is the visual bottom.</summary>
            RightBottom = 7,
            
            /// <summary>The 0th row is the visual left side of the image, and the 0th column is the visual bottom.</summary>
            LeftBottom = 8
        }
        
        public enum ExposureModes : int
        {
            AutoExposure = 0,
            ManualExposure = 1,
            AutoBracket = 2
        }

        public enum ExposurePrograms : uint
        {
            Unknown = 0,
            Manual = 1,
            NormalProgram = 2,
            AperturePriority = 3,
            ShutterPriority = 4,
            
            // Creative program (biased toward depth of field)
            LowSpeedMode = 5,

            // Action program (biased toward fast shutter speed)
            HighSpeedMode = 6,

            // Portrait mode (for closeup photos with the background out of focus)
            PortraitMode = 7,

            // Landscape mode (for landscape photos with the background in focus)
            LandscapeMode = 8
        }

        public enum MeteringModes : int
        {
            Unknown = 0,
            Average = 1,
            CenterWeightedAverage = 2,
            Spot = 3,
            MultiSpot = 4,
            Pattern = 5,
            Partial = 6,
            Other
        }

        public enum LightSources : int
        {
            Unknown = 0,
            Daylight = 1,
            Fluorescent = 2,
            Tungsten = 3,
            Flash = 4,
            FineWeather = 9,
            CloudyWeather = 10,
            Shade = 11,
            DaylightFluorescent = 12,
            DayWhiteFluorescent = 13,
            CoolWhiteFluorescent = 14,
            WhiteFluorescent = 15,
            StandardLightA = 17,
            StandardLightB = 18,
            StandardLightC = 19,
            D50 = 20,
            D55 = 21,
            D65 = 22,
            D75 = 23,
            IsoStudioTungsten = 24,
            Other
        }

        public enum ColorRepresentations : int
        {
            Unknown,
            sRGB,
            Uncalibrated
        }

        public enum Rating : int
        {
            Unknown = 0,
            OneStar = 1,
            TwoStar = 2,
            ThreeStar = 3,
            FourStar = 4,
            FiveStar = 5
        }

        public enum WhiteBalances : int
        {
            AutoWhiteBalance = 0,
            ManualWhiteBalance = 1
        }

        public enum ImageOrientations : int
        {
            Unknown,
            Landscape,
            Portrait
        }

        public enum SubjectDistanceRanges : int
        {
            Unknown = 0,
            Macro = 1,
            CloseView = 2,
            DistantView = 3
        }

        public enum FlashFired : int
        {
            Unknown,
            FlashFired,
            FlashDidNotFire
        }

        public enum FlashReturnedLight : int
        {
            Unknown,
            NoStrobeReturnDetectionFunction,
            StrobeReturnLightNotDetected,
            StrobeReturnLightDetected
        }

        public enum FlashMode : int
        {
            Unknown,
            CompulsoryFlashFiring,
            CompulsoryFlashSuppression,
            AutoMode
        }

        public enum FlashFunction : int
        {
            Unknown,
            FlashFunctionPresent,
            NoFlashFunction
        }

        public enum FlashRedEyeMode : int
        {
            Unknown,
            NoRedEyeReductionMode,
            RedEyeReductionSupported
        }
    }
}
