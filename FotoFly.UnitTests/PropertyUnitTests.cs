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
    public class PropertyUnitTests
    {
        public PropertyUnitTests()
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
        /// Check GpsPosition for all valid combinations
        /// </summary>
        [TestMethod]
        public void GpsPositionTest()
        {
            // Check Altitude only
            GpsPosition gpsPosition = new GpsPosition();
            gpsPosition.Altitude = 5;

            Assert.AreEqual<GpsPosition.Dimensions>(gpsPosition.Dimension, GpsPosition.Dimensions.OneDimensional, "AltitudeOnly Dimension");
            Assert.AreEqual<double>(gpsPosition.Altitude, 5, "AltitudeOnly Altitude");

            // Check Lat & Lon
            gpsPosition = new GpsPosition();
            gpsPosition.Latitude = new GpsCoordinate(GpsCoordinate.LatitudeRef.South, 15);
            gpsPosition.Longitude = new GpsCoordinate(GpsCoordinate.LongitudeRef.West, 100);

            Assert.AreEqual<GpsPosition.Dimensions>(gpsPosition.Dimension, GpsPosition.Dimensions.TwoDimensional, "Lat&Lon Dimension");
            Assert.AreEqual<double>(gpsPosition.Longitude.Numeric, -100, "Lat&Lon Longitude");
            Assert.AreEqual<double>(gpsPosition.Latitude.Numeric, -15, "Lat&Lon Latitude");

            // Check Lat, Lon & Altitude
            gpsPosition = new GpsPosition();
            gpsPosition.Altitude = 5;
            gpsPosition.Latitude = new GpsCoordinate(GpsCoordinate.LatitudeRef.North, 57);
            gpsPosition.Longitude = new GpsCoordinate(GpsCoordinate.LongitudeRef.East, 12);

            Assert.AreEqual<GpsPosition.Dimensions>(gpsPosition.Dimension, GpsPosition.Dimensions.ThreeDimensional, "LatLonAlt Dimension");
            Assert.AreEqual<double>(gpsPosition.Altitude, 5, "LatLonAlt Altitude");
            Assert.AreEqual<double>(gpsPosition.Latitude.Numeric, 57, "LatLonAlt Latitude");
            Assert.AreEqual<double>(gpsPosition.Longitude.Numeric, 12, "LatLonAlt Longitude");
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
