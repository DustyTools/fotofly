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
    using System.IO;
    using System.Xml.Serialization;
    using System.Threading;

    public static class PhotoMetadataTools
    {
        public static string SerializationPrefix = "fotofly";
        public static string SerializationNamespace = "http://www.tassography.com/fotofly";
        private static int serializationSleepTime = 5000;

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
            List<CompareResult> compareResults = new List<CompareResult>();

            PhotoMetadataTools.UseReflection(fileMetadata, photoMetadata, true, ref compareResults);

            // Use Reflection to Copy all values from fileMetadata to photoMetadata
            return photoMetadata;
        }

        public static void WriteBitmapMetadata(BitmapMetadata bitmapMetadata, PhotoMetadata photoMetadata)
        {
            FileMetadata fileMetadata = new FileMetadata(bitmapMetadata);

            // List of changes, used for debugging
            List<CompareResult> compareResults = new List<CompareResult>();

            // Use Reflection to Copy all values from photoMetadata to FileMetadata
            PhotoMetadataTools.UseReflection(photoMetadata, fileMetadata, true, ref compareResults);
        }

        public static void CompareMetadata(object source, object destination, ref List<CompareResult> changes)
        {
            PhotoMetadataTools.UseReflection(source, destination, false, ref changes);
        }

        public static PhotoMetadata ReadPhotoMetadataFromXml(string fileName)
        {
            PhotoMetadata photoMetadata;

            if (File.Exists(fileName))
            {
                try
                {
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            // Create the seraliser
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PhotoMetadata), PhotoMetadataTools.SerializationNamespace); 
                            
                            photoMetadata = (PhotoMetadata)xmlSerializer.Deserialize(reader);
                        }

                        // Try and force the file lock to be released
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to read the file: " + fileName, e);
                }
            }
            else
            {
                throw new Exception("File not found: " + fileName);
            }

            return photoMetadata;
        }

        public static void WritePhotoMetadataToXml(PhotoMetadata photoMetadata, string fileName)
        {
            int retryCount = 3;

            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("FileName is not valid");
            }

            Exception serializerException = null;
            bool serializerSucceeded = false;

            for (int i = 0; i < retryCount; i++)
            {
                // Try saving the file
                try
                {
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (StreamWriter writer = new StreamWriter(fileStream))
                        {

                            // Create the seraliser
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PhotoMetadata));

                            // Write the file
                            xmlSerializer.Serialize(writer, photoMetadata, PhotoMetadataTools.XmlSerializerNamespaces);
                        }

                        // Try and force the file lock to be released
                        fileStream.Close();
                        fileStream.Dispose();
                    }

                    serializerSucceeded = true;
                }
                catch (Exception e)
                {
                    serializerException = e;
                    serializerSucceeded = false;
                }

                if (serializerSucceeded)
                {
                    break;
                }
                else
                {
                    // Sleep before trying again
                    Thread.Sleep(PhotoMetadataTools.serializationSleepTime);
                }
            }

            if (serializerSucceeded == false)
            {
                throw new Exception("Unable to save the file: " + fileName, serializerException);
            }
        }

        private static void UseReflection(object source, object destination, bool applyChanges, ref List<CompareResult> compareResults)
        {
            // Use Reflection to copy properties of the same name and type
            // This is done to reduce the risk of overwriting data in the file
            if (compareResults == null)
            {
                compareResults = new List<CompareResult>();
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

                        CompareResult compareResult = new CompareResult();
                        compareResult.PropertyName = destination.GetType().Name + "." + sourceName;
                        compareResult.SourceObject = sourceValue;
                        compareResult.DestinationObject = destinationValue;

                        compareResults.Add(compareResult);
                    }
                }
            }
        }

        private static XmlSerializerNamespaces XmlSerializerNamespaces
        {
            get
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, PhotoMetadataTools.SerializationNamespace);
                return xmlSerializerNamespaces;
            }
        }
    }
}
