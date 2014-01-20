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
    using Fotofly.MetadataProviders;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Fotofly.MetadataQueries;
	using System.Globalization;

    [TestClass]
    public class BitmapMetadataUnitTests
    {
        private string samplePhotosFolder = @"..\..\..\~Sample Files\JpgPhotos\";

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
        /// Check MetadataDump
        /// </summary>
        [TestMethod]
        public void ReadMetadataDump()
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.SchemaXmpTiff))
            {
                MetadataDump metadataDump = new MetadataDump(wpfFileManager.BitmapMetadata);

                // Check total count
                Assert.AreEqual<int>(metadataDump.StringList.Count, 187);
            }
        }

        /// <summary>
        /// Check Xmp Xap Schema Metadata provider
        /// </summary>
        [TestMethod]
        public void WriteTextMetadata()
        {
            File.Copy(this.samplePhotosFolder + TestPhotos.UnitTest1, this.samplePhotosFolder + TestPhotos.UnitTestTemp2, true);

            string testString = " " + DateTime.Now.ToString();

            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.UnitTestTemp2, true))
            {
                FileMetadata fileMetadata = new FileMetadata(wpfFileManager.BitmapMetadata);

                fileMetadata.Description = "Description" + testString;
                fileMetadata.Comment = "Comment" + testString;
                fileMetadata.Copyright = "Copyright" + testString;
                fileMetadata.Subject = "Subject" + testString;

                wpfFileManager.WriteMetadata();
            }

            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.UnitTestTemp2))
            {
                FileMetadata fileMetadata = new FileMetadata(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<string>(fileMetadata.Description, "Description" + testString, "Description");
                Assert.AreEqual<string>(fileMetadata.Comment, "Comment" + testString, "Comment");
                Assert.AreEqual<string>(fileMetadata.Copyright, "Copyright" + testString, "Copyright");
                Assert.AreEqual<string>(fileMetadata.Subject, "Subject" + testString, "Subject");

                // Check underlying Description values
                Assert.AreEqual<string>(fileMetadata.IptcProvider.Caption, "Description" + testString, "Iptc.Caption");
                Assert.AreEqual<string>(fileMetadata.ExifProvider.Description, "Description" + testString, "Exif.Description");

                // TODO Add unerlying check for:
                // Xmp dc:title
                // Xmp dc:description
            }

            // Clean up from previous test
            if (File.Exists(this.samplePhotosFolder + TestPhotos.UnitTestTemp2))
            {
                File.Delete(this.samplePhotosFolder + TestPhotos.UnitTestTemp2);
            }
        }

        /// <summary>
        /// ReadPadding
        /// </summary>
        [TestMethod]
        public void ReadPadding()
        {
            // Load a file with default padding set
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.UnitTest1))
            {
                // Check Padding Amounts
                Assert.AreEqual<UInt32>(wpfFileManager.BitmapMetadata.GetQuery<UInt32>(ExifQueries.Padding.Query), 5120, "UnitTest1.Exif Padding");
                Assert.AreEqual<UInt32>(wpfFileManager.BitmapMetadata.GetQuery<UInt32>(IptcQueries.Padding.Query), 5120, "UnitTest1.Iptc Padding");

                // Don't understand why this is 0, it should be 5120
                Assert.AreEqual<UInt32>(wpfFileManager.BitmapMetadata.GetQuery<UInt32>(XmpCoreQueries.Padding.Query), 0);
            }

            // Load a file with no padding set
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.UnitTest2))
            {
                // Check Padding Amounts
                Assert.AreEqual<UInt32>(wpfFileManager.BitmapMetadata.GetQuery<UInt32>(ExifQueries.Padding.Query), 5120, "UnitTest2.Exif Padding");

                Assert.AreEqual<object>(wpfFileManager.BitmapMetadata.GetQuery(XmpCoreQueries.Padding.Query), null, "UnitTest2.Xmp Padding");

                Assert.AreEqual<UInt32>(wpfFileManager.BitmapMetadata.GetQuery<UInt32>(IptcQueries.Padding.Query), 5120, "UnitTest2.Iptc Padding");
            }
        }

        /// <summary>
        /// Check BitmapMetadata Tags
        /// </summary>
        [TestMethod]
        public void ReadMetadataTags()
        {
            string expectedString = "Île-de-France";

            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.TagsWithUnicode))
            {
                Assert.AreEqual<string>(wpfFileManager.BitmapMetadata.Keywords.First(), expectedString, "Xmp Keywords");
                Assert.AreEqual<string>(wpfFileManager.BitmapMetadata.Title, expectedString, "Title");

                IptcProvider iptcProvider = new IptcProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<Tag>(iptcProvider.Keywords.First(), new Tag(expectedString), "Iptc Keywords");
            }
        }

        /// <summary>
        /// Check Xmp Xap Schema Metadata provider
        /// </summary>
        [TestMethod]
        public void ReadXmpXapMetadata()
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.SchemaXmpXap))
            {
                XmpXapProvider xmpXapProvider = new XmpXapProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<string>(xmpXapProvider.CreatorTool, "Tassography PhotoManager");
                Assert.AreEqual<Rating.Ratings>(xmpXapProvider.Rating.AsEnum, Rating.Ratings.FourStar);
            }
        }

        /// <summary>
        /// Check Xmp Microsoft Schema Metadata provider
        /// </summary>
        [TestMethod]
        public void ReadXmpMicrosoftMetadata()
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.SchemaXmpMicrosoft))
            {
                XmpMicrosoftProvider xmpMicrosoftProvider = new XmpMicrosoftProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<Rating.Ratings>(xmpMicrosoftProvider.Rating.AsEnum, Rating.Ratings.ThreeStar);
                Assert.AreEqual<string>(xmpMicrosoftProvider.RegionInfo.Regions[0].PersonDisplayName, "Ben Vincent");
                Assert.AreEqual<string>(xmpMicrosoftProvider.RegionInfo.Regions[0].PersonEmailDigest, "68A7D36853D6CBDEC05624C1516B2533F8F665E0");
                Assert.AreEqual<string>(xmpMicrosoftProvider.RegionInfo.Regions[0].PersonLiveIdCID, "3058747437326753075");
                Assert.AreEqual<string>(xmpMicrosoftProvider.RegionInfo.Regions[0].RectangleString, "0.2875, 0.191667, 0.182812, 0.24375");
            }
        }

        /// <summary>
        /// Check Xmp Tiff Schema Metadata provider
        /// </summary>
        [TestMethod]
        public void ReadXmpTiffMetadata()
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.SchemaXmpTiff))
            {
                XmpTiffProvider xmpTiffProvider = new XmpTiffProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<string>(xmpTiffProvider.Make, "Canon");
                Assert.AreEqual<string>(xmpTiffProvider.Model, "Canon PowerShot SD790 IS");
                Assert.AreEqual<string>(xmpTiffProvider.Orientation, "6");
                Assert.AreEqual<string>(xmpTiffProvider.Software, "Tassography PhotoManager");
            }
        }

        /// <summary>
        /// Check Iptc Schema Metadata provider
        /// </summary>
        [TestMethod]
        public void ReadIptcMetadata()
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.SchemaXmpTiff))
            {
                IptcProvider iptcProvider = new IptcProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<string>(iptcProvider.Byline, "Ben Vincent");
                Assert.AreEqual<string>(iptcProvider.LocationCreatedCity, "San Mateo");
                Assert.AreEqual<string>(iptcProvider.CopyrightNotice, "Tassography");
                Assert.AreEqual<string>(iptcProvider.LocationCreatedCountry, "United States");
                Assert.AreEqual<string>(iptcProvider.LocationCreatedRegion, "California");
                Assert.AreEqual<string>(iptcProvider.LocationCreatedSubLocation, "San Mateo County Expo Center");

                Assert.AreEqual<DateTime>(iptcProvider.DateCreated, new DateTime(2008, 05, 04));
                Assert.AreEqual<TimeSpan>(iptcProvider.TimeCreated, new TimeSpan(20, 29, 00));
            }
        }

        /// <summary>
        /// Check XmpForExif Metadata provider
        /// </summary>
        [TestMethod]
        public void ReadXmpExifMetadata()
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.SchemaXmpExif))
            {
                XmpExifProvider xmpExifProvider = new XmpExifProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<string>(xmpExifProvider.FocalLength, "6200/1000");
                Assert.AreEqual<Aperture>(xmpExifProvider.MaxApertureValue, new Aperture(95, 32));
                Assert.AreEqual<Aperture>(xmpExifProvider.Aperture, new Aperture(80, 10));
                Assert.AreEqual<string>(xmpExifProvider.ExifVersion, "0220");
                Assert.AreEqual<MetadataEnums.MeteringModes>(xmpExifProvider.MeteringMode, MetadataEnums.MeteringModes.Pattern);
                Assert.AreEqual<IsoSpeed>(xmpExifProvider.IsoSpeed, new IsoSpeed(80));
                Assert.AreEqual<string>(xmpExifProvider.WhiteBalance, "0");
                Assert.AreEqual<double?>(xmpExifProvider.DigitalZoomRatio, 0.0);
                Assert.AreEqual<ExposureBias>(xmpExifProvider.ExposureBias, new ExposureBias("0/3"));
            }
        }

        /// <summary>
        /// Check Fotofly metadata provider
        /// </summary>
        [TestMethod]
        public void WriteFotoflyMetadata()
        {
            // Copy file
            File.Copy(this.samplePhotosFolder + TestPhotos.UnitTest1, this.samplePhotosFolder + TestPhotos.UnitTestTemp1, true);

            // Test Values
            DateTime testDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.UnitTestTemp1, true))
            {
                XmpFotoflyProvider xmpFotoflyProvider = new XmpFotoflyProvider(wpfFileManager.BitmapMetadata);
                xmpFotoflyProvider.AddressCreatedLookupDate = testDate;
                xmpFotoflyProvider.AddressCreatedSource = "Bing Maps for Enterprise";
                xmpFotoflyProvider.DateLastSave = testDate;
                xmpFotoflyProvider.OriginalCameraDate = testDate;
                xmpFotoflyProvider.OriginalCameraFilename = "img_123.jpg";
                xmpFotoflyProvider.DateUtc = testDate;
                xmpFotoflyProvider.UtcOffset = 5;

                wpfFileManager.WriteMetadata();
            }

            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.UnitTestTemp1))
            {
                XmpFotoflyProvider xmpFotoflyProvider = new XmpFotoflyProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<DateTime>(xmpFotoflyProvider.AddressCreatedLookupDate, testDate);
                Assert.AreEqual<string>(xmpFotoflyProvider.AddressCreatedSource, "Bing Maps for Enterprise");
                Assert.AreEqual<DateTime>(xmpFotoflyProvider.DateLastSave, testDate);
                Assert.AreEqual<DateTime>(xmpFotoflyProvider.OriginalCameraDate, testDate);
                Assert.AreEqual<string>(xmpFotoflyProvider.OriginalCameraFilename, "img_123.jpg");
                Assert.AreEqual<DateTime>(xmpFotoflyProvider.DateUtc, testDate);
                Assert.AreEqual<double>(xmpFotoflyProvider.UtcOffset.Value, 5);
            }

            if (File.Exists(this.samplePhotosFolder + TestPhotos.UnitTestTemp1))
            {
                File.Delete(this.samplePhotosFolder + TestPhotos.UnitTestTemp1);
            }
        }

        /// <summary>
        /// Check Xmp Gps metadata provider
        /// </summary>
        [TestMethod]
        public void ReadExifGpsMetadata()
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.GeotaggedExif1))
            {
                GpsProvider gpsProvider = new GpsProvider(wpfFileManager.BitmapMetadata);

                // Expected 37° 48.41667 N 122° 25.38333 W
                Assert.AreEqual<GpsCoordinate>(gpsProvider.Latitude, new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 37, 48.41667), "Latitude");
                Assert.AreEqual<GpsCoordinate>(gpsProvider.Longitude, new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 122, 25.38333), "Longitude");
                Assert.AreEqual<string>(gpsProvider.DateTimeStamp.ToString("u"), "2009-10-10 21:46:24Z", "Satalite Time");
                Assert.AreEqual<double>(gpsProvider.Altitude, -17.464, "Altitude");
                Assert.AreEqual<GpsPosition.Dimensions>(gpsProvider.MeasureMode, GpsPosition.Dimensions.ThreeDimensional, "Measuremode");
                Assert.AreEqual<string>(gpsProvider.VersionID, "2200", "Gps VersionID");
            }
        }
        
        /// <summary>
        /// Check Xmp Gps metadata provider
        /// </summary>
        [TestMethod]
        public void ReadXmpGpsMetadata()
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.GeotaggedXmp1))
            {
                // Expected 51° 55.6784 N 4° 26.6922 E
                XmpExifProvider xmpExifProvider = new XmpExifProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 51, 55.6784), "Latitude");
                Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 4, 26.6922), "Longitude");
                Assert.AreEqual<double>(xmpExifProvider.GpsAltitude, Double.NaN, "Altitude");
                Assert.AreEqual<string>(xmpExifProvider.GpsVersionID, "2.2.0.0", "Gps VersionID");
                Assert.AreEqual<string>(xmpExifProvider.GpsDateTimeStamp.ToString("u", DateTimeFormatInfo.InvariantInfo), "2010-02-01 22:02:34Z", "Satalite Time");
            }

            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.GeotaggedXmp2))
            {
                // Expected 51° 55.7057 N 4° 26.9547 E
                XmpExifProvider xmpExifProvider = new XmpExifProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 51, 55.7057));
                Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 4, 26.9547));
                Assert.AreEqual<string>(xmpExifProvider.GpsDateTimeStamp.ToString("u"), "2010-02-01 22:25:12Z");
            }

            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.GeotaggedXmp3))
            {
                // Expected 51° 55.6767 N 4° 27.2393 E
                XmpExifProvider xmpExifProvider = new XmpExifProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 51, 55.6767));
                Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 4, 27.2393));
                Assert.AreEqual<string>(xmpExifProvider.GpsDateTimeStamp.ToString("u"), "2010-02-01 23:35:07Z");
            }

            using (WpfFileManager wpfFileManager = new WpfFileManager(this.samplePhotosFolder + TestPhotos.GeotaggedXmp4))
            {
                // Expected: GPSAltitude 2/1 GPSLatitude 51,55.4804N GPSLongitude 4,27,9E
                XmpExifProvider xmpExifProvider = new XmpExifProvider(wpfFileManager.BitmapMetadata);

                Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLatitude, new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 51, 55.4804));
                Assert.AreEqual<GpsCoordinate>(xmpExifProvider.GpsLongitude, new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 4, 27, 9));
                Assert.AreEqual<string>(xmpExifProvider.GpsDateTimeStamp.ToString("u"), "2010-02-01 23:00:52Z");
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
