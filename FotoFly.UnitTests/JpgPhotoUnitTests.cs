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
    using Fotofly.BitmapMetadataTools;
    using Fotofly.Geotagging;
    using Fotofly.MetadataQueries;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JpgPhotoUnitTests
    {
        // Basic file properties
        private JpgPhoto jpgPhotoOne;

        // Contains GPS Data
        private JpgPhoto jpgPhotoTwo;

        public JpgPhotoUnitTests()
        {
            this.jpgPhotoOne = TestPhotos.Load(TestPhotos.UnitTest1);
            this.jpgPhotoTwo = TestPhotos.Load(TestPhotos.UnitTest2);
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
            try
            {
                this.jpgPhotoOne.ReadMetadata();
            }
            catch
            {
                Assert.Fail("Metadata could not be read (" + this.jpgPhotoOne.FileFullName + ")");
            }

            try
            {
                this.jpgPhotoTwo.ReadMetadata();
            }
            catch
            {
                Assert.Fail("Metadata could not be read (" + this.jpgPhotoTwo.FileFullName + ")");
            }
        }

        /// <summary>
        /// ReadStringMetadata
        /// </summary>
        [TestMethod]
        public void ReadStringMetadata()
        {
            // All string values
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Authors[0], new Regex("Test Author"), "Author");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.CameraManufacturer, new Regex("Canon"), "CameraManufacturer");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.CameraModel, new Regex("Canon"), "CameraModel");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Comment, new Regex("Test Comment"), "Comment");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.CreationSoftware, new Regex(FotoflyAssemblyInfo.ShortBuildVersion), "CreationSoftware");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Copyright, new Regex("Test Copyright"), "Copyright");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.FocalLength, new Regex("380 mm"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Tags.Last().Name, new Regex(@"Test Tag Î"));

            StringAssert.Matches(this.jpgPhotoOne.Metadata.Description, new Regex(@"Test Title"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Comment, new Regex(@"Test Comment"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Subject, new Regex(@"Test Subject"));
        }

        /// <summary>
        /// ReadNumericMetadata
        /// </summary>
        [TestMethod]
        public void ReadNumericMetadata()
        {
            Assert.AreEqual<int>(this.jpgPhotoOne.Metadata.ImageHeight, 480, "ImageHeight");
            Assert.AreEqual<int>(this.jpgPhotoOne.Metadata.ImageWidth, 640, "ImageWidth");

            Assert.AreEqual<int>(this.jpgPhotoOne.Metadata.HorizontalResolution, 350, "HorizontalResolution");
            Assert.AreEqual<int>(this.jpgPhotoOne.Metadata.VerticalResolution, 350, "VerticalResolution");

            // TODO Get test file
            Assert.AreEqual<double?>(this.jpgPhotoOne.Metadata.DigitalZoomRatio, null, "DigitalZoomRatio");
        }

        /// <summary>
        /// Read Ratings
        /// </summary>
        [TestMethod]
        public void ReadRatingMetadata()
        {
            Assert.AreEqual<Rating.Ratings>(this.jpgPhotoOne.Metadata.Rating.AsEnum, Rating.Ratings.ThreeStar, "Rating");

            Assert.AreEqual<Rating.Ratings>(this.jpgPhotoTwo.Metadata.Rating.AsEnum, Rating.Ratings.NoRating, "Rating");
        }

        /// <summary>
        /// ReadEnumMetadata
        /// </summary>
        [TestMethod]
        public void ReadEnumMetadata()
        {
            Assert.AreEqual<MetadataEnums.MeteringModes>(this.jpgPhotoTwo.Metadata.MeteringMode, MetadataEnums.MeteringModes.CenterWeightedAverage);
        }

        /// <summary>
        /// Check Dates
        /// </summary>
        [TestMethod]
        public void ReadDateMetadata()
        {
            StringAssert.Matches(this.jpgPhotoOne.Metadata.DateDigitised.ToString(), new Regex(new DateTime(2009, 10, 10, 21, 46, 37).ToString()));
			StringAssert.Matches(this.jpgPhotoOne.Metadata.DateTaken.ToString(), new Regex(new DateTime(2009, 10, 10, 14, 46, 37).ToString()));
			StringAssert.Matches(this.jpgPhotoOne.Metadata.DateAquired.ToString(), new Regex(new DateTime(2009, 11, 15, 0, 5, 58).ToString()));
        }

        /// <summary>
        /// Read and Write Gps metadata
        /// </summary>
        [TestMethod]
        public void ReadGpsMetadata()
        {
            StringAssert.Matches(this.jpgPhotoOne.Metadata.GpsPositionOfLocationCreated.ToString(), new Regex("N 037° 48' 25.00\" W 122° 25' 23.00\" -17.464m"), "GpsPosition of Location");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.GpsPositionOfLocationCreated.Source, new Regex("Garmin Dakota 20"), "GPS Device");

            Assert.AreEqual<double>(this.jpgPhotoOne.Metadata.GpsPositionOfLocationCreated.Altitude, -17.464, "Altitude");
            Assert.AreEqual<GpsPosition.Dimensions>(this.jpgPhotoOne.Metadata.GpsPositionOfLocationCreated.Dimension, GpsPosition.Dimensions.ThreeDimensional, "Dimensions");
            Assert.AreEqual<DateTime>(this.jpgPhotoOne.Metadata.GpsPositionOfLocationCreated.Time, new DateTime(2009, 10, 10, 21, 46, 24), "Satalite Time");
        }

        /// <summary>
        /// Check Microsoft RegionInfo regions data are read correctly
        /// </summary>
        [TestMethod]
        public void ReadImageRegionMetadata()
        {
            if (this.jpgPhotoOne.Metadata.MicrosoftRegionInfo == null)
            {
                Assert.Fail("Metadata contains no Regions");
            }
            else if (this.jpgPhotoOne.Metadata.MicrosoftRegionInfo.Regions.Count != 1)
            {
                Assert.Fail("Metadata doesn't contain 1 region");
            }

            StringAssert.Matches(this.jpgPhotoOne.Metadata.MicrosoftRegionInfo.Regions[0].PersonDisplayName, new Regex("Ben Vincent"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.MicrosoftRegionInfo.Regions[0].PersonEmailDigest, new Regex("68A7D36853D6CBDEC05624C1516B2533F8F665E0"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.MicrosoftRegionInfo.Regions[0].PersonLiveIdCID, new Regex("3058747437326753075"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.MicrosoftRegionInfo.Regions[0].RectangleString, new Regex("0.365625, 0.145833, 0.126562, 0.16875"));
        }

        /// <summary>
        /// Read Aperture, Iso Speed and Shutter Speed
        /// </summary>
        [TestMethod]
        public void ReadIso()
        {
            // Read as Property
            Assert.AreEqual<IsoSpeed>(this.jpgPhotoOne.Metadata.IsoSpeed, new IsoSpeed(400));

            // Read as String
            Assert.AreEqual<string>(this.jpgPhotoOne.Metadata.IsoSpeed.ToString(), "ISO-400");
        }

        /// <summary>
        /// Read Aperture, Iso Speed and Shutter Speed
        /// </summary>
        [TestMethod]
        public void ReadShuttersSpeedProperties()
        {
            JpgPhoto shutterSpeed10Seconds = TestPhotos.Load(TestPhotos.ShutterSpeed10Seconds);
            JpgPhoto shutterSpeed1Over10 = TestPhotos.Load(TestPhotos.ShutterSpeed1Over10);
            JpgPhoto shutterSpeed1Over1000 = TestPhotos.Load(TestPhotos.ShutterSpeed1Over1000);
            JpgPhoto shutterSpeed1Over2 = TestPhotos.Load(TestPhotos.ShutterSpeed1Over2);
            JpgPhoto shutterSpeed1Over285 = TestPhotos.Load(TestPhotos.ShutterSpeed1Over285);
            JpgPhoto shutterSpeed1Over60 = TestPhotos.Load(TestPhotos.ShutterSpeed1Over60);
            JpgPhoto shutterSpeed2Seconds5 = TestPhotos.Load(TestPhotos.ShutterSpeed2Seconds5);

            // Read as Property
            Assert.AreEqual<ShutterSpeed>(shutterSpeed10Seconds.Metadata.ShutterSpeed, new ShutterSpeed(10), "ShutterSpeed 10 secs As Property");
            Assert.AreEqual<ShutterSpeed>(shutterSpeed1Over10.Metadata.ShutterSpeed, new ShutterSpeed(0.1), "ShutterSpeed 1/10 As Property");
            Assert.AreEqual<ShutterSpeed>(shutterSpeed1Over1000.Metadata.ShutterSpeed, new ShutterSpeed(0.001), "ShutterSpeed 1/1000 As Property");
            Assert.AreEqual<ShutterSpeed>(shutterSpeed1Over2.Metadata.ShutterSpeed, new ShutterSpeed(0.5), "ShutterSpeed 1/2 As Property");
            Assert.AreEqual<ShutterSpeed>(shutterSpeed1Over285.Metadata.ShutterSpeed, new ShutterSpeed(0.003509), "ShutterSpeed 1/285 As Property");
            Assert.AreEqual<ShutterSpeed>(shutterSpeed1Over60.Metadata.ShutterSpeed, new ShutterSpeed(0.016667), "ShutterSpeed 1/60 As Property");
            Assert.AreEqual<ShutterSpeed>(shutterSpeed2Seconds5.Metadata.ShutterSpeed, new ShutterSpeed(2.5), "ShutterSpeed 2.5 secs As Property");

            // Read as String
            Assert.AreEqual<string>(shutterSpeed10Seconds.Metadata.ShutterSpeed.ToString(), "10 sec.", "ShutterSpeed 10 secs As String");
            Assert.AreEqual<string>(shutterSpeed1Over10.Metadata.ShutterSpeed.ToString(), "1/10 sec.", "ShutterSpeed 1/10 As String");
            Assert.AreEqual<string>(shutterSpeed1Over1000.Metadata.ShutterSpeed.ToString(), "1/1000 sec.", "ShutterSpeed 1/1000 As String");
            Assert.AreEqual<string>(shutterSpeed1Over2.Metadata.ShutterSpeed.ToString(), "1/2 sec.", "ShutterSpeed 1/2 As String");
            Assert.AreEqual<string>(shutterSpeed1Over285.Metadata.ShutterSpeed.ToString(), "1/285 sec.", "ShutterSpeed 1/285 As String");
            Assert.AreEqual<string>(shutterSpeed1Over60.Metadata.ShutterSpeed.ToString(), "1/60 sec.", "ShutterSpeed 1/60 As String");
            Assert.AreEqual<string>(shutterSpeed2Seconds5.Metadata.ShutterSpeed.ToString(), "2.5 sec.", "ShutterSpeed 2.5 secs As String");
        }

        /// <summary>
        /// Read Aperture, Iso Speed and Shutter Speed
        /// </summary>
        [TestMethod]
        public void ReadApertureProperties()
        {
            JpgPhoto aperture28 = TestPhotos.Load(TestPhotos.Aperture28);
            JpgPhoto aperture71 = TestPhotos.Load(TestPhotos.Aperture71);
            JpgPhoto aperture80 = TestPhotos.Load(TestPhotos.Aperture80);

            // Read as Property
            Assert.AreEqual<Aperture>(aperture28.Metadata.Aperture, new Aperture(2.8), "Aperture28 As Property");
            Assert.AreEqual<Aperture>(aperture71.Metadata.Aperture, new Aperture(7.1), "Aperture71 As Property");
            Assert.AreEqual<Aperture>(aperture80.Metadata.Aperture, new Aperture(8.0), "Aperture80 As Property");

            // Read as String
            Assert.AreEqual<string>(aperture28.Metadata.Aperture.ToString(), "f/2.8", "Aperture28 As String");
            Assert.AreEqual<string>(aperture71.Metadata.Aperture.ToString(), "f/7.1", "Aperture71 As String");
            Assert.AreEqual<string>(aperture80.Metadata.Aperture.ToString(), "f/8", "Aperture80 As String");
        }

        /// <summary>
        /// ReadExposureBias
        /// </summary>
        [TestMethod]
        public void ReadExposureBias()
        {
            // 0
            Assert.AreEqual<ExposureBias>(this.jpgPhotoOne.Metadata.ExposureBias, new ExposureBias());

            // -1.3 step
            JpgPhoto jpgPhoto = TestPhotos.Load(TestPhotos.ExposureBiasMinus13);
            Assert.AreEqual<ExposureBias>(jpgPhoto.Metadata.ExposureBias, new ExposureBias("-4/3"));

            // +1.3 step
            jpgPhoto = TestPhotos.Load(TestPhotos.ExposureBiasPlus13);
            Assert.AreEqual<ExposureBias>(jpgPhoto.Metadata.ExposureBias, new ExposureBias("4/3"));
        }

        /// <summary>
        /// Read Files from different cameras
        /// </summary>
        [TestMethod]
        public void ReadCameraMake()
        {
            // Read photos from a couple of different cameras, the aim is to ensure no exception is thrown reading the data
            JpgPhoto photo = TestPhotos.Load(TestPhotos.MakeKodakDX4900);
            photo.ReadMetadata();

            photo = TestPhotos.Load(TestPhotos.MakeNikonCoolPixP80);
            photo.ReadMetadata();

            photo = TestPhotos.Load(TestPhotos.MakeNikonD70);
            photo.ReadMetadata();

            photo = TestPhotos.Load(TestPhotos.MakePentaxOptioS);
            photo.ReadMetadata();

            photo = TestPhotos.Load(TestPhotos.MakeSonyDSCT30);
            photo.ReadMetadata();

            photo = TestPhotos.Load(TestPhotos.MakeiPhone3GsUntouched);
            photo.ReadMetadata();

            Assert.AreEqual<string>(photo.Metadata.CameraManufacturer, "Apple", "CameraManufacturer");
            Assert.AreEqual<string>(photo.Metadata.CameraModel, "iPhone 3GS", "CameraModel");
            Assert.AreEqual<DateTime>(photo.Metadata.DateDigitised, new DateTime(2010, 02, 01, 08, 24, 40), "DateDigitised");
            Assert.AreEqual<MetadataEnums.MeteringModes>(photo.Metadata.MeteringMode, MetadataEnums.MeteringModes.Average, "MeteringMode");
            Assert.AreEqual<string>(photo.Metadata.Aperture.ToString(), "f/2.8", "Aperture");
            Assert.AreEqual<string>(photo.Metadata.ShutterSpeed.ToString(), "1/170 sec.", "ShutterSpeed");

            photo = TestPhotos.Load(TestPhotos.MakeiPhone3GsWithTags);
            photo.ReadMetadata();

            Assert.AreEqual<Tag>(photo.Metadata.Tags.First(), new Tag("Test"), "Tag");
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void BulkRead()
        {
            foreach (string file in Directory.GetFiles(TestPhotos.PhotosFolder, "*,jpg"))
            {
                JpgPhoto jpgPhoto = new JpgPhoto(file);

                try
                {
                    jpgPhoto.ReadMetadata();
                }
                catch
                {
                    Assert.Fail("Unable to read Metadata: " + file);
                }
            }
        }

        /// <summary>
        /// Read and Write Gps metadata
        /// </summary>
        [TestMethod]
        public void WriteGpsMetadata()
        {
            // Copy Test file
            JpgPhoto testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp9);
            File.Copy(this.jpgPhotoOne.FileFullName, testPhoto.FileFullName, true);

            // Test data, includes Unicode strings
            GpsPosition positionCreated = new GpsPosition(101.23, -34.321, -99.8);
            GpsPosition positionShow = new GpsPosition(-123.0, 179);

            // Scrub existing data
            testPhoto.Metadata.GpsPositionOfLocationCreated = new GpsPosition();
            testPhoto.Metadata.GpsPositionOfLocationShown = new GpsPosition();
            testPhoto.WriteMetadata();

            // Check for empty data
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp9);
            Assert.AreEqual<GpsPosition>(testPhoto.Metadata.GpsPositionOfLocationCreated, new GpsPosition(), "Blank GpsPosition Created");
            Assert.AreEqual<GpsPosition>(testPhoto.Metadata.GpsPositionOfLocationShown, new GpsPosition(), "Blank GpsPosition Shown");

            // Write GpsPosition Created
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp9);
            testPhoto.Metadata.GpsPositionOfLocationCreated = positionCreated;
            testPhoto.Metadata.GpsPositionOfLocationShown = new GpsPosition();
            testPhoto.WriteMetadata();

            // And Check Created
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp9);
            Assert.AreEqual<GpsPosition>(testPhoto.Metadata.GpsPositionOfLocationCreated, positionCreated, "WriteCreated Created");
            Assert.AreEqual<GpsPosition>(testPhoto.Metadata.GpsPositionOfLocationShown, new GpsPosition(), "WriteCreated Shown");

            // Write GpsPosition Shown
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp9);
            testPhoto.Metadata.GpsPositionOfLocationCreated = new GpsPosition();
            testPhoto.Metadata.GpsPositionOfLocationShown = positionShow;
            testPhoto.WriteMetadata();

            // And Check Shown
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp9);
            Assert.AreEqual<GpsPosition>(testPhoto.Metadata.GpsPositionOfLocationCreated, new GpsPosition(), "WriteShown Created");
            Assert.AreEqual<GpsPosition>(testPhoto.Metadata.GpsPositionOfLocationShown, positionShow, "WriteShown Shown");

            // Tidy up
            File.Delete(testPhoto.FileFullName);
        }

        /// <summary>
        /// Read and Write Address metadata
        /// </summary>
        [TestMethod]
        public void WriteAddressMetadata()
        {
            // Copy Test file
            JpgPhoto testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp10);
            File.Copy(this.jpgPhotoOne.FileFullName, testPhoto.FileFullName, true);

            // Test data, includes Unicode strings
            Address addressCreated = new Address(@"CòuntryCreàted/RegÎon/Ĉity/Stréét");
            Address addressShown = new Address(@"CòuntryShówn/RegÎon/Ĉity/Stréét");

            // Scrub existing data
            testPhoto.Metadata.AddressOfLocationCreated = new Address();
            testPhoto.Metadata.AddressOfLocationShown = new Address();
            testPhoto.WriteMetadata();

            // Check for empty data
            Assert.AreEqual<Address>(testPhoto.Metadata.AddressOfLocationCreated, new Address(), "Blank Address Created");
            Assert.AreEqual<Address>(testPhoto.Metadata.AddressOfLocationShown, new Address(), "Blank Address Shown");

            // Write Address Created
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp10);
            testPhoto.Metadata.AddressOfLocationCreated = addressCreated;
            testPhoto.Metadata.AddressOfLocationShown = new Address();
            testPhoto.WriteMetadata();

            // And Check Created
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp10);
            Assert.AreEqual<Address>(testPhoto.Metadata.AddressOfLocationCreated, addressCreated, "WriteAddress Created");
            Assert.AreEqual<Address>(testPhoto.Metadata.AddressOfLocationShown, new Address(), "WriteAddress Shown");

            // Write Address Shown
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp10);
            testPhoto.Metadata.AddressOfLocationCreated = new Address();
            testPhoto.Metadata.AddressOfLocationShown = addressShown;
            testPhoto.WriteMetadata();

            // And Check Shown
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp10);
            Assert.AreEqual<Address>(testPhoto.Metadata.AddressOfLocationCreated, new Address(), "WriteShown Created");
            Assert.AreEqual<Address>(testPhoto.Metadata.AddressOfLocationShown, addressShown, "WriteShown Shown");

            // Tidy up
            File.Delete(testPhoto.FileFullName);
        }

        /// <summary>
        /// Check Microsoft RegionInfo regions are written correctly
        /// </summary>
        [TestMethod]
        public void WriteImageRegionMetadata()
        {
            // Copy Test file
            JpgPhoto testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp4);
            File.Copy(this.jpgPhotoOne.FileFullName, testPhoto.FileFullName, true);

            string testValueSuffix = DateTime.Now.ToUniversalTime().ToString();

            testPhoto.Metadata.MicrosoftRegionInfo.Regions[0].PersonDisplayName = "PersonDisplayName" + testValueSuffix;
            testPhoto.Metadata.MicrosoftRegionInfo.Regions[0].PersonEmailDigest = "PersonEmailDigest" + testValueSuffix;
            testPhoto.Metadata.MicrosoftRegionInfo.Regions[0].PersonLiveIdCID = "PersonLiveIdCID" + testValueSuffix;
            testPhoto.Metadata.MicrosoftRegionInfo.Regions[0].RectangleString = "0.1, 0.2, 0.3, 0.4";

            // And save
            testPhoto.WriteMetadata();

            // Now read it from scratch
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp4);

            StringAssert.Matches(testPhoto.Metadata.MicrosoftRegionInfo.Regions[0].PersonDisplayName, new Regex("PersonDisplayName" + testValueSuffix));
            StringAssert.Matches(testPhoto.Metadata.MicrosoftRegionInfo.Regions[0].PersonEmailDigest, new Regex("PersonEmailDigest" + testValueSuffix));
            StringAssert.Matches(testPhoto.Metadata.MicrosoftRegionInfo.Regions[0].PersonLiveIdCID, new Regex("PersonLiveIdCID" + testValueSuffix));
            StringAssert.Matches(testPhoto.Metadata.MicrosoftRegionInfo.Regions[0].RectangleString, new Regex("0.1, 0.2, 0.3, 0.4"));

            File.Delete(testPhoto.FileFullName);
        }

        /// <summary>
        /// WriteMetadataToFile
        /// </summary>
        [TestMethod]
        public void WriteMetadataToFile()
        {
            // Copy Test file
            JpgPhoto testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp3);
            File.Copy(this.jpgPhotoTwo.FileFullName, testPhoto.FileFullName, true);

            // Test value to write
            string testString = "Test " + DateTime.Now.ToString();
            DateTime testDate = DateTime.Now.AddTicks(-DateTime.Now.TimeOfDay.Ticks);
            string testTag = "Test Tag Î";

            // Write text
            testPhoto.Metadata.Description = testString;
            testPhoto.Metadata.Comment = testString;
            testPhoto.Metadata.Copyright = testString;
            testPhoto.Metadata.AddressOfLocationCreatedSource = testString;
            testPhoto.Metadata.Tags = new TagList();
            testPhoto.Metadata.Tags.Add(new Tag(testTag));

            // Write dates
            testPhoto.Metadata.DateAquired = testDate;
            testPhoto.Metadata.DateDigitised = testDate;
            testPhoto.Metadata.DateTaken = testDate;
            testPhoto.Metadata.DateUtc = testDate;
            testPhoto.Metadata.AddressOfLocationCreatedLookupDate = testDate;

            // Save Photo Three
            testPhoto.WriteMetadata();

            // Check the file was created
            if (!File.Exists(testPhoto.FileFullName))
            {
                Assert.Fail("File save failed");
            }

            // Read metadata
            testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp3);
            testPhoto.ReadMetadata();

            // Check the file was created
            if (testPhoto.Metadata == null)
            {
                Assert.Fail("Unable to read saved files metadata");
            }

            // Check the text was set correctly
            StringAssert.Matches(testPhoto.Metadata.Description, new Regex(testString), "Description");
            StringAssert.Matches(testPhoto.Metadata.Comment, new Regex(testString), "Comment");
            StringAssert.Matches(testPhoto.Metadata.Copyright, new Regex(testString), "Copyright");
            StringAssert.Matches(testPhoto.Metadata.AddressOfLocationCreatedSource, new Regex(testString), "AddressOfLocationCreatedSource");

            // Check date was written
            Assert.AreEqual<DateTime>(testPhoto.Metadata.DateAquired, testDate, "DateAquired");
            Assert.AreEqual<DateTime>(testPhoto.Metadata.DateDigitised, testDate, "DateDigitised");
            Assert.AreEqual<DateTime>(testPhoto.Metadata.DateTaken, testDate, "DateTaken");
            Assert.AreEqual<DateTime>(testPhoto.Metadata.DateUtc, testDate, "UtcDate");
            Assert.AreEqual<DateTime>(testPhoto.Metadata.AddressOfLocationCreatedLookupDate, testDate, "AddressOfLocationCreatedLookupDate");
            Assert.AreEqual<Tag>(testPhoto.Metadata.Tags.Last(), new Tag(testTag), "Tags");

            if (new FileInfo(this.jpgPhotoTwo.FileFullName).Length > new FileInfo(testPhoto.FileFullName).Length)
            {
                Assert.Fail("Photo has decreased in size after saving");
            }

            File.Delete(testPhoto.FileFullName);
        }

        /// <summary>
        /// WriteMetadataToFile
        /// </summary>
        [TestMethod]
        public void WriteMetadataAndCheckForMetadataLoss()
        {
            JpgPhoto beforePhoto = TestPhotos.Load(TestPhotos.UnitTest3);
            JpgPhoto afterPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp5);

            // Copy Test file
            File.Copy(beforePhoto.FileFullName, afterPhoto.FileFullName, true);

            // Change date and save
            afterPhoto.Metadata.FotoflyDateLastSave = DateTime.Now.AddTicks(-DateTime.Now.TimeOfDay.Ticks);
            afterPhoto.WriteMetadata();

            MetadataDump beforeDump;
            MetadataDump afterDump;

            using (WpfFileManager wpfFileManager = new WpfFileManager(beforePhoto.FileFullName))
            {
                beforeDump = new MetadataDump(wpfFileManager.BitmapMetadata);
                beforeDump.GenerateStringList();
            }

            using (WpfFileManager wpfFileManager = new WpfFileManager(afterPhoto.FileFullName))
            {
                afterDump = new MetadataDump(wpfFileManager.BitmapMetadata);
                afterDump.GenerateStringList();
            }

            for (int i = 0; i < beforeDump.StringList.Count; i++)
            {
                // Ignore schema changes, edit dates and created software
                if (beforeDump.StringList[i] != afterDump.StringList[i]
                    && !beforeDump.StringList[i].Contains("DateLastSave")
                    && !beforeDump.StringList[i].Contains("LastEditDate")
                    && !beforeDump.StringList[i].Contains("ushort=513")
                    && !beforeDump.StringList[i].Contains("OffsetSchema"))
                {
                    Assert.Fail("Metadata mismatch " + beforeDump.StringList[i] + " != " + afterDump.StringList[i]);
                }
            }

            if (new FileInfo(afterPhoto.FileFullName).Length > new FileInfo(beforePhoto.FileFullName).Length)
            {
                Assert.Fail("Photo has decreased in size after saving");
            }

            // Clean up
            File.Delete(afterPhoto.FileFullName);
        }

        /// <summary>
        /// WriteMetadataToFile Lossless Check
        /// </summary>
        [TestMethod]
        public void WriteMetadataAndCheckForImageLoss()
        {
            // File that will be changed
            JpgPhoto beforePhoto = new JpgPhoto(TestPhotos.UnitTestTemp6);
            JpgPhoto afterPhoto = new JpgPhoto(TestPhotos.UnitTestTemp7);

            // Get a copy of a file
            File.Copy(this.jpgPhotoOne.FileFullName, beforePhoto.FileFullName, true);
            File.Copy(this.jpgPhotoOne.FileFullName, afterPhoto.FileFullName, true);

            // Change some metadata
            afterPhoto.Metadata.Description = "Test Description" + DateTime.Now.ToString();
            afterPhoto.Metadata.Comment = "Test Comment" + DateTime.Now.ToString();
            afterPhoto.Metadata.Copyright = "Test Copyright" + DateTime.Now.ToString();
            afterPhoto.Metadata.Subject = "Subject Copyright" + DateTime.Now.ToString();

            // Save as temp file
            afterPhoto.WriteMetadata();

            // Open Original File
            using (Stream beforeStream = File.Open(beforePhoto.FileFullName, FileMode.Open, FileAccess.Read))
            {
                // Open the Saved File
                using (Stream afterStream = File.Open(afterPhoto.FileFullName, FileMode.Open, FileAccess.Read))
                {
                    // Compare every pixel to ensure it has changed
                    BitmapSource beforeBitmap = BitmapDecoder.Create(beforeStream, BitmapCreateOptions.None, BitmapCacheOption.OnDemand).Frames[0];
                    BitmapSource afterBitmap = BitmapDecoder.Create(afterStream, BitmapCreateOptions.None, BitmapCacheOption.OnDemand).Frames[0];

                    PixelFormat pf = PixelFormats.Bgra32;

                    FormatConvertedBitmap fcbOne = new FormatConvertedBitmap(beforeBitmap, pf, null, 0);
                    FormatConvertedBitmap fcbTwo = new FormatConvertedBitmap(afterBitmap, pf, null, 0);

                    GC.AddMemoryPressure(((fcbOne.Format.BitsPerPixel * (fcbOne.PixelWidth + 7)) / 8) * fcbOne.PixelHeight);
                    GC.AddMemoryPressure(((fcbTwo.Format.BitsPerPixel * (fcbTwo.PixelWidth + 7)) / 8) * fcbTwo.PixelHeight);

                    int width = fcbOne.PixelWidth;
                    int height = fcbOne.PixelHeight;

                    int bpp = pf.BitsPerPixel;
                    int stride = (bpp * (width + 7)) / 8;

                    byte[] scanline0 = new byte[stride];
                    byte[] scanline1 = new byte[stride];

                    Int32Rect lineRect = new Int32Rect(0, 0, width, 1);

                    // Loop through each row
                    for (int y = 0; y < height; y++)
                    {
                        lineRect.Y = y;

                        fcbOne.CopyPixels(lineRect, scanline0, stride, 0);
                        fcbTwo.CopyPixels(lineRect, scanline1, stride, 0);

                        // Loop through each column
                        for (int b = 0; b < stride; b++)
                        {
                            if (Math.Abs(scanline0[b] - scanline1[b]) > 0)
                            {
                                Assert.Fail("Saved file was not solved losslessly");
                            }
                        }
                    }
                }
            }

            // Tidy UP
            File.Delete(beforePhoto.FileFullName);
            File.Delete(afterPhoto.FileFullName);
        }

        /// <summary>
        /// WriteMetadataToXmlFile
        /// </summary>
        [TestMethod]
        public void WriteAndReadMetadataToXmlFile()
        {
            JpgPhoto testPhoto = TestPhotos.Load(TestPhotos.UnitTestTemp8);
            string xmlFile = testPhoto.FileFullName.Replace(".jpg", ".xml");

            // Clean up from previous test
            if (File.Exists(xmlFile))
            {
                File.Delete(xmlFile);
            }

            File.Copy(this.jpgPhotoOne.FileFullName, testPhoto.FileFullName, true);

            testPhoto.ReadMetadata();
            testPhoto.WriteMetadataToXml(xmlFile);

            if (!File.Exists(xmlFile))
            {
                Assert.Fail("Serialised File was not found: " + xmlFile);
            }

            JpgPhoto xmlPhoto = new JpgPhoto(testPhoto.FileFullName);
            xmlPhoto.ReadMetadataFromXml(xmlFile);

            List<CompareResult> changes = new List<CompareResult>();

            PhotoMetadataTools.CompareMetadata(testPhoto.Metadata, xmlPhoto.Metadata, ref changes);

            if (changes.Count > 0)
            {
                Assert.Fail("Serialised File was incompleted: " + changes[0]);
            }

            File.Delete(testPhoto.FileFullName);
            File.Delete(xmlFile);
        }

        [TestCleanup()]
        public void PostTestCleanup()
        {
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
