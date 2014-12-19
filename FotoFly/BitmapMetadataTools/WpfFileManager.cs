// <copyright file="WpfFileManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>Class for reading and writing BitmapMetadata</summary>
namespace Fotofly.BitmapMetadataTools
{
    using Fotofly.MetadataQueries;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Media.Imaging;

    public class WpfFileManager : IDisposable
    {
        public static bool AlwaysCloneBitmap = false;
        public static readonly uint PaddingAmount = 5120;
        private static BitmapCreateOptions createOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;

        private bool disposed = false;
        private Stream sourceStream;
        private BitmapMetadata bitmapMetadata;
        private BitmapDecoder bitmapDecoder;
        private string sourceFilename;
        private string destinationfilename;
        private bool openForEditing;

        public WpfFileManager(string filename)
        {
            // Read metadata
            this.ReadMetadata(filename, false);
        }

        public WpfFileManager(string filename, bool openForEditing)
        {
            this.ReadMetadata(filename, openForEditing);
        }

        public BitmapMetadata BitmapMetadata
        {
            get
            {
                return this.bitmapMetadata;
            }
        }

        public BitmapDecoder BitmapDecoder
        {
            get
            {
                return this.bitmapDecoder;
            }
        }

        public static void WriteBitmapMetadata(string outputFile, BitmapMetadata bitmapMetadata, string sourceFileForImage)
        {
            // Open Source File
            using (Stream sourceStream = File.Open(sourceFileForImage, FileMode.Open, FileAccess.ReadWrite))
            {
                // Create the decoder, caching is not needed because file remains open
                BitmapDecoder bitmapDecoder = BitmapDecoder.Create(sourceStream, WpfFileManager.createOptions, BitmapCacheOption.None);

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
            string sourceFile = outputFile.ToLower().Replace(".jpg", ".fotoflytmp");

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

            // Create backup of the file so you can read and write to different files
            string tempFile = destinationFile + ".fotoflytemp";
            File.Copy(destinationFile, tempFile, true);

            // Open the source file
            using (Stream sourceStream = File.Open(sourceFile, FileMode.Open, FileAccess.Read))
            {
                BitmapDecoder sourceDecoder = BitmapDecoder.Create(sourceStream, WpfFileManager.createOptions, BitmapCacheOption.None);

                // Check source is has valid frames
                if (sourceDecoder.Frames[0] != null && sourceDecoder.Frames[0].Metadata != null)
                {
                    // Get a clone copy of the metadata
                    BitmapMetadata sourceMetadata = sourceDecoder.Frames[0].Metadata.Clone() as BitmapMetadata;

                    // Open the temp file
                    using (Stream tempStream = File.Open(tempFile, FileMode.Open, FileAccess.Read))
                    {
                        BitmapDecoder tempDecoder = BitmapDecoder.Create(tempStream, WpfFileManager.createOptions, BitmapCacheOption.None);

                        // Check temp file has valid frames
                        if (tempDecoder.Frames[0] != null && tempDecoder.Frames[0].Metadata != null)
                        {
                            // Open the destination file
                            using (Stream destinationStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite))
                            {
                                // Create a new jpeg frame, replacing the destination metadata with the source
                                BitmapFrame destinationFrame = BitmapFrame.Create(tempDecoder.Frames[0], tempDecoder.Frames[0].Thumbnail, sourceMetadata, tempDecoder.Frames[0].ColorContexts);

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

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.Dispose(true);
            }

            // Take yourself off the Finalization queue
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        public void WriteMetadata()
        {
            // Create a new Jpg encoder
            JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
            jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(this.bitmapDecoder.Frames[0], this.bitmapDecoder.Frames[0].Thumbnail, this.bitmapMetadata, this.bitmapDecoder.Frames[0].ColorContexts));

            // Save the output file
            using (Stream outputStream = File.Open(this.destinationfilename, FileMode.Create, FileAccess.ReadWrite))
            {
                jpegBitmapEncoder.Save(outputStream);
            }
        }

        private void ReadMetadata(string filename, bool openForEditing)
        {
            // Save filename
            this.sourceFilename = filename;
            this.destinationfilename = filename;
            this.openForEditing = openForEditing;

            // If open for editing, do additional checks
            if (openForEditing)
            {
                // Validate the threading model
                WpfFileManager.ValidateThreadingModel();

                // The file must be JPG because the TIFF padding queries are different
                WpfFileManager.ValidateFileIsJpeg(filename);

                // Create backup of the file so you can read and write to different files
                string tempFile = filename + ".fotoflytemp";
                File.Copy(filename, tempFile, true);

                // Replace the filename with the temp filename
                this.sourceFilename = tempFile;
            }

            this.ReadMetadata();

            if (openForEditing)
            {
                // Ensure the metadata has the right padding in place for new data
                this.AddMetadataPadding();
            }
        }

        public static BitmapDecoder CreateBitmapDecoder(Stream source)
        {
            return BitmapDecoder.Create(source, WpfFileManager.createOptions, BitmapCacheOption.None);
        }

        public static BitmapMetadata ReadMetadata(Stream stream)
        {
            return ReadMetadata(stream, AlwaysCloneBitmap);
        }

        public static BitmapMetadata ReadMetadata(Stream stream, bool cloneBitmap)
        {
            return ReadMetadata(CreateBitmapDecoder(stream), cloneBitmap);
        }

        public static BitmapMetadata ReadMetadata(BitmapDecoder decoder, bool cloneBitmap)
        {
            // Check the contents of the file is valid
            if (decoder.Frames[0] == null || decoder.Frames[0].Metadata == null)
            {
                return null;
            }

            // Grab the metadata
            if (cloneBitmap)
            {
                // Clone so we have an unfrozen object
                return decoder.Frames[0].Metadata.Clone() as BitmapMetadata;
            }

            return decoder.Frames[0].Metadata as BitmapMetadata;
        }

        private void ReadMetadata()
        {
            // Create a decoder, cache all content on load because we'll close the stream
            this.sourceStream = File.Open(this.sourceFilename, FileMode.Open, FileAccess.Read);

            // Create a Bitmap Decoder
            this.bitmapDecoder = CreateBitmapDecoder(this.sourceStream);

            this.bitmapMetadata = ReadMetadata(this.bitmapDecoder, this.openForEditing || WpfFileManager.AlwaysCloneBitmap);
            if (this.bitmapMetadata == null)
            {
                throw new Exception("No Frames of Metadata in the file:\n\n" + this.sourceFilename);
            }
        }


        private void AddMetadataPadding()
        {
            // Ensure there's enough EXIF Padding
            if (this.BitmapMetadata.GetQuery<UInt32>(ExifQueries.Padding.Query) < WpfFileManager.PaddingAmount)
            {
                this.BitmapMetadata.SetQuery(ExifQueries.Padding.Query, WpfFileManager.PaddingAmount);
            }

            // Ensure there's enough XMP Padding
            if (this.BitmapMetadata.GetQuery<UInt32>(XmpCoreQueries.Padding.Query) < WpfFileManager.PaddingAmount)
            {
                this.BitmapMetadata.SetQuery(XmpCoreQueries.Padding.Query, WpfFileManager.PaddingAmount);
            }

            // Ensure there's enough IPTC Padding
            if (this.BitmapMetadata.GetQuery<UInt32>(IptcQueries.Padding.Query) < WpfFileManager.PaddingAmount)
            {
                this.BitmapMetadata.SetQuery(IptcQueries.Padding.Query, WpfFileManager.PaddingAmount);
            }
        }

        private static void ValidateFileIsJpeg(string file)
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

            fileInfo = null;
        }

        private static void ValidateThreadingModel()
        {
            // TODO: Don't need this check for Win 7 or Vista with the Platform Update Package (KB971644)
            // Exception if the thread apartment state is not valid
            // https://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=2192976&SiteID=1

            // Try changing to STA
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
            Thread.CurrentThread.IsBackground = false;

            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                throw new Exception("The current thread is not ApartmentState.STA");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            // Force Garbage ObjCollection
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Dispose of everything
            this.bitmapMetadata = null;
            this.bitmapDecoder = null;
            this.sourceStream.Close();
            this.sourceStream.Dispose();

            // If open for editing, delete the temporary file
            if (this.openForEditing && File.Exists(this.sourceFilename))
            {
                File.Delete(this.sourceFilename);
            }

            this.disposed = true;
        }

        /* Broken Static Read methods
         * A bug in WIC stops these from working because non-ASCII characters get manggled
        public static BitmapMetadata ReadBitmapMetadata(string file)
        {
            return WpfFileManager.ReadBitmapMetadata(file, false);
        }

        public static BitmapMetadata ReadBitmapMetadata(string file, bool openForEditing)
        {
            // The Metadata we'll be returning
            BitmapMetadata bitmapMetadata;

            try
            {
                // Create a decoder, cache all content on load because we'll close the stream
                using (Stream sourceStream = File.Open(file, FileMode.Open, FileAccess.Read))
                {
                    // Create a Bitmap Decoder, loading all metadata on load
                    // This one is used to load the vast majority of data
                    BitmapDecoder bitmapDecoder = BitmapDecoder.Create(sourceStream, WpfFileManager.createOptions, BitmapCacheOption.OnDemand);

                    // Check the contents of the file is valid
                    if (bitmapDecoder.Frames[0] != null && bitmapDecoder.Frames[0].Metadata != null)
                    {
                        // Grab the metadata
                        // If BitmapCacheOption.None then the clone will be empty of any metadata
                        bitmapMetadata = bitmapDecoder.Frames[0].Metadata.CloneCurrentValue() as BitmapMetadata;
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

            // Return the metadata if it's not going to be edited
            if (!openForEditing)
            {
                return bitmapMetadata;
            }

            // Validate the threading model
            WpfFileManager.ValidateThreadingModel();

            // The file must be JPG because the TIFF padding queries are different
            WpfFileManager.ValidateFileIsJpeg(file);

            // Ensure the metadata has the right padding in place for new data
            WpfFileManager.AddMetadataPadding(bitmapMetadata);

            return bitmapMetadata;
        }
        */
    }
}