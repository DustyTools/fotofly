namespace Fotofly.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Fotofly;
    using Fotofly.Geotagging;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Fotofly.BitmapMetadataTools;
    using Fotofly.MetadataProviders;

    [TestClass]
    public class BitmapMetadataUnitTests
    {
        private string samplePhotosFolder = @"..\..\..\~Sample Files\JpgPhotos\";

        public BitmapMetadataUnitTests()
        {
        }

        /// <summary>
        /// Check MetadataDump
        /// </summary>
        [TestMethod]
        public void ReadMetadataDump()
        {
            MetadataDump metadataDump = new MetadataDump(WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.XmpTiff));

            // Check total count
            Assert.AreEqual<int>(metadataDump.StringList.Count, 186);
        }

        /// <summary>
        /// Check Xmp Xap Schema Metadata provider
        /// </summary>
        [TestMethod]
        public void ReadXmpXapMetadata()
        {
            BitmapMetadata bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.XmpXap);
            XmpXapProvider xmpXapProvider = new XmpXapProvider(bitmapMetadata);

            Assert.AreEqual<string>(xmpXapProvider.CreatorTool, "Tassography PhotoManager");
            Assert.AreEqual<MetadataEnums.Rating>(xmpXapProvider.Rating, MetadataEnums.Rating.FourStar);
        }

        /// <summary>
        /// Check Xmp Tiff Schema Metadata provider
        /// </summary>
        [TestMethod]
        public void ReadXmpTiffMetadata()
        {
            BitmapMetadata bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.XmpTiff);
            XmpTiffProvider xmpTiffProvider = new XmpTiffProvider(bitmapMetadata);

            Assert.AreEqual<string>(xmpTiffProvider.Make, "Canon");
            Assert.AreEqual<string>(xmpTiffProvider.Model, "Canon PowerShot SD790 IS");
            Assert.AreEqual<string>(xmpTiffProvider.Orientation, "6");
            Assert.AreEqual<string>(xmpTiffProvider.Software, "Tassography PhotoManager");
        }

        /// <summary>
        /// Check XmpForExif Metadata provider
        /// </summary>
        [TestMethod]
        public void ReadXmpExifMetadata()
        {
            BitmapMetadata bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.XmpExif);
            XmpExifProvider xmpExifProvider = new XmpExifProvider(bitmapMetadata);

            Assert.AreEqual<string>(xmpExifProvider.DateTimeOriginal, "2008-05-04T20:29:00Z");
            Assert.AreEqual<string>(xmpExifProvider.FocalLength, "6200/1000");
            Assert.AreEqual<string>(xmpExifProvider.MaxApertureValue, "95/32");
            Assert.AreEqual<string>(xmpExifProvider.FNumber, "80/10");
            Assert.AreEqual<string>(xmpExifProvider.Aperture, "192/32");
            Assert.AreEqual<string>(xmpExifProvider.ExposureTime, "1/320");
            Assert.AreEqual<string>(xmpExifProvider.ShutterSpeedValue, "266/32");
            Assert.AreEqual<string>(xmpExifProvider.ExposureBiasValue, "0/3");
            Assert.AreEqual<string>(xmpExifProvider.ExifVersion, "0220");
            Assert.AreEqual<string>(xmpExifProvider.MeteringMode, "5");
            Assert.AreEqual<string>(xmpExifProvider.ISOSpeed, "80");
            Assert.AreEqual<string>(xmpExifProvider.WhiteBalance, "0");
            Assert.AreEqual<string>(xmpExifProvider.DigitalZoomRatio, "3648/3648");
        }

        /// <summary>
        /// Check Fotofly metadata provider
        /// </summary>
        [TestMethod]
        public void WriteFotoflyMetadata()
        {
            BitmapMetadata bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.UnitTest1);

            DateTime testDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            XmpFotoflyProvider xmpFotoflyProvider = new XmpFotoflyProvider(bitmapMetadata);
            xmpFotoflyProvider.AccuracyOfGps = GpsPosition.Accuracies.Region;
            xmpFotoflyProvider.AddressOfGps = new Address("United States/California/San Francisco/Mission Street");
            xmpFotoflyProvider.AddressOfGpsLookupDate = testDate;
            xmpFotoflyProvider.AddressOfGpsSource = "Bing Maps for Enterprise";
            xmpFotoflyProvider.LastEditDate = testDate;
            xmpFotoflyProvider.OriginalCameraDate = testDate;
            xmpFotoflyProvider.OriginalCameraFilename = "img_123.jpg";
            xmpFotoflyProvider.UtcDate = testDate;
            xmpFotoflyProvider.UtcOffset = 5;

            WpfFileManager.WriteBitmapMetadata(this.samplePhotosFolder + TestPhotos.UnitTestX, bitmapMetadata, this.samplePhotosFolder + TestPhotos.UnitTest1);

            bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.UnitTestX);

            xmpFotoflyProvider = new XmpFotoflyProvider(bitmapMetadata);

            Assert.AreEqual<GpsPosition.Accuracies>(xmpFotoflyProvider.AccuracyOfGps, GpsPosition.Accuracies.Region);
            Assert.AreEqual<Address>(xmpFotoflyProvider.AddressOfGps, new Address("United States/California/San Francisco/Mission Street"));
            Assert.AreEqual<DateTime>(xmpFotoflyProvider.AddressOfGpsLookupDate, testDate);
            Assert.AreEqual<string>(xmpFotoflyProvider.AddressOfGpsSource, "Bing Maps for Enterprise");
            Assert.AreEqual<DateTime>(xmpFotoflyProvider.LastEditDate, testDate);
            Assert.AreEqual<DateTime>(xmpFotoflyProvider.OriginalCameraDate, testDate);
            Assert.AreEqual<string>(xmpFotoflyProvider.OriginalCameraFilename, "img_123.jpg");
            Assert.AreEqual<DateTime>(xmpFotoflyProvider.UtcDate, testDate);
            Assert.AreEqual<double>(xmpFotoflyProvider.UtcOffset.Value, 5);

            if (File.Exists(this.samplePhotosFolder + TestPhotos.UnitTestX))
            {
                File.Delete(this.samplePhotosFolder + TestPhotos.UnitTestX);
            }
        }

        /// <summary>
        /// Check Xmp Gps metadata provider
        /// </summary>
        [TestMethod]
        public void ReadExifGpsMetadata()
        {
            // Expected 37° 48.41667 N 122° 25.38333 W
            GpsProvider gpsProvider = new GpsProvider(WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.GeotaggedExif1));

            Assert.AreEqual<GpsCoordinate>(gpsProvider.Latitude, new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 37, 48.41667));
            Assert.AreEqual<GpsCoordinate>(gpsProvider.Longitude, new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 122, 25.38333));
            Assert.AreEqual<string>(gpsProvider.DateTimeStamp.ToString("u"), "2009-10-10 21:46:24Z");
            Assert.AreEqual<Double>(gpsProvider.Altitude, -17.464);
            Assert.AreEqual<GpsPosition.Dimensions>(gpsProvider.MeasureMode, GpsPosition.Dimensions.ThreeDimensional);
            Assert.AreEqual<string>(gpsProvider.VersionID, "2200");
        }
        
        /// <summary>
        /// Check Xmp Gps metadata provider
        /// </summary>
        [TestMethod]
        public void ReadXmpGpsMetadata()
        {
            // Expected 51° 55.6784 N 4° 26.6922 E
            XmpExifProvider xmpExifProvider = new XmpExifProvider(WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.GeotaggedXmp1));

            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 51, 55.6784));
            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 4, 26.6922));
            Assert.AreEqual<double>(xmpExifProvider.GpsAltitude, Double.NaN);
            Assert.AreEqual<string>(xmpExifProvider.GpsVersionID, "2.2.0.0");
            Assert.AreEqual<string>(xmpExifProvider.GpsDateTimeStamp.ToString("u"), "2010-02-01 13:02:34Z");

            // Expected 51° 55.7057 N 4° 26.9547 E
            xmpExifProvider = new XmpExifProvider(WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.GeotaggedXmp2));

            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 51, 55.7057));
            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 4, 26.9547));
            Assert.AreEqual<string>(xmpExifProvider.GpsDateTimeStamp.ToString("u"), "2010-02-01 13:25:12Z");

            // Expected 51° 55.6767 N 4° 27.2393 E
            xmpExifProvider = new XmpExifProvider(WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.GeotaggedXmp3));

            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 51, 55.6767));
            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 4, 27.2393));
            Assert.AreEqual<string>(xmpExifProvider.GpsDateTimeStamp.ToString("u"), "2010-02-01 14:35:07Z");

            // Expected: GPSAltitude 2/1 GPSLatitude 51,55.4804N GPSLongitude 4,27,9E
            xmpExifProvider = new XmpExifProvider(WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.GeotaggedXmp4));

            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 51, 55.4804));
            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 4, 27, 9));
            Assert.AreEqual<string>(xmpExifProvider.GpsDateTimeStamp.ToString("u"), "2010-02-01 14:00:52Z");
        }

        #region Pre & Post Test Pass Code, not currently used
        // Run code after all tests in a class have run
        [ClassCleanup()]
        public static void PostTestPassCleanup()
        {
        }

        // Run code before running the first test in the class
        [ClassInitialize()]
        public static void PreTestPassInitialize(TestContext testContext)
        {
        }
        #endregion

        #region Pre\Post Test Code
        [TestCleanup()]
        public void PostTestCleanup()
        {
        }

        [TestInitialize()]
        public void PreTestInitialize()
        {
        }
        #endregion
    }
}
