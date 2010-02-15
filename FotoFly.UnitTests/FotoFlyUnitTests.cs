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
    using Fotofly.WpfTools;
    using Fotofly.Geotagging;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FotoflyUnitTests
    {
        private string samplesFolder = @"..\..\..\~Sample Files\JpgPhotos\";

        public FotoflyUnitTests()
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
        public void ReadAndWriteFotoflyMetadata()
        {
            BitmapMetadata bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplesFolder + TestPhotos.UnitTest1);

            DateTime testDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            WpfFotoflyMetadata wpfFotoflyMetadata = new WpfFotoflyMetadata(bitmapMetadata);
            wpfFotoflyMetadata.AccuracyOfGps = GpsPosition.Accuracies.Region;
            wpfFotoflyMetadata.AddressOfGps = new Address("United States/California/San Francisco/Mission Street");
            wpfFotoflyMetadata.AddressOfGpsLookupDate = testDate;
            wpfFotoflyMetadata.AddressOfGpsSource = "Bing Maps for Enterprise";
            wpfFotoflyMetadata.LastEditDate = testDate;
            wpfFotoflyMetadata.OriginalCameraDate = testDate;
            wpfFotoflyMetadata.OriginalCameraFilename = "img_123.jpg";
            wpfFotoflyMetadata.UtcDate = testDate;
            wpfFotoflyMetadata.UtcOffset = 5;

            WpfFileManager.WriteBitmapMetadata(this.samplesFolder + TestPhotos.UnitTestX, bitmapMetadata, this.samplesFolder + TestPhotos.UnitTest1);

            bitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplesFolder + TestPhotos.UnitTestX);

            wpfFotoflyMetadata = new WpfFotoflyMetadata(bitmapMetadata);

            Assert.AreEqual<GpsPosition.Accuracies>(wpfFotoflyMetadata.AccuracyOfGps, GpsPosition.Accuracies.Region);
            Assert.AreEqual<Address>(wpfFotoflyMetadata.AddressOfGps, new Address("United States/California/San Francisco/Mission Street"));
            Assert.AreEqual<DateTime>(wpfFotoflyMetadata.AddressOfGpsLookupDate, testDate);
            Assert.AreEqual<string>(wpfFotoflyMetadata.AddressOfGpsSource, "Bing Maps for Enterprise");
            Assert.AreEqual<DateTime>(wpfFotoflyMetadata.LastEditDate, testDate);
            Assert.AreEqual<DateTime>(wpfFotoflyMetadata.OriginalCameraDate, testDate);
            Assert.AreEqual<string>(wpfFotoflyMetadata.OriginalCameraFilename, "img_123.jpg");
            Assert.AreEqual<DateTime>(wpfFotoflyMetadata.UtcDate, testDate);
            Assert.AreEqual<double>(wpfFotoflyMetadata.UtcOffset.Value, 5);

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
