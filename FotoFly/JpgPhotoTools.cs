// <copyright file="JpgPhotoTools.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-01-07</date>
// <summary>JpgPhotoTools</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Media.Imaging;
    using System.Xml.Serialization;

    using Fotofly.BitmapMetadataTools;

    public static class JpgPhotoTools
    {
        private static int metadataBackupImageMaxDimension = 100;

        /// <summary>
        /// Creates a small jpeg image containing a backup of all the metadata
        /// </summary>
        /// <param name="fileName">Filename of the file to create</param>
        /// <param name="overwrite">If true creates a new file, if false updates an existing file</param>
        public static void CreateMetadataBackup(JpgPhoto photo, string destinationFileName, bool overwrite)
        {
            if (!File.Exists(photo.FileFullName))
            {
                throw new Exception("Source file does not exist: " + photo.FileFullName);
            }
            else if (File.Exists(destinationFileName) && overwrite)
            {
                File.Delete(destinationFileName);
            }

            // Check to see if we need to create a new image
            if (!File.Exists(destinationFileName))
            {
                // Load source file
                using (Image sourceImage = Image.FromFile(photo.FileFullName))
                {
                    int destWidth = 0;
                    int destHeight = 0;

                    // Resize based on portrait\landscape
                    // Multiple denominate by 1.0 to ensure we get decimal places
                    if (sourceImage.Width < sourceImage.Height)
                    {
                        // Calculate new Width, use Max as Height
                        destHeight = JpgPhotoTools.metadataBackupImageMaxDimension;
                        destWidth = Convert.ToInt32(sourceImage.Width * (JpgPhotoTools.metadataBackupImageMaxDimension * 1.0 / sourceImage.Height));
                    }
                    else
                    {
                        // Calculate new newHeight, use Max as Width
                        destHeight = Convert.ToInt32(sourceImage.Height * (JpgPhotoTools.metadataBackupImageMaxDimension * 1.0 / sourceImage.Width));
                        destWidth = JpgPhotoTools.metadataBackupImageMaxDimension;
                    }

                    // Create the destination Bitmap
                    Image destinationImage = new Bitmap(destWidth, destHeight, sourceImage.PixelFormat);

                    // Create a graphics manipulate and paste in the source file
                    Graphics destinationGraphic = Graphics.FromImage(destinationImage);
                    destinationGraphic.CompositingQuality = CompositingQuality.HighQuality;
                    destinationGraphic.SmoothingMode = SmoothingMode.HighQuality;
                    destinationGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    destinationGraphic.DrawImage(sourceImage, new Rectangle(0, 0, destWidth, destHeight));

                    // Save
                    destinationImage.Save(destinationFileName, ImageFormat.Jpeg);
                }
            }

            // Update the new files metadata
            WpfFileManager.CopyBitmapMetadata(photo.FileFullName, destinationFileName);
        }
    }
}
