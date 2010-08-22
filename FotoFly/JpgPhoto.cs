// <copyright file="JpgPhoto.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>JpgPhoto</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Xml.Serialization;

    using Fotofly.BitmapMetadataTools;
    using Fotofly.MetadataProviders;
    using System.Reflection;

    public class JpgPhoto : GenericPhotoFile
    {
        /// <summary>
        /// Class representing a Jpeg Photo
        /// </summary>
        public JpgPhoto()
        {
        }

        /// <summary>
        /// Class representing a Jpeg Photo
        /// </summary>
        /// <param name="fileName">Filename</param>
        public JpgPhoto(string fileName)
        {
            this.SetFileName(fileName);
        }

        /// <summary>
        /// Standard Metadata
        /// </summary>
        public PhotoMetadata Metadata
        {
            get
            {
                // Attempt to read the Metadata if it's not already loaded
                if (this.InternalPhotoMetadata == null && this.IsFileNameValid)
                {
                    this.ReadMetadata();
                }

                return this.InternalPhotoMetadata;
            }
        }

        /// <summary>
        /// The filename is valid and the file exists
        /// </summary>
        public new bool IsFileNameValid
        {
            get
            {
                // Compliment Base checks with file extension checks
                return base.IsFileNameValid && this.ImageType == GenericPhotoEnums.ImageTypes.Jpeg;
            }
        }

        /// <summary>
        /// Metadata as stored in the file
        /// </summary>
        private PhotoMetadata MetadataInFile
        {
            get
            {
                // Attempt to read the Metadata if it's not already loaded
                if (this.InternalPhotoMetadataInFile == null && this.IsFileNameValid)
                {
                    this.ReadMetadata();
                }

                return this.InternalPhotoMetadataInFile;
            }
        }

        /// <summary>
        /// Reads the Jpeg Metadata
        /// </summary>
        public void ReadMetadata()
        {
            if (this.IsFileNameValid)
            {
                if (this.HandleExceptions)
                {
                    try
                    {
                        this.UnhandledReadMetadata();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error reading Metadata: " + this.FileFullName, e);
                    }
                }
                else
                {
                    this.UnhandledReadMetadata();
                }
            }
            else
            {
                if (this.HandleExceptions)
                {
                    throw new Exception("File does not exist or is not valid: " + this.FileFullName);
                }
            }
        }

        /// <summary>
        /// Write the changes made to the Metadata
        /// </summary>
        /// <param name="fileName">File to save the metadata changes to</param>
        public void WriteMetadata(string fileName)
        {
            // If the file name isn't set then save it
            if (!this.IsFileNameValid)
            {
                // Save filename
                this.SetFileName(fileName);
            }
            else
            {
                // The filename is set, so we're saving a copy
                // Grab a copy of the source file, we need this for image
                File.Copy(this.FileFullName, fileName, true);

                // Save filename
                this.SetFileName(fileName);
            }

            this.WriteMetadata();
        }

        /// <summary>
        /// Write the changes made to the Metadata
        /// </summary>
        public void WriteMetadata()
        {
            if (this.IsFileNameValid)
            {
                if (this.HandleExceptions)
                {
                    try
                    {
                        this.UnhandledWriteMetadata();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error saving metadata: " + this.FileFullName, e);
                    }
                }
                else
                {
                    this.UnhandledWriteMetadata();
                }
            }
            else
            {
                if (this.HandleExceptions)
                {
                    throw new Exception("File does not exist or is not valid: " + this.FileFullName);
                }
            }
        }

        /// <summary>
        /// Reads the Jpeg Metadata from an Xml file
        /// </summary>
        /// <param name="fileName"></param>
        public void ReadMetadataFromXml(string fileName)
        {
            this.SetFileName(fileName);

            if (!File.Exists(this.FileFullName))
            {
                throw new Exception("File does not exist: " + this.FileFullName);
            }

            this.InternalPhotoMetadata = PhotoMetadataTools.ReadPhotoMetadataFromXml(fileName);
            this.InternalPhotoMetadataInFile = PhotoMetadataTools.ReadPhotoMetadataFromXml(fileName);
        }

        /// <summary>
        /// Writes the Metadata to an Xml file in the same folder as the jpeg with an xml extension
        /// </summary>
        public void WriteMetadataToXml()
        {
            this.WriteMetadataToXml(Regex.Replace(this.FileFullName, this.FileExtension, ".xml", RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Writes the Metadata to an Xml file
        /// </summary>
        /// <param name="fileName">Filename of xml output</param>
        public void WriteMetadataToXml(string fileName)
        {
            if (this.Metadata == null)
            {
                throw new Exception("Metadata can not be read");
            }
            else
            {
                PhotoMetadataTools.WritePhotoMetadataToXml(this.Metadata, fileName);
            }
        }

        /// <summary>
        /// Compares the Metadata with the metadata stored in the original file
        /// </summary>
        /// <returns>A list of changes</returns>
        public List<CompareResult> CompareMetadataToFileMetadata()
        {
            if (!this.IsFileNameValid)
            {
                throw new Exception("File does not exist or is not valid: " + this.FileFullName);
            }

            List<CompareResult> compareResults = new List<CompareResult>();

            // Compare the two
            PhotoMetadataTools.CompareMetadata(this.MetadataInFile, this.Metadata, ref compareResults);

            // Sort
            var query = from x in compareResults
                        orderby x
                        select x;

            return query.ToList();
        }

        /// <summary>
        /// Read Metadata from the Jpeg file, with no expection handling
        /// </summary>
        private void UnhandledReadMetadata()
        {
            this.InternalPhotoMetadata = new PhotoMetadata();
            this.InternalPhotoMetadataInFile = new PhotoMetadata();

            // Read BitmapMetadata
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.FileFullName))
            {
                // Copy that gets changed
                this.InternalPhotoMetadata = PhotoMetadataTools.ReadBitmapMetadata(wpfFileManager.BitmapMetadata, wpfFileManager.BitmapDecoder);

                this.InternalPhotoMetadata.ImageHeight = wpfFileManager.BitmapDecoder.Frames[0].PixelHeight;
                this.InternalPhotoMetadata.ImageWidth = wpfFileManager.BitmapDecoder.Frames[0].PixelWidth;

                // Copy saved metadata for comparisons
                this.InternalPhotoMetadataInFile = PhotoMetadataTools.ReadBitmapMetadata(wpfFileManager.BitmapMetadata, wpfFileManager.BitmapDecoder);

                this.InternalPhotoMetadataInFile.ImageHeight = wpfFileManager.BitmapDecoder.Frames[0].PixelHeight;
                this.InternalPhotoMetadataInFile.ImageWidth = wpfFileManager.BitmapDecoder.Frames[0].PixelWidth;
            }
        }

        /// <summary>
        /// Write Metadata to the Jpeg file, with no expection handling
        /// </summary>
        private void UnhandledWriteMetadata()
        {
            List<CompareResult> changes = new List<CompareResult>();

            // Compare the two
            PhotoMetadataTools.CompareMetadata(this.InternalPhotoMetadataInFile, this.InternalPhotoMetadata, ref changes);

            // Save if there have been changes to the Metadata
            if (changes.Count > 0)
            {
                // Set the Last Edit Date
                this.Metadata.FotoflyDateLastSave = DateTime.Now;
                this.Metadata.CreationSoftware = FotoflyAssemblyInfo.ShortBuildVersion;

                // Read the file
                using (WpfFileManager wpfFileManager = new WpfFileManager(this.FileFullName, true))
                {
                    // Copy values across
                    PhotoMetadataTools.WriteBitmapMetadata(wpfFileManager.BitmapMetadata, this.InternalPhotoMetadata);

                    // Save file
                    wpfFileManager.WriteMetadata();

                    // Save new BitmapMetadata as MetadataInFile
                    this.InternalPhotoMetadataInFile = PhotoMetadataTools.ReadBitmapMetadata(wpfFileManager.BitmapMetadata);
                }
            }
        }
    }
}
