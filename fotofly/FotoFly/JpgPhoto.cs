// <copyright file="JpgPhoto.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>JpgPhoto</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public class JpgPhoto : GenericPhotoFile
    {
        public JpgPhoto()
        {
        }

        public JpgPhoto(string fileName)
        {
            this.FileName = fileName;
        }

        public new PhotoMetadata Metadata
        {
            get
            {
                // Attempt to read the Metadata if it's not already loaded
                if (base.Metadata == null && this.IsFileValid)
                {
                    this.ReadMetadata();
                }

                return base.Metadata;
            }
        }
        
        public new bool IsFileValid
        {
            get
            {
                // Compliment Base checks with file extension checks
                return base.IsFileValid && this.ImageType == GenericPhotoEnums.ImageTypes.Jpeg;
            }
        }

        public void ReadMetadata()
        {
            if (this.IsFileValid)
            {
                if (this.HandleExceptions)
                {
                    try
                    {
                        this.UnhandledReadMetadata();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error reading Metadata: " + this.FileName, e);
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
                    throw new Exception("File does not exist or is not valid: " + this.FileName);
                }
            }
        }

        public void SaveMetadata()
        {
            if (this.IsFileValid)
            {
                if (this.HandleExceptions)
                {
                    try
                    {
                        this.UnhandledSaveMetadata();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error saving metadata: " + this.FileName, e);
                    }
                }
                else
                {
                    this.UnhandledSaveMetadata();
                }
            }
            else
            {
                if (this.HandleExceptions)
                {
                    throw new Exception("File does not exist or is not valid: " + this.FileName);
                }
            }
        }

        public List<string> CompareMetadataToFileMetadata()
        {
            if (!this.IsFileValid)
            {
                throw new Exception("File does not exist or is not valid: " + this.FileName);
            }

            List<string> changes;

            if (this.HandleExceptions)
            {
                try
                {
                    changes = this.UnhandledCompare();
                }
                catch (Exception e)
                {
                    throw new Exception("Error reading metadata: " + this.FileName, e);
                }
            }
            else
            {
                changes = this.UnhandledCompare();
            }

            return changes;
        }

        private List<string> UnhandledCompare()
        {
            List<string> changes;

            // Read existing metadata in Using block to force garbage collection
            using (WpfMetadata metadataInFile = new WpfMetadata())
            {
                // Read Metadata from File
                metadataInFile.BitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.FileName);

                // Compate Metadata
                IPhotoMetadataTools.CompareMetadata(this.Metadata, metadataInFile, out changes);
            }

            return changes;
        }

        private void UnhandledReadMetadata()
        {
            // Read Photo Metadata
            base.Metadata = WpfFileManager.ReadPhotoMetadata(this.FileName);
        }

        private void UnhandledSaveMetadata()
        {
            // Read existing metadata in Using block to force garbage collection
            using (WpfMetadata metadataInFile = new WpfMetadata())
            {
                // Read Metadata from File
                metadataInFile.BitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.FileName);

                // Copy JpgMetadata across to file Metadata
                IPhotoMetadataTools.CopyMetadata(base.Metadata, metadataInFile);

                // Save
                 WpfFileManager.WriteBitmapMetadata(this.FileName, metadataInFile.BitmapMetadata);
            }
        }
    }
}
