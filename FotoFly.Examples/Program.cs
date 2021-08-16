namespace Fotofly.Examples
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows.Media.Imaging;

    using Fotofly.UnitTests;
	using Fotofly.BitmapMetadataTools;
	using System.IO;

    public class Program
    {
        private static string testPhotoPath = @"..\..\..\~Sample Files\JpgPhotos\";

        [STAThread]
        public static void Main(string[] args)
        {
			//foreach (string fileName in Directory.GetFiles(@"..\..\..\~Sample Files\IptcPhotos", "*.jpg"))
			//{
			//    using (WpfFileManager wpfFileManager = new WpfFileManager(fileName))
			//    {
			//        MetadataDump metadataDump = new MetadataDump(wpfFileManager.BitmapMetadata);

			//        //foreach (var x in metadataDump.PropertyList)
			//        //    Console.WriteLine(x.Query + "\t" + x.Value);

			//        foreach (var s in metadataDump.StringList)
			//            Console.WriteLine(s);

			//        //// Check total count
			//        //Assert.AreEqual<int>(metadataDump.StringList.Count, 187);
			//    }
			//}

			//return;

            // Wpf Examples:
            // WpfFileManager uses WPF to read and write BitmapMetadata in a valid jpg file
            // BitmapMetadataHelper provides Extension Methods on top of BitmapMetadata
            // WpfMetadata provides additional properties that are not in BitmapMetadata
            // ExifQueries, XmpQueries, IptcQueries provides all the queries for common metadata properties
            BitmapMetadataExamples.ReadMetadata(Program.testPhotoPath + TestPhotos.Regions0);
            BitmapMetadataExamples.WriteMetadata(Program.testPhotoPath + TestPhotos.Regions0);
            BitmapMetadataExamples.CreateWLPGRegions(Program.testPhotoPath + TestPhotos.Regions0);
            BitmapMetadataExamples.UpdateWLPGRegions(Program.testPhotoPath + TestPhotos.Regions0);
            BitmapMetadataExamples.ReadGpsAltitude(Program.testPhotoPath + TestPhotos.GeotaggedExif1);
            BitmapMetadataExamples.ReadGpsLatitude(Program.testPhotoPath + TestPhotos.GeotaggedExif1);
            BitmapMetadataExamples.ReadIPTCAddress(Program.testPhotoPath + TestPhotos.GeotaggedExif1);
            BitmapMetadataExamples.RemoveIPTCAddres(Program.testPhotoPath + TestPhotos.GeotaggedExif1);

            // JpgPhoto Examples:
            // JpgPhoto for managing Jpg files and their metadata
            // JpgMetadata provides properties for various metadata properties
            JpgPhotoExamples.ReadMetadata(Program.testPhotoPath + TestPhotos.Regions1);
            JpgPhotoExamples.WriteMetadata(Program.testPhotoPath + TestPhotos.Regions1);
            JpgPhotoExamples.AddTag(Program.testPhotoPath + TestPhotos.Regions1);
            JpgPhotoExamples.AddRegion(Program.testPhotoPath + TestPhotos.Regions1);
            JpgPhotoExamples.AddGpsCoor(Program.testPhotoPath + TestPhotos.Regions1);
            JpgPhotoExamples.GenerateFileNames(Program.testPhotoPath + TestPhotos.Regions1);
        }
    }
}
