// <copyright file="GpxFileManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that reads and writes Gpx Files</summary>
namespace Fotofly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Threading;

    public static class GpxFileManager
    {
        private static int serializerSleepTime = 5000;
        public static readonly string GpxFileExtension = ".gpx";
        public static string SerializationPrefix = "gpx";
        public static string SerializationNamespace = "http://www.topografix.com/GPX/1/1";

        public static GpxFile Read(string fileName)
        {
            GpxFile newGpxFile = new GpxFile();

            if (File.Exists(fileName))
            {
                try
                {
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            // Create the seraliser
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GpxRootNode));

                            newGpxFile.RootNode = (GpxRootNode)xmlSerializer.Deserialize(reader);
                        }

                        // Try and force the file lock to be released
                        fileStream.Close();
                        fileStream.Dispose();
                    }

                    newGpxFile.Filename = fileName;
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to read the file: " + fileName, e);
                }
            }
            else
            {
                throw new Exception("Gpx file does not exist: " + fileName);
            }

            return newGpxFile;
        }

        public static void Write(GpxFile gpxFile, string fileName)
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
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GpxRootNode));

                            // Write the file
                            xmlSerializer.Serialize(writer, gpxFile.RootNode);
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
                    Thread.Sleep(GpxFileManager.serializerSleepTime);
                }
            }

            if (serializerSucceeded == false)
            {
                throw new Exception("Unable to save the file: " + fileName, serializerException);
            }
        }
    }
}
