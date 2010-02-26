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

            // List of changes, used for debugging
            List<string> changes = new List<string>();

            PhotoMetadataTools.UseReflection(fileMetadata, photoMetadata, true, ref changes);

            // Use Reflection to Copy all values from fileMetadata to photoMetadata
            return photoMetadata;
        }

        public static void WriteBitmapMetadata(BitmapMetadata bitmapMetadata, PhotoMetadata photoMetadata)
        {
            FileMetadata fileMetadata = new FileMetadata(bitmapMetadata);

            // List of changes, used for debugging
            List<string> changes = new List<string>();

            // Use Reflection to Copy all values from photoMetadata to FileMetadata
            PhotoMetadataTools.UseReflection(photoMetadata, fileMetadata, true, ref changes);
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
