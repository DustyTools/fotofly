// <copyright file="GpxFileManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that reads and writes Gpx Files</summary>
namespace FotoFly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using FotoFly.XmlTools;

    public static class GpxFileManager
    {
        public static readonly string GpxFileExtension = ".gpx";

        public static GpxFile Read(string fileName)
        {
            GpxFile newGpxFile = new GpxFile();

            if (File.Exists(fileName))
            {
                newGpxFile.RootNode = GenericSerialiser.Read<GpxRootNode>(fileName);
                newGpxFile.Filename = fileName;

                // Convert the File to DateTime, this represents the local date for the track
                StringBuilder date = new StringBuilder();
                date.Append(newGpxFile.Filename.Substring(6, 2));
                date.Append("/");
                date.Append(newGpxFile.Filename.Substring(4, 2));
                date.Append("/");
                date.Append(newGpxFile.Filename.Substring(0, 4));

                newGpxFile.LocalDate = DateTime.ParseExact(date.ToString(), "dd/MM/yyyy", new CultureInfo("en-GB"));
            }
            else
            {
                throw new Exception("Gpx file does not exist: " + fileName);
            }

            return newGpxFile;
        }

        public static void Write(GpxFile gpxFile, string fileName)
        {
            GenericSerialiser.Write<GpxRootNode>(gpxFile.RootNode, fileName);
        }
    }
}
