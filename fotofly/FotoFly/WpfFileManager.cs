namespace FotoFly
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class WpfFileManager
    {
        public static readonly uint PaddingAmount = 5120;

        // XMP Types
        public BitmapMetadata Read(string file)
        {
            // The Metadata we'll be returning
            BitmapMetadata bitmapMetadata;

            // Check file exists and is a JPG
            this.ValidateFileIsJpg(file);

            // Open the file
            try
            {
                BitmapCreateOptions createOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;

                using (Stream sourceStream = File.Open(file, FileMode.Open, FileAccess.Read))
                {
                    BitmapDecoder bitmapDecoder = BitmapDecoder.Create(sourceStream, createOptions, BitmapCacheOption.OnLoad);

                    // Check the contents of the file is valid
                    if (bitmapDecoder.Frames[0] != null && bitmapDecoder.Frames[0].Metadata != null)
                    {
                        bitmapMetadata = bitmapDecoder.Frames[0].Metadata.Clone() as BitmapMetadata;
                    }
                    else
                    {
                        throw new Exception("No Frames of Metadata in the file:\n\n" + file);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to read the file:\n\n", e);
            }

            return bitmapMetadata;
        }

        public BitmapMetadata Read(string file, bool openForEditing)
        {
            // Validate the threading model
            this.ValidateThreadingModel();

            // The Metadata we'll be returning
            BitmapMetadata bitmapMetadata = this.Read(file);

            // Return the metadata if it's not going to be edited
            if (!openForEditing)
            {
                return this.Read(file);
            }

            // Ensure the metadata has the right padding in place for new data
            if (Convert.ToInt32(bitmapMetadata.GetQuery(ExifQueries.Padding)) < WpfFileManager.PaddingAmount
                || Convert.ToInt32(bitmapMetadata.GetQuery(XmpQueries.Padding)) < WpfFileManager.PaddingAmount
                || Convert.ToInt32(bitmapMetadata.GetQuery(IptcQueries.Padding)) < WpfFileManager.PaddingAmount)
            {
                this.AddMetadataPadding(bitmapMetadata);
            }

            return bitmapMetadata;
        }

        public void Write(string outputFile, BitmapMetadata bitmapMetadata, string sourceFileForImage)
        {
            // Open Source File
            using (Stream sourceStream = File.Open(sourceFileForImage, FileMode.Open, FileAccess.ReadWrite))
            {
                BitmapCreateOptions createOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;
                BitmapDecoder bitmapDecoder = BitmapDecoder.Create(sourceStream, createOptions, BitmapCacheOption.OnLoad);

                JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
                jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapDecoder.Frames[0], bitmapDecoder.Frames[0].Thumbnail, bitmapMetadata, bitmapDecoder.Frames[0].ColorContexts));

                // Open Output File
                using (Stream outputStream = File.Open(outputFile, FileMode.Create, FileAccess.ReadWrite))
                {
                    jpegBitmapEncoder.Save(outputStream);
                }
            }
        }

        public void Write(string outputFile, BitmapMetadata bitmapMetadata)
        {
            this.Write(outputFile, bitmapMetadata, 3);
        }

        public void Write(string outputFile, BitmapMetadata bitmapMetadata, int retryCount)
        {
            // Check file exists and is a valid jpg\jpeg file
            this.ValidateFileIsJpg(outputFile);

            // Validate Threading Model
            this.ValidateThreadingModel();

            // Source file is is used as source of the the image & thumbnail for the new file
            string sourceFile = outputFile.ToLower().Replace(".jpg", ".temp");

            // Try saving the file as needed
            bool fileSaved = false;

            while (true)
            {
                if (fileSaved || retryCount == 0)
                {
                    break;
                }

                // Copy file so we have a source file
                try
                {
                    if (File.Exists(sourceFile))
                    {
                        File.Delete(sourceFile);
                    }

                    File.Move(outputFile, sourceFile);
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to create the backup file.", e);
                }

                try
                {
                    this.Write(outputFile, bitmapMetadata, sourceFile);

                    File.Delete(sourceFile);

                    fileSaved = true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());

                    fileSaved = false;

                    retryCount--;
                }

                // Save file failed so restore files to original location
                if (!fileSaved)
                {
                    try
                    {
                        File.Delete(outputFile);
                        File.Move(sourceFile, outputFile);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Unable to recover from failed save\n" + e);
                    }
                }
            }

            if (!fileSaved)
            {
                throw new Exception("Unable to save the file:\n\n" + outputFile);
            }
        }

        private void ValidateFileIsJpg(string file)
        {
            FileInfo fileInfo = new FileInfo(file);

            if (!fileInfo.Exists)
            {
                throw new Exception("File does not exist: " + fileInfo.FullName);
            }
            
            if (!Regex.IsMatch(fileInfo.Extension, ".jpg", RegexOptions.IgnoreCase))
            {
                throw new Exception(@"File is not a jpg: " + fileInfo.FullName);
            }
        }

        private void ValidateThreadingModel()
        {
            // TODO: Don't need this check for Win 7 or Vista with the Platform Update Package (KB971644
            // Try changing to STA
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
            Thread.CurrentThread.IsBackground = false;

            // Exception if the thread apartment state is not valid
            // https://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=2192976&SiteID=1
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                throw new Exception("The current thread is not ApartmentState.STA");
            }
        }

        private void AddMetadataPadding(BitmapMetadata bitmapMetadata)
        {
            // Ensure there's enough EXIF Padding
            if (Convert.ToInt32(bitmapMetadata.GetQuery(ExifQueries.Padding)) < WpfFileManager.PaddingAmount)
            {
                bitmapMetadata.SetQuery(ExifQueries.Padding, WpfFileManager.PaddingAmount);
            }

            // Ensure there's enough XMP Padding
            if (Convert.ToInt32(bitmapMetadata.GetQuery(XmpQueries.Padding)) < WpfFileManager.PaddingAmount)
            {
                bitmapMetadata.SetQuery(XmpQueries.Padding, WpfFileManager.PaddingAmount);
            }

            // Ensure there's enough IFD Padding
            if (Convert.ToInt32(bitmapMetadata.GetQuery(IptcQueries.Padding)) < WpfFileManager.PaddingAmount)
            {
                bitmapMetadata.SetQuery(IptcQueries.Padding, WpfFileManager.PaddingAmount);
            }
        }
    }
}
