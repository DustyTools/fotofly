namespace FotoFly.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using FotoFly;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JpgPhotoUnitTests
    {
        private JpgPhoto jpgPhoto;

        public JpgPhotoUnitTests()
        {
            this.jpgPhoto = new JpgPhoto(@"..\..\..\~Sample Photos\" + TestPhotos.UnitTest1);
        }

        #region Pre & Post Test Pass Code, not currently used
        // Run code before running the first test in the class
        [ClassInitialize()]
        public static void PreTestPassInitialize(TestContext testContext)
        {
        }

        // Run code after all tests in a class have run
        [ClassCleanup()]
        public static void PostTestPassCleanup()
        {
        }
        #endregion

        /// <summary>
        /// Check test photo can be read and metadata loaded into memory
        /// </summary>
        [TestMethod]
        public void ReadMetadataFromFile()
        {
            this.jpgPhoto.ReadMetadata();

            if (this.jpgPhoto.Metadata == null)
            {
                Assert.Fail("Metadata could not be read");
            }
        }

        /// <summary>
        /// Check the most common metadata properties
        /// </summary>
        [TestMethod]
        public void CheckSimpleMetadata()
        {
            StringAssert.Matches(this.jpgPhoto.Metadata.Aperture, new Regex(@"f/9"), "Aperture");
            StringAssert.Matches(this.jpgPhoto.Metadata.CameraManufacturer, new Regex("Canon"), "CameraManufacturer");
            StringAssert.Matches(this.jpgPhoto.Metadata.CameraModel, new Regex("Canon"), "CameraModel");
            StringAssert.Matches(this.jpgPhoto.Metadata.Comment, new Regex("Test Comment"), "Comment");
            StringAssert.Matches(this.jpgPhoto.Metadata.Copyright, new Regex("Test Copyright"), "Copyright");
            StringAssert.Matches(this.jpgPhoto.Metadata.ExposureBias, new Regex("0 eV"));
            StringAssert.Matches(this.jpgPhoto.Metadata.FocalLength, new Regex("380 mm"));
            StringAssert.Matches(this.jpgPhoto.Metadata.HorizontalResolution.ToString(), new Regex("350"));
            StringAssert.Matches(this.jpgPhoto.Metadata.ImageHeight.ToString(), new Regex("480"));
            StringAssert.Matches(this.jpgPhoto.Metadata.ImageWidth.ToString(), new Regex("640"));
            StringAssert.Matches(this.jpgPhoto.Metadata.Iso.ToString(), new Regex("ISO-400"));
            StringAssert.Matches(this.jpgPhoto.Metadata.Rating.ToString(), new Regex("3"));
            StringAssert.Matches(this.jpgPhoto.Metadata.ShutterSpeed, new Regex("1/1000 sec."));
            StringAssert.Matches(this.jpgPhoto.Metadata.Subject, new Regex(@"Test Caption\\Title\\Subject"));
            StringAssert.Matches(this.jpgPhoto.Metadata.Title, new Regex(@"Test Caption\\Title\\Subject"));
            StringAssert.Matches(this.jpgPhoto.Metadata.VerticalResolution.ToString(), new Regex("350"));

            StringAssert.Matches(this.jpgPhoto.Metadata.DateDigitised.ToString(), new Regex("10/10/2009 21:46:37"));
            StringAssert.Matches(this.jpgPhoto.Metadata.DateTaken.ToString(), new Regex("10/10/2009 14:46:37"));
        }

        /// <summary>
        /// Check Gps metadata
        /// </summary>
        [TestMethod]
        public void CheckGpsMetadata()
        {
            StringAssert.Matches(this.jpgPhoto.Metadata.GpsPosition.Accuracy.ToString(), new Regex("Unknown"));
            StringAssert.Matches(this.jpgPhoto.Metadata.GpsPosition.Altitude.ToString(), new Regex("-17.464"));
            StringAssert.Matches(this.jpgPhoto.Metadata.GpsPosition.DegreesMinutesSecondsAltitude, new Regex("N 037° 48' 25.00\" W 122° 25' 23.00\" -17.464m"));
            StringAssert.Matches(this.jpgPhoto.Metadata.GpsPosition.Dimension.ToString(), new Regex("ThreeDimensional"));
            StringAssert.Matches(this.jpgPhoto.Metadata.GpsPosition.SatelliteTime.ToString(), new Regex(@"10/10/2009 21:46:24"));
            StringAssert.Matches(this.jpgPhoto.Metadata.GpsPosition.Source, new Regex("Garmin Dakota 20"));
        }

        /// <summary>
        /// Check Microsoft RegionInfo regions data
        /// </summary>
        [TestMethod]
        public void CheckXmpRegionMetadata()
        {
            if (this.jpgPhoto.Metadata.RegionInfo == null)
            {
                Assert.Fail("Metadata contains no Regions");
            }
            else if (this.jpgPhoto.Metadata.RegionInfo.Regions.Count != 1)
            {
                Assert.Fail("Metadata doesn't contain 1 region");
            }

            StringAssert.Matches(this.jpgPhoto.Metadata.RegionInfo.Regions[0].PersonDisplayName, new Regex("Ben Vincent"));
            StringAssert.Matches(this.jpgPhoto.Metadata.RegionInfo.Regions[0].PersonEmailDigest, new Regex("68A7D36853D6CBDEC05624C1516B2533F8F665E0"));
            StringAssert.Matches(this.jpgPhoto.Metadata.RegionInfo.Regions[0].PersonLiveIdCID, new Regex("3058747437326753075"));
            StringAssert.Matches(this.jpgPhoto.Metadata.RegionInfo.Regions[0].RectangleString, new Regex("0.365625, 0.145833, 0.126562, 0.168750"));
        }

        #region Pre\Post Test Code
        // Run code before running each test 
        [TestInitialize()]
        public void PreTestInitialize()
        {
        }

        // Run code after each test has run
        [TestCleanup()]
        public void PostTestCleanup()
        {
        }
        #endregion
    }
}
