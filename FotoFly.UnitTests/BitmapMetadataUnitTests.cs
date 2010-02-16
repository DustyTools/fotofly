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
        public void ReadXmpGpsMetadata()
        {
            BitmapMetadata bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.GeotaggedXmp1);

            XmpExifProvider xmpExifProvider = new XmpExifProvider(bitmapMetadata);

            Assert.AreEqual<double>(xmpExifProvider.GpsAltitude, Double.NaN);
            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, true, 51, 55.6784));
            Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, true, 4, 26.6922));
            Assert.AreEqual<string>(xmpExifProvider.GpsVersionID, "2.2.0.0");

            // TODO:
            ////Assert.AreEqual<DateTime>(xmpExifProvider.DateTimeStamp, new DateTime());
            ////"2010-02-01T22:02:34+01:00"

            // Not set in this file
            // Assert.AreEqual<string>(xmpExifProvider.GpsProcessingMethod, string.Empty);
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
