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
        /// List of Authors, also known as Photographer
        /// </summary>
        public PeopleList Authors
        {
            get
            {
                ReadOnlyCollection<string> readOnlyCollectionString = this.BitmapMetadata.Author;

                if (readOnlyCollectionString == null || readOnlyCollectionString.Count == 0)
                {
                    return new PeopleList();
                }
                else
                {
                    return new PeopleList(readOnlyCollectionString);
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.Authors))
                {
                    this.BitmapMetadata.Author = new ReadOnlyCollection<string>(value);
                }
            }
        }
        
        /// <summary>
        /// Aperture
        /// </summary>
        public string Aperture
        {
            get
            {
                URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.Aperture.Query);

                if (urational != null)
                {
                    return "f/" + urational.ToDouble().ToString();
                }
                else
                {
                    return string.Empty;
                }
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
                string formattedString = string.Empty;

                try
                {
                    formattedString = this.BitmapMetadata.Copyright;
                }
                catch
                {
                    return String.Empty;
                }

                return formattedString;
            }

            set
            {
                if (this.ValueHasChanged(value, this.Copyright))
                {
                    this.BitmapMetadata.Copyright = value;
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
        /// DateTaken, recorded by the camera when the photo is taken
        /// </summary>
        public DateTime DateTaken
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
                if (this.ValueHasChanged(value, this.DateTaken))
                {
                    // Write to tempBitmap count
                    // Use sortable string to avoid US date format issue with Month\Date
                    this.BitmapMetadata.DateTaken = value.ToString("s");

                    // Use specific format for EXIF data, 2008:12:01 13:14:10
                    this.BitmapMetadata.SetQuery(ExifQueries.DateTaken.Query, value.ToString("yyyy:MM:dd HH:mm:ss"));
                }
            }
        }

        /// <summary>
        /// DigitalZoomRatio
        /// </summary>
        public double DigitalZoomRatio
        {
            get
            {
                URational rational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.DigitalZoomRatio.Query);

                if (rational == null || rational.Numerator == 0)
                {
                    return 0;
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
        public string ExposureBias
        {
            get
            {
                SRational rational = this.BitmapMetadata.GetQuery<SRational>(ExifQueries.ExposureBias.Query);

                if (rational != null && rational.ToInt() != 0)
                {
                    if (rational.ToInt() > 0)
                    {
                        return "+" + Math.Round(rational.ToDouble(), 1) + " step";
                    }
                    else
                    {
                        return Math.Round(rational.ToDouble(), 1) + " step";
                    }
                }

                return "0 step";
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
        public string Iso
        {
            get
            {
                if (this.BitmapMetadata.IsQueryOfType(ExifQueries.IsoSpeedRating.Query, ExifQueries.IsoSpeedRating.BitmapMetadataType))
                {
                    UInt16? iso = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.IsoSpeedRating.Query);

                    if (iso == null)
                    {
                        return string.Empty;
                    }
                    else if (iso.Value > 0 && iso.Value < 10000)
                    {
                        return "ISO-" + iso.Value.ToString();
                    }
                }

                return string.Empty;
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
                int? imageHeight = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.ImageHeight.Query);

                if (imageHeight == null)
                {
                    return 0;
                }
                else
                {
                    return imageHeight.Value;
                }
            }
        }

        /// <summary>
        /// Image Width measured in Pixels
        /// </summary>
        public int ImageWidth
        {
            get
            {
                int? imageWidth = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.ImageWidth.Query);

                if (imageWidth == null)
                {
                    return 0;
                }
                else
                {
                    return imageWidth.Value;
                }
            }
        }

        /// <summary>
        /// Shutter Speed
        /// </summary>
        public string ShutterSpeed
        {
            get
            {
                try
                {
                    URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.ShutterSpeed.Query);

                    if (urational != null)
                    {
                        double exposureTime = urational.ToDouble();

                        string formattedString = String.Empty;

                        if (exposureTime > 1)
                        {
                            formattedString = exposureTime.ToString();
                        }
                        else
                        {
                            // Convert Decimal to Integer
                            double newDecimal = System.Math.Round((1 / exposureTime), 0);

                            formattedString = newDecimal.ToString();

                            formattedString = "1/" + formattedString;
                        }

                        if (formattedString != String.Empty)
                        {
                            formattedString += " sec.";
                        }

                        return formattedString;
                    }
                }
                catch
                {
                }

                return String.Empty;
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
                UInt16? exposureMode = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.ExposureMode.Query);

                if (exposureMode == null)
                {
                    return MetadataEnums.ExposureModes.AutoExposure;
                }
                else
                {
                    return (MetadataEnums.ExposureModes)exposureMode.Value;
                }
            }
        }

        /// <summary>
        /// SubjectDistanceRange
        /// </summary>
        public MetadataEnums.SubjectDistanceRanges SubjectDistanceRange
        {
            get
            {
                uint? subjectDistanceRange = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.SubjectDistanceRange.Query);

                if (subjectDistanceRange == null)
                {
                    return MetadataEnums.SubjectDistanceRanges.Unknown;
                }
                else
                {
                    return (MetadataEnums.SubjectDistanceRanges)subjectDistanceRange.Value;
                }
            }
        }

        /// <summary>
        /// Metering Mode
        /// </summary>
        public MetadataEnums.MeteringModes MeteringMode
        {
            get
            {
                UInt16? meteringMode = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.MeteringMode.Query);

                if (meteringMode == null)
                {
                    return MetadataEnums.MeteringModes.Unknown;
                }
                else
                {
                    return (MetadataEnums.MeteringModes)meteringMode.Value;
                }
            }
        }

        /// <summary>
        /// Orientation
        /// </summary>
        public MetadataEnums.Orientations Orientation
        {
            get
            {
                UInt16? orientation = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.Orientation.Query);

                if (orientation == null)
                {
                    return MetadataEnums.Orientations.Unknown;
                }
                else
                {
                    return (MetadataEnums.Orientations)orientation.Value;
                }
            }
        }

        /// <summary>
        /// WhiteBalance
        /// </summary>
        public MetadataEnums.WhiteBalances WhiteBalance
        {
            get
            {
                UInt16? whiteBalance = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.WhiteBalance.Query);

                if (whiteBalance == null)
                {
                    return MetadataEnums.WhiteBalances.AutoWhiteBalance;
                }
                else
                {
                    return (MetadataEnums.WhiteBalances)whiteBalance.Value;
                }
            }
        }

        /// <summary>
        /// WhiteBalanceMode
        /// </summary>
        public MetadataEnums.LightSources LightSource
        {
            get
            {
                uint? lightSource = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.LightSource.Query);

                if (lightSource == null)
                {
                    return MetadataEnums.LightSources.Unknown;
                }
                else
                {
                    return (MetadataEnums.LightSources)lightSource.Value;
                }
            }
        }

        /// <summary>
        /// ColorRepresentation
        /// </summary>
        public MetadataEnums.ColorRepresentations ColorRepresentation
        {
            get
            {
                UInt16? colour = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.ColorRepresentation.Query);

                if (colour == null || colour.Value != 1)
                {
                    return MetadataEnums.ColorRepresentations.Unknown;
                }
                else
                {
                    return MetadataEnums.ColorRepresentations.sRGB;
                }
            }
        }

        /// <summary>
        /// ExposureMode
        /// </summary>
        public MetadataEnums.ExposurePrograms ExposureProgram
        {
            get
            {
                uint? exposureProgram = this.BitmapMetadata.GetQuery<UInt16?>(ExifQueries.ExposureProgram.Query);

                if (exposureProgram == null)
                {
                    return MetadataEnums.ExposurePrograms.Unknown;
                }
                else
                {
                    return (MetadataEnums.ExposurePrograms)exposureProgram.Value;
                }
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
