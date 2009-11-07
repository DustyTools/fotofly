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

    public class JpgPhoto
    {
        private JpgMetadata metadata;

        public JpgPhoto()
        {
        }

        public JpgPhoto(string filename)
        {
            this.Filename = filename;
        }

        public bool HandleExceptions
        {
            get;
            set;
        }
    
        public JpgMetadata Metadata
        {
            get
            {
                return this.metadata;
            }
        }
        
        public string Filename
        {
            get;
            set;
        }

        public bool IsFileValid
        {
            get
            {
                return !string.IsNullOrEmpty(this.Filename) && File.Exists(this.Filename) && new FileInfo(this.Filename).Extension.ToLower() == ".jpg";
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
                        throw new Exception("Error reading Metadata: " + this.Filename, e);
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
                    throw new Exception("File does not exist or is not valid: " + this.Filename);
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
                        throw new Exception("Error saving metadata: " + this.Filename, e);
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
                    throw new Exception("File does not exist or is not valid: " + this.Filename);
                }
            }
        }

        public List<string> CompareMetadataToFileMetadata()
        {
            if (!this.IsFileValid)
            {
                throw new Exception("File does not exist or is not valid: " + this.Filename);
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
                    throw new Exception("Error reading metadata: " + this.Filename, e);
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
                metadataInFile.BitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.Filename);

                // Compate Metadata
                IPhotoMetadataTools.CompareMetadata(this.metadata, metadataInFile, out changes);
            }

            return changes;
        }

        private void UnhandledReadMetadata()
        {
            // Create a new instance of JpgMetadata
            this.metadata = new JpgMetadata();

            // Read existing metadata in Using block to force garbage collection
            using (WpfMetadata metadataInFile = new WpfMetadata())
            {
                // Read Metadata from File
                metadataInFile.BitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.Filename);

                // Copy in file metadata across to jpgmetadata class
                IPhotoMetadataTools.CopyMetadata(metadataInFile, this.metadata);
            }
        }

        private void UnhandledSaveMetadata()
        {
            // Read existing metadata in Using block to force garbage collection
            using (WpfMetadata metadataInFile = new WpfMetadata())
            {
                // Read Metadata from File
                metadataInFile.BitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.Filename);

                // Copy JpgMetadata across to file Metadata
                IPhotoMetadataTools.CopyMetadata(this.metadata, metadataInFile);

                // Save
                 WpfFileManager.WriteBitmapMetadata(this.Filename, metadataInFile.BitmapMetadata);
            }
        }
    }
}
