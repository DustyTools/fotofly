namespace FotoFly.UnitTests
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

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using FotoFly;
    using FotoFly.Geotagging;

    [TestClass]
    public class FotoFlyUnitTests
    {
        private string samplesFolder = @"..\..\..\~Sample Files\JpgPhotos\";

        public FotoFlyUnitTests()
        {
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

        /// <summary>
        /// Check test photo can be read and metadata loaded into memory
        /// </summary>
        [TestMethod]
        public void ReadAndWriteFotoFlyMetadata()
        {
            BitmapMetadata bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplesFolder + TestPhotos.UnitTest1);

            DateTime testDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            WpfFotoFlyMetadata wpfFotoFlyMetadata = new WpfFotoFlyMetadata(bitmapMetadata);
            wpfFotoFlyMetadata.AccuracyOfGps = GpsPosition.Accuracies.Region;
            wpfFotoFlyMetadata.AddressOfGps = new Address("United States/California/San Francisco/Mission Street");
            wpfFotoFlyMetadata.AddressOfGpsLookupDate = testDate;
            wpfFotoFlyMetadata.AddressOfGpsSource = "Bing Maps for Enterprise";
            wpfFotoFlyMetadata.LastEditDate = testDate;
            wpfFotoFlyMetadata.OriginalCameraDate = testDate;
            wpfFotoFlyMetadata.OriginalCameraFilename = "img_123.jpg";
            wpfFotoFlyMetadata.UtcDate = testDate;
            wpfFotoFlyMetadata.UtcOffset = 5;

            WpfFileManager.WriteBitmapMetadata(this.samplesFolder + TestPhotos.UnitTestX, bitmapMetadata, this.samplesFolder + TestPhotos.UnitTest1);

            bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplesFolder + TestPhotos.UnitTestX);

            wpfFotoFlyMetadata = new WpfFotoFlyMetadata(bitmapMetadata);

            Assert.AreEqual<GpsPosition.Accuracies>(wpfFotoFlyMetadata.AccuracyOfGps, GpsPosition.Accuracies.Region);
            Assert.AreEqual<Address>(wpfFotoFlyMetadata.AddressOfGps, new Address("United States/California/San Francisco/Mission Street"));
            Assert.AreEqual<DateTime>(wpfFotoFlyMetadata.AddressOfGpsLookupDate, testDate);
            Assert.AreEqual<string>(wpfFotoFlyMetadata.AddressOfGpsSource, "Bing Maps for Enterprise");
            Assert.AreEqual<DateTime>(wpfFotoFlyMetadata.LastEditDate, testDate);
            Assert.AreEqual<DateTime>(wpfFotoFlyMetadata.OriginalCameraDate, testDate);
            Assert.AreEqual<string>(wpfFotoFlyMetadata.OriginalCameraFilename, "img_123.jpg");
            Assert.AreEqual<DateTime>(wpfFotoFlyMetadata.UtcDate, testDate);
            Assert.AreEqual<double>(wpfFotoFlyMetadata.UtcOffset, 5);

            if (File.Exists(this.samplesFolder + TestPhotos.UnitTestX))
            {
                File.Delete(this.samplesFolder + TestPhotos.UnitTestX);
            }
        }

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
