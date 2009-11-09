namespace FotoFly.Examples
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows.Media.Imaging;

    using FotoFly.UnitTests;

    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Wpf Examples, consisting of four classes
            // WpfFileManager uses WPF to read and write BitmapMetadata in a valid jpg file
            // WpfBitmapMetadataExtender provides additional methods on top of BitmapMetadata to set and retrieve metadata
            // WpfMetadata provides additional properties that use WpfBitmapMetadataExtender
            // WpfProperties provides all the queries for common metadata properties
            BitmapMetadataExamples.ReadMetadata(TestPhotos.Regions0);
            BitmapMetadataExamples.WriteMetadata(TestPhotos.Regions0);

            BitmapMetadataExamples.CreateWLPGRegions(TestPhotos.Regions0);
            BitmapMetadataExamples.UpdateWLPGRegions(TestPhotos.Regions0);

            BitmapMetadataExamples.ReadGpsAltitude(TestPhotos.Geotagged);
            BitmapMetadataExamples.ReadGpsLatitude(TestPhotos.Geotagged);
            BitmapMetadataExamples.ReadIPTCAddres(TestPhotos.Geotagged);
            BitmapMetadataExamples.RemoveIPTCAddres(TestPhotos.Geotagged);

            // JpgPhoto Examples, consisting of two classes, which sit on top of the Wpf classes above
            // JpgPhoto for managing Jpg files and their metadata
            // JpgMetadata provides properties for various metadata properties
            JpgPhotoExamples.ReadMetadata(TestPhotos.Regions1);
            JpgPhotoExamples.WriteMetadata(TestPhotos.Regions1);
            JpgPhotoExamples.AddTag(TestPhotos.Regions1);
            JpgPhotoExamples.AddRegion(TestPhotos.Regions1);
            JpgPhotoExamples.AddGpsCoor(TestPhotos.Regions1);
            JpgPhotoExamples.GenerateFileNames(TestPhotos.Regions1);
        }
    }
}
