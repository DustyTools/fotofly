namespace FotoFly.Examples
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows.Media.Imaging;

    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Test files
            string testNoRegion = "Test-NoRegions.jpg";
            string testOneRegion = "Test-OneRegion.jpg";
            string testGeotagged = "Test-Geotagged.jpg";

            // Wpf Examples, consisting of four classes
            // WpfFileManager uses WPF to read and write BitmapMetadata in a valid jpg file
            // WpfBitmapMetadataExtender provides additional methods on top of BitmapMetadata to set and retrieve metadata
            // WpfMetadata provides additional properties that use WpfBitmapMetadataExtender
            // WpfProperties provides all the queries for common metadata properties
            BitmapMetadataExamples.ReadMetadata(testNoRegion);
            BitmapMetadataExamples.WriteMetadata(testNoRegion);

            BitmapMetadataExamples.CreateWLPGRegions(testNoRegion);
            BitmapMetadataExamples.UpdateWLPGRegions(testNoRegion);

            BitmapMetadataExamples.ReadGpsAltitude(testGeotagged);
            BitmapMetadataExamples.ReadGpsLatitude(testGeotagged);
            BitmapMetadataExamples.ReadIPTCAddres(testGeotagged);
            BitmapMetadataExamples.RemoveIPTCAddres(testGeotagged);

            // JpgPhoto Examples, consisting of two classes, which sit on top of the Wpf classes above
            // JpgPhoto for managing Jpg files and their metadata
            // JpgMetadata provides properties for various metadata properties
            JpgPhotoExamples.ReadMetadata(testOneRegion);
            JpgPhotoExamples.WriteMetadata(testOneRegion);
            JpgPhotoExamples.AddTag(testOneRegion);
            JpgPhotoExamples.AddRegion(testOneRegion);
            JpgPhotoExamples.AddGpsCoor(testOneRegion);
            JpgPhotoExamples.GenerateFileNames(testOneRegion);
        }
    }
}
