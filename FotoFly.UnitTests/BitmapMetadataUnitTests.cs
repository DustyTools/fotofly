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

            Assert.AreEqual<GpsCoordinate>(gpsProvider.Latitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, true, 37, 48.41667));
            Assert.AreEqual<GpsCoordinate>(gpsProvider.Longitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, false, 122, 25.38333));
        }
        
        /// <summary>
        /// Check Xmp Gps metadata provider
        /// </summary>
        [TestMethod]
        public void ReadXmpGpsMetadata()
        {
            // Expected 51° 55.6784 N 4° 26.6922 E
            XmpExifProvider xmpExifProvider = new XmpExifProvider(WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.GeotaggedXmp1));

            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, true, 51, 55.6784));
            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, true, 4, 26.6922));
            Assert.AreEqual<double>(xmpExifProvider.GpsAltitude, Double.NaN);
            Assert.AreEqual<string>(xmpExifProvider.GpsVersionID, "2.2.0.0");
            //// Assert.AreEqual<string>(xmpExifProvider.GpsDateTimeStamp.ToString("z"), "2010-02-01T22:02:34+01:00");

            // Expected 51° 55.7057 N 4° 26.9547 E
            xmpExifProvider = new XmpExifProvider(WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.GeotaggedXmp2));

            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, true, 51, 55.7057));
            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, true, 4, 26.9547));

            // Expected 51° 55.6767 N 4° 27.2393 E
            xmpExifProvider = new XmpExifProvider(WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.GeotaggedXmp3));

            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, true, 51, 55.6767));
            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, true, 4, 27.2393));

            // Expected: GPSAltitude 2/1 GPSLatitude 51,55.4804N GPSLongitude 4,27,9E
            xmpExifProvider = new XmpExifProvider(WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.GeotaggedXmp4));

            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, true, 51, 55.4804));
            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, true, 4, 27, 9));
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
