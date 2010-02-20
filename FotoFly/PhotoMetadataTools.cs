// <copyright file="PhotoMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>PhotoMetadata</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Media.Imaging;
    using Fotofly.MetadataProviders;

    public static class PhotoMetadataTools
    {
        public static PhotoMetadata ReadBitmapMetadata(BitmapMetadata bitmapMetadata)
        {
            return PhotoMetadataTools.ReadBitmapMetadata(bitmapMetadata, null);
        }

        public static PhotoMetadata ReadBitmapMetadata(BitmapMetadata bitmapMetadata, BitmapDecoder bitmapDecoder)
        {
            PhotoMetadata photoMetadata = new PhotoMetadata();

            // Load Metadata Reader
            FileMetadata fileMetadata = new FileMetadata(bitmapMetadata);

            // Copy across all base properties
            photoMetadata.Aperture = fileMetadata.ExifProvider.Aperture;
            photoMetadata.Authors = fileMetadata.ExifProvider.Authors;
            photoMetadata.CameraManufacturer = fileMetadata.ExifProvider.CameraManufacturer;
            photoMetadata.CameraModel = fileMetadata.ExifProvider.CameraModel;
            photoMetadata.Comment = fileMetadata.XmpCoreProvider.Comment;
            photoMetadata.Copyright = fileMetadata.ExifProvider.Copyright;
            photoMetadata.CreationSoftware = fileMetadata.ExifProvider.CreationSoftware;
            photoMetadata.DateAquired = fileMetadata.XmpMicrosoftProvider.DateAquired;
            photoMetadata.DateDigitised = fileMetadata.ExifProvider.DateDigitised;
            photoMetadata.DateTaken = fileMetadata.ExifProvider.DateTaken;
            photoMetadata.DigitalZoomRatio = fileMetadata.ExifProvider.DigitalZoomRatio;
            photoMetadata.ExposureBias = fileMetadata.ExifProvider.ExposureBias;
            photoMetadata.FocalLength = fileMetadata.ExifProvider.FocalLength;
            photoMetadata.HorizontalResolution = fileMetadata.ExifProvider.HorizontalResolution;
            photoMetadata.ImageHeight = fileMetadata.ExifProvider.ImageHeight;
            photoMetadata.ImageWidth = fileMetadata.ExifProvider.ImageWidth;
            photoMetadata.Iso = fileMetadata.ExifProvider.Iso;
            photoMetadata.MeteringMode = fileMetadata.ExifProvider.MeteringMode;
            photoMetadata.RegionInfo = fileMetadata.XmpMicrosoftProvider.RegionInfo;
            photoMetadata.ShutterSpeed = fileMetadata.ExifProvider.ShutterSpeed;
            photoMetadata.Subject = fileMetadata.XmpCoreProvider.Subject;
            photoMetadata.Tags = fileMetadata.XmpCoreProvider.Tags;
            photoMetadata.Title = fileMetadata.ExifProvider.Title;
            photoMetadata.VerticalResolution = fileMetadata.ExifProvider.VerticalResolution;

            // Rating
            // Check Xap, then Microsoft
            if (fileMetadata.XmpXapProvider.Rating != MetadataEnums.Rating.Unknown)
            {
                photoMetadata.Rating = fileMetadata.XmpXapProvider.Rating;
            }
            else if (fileMetadata.XmpMicrosoftProvider.Rating != MetadataEnums.Rating.Unknown)
            {
                photoMetadata.Rating = fileMetadata.XmpMicrosoftProvider.Rating;
            }
            else
            {
                photoMetadata.Rating = MetadataEnums.Rating.Unknown;
            }
            

            // Retrieve Fotofly Data
            photoMetadata.AddressOfGps = fileMetadata.XmpFotoflyProvider.AddressOfGps;
            photoMetadata.AddressOfGpsLookupDate = fileMetadata.XmpFotoflyProvider.AddressOfGpsLookupDate;
            photoMetadata.AddressOfGpsSource = fileMetadata.XmpFotoflyProvider.AddressOfGpsSource;
            photoMetadata.FotoflyLastEditDate = fileMetadata.XmpFotoflyProvider.LastEditDate;
            photoMetadata.OriginalCameraDate = fileMetadata.XmpFotoflyProvider.OriginalCameraDate;
            photoMetadata.OriginalCameraFilename = fileMetadata.XmpFotoflyProvider.OriginalCameraFilename;
            photoMetadata.UtcDate = fileMetadata.XmpFotoflyProvider.UtcDate;
            photoMetadata.UtcOffset = fileMetadata.XmpFotoflyProvider.UtcOffset;

            // GPS
            // Check Exif, then XMP
            if (fileMetadata.GpsProvider.GpsPosition.IsValidCoordinate)
            {
                photoMetadata.GpsPosition = fileMetadata.GpsProvider.GpsPosition;
            }
            else if (fileMetadata.XmpExifProvider.GpsPosition.IsValidCoordinate)
            {
                photoMetadata.GpsPosition = fileMetadata.XmpExifProvider.GpsPosition;
            }
            else
            {
                photoMetadata.GpsPosition = new GpsPosition();
            }

            // Get Address
            // Order of precidence is XMP for IPTC, then IPTC
            if (fileMetadata.XmpIptcProvider.Address.IsValidAddress)
            {
                photoMetadata.Address = fileMetadata.XmpIptcProvider.Address;
            }
            else if (fileMetadata.IptcProvider.Address.IsValidAddress)
            {
                photoMetadata.Address = fileMetadata.IptcProvider.Address;
            }
            else
            {
                photoMetadata.Address = new Address();
            }

            if (bitmapDecoder != null)
            {
                // Manually copy across ImageHeight & ImageWidth if they are not set in metadata
                // This should be pretty rare but can happen if the image has been resized or manipulated and the metadata not copied across
                if (photoMetadata.ImageHeight == 0 || photoMetadata.ImageWidth == 0)
                {
                    photoMetadata.ImageHeight = bitmapDecoder.Frames[0].PixelHeight;
                    photoMetadata.ImageWidth = bitmapDecoder.Frames[0].PixelWidth;
                }
            }

            return photoMetadata;
        }

        public static void WriteBitmapMetadata(BitmapMetadata bitmapMetadata, PhotoMetadata photoMetadata)
        {
            FileMetadata fileMetadata = new FileMetadata(bitmapMetadata);

            // Copy across all base properties
            // NOTE: A lot of PhotoMetadata values are Readonly (mainly camera settings such as Aperture)
            fileMetadata.ExifProvider.Authors = photoMetadata.Authors;
            fileMetadata.ExifProvider.CameraManufacturer = photoMetadata.CameraManufacturer;
            fileMetadata.ExifProvider.CameraModel = photoMetadata.CameraModel;
            fileMetadata.XmpCoreProvider.Comment = photoMetadata.Comment;
            fileMetadata.ExifProvider.Copyright = photoMetadata.Copyright;
            fileMetadata.ExifProvider.CreationSoftware = photoMetadata.CreationSoftware;
            fileMetadata.XmpMicrosoftProvider.DateAquired = photoMetadata.DateAquired;
            fileMetadata.ExifProvider.DateDigitised = photoMetadata.DateDigitised;
            fileMetadata.ExifProvider.DateTaken = photoMetadata.DateTaken;
            fileMetadata.XmpMicrosoftProvider.RegionInfo = photoMetadata.RegionInfo;
            fileMetadata.XmpCoreProvider.Subject = photoMetadata.Subject;
            fileMetadata.XmpCoreProvider.Tags = photoMetadata.Tags;
            fileMetadata.ExifProvider.Title = photoMetadata.Title;

            // Save Fotofly Data
            fileMetadata.XmpFotoflyProvider.AccuracyOfGps = photoMetadata.AccuracyOfGps;
            fileMetadata.XmpFotoflyProvider.AddressOfGps = photoMetadata.AddressOfGps;
            fileMetadata.XmpFotoflyProvider.AddressOfGpsLookupDate = photoMetadata.AddressOfGpsLookupDate;
            fileMetadata.XmpFotoflyProvider.AddressOfGpsSource = photoMetadata.AddressOfGpsSource;
            fileMetadata.XmpFotoflyProvider.LastEditDate = photoMetadata.FotoflyLastEditDate;
            fileMetadata.XmpFotoflyProvider.OriginalCameraDate = photoMetadata.OriginalCameraDate;
            fileMetadata.XmpFotoflyProvider.OriginalCameraFilename = photoMetadata.OriginalCameraFilename;
            fileMetadata.XmpFotoflyProvider.UtcDate = photoMetadata.UtcDate;
            fileMetadata.XmpFotoflyProvider.UtcOffset = photoMetadata.UtcOffset;

            // Set Address, both XMP4IPTC & IPTC
            fileMetadata.XmpIptcProvider.Address = photoMetadata.Address;
            fileMetadata.IptcProvider.Address = photoMetadata.Address;

            // Save Rating
            // Only save to XmpMicrosoft if it's been set
            fileMetadata.XmpXapProvider.Rating = photoMetadata.Rating;

            if (fileMetadata.XmpMicrosoftProvider.Rating != MetadataEnums.Rating.Unknown)
            {
                fileMetadata.XmpMicrosoftProvider.Rating = photoMetadata.Rating;
            }

            // GPS
            // Save to Gps & XMP
            fileMetadata.GpsProvider.GpsPosition = photoMetadata.GpsPosition;

            if (fileMetadata.XmpExifProvider.GpsPosition.IsValidCoordinate)
            {
                fileMetadata.XmpExifProvider.GpsPosition = photoMetadata.GpsPosition;
            }
        }

        public static void CompareMetadata(object source, object destination, ref List<string> changes)
        {
            PhotoMetadataTools.UseReflection(source, destination, false, ref changes);
        }

        private static void UseReflection(object source, object destination, bool applyChanges, ref List<string> changes)
        {
            // Use Reflection to copy properties of the same name and type
            // This is done to reduce the risk of overwriting data in the file
            if (changes == null)
            {
                changes = new List<string>();
            }

            // Loop through every property in the source
            foreach (PropertyInfo sourcePropertyInfo in source.GetType().GetProperties())
            {
                string sourceName = sourcePropertyInfo.Name;
                object sourceValue = sourcePropertyInfo.GetValue(source, null);
                Type sourceType = sourcePropertyInfo.PropertyType;

                // Look for a matching property in the destination
                var destinationProperty = from x in destination.GetType().GetProperties()
                                          where x.Name == sourceName
                                          && x.PropertyType == sourceType
                                          && x.CanWrite
                                          select x;

                PropertyInfo destinationPropertyInfo = destinationProperty.FirstOrDefault();

                // Check if there's a matching property in the destination
                if (destinationPropertyInfo != null && destinationPropertyInfo.CanWrite)
                {
                    object destinationValue = destinationPropertyInfo.GetValue(destination, null);

                    if (destinationValue == null && sourceValue == null)
                    {
                        // Both null, do nothing
                    }
                    else if ((destinationValue == null && sourceValue != null) || !destinationValue.Equals(sourceValue))
                    {
                        if (applyChanges)
                        {
                            // Copy across the matching property
                            // Either as null, using cloning or a straight copy
                            if (sourceValue == null)
                            {
                                destinationPropertyInfo.SetValue(destination, null, null);
                            }
                            else if (sourceValue.GetType().GetInterface("ICloneable", true) == null)
                            {
                                destinationPropertyInfo.SetValue(destination, sourceValue, null);
                            }
                            else
                            {
                                destinationPropertyInfo.SetValue(destination, ((ICloneable)sourceValue).Clone(), null);
                            }
                        }

                        StringBuilder change = new StringBuilder();
                        change.Append(destination.GetType().Name + "." + sourceName);
                        change.Append(" ('");
                        change.Append(sourceValue == null ? "{null}" : (sourceValue.ToString() == string.Empty ? "{empty}" : sourceValue));
                        change.Append("' vs '");
                        change.Append(destinationValue == null ? "{null}" : (destinationValue.ToString() == string.Empty ? "{empty}" : destinationValue));
                        change.Append("')");

                        changes.Add(change.ToString());
                    }
                }
            }
        }
    }
}
