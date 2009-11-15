// <copyright file="WpfFileManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>Class for reading and writing BitmapMetadata</summary>
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
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static class WpfFileManager
    {
        public static readonly uint PaddingAmount = 5120;

        public static PhotoMetadata ReadPhotoMetadata(string file)
        {
            // The Metadata we'll be returning
            PhotoMetadata photoMetadata = new PhotoMetadata();

            try
            {
                BitmapCreateOptions createOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;

                // Open the stream, readonly
                using (Stream sourceStream = File.Open(file, FileMode.Open, FileAccess.Read))
                {
                    // Create a decoder with no cache options set
                    BitmapDecoder bitmapDecoder = BitmapDecoder.Create(sourceStream, createOptions, BitmapCacheOption.None);

                    // Check the contents of the file is valid
                    if (bitmapDecoder.Frames[0] != null && bitmapDecoder.Frames[0].Metadata != null)
                    {
                        // Create a new WpfMetadata class that exposes all the right fields
                        WpfMetadata wpfMetadata = new WpfMetadata(bitmapDecoder.Frames[0].Metadata as BitmapMetadata);

                        // Copy the common metadata across using reflection tool
                        IPhotoMetadataTools.CopyMetadata(wpfMetadata, photoMetadata);

                        // Manually copy across ImageHeight & ImageWidth if they are not set in metadata
                        // This should be pretty rare but can happen if the image has been resized or manipulated and the metadata not copied across
                        if (photoMetadata.ImageHeight == 0 || photoMetadata.ImageWidth == 0)
                        {
                            photoMetadata.ImageHeight = bitmapDecoder.Frames[0].PixelHeight;
                            photoMetadata.ImageWidth = bitmapDecoder.Frames[0].PixelWidth;
                        }
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

            return photoMetadata;
        }

        public static BitmapMetadata ReadBitmapMetadata(string file)
        {
            // The Metadata we'll be returning
            BitmapMetadata bitmapMetadata;

            try
            {
                // Open the stream, readonly
                BitmapCreateOptions createOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;

                // Create a decoder, cache all content on load because we'll close the stream
                using (Stream sourceStream = File.Open(file, FileMode.Open, FileAccess.Read))
                {
                    // Create a Bitmap Decoder, loading all metadata on load
                    BitmapDecoder bitmapDecoder = BitmapDecoder.Create(sourceStream, createOptions, BitmapCacheOption.OnLoad);

                    // Check the contents of the file is valid
                    if (bitmapDecoder.Frames[0] != null && bitmapDecoder.Frames[0].Metadata != null)
                    {
                        // Grab the metadata
                        // If BitmapCacheOption.None then the clone will be empty of any metadata
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

        public static BitmapMetadata ReadBitmapMetadata(string file, bool openForEditing)
        {
            // The Metadata we'll be returning
            BitmapMetadata bitmapMetadata = WpfFileManager.ReadBitmapMetadata(file);

            // Return the metadata if it's not going to be edited
            if (!openForEditing)
            {
                return WpfFileManager.ReadBitmapMetadata(file);
            }

            // Validate the threading model
            WpfFileManager.ValidateThreadingModel();

            // The file must be JPG because the TIFF padding queries are different
            WpfFileManager.ValidateFileIsJpeg(file);

            // Ensure the metadata has the right padding in place for new data
            WpfFileManager.AddMetadataPadding(bitmapMetadata);

            return bitmapMetadata;
        }

        public static void WriteBitmapMetadata(string outputFile, BitmapMetadata bitmapMetadata, string sourceFileForImage)
        {
            // Open Source File
            using (Stream sourceStream = File.Open(sourceFileForImage, FileMode.Open, FileAccess.ReadWrite))
            {
                BitmapCreateOptions createOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;

                // Create the decoder, caching is not needed because file remains open
                BitmapDecoder bitmapDecoder = BitmapDecoder.Create(sourceStream, createOptions, BitmapCacheOption.None);

                JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
                jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapDecoder.Frames[0], bitmapDecoder.Frames[0].Thumbnail, bitmapMetadata, bitmapDecoder.Frames[0].ColorContexts));

                // Open Output File
                using (Stream outputStream = File.Open(outputFile, FileMode.Create, FileAccess.ReadWrite))
                {
                    jpegBitmapEncoder.Save(outputStream);
                }
            }
        }

        public static void WriteBitmapMetadata(string outputFile, BitmapMetadata bitmapMetadata)
        {
            WpfFileManager.WriteBitmapMetadata(outputFile, bitmapMetadata, 3);
        }

        public static void WriteBitmapMetadata(string outputFile, BitmapMetadata bitmapMetadata, int retryCount)
        {
            // Check file exists and is a valid jpg\jpeg file
            WpfFileManager.ValidateFileIsJpeg(outputFile);

            // Validate Threading Model
            WpfFileManager.ValidateThreadingModel();

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
                    WpfFileManager.WriteBitmapMetadata(outputFile, bitmapMetadata, sourceFile);

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

        public static void CopyBitmapMetadata(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new Exception("SourceFile does not exist: " + sourceFile);
            }
            else if (!File.Exists(destinationFile))
            {
                throw new Exception("DestinationFile does not exist: " + destinationFile);
            }

            BitmapCreateOptions createOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;

            // Create backup of the file so you can read and write to different files
            string tempFile = destinationFile + ".tmp";
            File.Copy(destinationFile, tempFile, true);

            // Open the source file
            using (Stream sourceStream = File.Open(sourceFile, FileMode.Open, FileAccess.Read))
            {
                BitmapDecoder sourceDecoder = BitmapDecoder.Create(sourceStream, createOptions, BitmapCacheOption.None);

                // Check source is has valid frames
                if (sourceDecoder.Frames[0] != null && sourceDecoder.Frames[0].Metadata != null)
                {
                    // Get a clone copy of the metadata
                    BitmapMetadata sourceMetadata = sourceDecoder.Frames[0].Metadata.Clone() as BitmapMetadata;

                    // Open the temp file
                    using (Stream tempStream = File.Open(tempFile, FileMode.Open, FileAccess.Read))
                    {
                        BitmapDecoder tempDecoder = BitmapDecoder.Create(tempStream, createOptions, BitmapCacheOption.None);

                        // Check temp file has valid frames
                        if (tempDecoder.Frames[0] != null && tempDecoder.Frames[0].Metadata != null)
                        {
                            // Open the destination file
                            using (Stream destinationStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite))
                            {
                                // Create a new jpeg frame, replacing the destination metadata with the source
                                BitmapFrame destinationFrame = BitmapFrame.Create(tempDecoder.Frames[0],
                                      tempDecoder.Frames[0].Thumbnail,
                                      sourceMetadata,
                                      tempDecoder.Frames[0].ColorContexts);

                                // Save the file
                                JpegBitmapEncoder destinationEncoder = new JpegBitmapEncoder();
                                destinationEncoder.Frames.Add(destinationFrame);
                                destinationEncoder.Save(destinationStream);
                            }
                        }
                    }
                }
            }

            // Delete the temp file
            File.Delete(tempFile);
        }

        public static void AddMetadataPadding(BitmapMetadata bitmapMetadata)
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

        public static void ValidateFileIsJpeg(string file)
        {
            // TODO: Add code to check the first bits of the file to check it really us a jpeg
            // Something like this
            // if (buf[0]==0xFF && buf[1]==0xD8 && buf[2]==0xFF && buf[3]==0xE0 && buf[6]=='J' && buf[7]=='F' && buf[8]=='I' && buf[9]=='F')
            FileInfo fileInfo = new FileInfo(file);

            if (!fileInfo.Exists)
            {
                throw new Exception("File does not exist: " + fileInfo.FullName);
            }

            if (!Regex.IsMatch(fileInfo.Extension, ".jpg", RegexOptions.IgnoreCase) && !Regex.IsMatch(fileInfo.Extension, ".jpeg", RegexOptions.IgnoreCase))
            {
                throw new Exception(@"File does not have the extension jpg or jpeg: " + fileInfo.FullName);
            }
        }

        private static void ValidateThreadingModel()
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
    }
}
