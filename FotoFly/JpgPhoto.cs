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
    using System.Windows.Media.Imaging;
    using System.Xml.Serialization;

    using Fotofly.WpfTools;
    using Fotofly.XmlTools;

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
        /// Industry Standard Metadata
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
        /// Custom Metadata used by Fotofly
        /// </summary>
        public FotoflyMetadata FotoflyMetadata
        {
            get
            {
                // Attempt to read the Metadata if it's not already loaded
                if (this.InternalFotoflyMetadata == null && this.IsFileNameValid)
                {
                    this.ReadMetadata();
                }

                return this.InternalFotoflyMetadata;
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
            if (!this.IsFileNameValid)
            {
                throw new Exception("Source file does not exist or is not valid: " + fileName);
            }

            // Grab a copy of the source file, we need this for image
            File.Copy(this.FileFullName, fileName, true);
                
            // Save filename
            this.SetFileName(fileName);

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
            else
            {
                try
                {
                    using (FileStream fileStream = new FileStream(this.FileFullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PhotoMetadata));

                            this.InternalPhotoMetadata = xmlSerializer.Deserialize(reader) as PhotoMetadata;
                        }

                        // Try and force the file lock to be released
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to read the file: " + this.FileFullName, e);
                }
            }
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
                GenericSerialiser.Write<PhotoMetadata>(this.Metadata, fileName);
            }
        }

        /// <summary>
        /// Read Metadata from the Jpeg file, with no expection handling
        /// </summary>
        private void UnhandledReadMetadata()
        {
            this.InternalPhotoMetadata = new PhotoMetadata();
            this.InternalFotoflyMetadata = new FotoflyMetadata();
            bool containsOldNamespaceMetadata = false;

            // Read BitmapMetadata
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.FileFullName))
            {
                // Get generic Metadata
                WpfMetadata wpfMetadata = new WpfMetadata(wpfFileManager.BitmapMetadata);

                // Copy the common metadata across using reflection tool
                IPhotoMetadataTools.CopyMetadata(wpfMetadata, this.InternalPhotoMetadata);

                // Get Fotofly Custom Metadata
                WpfFotoflyMetadata wpfFotoflyMetadata = new WpfFotoflyMetadata(wpfFileManager.BitmapMetadata);

                // Check there's data for the old FotoFly namespace
                containsOldNamespaceMetadata = wpfFotoflyMetadata.ContainsOldNamespaceMetadata;
                
                // Copy the common metadata across using reflection tool
                IPhotoMetadataTools.CopyMetadata(wpfFotoflyMetadata, this.FotoflyMetadata);

                // Manually copy across ImageHeight & ImageWidth if they are not set in metadata
                // This should be pretty rare but can happen if the image has been resized or manipulated and the metadata not copied across
                if (this.InternalPhotoMetadata.ImageHeight == 0 || this.InternalPhotoMetadata.ImageWidth == 0)
                {
                    this.InternalPhotoMetadata.ImageHeight = wpfFileManager.BitmapDecoder.Frames[0].PixelHeight;
                    this.InternalPhotoMetadata.ImageWidth = wpfFileManager.BitmapDecoder.Frames[0].PixelWidth;
                }
            }

            // Force a save, which will migrate the metadata
            if (containsOldNamespaceMetadata)
            {
                this.UnhandledWriteMetadata();
            }
        }

        /// <summary>
        /// Write Metadata to the Jpeg file, with no expection handling
        /// </summary>
        private void UnhandledWriteMetadata()
        {
            List<string> changes = new List<string>();

            // Get original File metadata
            BitmapMetadata bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.FileFullName, true);

            WpfMetadata wpfMetadata = new WpfMetadata(bitmapMetadata);

            // Copy JpgMetadata across to file Metadata
            IPhotoMetadataTools.CopyMetadata(this.InternalPhotoMetadata, wpfMetadata, ref changes);

            WpfFotoflyMetadata wpfFotoflyMetadata = new WpfFotoflyMetadata(bitmapMetadata);

            // Copy JpgMetadata across to file Metadata
            IPhotoMetadataTools.CopyMetadata(this.FotoflyMetadata, wpfFotoflyMetadata, ref changes);

            // Save if there have been changes to the Metadata
            if (changes.Count > 0)
            {
                // Set the Last Edit Date
                this.FotoflyMetadata.LastEditDate = DateTime.Now;
                wpfFotoflyMetadata.LastEditDate = this.FotoflyMetadata.LastEditDate;

                // Write changes
                WpfFileManager.WriteBitmapMetadata(this.FileFullName, bitmapMetadata);
            }
        }
    }
}
