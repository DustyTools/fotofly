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

    using FotoFly;
    using FotoFly.Geotagging;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JpgPhotoUnitTests
    {
        private string samplePhotosFolder = @"..\..\..\~Sample Files\JpgPhotos\";
        private string sampleMetadataXmlFolder = @"..\..\..\~Sample Files\MetadataXml\";

        // Basic file properties
        private JpgPhoto jpgPhotoOne;

        // Contains GPS Data
        private JpgPhoto jpgPhotoTwo;

        // This file is transient and deleted at the end of the test run
        private JpgPhoto jpgPhotoX;

        public JpgPhotoUnitTests()
        {
            this.jpgPhotoOne = new JpgPhoto(this.samplePhotosFolder + TestPhotos.UnitTest1);
            this.jpgPhotoTwo = new JpgPhoto(this.samplePhotosFolder + TestPhotos.UnitTest2);
            this.jpgPhotoX = new JpgPhoto(this.samplePhotosFolder + TestPhotos.UnitTestX);
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
                Assert.Fail("Metadata could not be read (" + this.jpgPhotoOne.FileName + ")");
            }

            try
            {
                this.jpgPhotoTwo.ReadMetadata();
            }
            catch
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
            if (File.Exists(this.jpgPhotoX.FileName))
            {
                File.Delete(this.jpgPhotoX.FileName);
            }

            // Test value to write
            string testSubject = "Test " + DateTime.Now.ToString();

            // Write to Photo Two
            this.jpgPhotoTwo.Metadata.Subject = testSubject;

            // Save Photo Three
            this.jpgPhotoTwo.WriteMetadata(this.jpgPhotoX.FileName);

            // Check the file was created
            if (!File.Exists(this.jpgPhotoX.FileName))
            {
                Assert.Fail("File save failed");
            }

            // Read metadata
            this.jpgPhotoX.ReadMetadata();

            // Check the file was created
            if (this.jpgPhotoX.Metadata == null)
            {
                Assert.Fail("Unable to read saved files metadata");
            }

            // Check the subject was set correctly
            StringAssert.Matches(this.jpgPhotoX.Metadata.Subject, new Regex(testSubject));

            if (new FileInfo(this.jpgPhotoTwo.FileName).Length > new FileInfo(this.jpgPhotoX.FileName).Length)
            {
                Assert.Fail("Photo has decreased in size after saving");
            }
        }

        /// <summary>
        /// WriteMetadataToFile Lossless Check
        /// </summary>
        [TestMethod]
        public void WriteMetadataToFileLossless()
        {
            string beforeFile = this.samplePhotosFolder + "WriteMetadataToFileLossless_Before.jpg";
            string afterFile = this.samplePhotosFolder + "WriteMetadataToFileLossless_After.jpg";

            // Get a copy of a file
            File.Copy(this.jpgPhotoOne.FileName, beforeFile);

            // Change some metadata
            JpgPhoto beforePhoto = new JpgPhoto(beforeFile);
            beforePhoto.Metadata.Subject = "Test " + DateTime.Now.ToString();
            beforePhoto.Metadata.Comment = "Test " + DateTime.Now.ToString();
            beforePhoto.Metadata.Copyright = "Test " + DateTime.Now.ToString();

            // Save as temp file
            beforePhoto.WriteMetadata(afterFile);

            // Open Original File
            using (Stream beforeStream = File.Open(beforeFile, FileMode.Open, FileAccess.Read))
            {
                // Open the Saved File
                using (Stream afterStream = File.Open(afterFile, FileMode.Open, FileAccess.Read))
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
            File.Delete(this.samplePhotosFolder + "WriteMetadataToFileLossless_Before.jpg");
            File.Delete(this.samplePhotosFolder + "WriteMetadataToFileLossless_After.jpg");
        }

        /// <summary>
        /// ReadStringMetadata
        /// </summary>
        [TestMethod]
        public void ReadStringMetadata()
        {
            // All string values
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Authors[0], new Regex("Test Author"), "Author");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Aperture, new Regex(@"f/9"), "Aperture");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.CameraManufacturer, new Regex("Canon"), "CameraManufacturer");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.CameraModel, new Regex("Canon"), "CameraModel");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Comment, new Regex("Test Comment"), "Comment");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.CreationSoftware, new Regex("Digital Photo Professional"), "CreationSoftware");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Copyright, new Regex("Test Copyright"), "Copyright");
            StringAssert.Matches(this.jpgPhotoOne.Metadata.ExposureBias, new Regex("0 step"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.FocalLength, new Regex("380 mm"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Iso, new Regex("ISO-400"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.ShutterSpeed, new Regex("1/1000 sec."));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Subject, new Regex(@"Test Caption\\Title\\Subject"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.Title, new Regex(@"Test Caption\\Title\\Subject"));
        }
            
        /// <summary>
        /// ReadNumericMetadata
        /// </summary>
        [TestMethod]
        public void ReadNumericMetadata()
        {
            Assert.AreEqual<int>(this.jpgPhotoOne.Metadata.Rating, 3, "Rating");

            Assert.AreEqual<double>(this.jpgPhotoOne.Metadata.DigitalZoomRatio, 0, "DigitalZoomRatio");
            
            Assert.AreEqual<int>(this.jpgPhotoOne.Metadata.ImageHeight, 480, "ImageHeight");
            Assert.AreEqual<int>(this.jpgPhotoOne.Metadata.ImageWidth, 640, "ImageWidth");

            Assert.AreEqual<int>(this.jpgPhotoOne.Metadata.HorizontalResolution, 350, "HorizontalResolution");
            Assert.AreEqual<int>(this.jpgPhotoOne.Metadata.VerticalResolution, 350, "VerticalResolution");
        }

        /// <summary>
        /// ReadEnumMetadata
        /// </summary>
        [TestMethod]
        public void ReadEnumMetadata()
        {
            Assert.AreEqual<PhotoMetadataEnums.MeteringModes>(this.jpgPhotoTwo.Metadata.MeteringMode, PhotoMetadataEnums.MeteringModes.CenterWeightedAverage);
        }
        
        /// <summary>
        /// Check Dates
        /// </summary>
        [TestMethod]
        public void ReadDateMetadata()
        {
            StringAssert.Matches(this.jpgPhotoOne.Metadata.DateDigitised.ToString(), new Regex("10/10/2009 21:46:37"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.DateTaken.ToString(), new Regex("10/10/2009 14:46:37"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.DateAquired.ToString(), new Regex("15/11/2009 00:05:58"));
        }

        /// <summary>
        /// Check Gps metadata
        /// </summary>
        [TestMethod]
        public void ReadGpsMetadata()
        {
            Assert.AreEqual<int>(this.jpgPhotoOne.Metadata.GpsPosition.Accuracy, 0);

            StringAssert.Matches(this.jpgPhotoOne.Metadata.GpsPosition.DegreesMinutesSecondsAltitude, new Regex("N 037° 48' 25.00\" W 122° 25' 23.00\" -17.464m"));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.GpsPosition.Source, new Regex("Garmin Dakota 20"));

            Assert.AreEqual<double>(this.jpgPhotoOne.Metadata.GpsPosition.Altitude, -17.464, "Altitude");

            Assert.AreEqual<GpsPosition.Dimensions>(this.jpgPhotoOne.Metadata.GpsPosition.Dimension, GpsPosition.Dimensions.ThreeDimensional);

            Assert.AreEqual<DateTime>(this.jpgPhotoOne.Metadata.GpsPosition.SatelliteTime, new DateTime(2009, 10, 10, 21, 46, 24));
        }

        /// <summary>
        /// Check Microsoft RegionInfo regions data are read correctly
        /// </summary>
        [TestMethod]
        public void ReadXmpRegionMetadata()
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
        /// Check Microsoft RegionInfo regions are written correctly
        /// </summary>
        [TestMethod]
        public void WriteXmpRegionMetadata()
        {
            string testValueSuffix = DateTime.Now.ToUniversalTime().ToString();

            this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].PersonDisplayName = "PersonDisplayName" + testValueSuffix;
            this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].PersonEmailDigest = "PersonEmailDigest" + testValueSuffix;
            this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].PersonLiveIdCID = "PersonLiveIdCID" + testValueSuffix;
            this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].RectangleString = "RectangleString" + testValueSuffix;

            this.jpgPhotoOne.WriteMetadata(this.jpgPhotoX.FileName);

            this.jpgPhotoX.ReadMetadata();

            StringAssert.Matches(this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].PersonDisplayName, new Regex("PersonDisplayName" + testValueSuffix));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].PersonEmailDigest, new Regex("PersonEmailDigest" + testValueSuffix));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].PersonLiveIdCID, new Regex("PersonLiveIdCID" + testValueSuffix));
            StringAssert.Matches(this.jpgPhotoOne.Metadata.RegionInfo.Regions[0].RectangleString, new Regex("RectangleString" + testValueSuffix));
        }

        /// <summary>
        /// Check MetadataDump
        /// </summary>
        [TestMethod]
        public void ReadMetadataDump()
        {
            MetadataDump metadataDump = new MetadataDump(WpfFileManager.ReadBitmapMetadata(this.jpgPhotoTwo.FileName));

            // Check total count
            Assert.AreEqual<int>(metadataDump.StringList.Count, 63);
        }

        /// <summary>
        /// Check ReadCorruptMetadata, reads a known bad image and checks the values are read correctly
        /// </summary>
        [TestMethod]
        public void ReadCorruptMetadata()
        {
            JpgPhoto jpgPhoto = new JpgPhoto(this.samplePhotosFolder + TestPhotos.CorruptExposureBias);
            StringAssert.Matches(jpgPhoto.Metadata.ExposureBias, new Regex("0 step"));
        }

        /// <summary>
        /// ReadExposureBias
        /// </summary>
        [TestMethod]
        public void ReadExposureBias()
        {
            // -1.3 step
            JpgPhoto jpgPhoto = new JpgPhoto(this.samplePhotosFolder + TestPhotos.ExposureBiasMinus13);
            StringAssert.Matches(jpgPhoto.Metadata.ExposureBias, new Regex(Regex.Escape("-1.3 step")));

            // +1.3 step
            jpgPhoto = new JpgPhoto(this.samplePhotosFolder + TestPhotos.ExposureBiasPlus13);
            StringAssert.Matches(jpgPhoto.Metadata.ExposureBias, new Regex(Regex.Escape("+1.3 step")));
        }

        /// <summary>
        /// ReadPadding
        /// </summary>
        [TestMethod]
        public void ReadPadding()
        {
            // Load a file with no padding set
            BitmapMetadata noPaddingBitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.NoPadding);

            // Check Padding Amounts
            Assert.AreEqual<object>(noPaddingBitmapMetadata.GetQuery(ExifQueries.Padding.Query), null);
            Assert.AreEqual<object>(noPaddingBitmapMetadata.GetQuery(XmpQueries.Padding.Query), null);
            Assert.AreEqual<object>(noPaddingBitmapMetadata.GetQuery(IptcQueries.Padding.Query), null);

            // Load a file with default padding set
            BitmapMetadata paddingBitmapMetadata = WpfFileManager.ReadBitmapMetadata(this.samplePhotosFolder + TestPhotos.UnitTest1);

            // Check Padding Amounts
            Assert.AreEqual<uint>(paddingBitmapMetadata.GetQuery<uint>(ExifQueries.Padding.Query), WpfFileManager.PaddingAmount);
            Assert.AreEqual<uint>(paddingBitmapMetadata.GetQuery<uint>(IptcQueries.Padding.Query), WpfFileManager.PaddingAmount);

            // Don't understand why this is 0, it should be 5120
            Assert.AreEqual<uint>(paddingBitmapMetadata.GetQuery<uint>(XmpQueries.Padding.Query), 0);
        }

        /// <summary>
        /// Read Files from different cameras
        /// </summary>
        [TestMethod]
        public void ReadCameraMake()
        {
            // Read photos from a couple of different cameras, the aim is to ensure no expection is thrown reading the data
            JpgPhoto photo = new JpgPhoto(this.samplePhotosFolder + TestPhotos.MakeKodakDX4900);
            photo.ReadMetadata();

            photo = new JpgPhoto(this.samplePhotosFolder + TestPhotos.MakeNikonCoolPixP80);
            photo.ReadMetadata();

            photo = new JpgPhoto(this.samplePhotosFolder + TestPhotos.MakeNikonD70);
            photo.ReadMetadata();

            photo = new JpgPhoto(this.samplePhotosFolder + TestPhotos.MakePentaxOptioS);
            photo.ReadMetadata();

            photo = new JpgPhoto(this.samplePhotosFolder + TestPhotos.MakeSonyDSCT30);
            photo.ReadMetadata();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void BulkRead()
        {
            foreach (string file in Directory.GetFiles(this.samplePhotosFolder, "*,jpg"))
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

        [TestCleanup()]
        public void PostTestCleanup()
        {
            // Clean up
            if (File.Exists(this.jpgPhotoX.FileName))
            {
                File.Delete(this.jpgPhotoX.FileName);
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
