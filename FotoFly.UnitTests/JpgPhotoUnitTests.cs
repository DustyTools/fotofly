namespace FotoFly.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Media.Imaging;

    using FotoFly;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JpgPhotoUnitTests
    {
        private JpgPhoto jpgPhotoOne = new JpgPhoto(@"..\..\..\~Sample Photos\" + TestPhotos.UnitTest1);
        private JpgPhoto jpgPhotoTwo = new JpgPhoto(@"..\..\..\~Sample Photos\" + TestPhotos.UnitTest2);

        // This file is transient and deleted at the end of the test run
        private JpgPhoto jpgPhotoThree = new JpgPhoto(@"..\..\..\~Sample Photos\" + TestPhotos.UnitTest3);

        public JpgPhotoUnitTests()
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
        public void ReadMetadataFromFile()
        {
            this.jpgPhotoOne.ReadMetadata();
            this.jpgPhotoTwo.ReadMetadata();

            if (this.jpgPhotoOne.Metadata == null)
            {
                Assert.Fail("Metadata could not be read (" + this.jpgPhotoOne.FileName + ")");
            }
            else if (this.jpgPhotoTwo.Metadata == null)
            {
                Assert.Fail("Metadata could not be read (" + this.jpgPhotoTwo.FileName + ")");
            }
        }

        /// <summary>
        /// WriteMetadataToFile
        /// </summary>
        [TestMethod]
        public void WriteMetadataToFile()
        {
            // Clean up from previous test
            if (File.Exists(this.jpgPhotoThree.FileName))
            {
                File.Delete(this.jpgPhotoThree.FileName);
            }

            // Test value to write
            string testSubject = "Test " + DateTime.Now.ToString();

            // Write to Photo Two
            this.jpgPhotoTwo.Metadata.Subject = testSubject;

            // Save Photo Three
            this.jpgPhotoTwo.SaveMetadata(this.jpgPhotoThree.FileName);

            // Check the file was created
            if (!File.Exists(this.jpgPhotoThree.FileName))
            {
                Assert.Fail("File save failed");
            }

            // Read metadata
            this.jpgPhotoThree.ReadMetadata();

            // Check the file was created
            if (this.jpgPhotoThree.Metadata == null)
            {
                Assert.Fail("Unable to read saved files metadata");
            }

            // Check the subject was set correctly
            StringAssert.Matches(this.jpgPhotoThree.Metadata.Subject, new Regex(testSubject));
        }

        /// <summary>
        /// Check the most common metadata properties
        /// </summary>
        [TestMethod]
        public void CheckSimpleMetadata()
        {
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Aperture, new Regex(@"f/9"), "Aperture");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.CameraManufacturer, new Regex("Canon"), "CameraManufacturer");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.CameraModel, new Regex("Canon"), "CameraModel");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Comment, new Regex("Test Comment"), "Comment");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Copyright, new Regex("Test Copyright"), "Copyright");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.ExposureBias, new Regex("0 eV"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.FocalLength, new Regex("380 mm"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.HorizontalResolution.ToString(), new Regex("350"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.ImageHeight.ToString(), new Regex("480"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.ImageWidth.ToString(), new Regex("640"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Iso.ToString(), new Regex("ISO-400"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Rating.ToString(), new Regex("3"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.ShutterSpeed, new Regex("1/1000 sec."));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Subject, new Regex(@"Test Caption\\Title\\Subject"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Title, new Regex(@"Test Caption\\Title\\Subject"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.VerticalResolution.ToString(), new Regex("350"));

            StringAssert.Matches(this.jpgPhotoTwo.Metadata.MeteringMode.ToString(), new Regex("CenterWeightedAverage"));
        }

        /// <summary>
        /// Check Dates
        /// </summary>
        [TestMethod]
        public void CheckDateMetadata()
        {
            StringAssert.Matches(this.jpgPhotoOne.Metadata.DateDigitised.ToString(), new Regex("10/10/2009 21:46:37"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.DateTaken.ToString(), new Regex("10/10/2009 14:46:37"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.DateAquired.ToString(), new Regex("15/11/2009 00:05:58"));
        }

        /// <summary>
        /// Check Gps metadata
        /// </summary>
        [TestMethod]
        public void CheckGpsMetadata()
        {
            StringAssert.Matches(this.jpgPhotoOne.Metadata.GpsPosition.Accuracy.ToString(), new Regex("Unknown"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.GpsPosition.Altitude.ToString(), new Regex("-17.464"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.GpsPosition.DegreesMinutesSecondsAltitude, new Regex("N 037° 48' 25.00\" W 122° 25' 23.00\" -17.464m"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.GpsPosition.Dimension.ToString(), new Regex("ThreeDimensional"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.GpsPosition.SatelliteTime.ToString(), new Regex(@"10/10/2009 21:46:24"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.GpsPosition.Source, new Regex("Garmin Dakota 20"));
        }

        /// <summary>
        /// Check Microsoft RegionInfo regions data
        /// </summary>
        [TestMethod]
        public void CheckXmpRegionMetadata()
        {
            if (this.jpgPhotoOne.Metadata.RegionInfo == null)
            {
                Assert.Fail("Metadata contains no Regions");
            }
            else if (this.jpgPhotoOne.Metadata.RegionInfo.Regions.Count != 1)
            {
                Assert.Fail("Metadata doesn't contain 1 region");
            }

            StringAssert.Matches(this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].PersonDisplayName, new Regex("Ben Vincent"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].PersonEmailDigest, new Regex("68A7D36853D6CBDEC05624C1516B2533F8F665E0"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].PersonLiveIdCID, new Regex("3058747437326753075"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].RectangleString, new Regex("0.365625, 0.145833, 0.126562, 0.168750"));
        }

        /// <summary>
        /// Check MetadataDump
        /// </summary>
        [TestMethod]
        public void CheckMetadataDump()
        {
            MetadataDump metadataDump = new MetadataDump(WpfFileManager.ReadBitmapMetadata(this.jpgPhotoTwo.FileName));

            Assert.AreEqual<int>(metadataDump.StringList.Count, 63);
        }

        [TestCleanup()]
        public void PostTestCleanup()
        {
            // Clean up
            if (File.Exists(this.jpgPhotoThree.FileName))
            {
                File.Delete(this.jpgPhotoThree.FileName);
            }
        }

        #region Pre\Post Test Code
        // Run code before running each test 
        [TestInitialize()]
        public void PreTestInitialize()
        {
        }
        #endregion
    }
}
