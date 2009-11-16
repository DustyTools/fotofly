// <copyright file="WpfFileManipulator.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>Class for manipulating images</summary>
namespace FotoFly
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    public class WpfFileManipulator
    {
        public void CopyImageAndResize(string sourceFileName, string destinationFileName, int newMaxDimension)
        {
            // Load source file
            using (Image sourceImage = Image.FromFile(sourceFileName))
            {
                int destWidth = 0;
                int destHeight = 0;

                // Resize based on portrait\landscape
                // Multiple denominate by 1.0 to ensure we get decimal places
                if (sourceImage.Width < sourceImage.Height)
                {
                    // Calculate new Width, use Max as Height
                    destHeight = newMaxDimension;
                    destWidth = Convert.ToInt32(sourceImage.Width * (newMaxDimension * 1.0 / sourceImage.Height));
                }
                else
                {
                    // Calculate new newHeight, use Max as Width
                    destHeight = Convert.ToInt32(sourceImage.Height * (newMaxDimension * 1.0 / sourceImage.Width));
                    destWidth = newMaxDimension;
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
    }
}
