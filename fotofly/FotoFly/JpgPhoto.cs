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
        public bool HandleExceptions
        {
            get;
            set;
        }
    
        private JpgMetadata metadata;
        private WpfMetadata metadataInFile;

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

        public JpgPhoto()
        {
        }

        public JpgPhoto(string filename)
        {
            this.Filename = filename;
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
            if (this.IsFileValid)
            {
                List<string> changes;

                try
                {
                    WpfFileManager wpfFileManager = new WpfFileManager();

                    BitmapMetadata fileMetadata = wpfFileManager.Read(this.Filename);

                    IImageMetadataTools.CompareMetadata(this.metadata, fileMetadata, out changes);
                }
                catch (Exception e)
                {
                    throw new Exception("Error reading metadata: " + this.Filename, e);
                }

                return changes;
            }
            else
            {
                throw new Exception("File does not exist or is not valid: " + this.Filename);
            }
        }

        private void UnhandledReadMetadata()
        {
            // Get clean JpgMetadata
            this.metadata = new JpgMetadata();

            // Get file metadata
            WpfFileManager wpfFileManager = new WpfFileManager();
            this.metadataInFile = new WpfMetadata(wpfFileManager.Read(this.Filename));

            // Copy across
            IImageMetadataTools.CopyMetadata(this.metadataInFile, this.metadata);
        }

        private void UnhandledSaveMetadata()
        {
            // Copy JpgMetadata across to file Metadata
            IImageMetadataTools.CopyMetadata(this.metadata, this.metadataInFile);

            // Save
            WpfFileManager wpfFileManager = new WpfFileManager();
            wpfFileManager.Write(this.Filename, this.metadataInFile.BitmapMetadata);
        }
    }
}
