// <copyright file="ExifProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>ExifProvider Class</summary>
namespace Fotofly.MetadataProviders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Media.Imaging;

    using Fotofly.BitmapMetadataTools;
    using Fotofly.MetadataQueries;

    public class ExifProvider : BaseProvider
    {
        public ExifProvider(BitmapMetadata bitmapMetadata)
            : base(bitmapMetadata)
        {
        }

        /// <summary>
        /// Aperture
        /// </summary>
        public Aperture Aperture
        {
            get
            {
                URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.Aperture.Query);

                if (urational != null)
                {
                    return new Aperture(urational);
                }

                return new Aperture();
            }
        }

        /// <summary>
        /// Camera Manufacturer
        /// </summary>
        public string CameraManufacturer
        {
            get
            {
                string formattedString = String.Empty;

                try
                {
                    formattedString = this.BitmapMetadata.CameraManufacturer;
                }
                catch
                {
                    formattedString = String.Empty;
                }

                return formattedString;
            }

            set
            {
                if (this.ValueHasChanged(value, this.CameraManufacturer))
                {
                    if (value == null)
                    {
                        this.BitmapMetadata.CameraManufacturer = string.Empty;
                    }
                    else
                    {
                        this.BitmapMetadata.CameraManufacturer = value;
                    }
                }
            }
        }

        /// <summary>
        /// Camera Model, normally includes camera Manufacturer
        /// </summary>
        public string CameraModel
        {
            get
            {
                string cameraModel = this.BitmapMetadata.CameraModel;

                if (string.IsNullOrEmpty(cameraModel))
                {
                    object cameraModelArray = this.BitmapMetadata.GetQuery<object>(ExifQueries.CameraModel.Query);

                    if (cameraModelArray is string[])
                    {
                        cameraModel = (cameraModelArray as string[])[0];
                    }
                }

                return cameraModel;
            }

            set
            {
                if (this.ValueHasChanged(value, this.CameraModel))
                {
                    if (value == null)
                    {
                        this.BitmapMetadata.CameraModel = string.Empty;
                    }
                    else
                    {
                        this.BitmapMetadata.CameraModel = value;
                    }
                }
            }
        }

        /// <summary>
        /// Copyright owner of the photo
        /// </summary>
        public string Copyright
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(ExifQueries.Copyright.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Copyright))
                {
                    this.BitmapMetadata.SetQuery(ExifQueries.Copyright.Query, value);
                }
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(ExifQueries.Description.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Description))
                {
                    this.BitmapMetadata.SetQuery(ExifQueries.Description.Query, value);
                }
            }
        }

        /// <summary>
        /// Software used to last modify the photo
        /// </summary>
        public string CreationSoftware
        {
            get
            {
                return this.BitmapMetadata.ApplicationName;
            }

            set
            {
                if (this.ValueHasChanged(value, this.CreationSoftware))
                {
                    this.BitmapMetadata.ApplicationName = value;
                }
            }
        }

        /// <summary>
        /// DateDigitized, recorded by the camera when the photo is taken
        /// </summary>
        public DateTime DateDigitised
        {
            get
            {
                ExifDateTime exifDateTime = this.BitmapMetadata.GetQuery<ExifDateTime>(ExifQueries.DateDigitized.Query);

                if (exifDateTime == null)
                {
                    return new DateTime();
                }
                else
                {
                    return exifDateTime.ToDateTime();
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateDigitised))
                {
                    this.BitmapMetadata.SetQuery(ExifQueries.DateDigitized.Query, new ExifDateTime(value).ToExifString());
                }
            }
        }

        /// <summary>
        /// DateModified
        /// </summary>
        public DateTime DateTimeModified
        {
            get
            {
                ExifDateTime exifDateTime = this.BitmapMetadata.GetQuery<ExifDateTime>(ExifQueries.DateModified.Query);

                if (exifDateTime == null)
                {
                    return new DateTime();
                }
                else
                {
                    return exifDateTime.ToDateTime();
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateTimeModified))
                {
                    this.BitmapMetadata.SetQuery(ExifQueries.DateModified.Query, new ExifDateTime(value).ToExifString());
                }
            }
        }

        /// <summary>
        /// DateTaken, recorded by the camera when the photo is taken
        /// </summary>
        public DateTime DateTimeTaken
        {
            get
            {
                DateTime datetimeTaken = Convert.ToDateTime(this.BitmapMetadata.DateTaken);

                if (datetimeTaken == null)
                {
                    return new DateTime();
                }
                else
                {
                    return datetimeTaken;
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateTimeTaken))
                {
                    // Write to tempBitmap count
                    // Use sortable string to avoid US date format issue with Month\Date
                    this.BitmapMetadata.DateTaken = value.ToString("s");
                }
            }
        }

        /// <summary>
        /// DigitalZoomRatio
        /// </summary>
        public double? DigitalZoomRatio
        {
            get
            {
                URational rational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.DigitalZoomRatio.Query);

                if (rational == null || rational.Numerator == 0)
                {
                    return null;
                }
                else
                {
                    return rational.ToDouble();
                }
            }
        }

        /// <summary>
        /// Exposure Bias
        /// </summary>
        public ExposureBias ExposureBias
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.ExposureBias.Query, typeof(SRational)))
                {
                    return new ExposureBias(this.BitmapMetadata.GetQuery<SRational>(ExifQueries.ExposureBias.Query));
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Focal Length
        /// </summary>
        public string FocalLength
        {
            get
            {
                URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.FocalLenght.Query);

                if (urational != null)
                {
                    return urational.ToDouble() + " mm";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// ISO Speed rating 
        /// </summary>
        public IsoSpeed IsoSpeed
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.IsoSpeedRating.Query, typeof(UInt16)))
                {
                    UInt16 isoSpeed = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.IsoSpeedRating.Query);

                    return new IsoSpeed(isoSpeed);
                }

                return new IsoSpeed();
            }
        }

        /// <summary>
        /// Horizontal Resolution
        /// </summary>
        public int HorizontalResolution
        {
            get
            {
                URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.HorizontalResolution.Query);

                if (urational == null || urational.Numerator == 0)
                {
                    return 0;
                }
                else
                {
                    return urational.ToInt();
                }
            }
        }

        /// <summary>
        /// Image Height measured in Pixels
        /// </summary>
        public int ImageHeight
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.ImageHeight.Query, typeof(UInt16)))
                {
                    int imageHeight = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.ImageHeight.Query);

                    return imageHeight;
                }

                return 0;
            }
        }

        /// <summary>
        /// Image Width measured in Pixels
        /// </summary>
        public int ImageWidth
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.ImageWidth.Query, typeof(string)))
                {
                 int imageWidth = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.ImageWidth.Query);

                    return imageWidth;
                }

                return 0;
            }
        }

        /// <summary>
        /// Shutter Speed
        /// </summary>
        public ShutterSpeed ShutterSpeed
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.ShutterSpeed.Query, typeof(URational)))
                {
                    URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.ShutterSpeed.Query);

                    if (urational != null)
                    {
                        return new ShutterSpeed(urational);
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Vertical Resolution of Thumbnail
        /// </summary>
        public int ThumbnailVerticalResolution
        {
            get
            {
                URational rational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.ThumbnailVerticalResolution.Query);

                if (rational == null || rational.Numerator == 0)
                {
                    return 0;
                }
                else
                {
                    return rational.ToInt();
                }
            }
        }

        /// <summary>
        /// Horizontal Resolution of Thumbnail
        /// </summary>
        public int ThumbnailHorizontalResolution
        {
            get
            {
                URational rational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.ThumbnailHorizontalResolution.Query);

                if (rational == null || rational.Numerator == 0)
                {
                    return 0;
                }
                else
                {
                    return rational.ToInt();
                }
            }
        }

        /// <summary>
        /// Vertical Resolution of Main Photo
        /// </summary>
        public int VerticalResolution
        {
            get
            {
                URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.VerticalResolution.Query);

                if (urational == null || urational.Numerator == 0)
                {
                    return 0;
                }
                else
                {
                    return urational.ToInt();
                }
            }
        }

        /// <summary>
        /// ExposureMode
        /// </summary>
        public MetadataEnums.ExposureModes ExposureMode
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.ExposureMode.Query, typeof(UInt16)))
                {
                    UInt16 exposureMode = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.ExposureMode.Query);

                    return (MetadataEnums.ExposureModes)exposureMode;
                }

                return MetadataEnums.ExposureModes.AutoExposure;
            }
        }

        /// <summary>
        /// SubjectDistanceRange
        /// </summary>
        public MetadataEnums.SubjectDistanceRanges SubjectDistanceRange
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.SubjectDistanceRange.Query, typeof(UInt16)))
                {
                    uint subjectDistanceRange = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.SubjectDistanceRange.Query);

                    return (MetadataEnums.SubjectDistanceRanges)subjectDistanceRange;
                }

                return MetadataEnums.SubjectDistanceRanges.Unknown;
            }
        }

        /// <summary>
        /// Metering Mode
        /// </summary>
        public MetadataEnums.MeteringModes MeteringMode
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.MeteringMode.Query, typeof(UInt16)))
                {
                    UInt16 meteringMode = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.MeteringMode.Query);

                    return (MetadataEnums.MeteringModes)meteringMode;
                }

                return MetadataEnums.MeteringModes.Unknown;
            }
        }

        /// <summary>
        /// Orientation
        /// </summary>
        public MetadataEnums.Orientations Orientation
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.Orientation.Query, typeof(UInt16)))
                {
                    UInt16 orientation = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.Orientation.Query);

                    return (MetadataEnums.Orientations)orientation;
                }

                return MetadataEnums.Orientations.Unknown;
            }
        }

        /// <summary>
        /// WhiteBalance
        /// </summary>
        public MetadataEnums.WhiteBalances WhiteBalance
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.WhiteBalance.Query, typeof(UInt16)))
                {
                    UInt16 whiteBalance = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.WhiteBalance.Query);

                    return (MetadataEnums.WhiteBalances)whiteBalance;
                }

                return MetadataEnums.WhiteBalances.AutoWhiteBalance;
            }
        }

        /// <summary>
        /// WhiteBalanceMode
        /// </summary>
        public MetadataEnums.LightSources LightSource
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.LightSource.Query, typeof(UInt16)))
                {
                    uint lightSource = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.LightSource.Query);

                    return (MetadataEnums.LightSources)lightSource;
                }

                return MetadataEnums.LightSources.Unknown;
            }
        }

        /// <summary>
        /// ColorRepresentation
        /// </summary>
        public MetadataEnums.ColorRepresentations ColorRepresentation
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.ColorRepresentation.Query, typeof(UInt16)))
                {
                    UInt16 colour = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.ColorRepresentation.Query);

                    return MetadataEnums.ColorRepresentations.sRGB;
                }

                return MetadataEnums.ColorRepresentations.Unknown;
            }
        }

        /// <summary>
        /// ExposureMode
        /// </summary>
        public MetadataEnums.ExposurePrograms ExposureProgram
        {
            get
            {
                if (this.BitmapMetadata.IsQueryValidAndOfType(ExifQueries.ExposureProgram.Query, typeof(UInt16)))
                {
                    uint exposureProgram = this.BitmapMetadata.GetQuery<UInt16>(ExifQueries.ExposureProgram.Query);

                    return (MetadataEnums.ExposurePrograms)exposureProgram;
                }

                return MetadataEnums.ExposurePrograms.Unknown;
            }
        }

        /// <summary>
        /// Title, sometimes knows as Caption and Subject (there is also another attribute for Subject)
        /// </summary>
        public string Title
        {
            get
            {
                string formattedString = this.BitmapMetadata.Title;

                if (String.IsNullOrEmpty(formattedString))
                {
                    return string.Empty;
                }
                else
                {
                    return formattedString;
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.Title))
                {
                    this.BitmapMetadata.Title = value;
                }
            }
        }

        #region Flash Code that's not complete
        /// <summary>
        /// FlashStatus
        /// </summary>
        ////public PhotoMetadataEnums.FlashStatus Flash
        ////{
        ////    get
        ////    {
        ////        uint? unknownObject = this.BitmapMetadata.GetQueryAsUint(ExifQueries.FlashFired);

        ////        if (unknownObject == null)
        ////        {
        ////            return PhotoMetadataEnums.FlashStatus.NoFlash;
        ////        }
        ////        else
        ////        {
        ////            // Convert to Array
        ////            char[] flashBytes = Convert.ToString(unknownObject.Value, 2).PadLeft(8, '0').ToCharArray();

        ////            // Reverse the order
        ////            Array.Reverse(flashBytes);

        ////            if (flashBytes[0] == '1' && flashBytes[6] == '0')
        ////            {
        ////                return PhotoMetadataEnums.FlashStatus.Flash;
        ////            }
        ////            else if (flashBytes[0] == '1' && flashBytes[6] == '1')
        ////            {
        ////                return PhotoMetadataEnums.FlashStatus.FlashWithRedEyeReduction;
        ////            }
        ////            else
        ////            {
        ////                return PhotoMetadataEnums.FlashStatus.NoFlash;
        ////            }
        ////        }
        ////    }
        ////}
        #endregion
    }
}
