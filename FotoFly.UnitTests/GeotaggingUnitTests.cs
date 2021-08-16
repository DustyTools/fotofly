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

    [TestClass]
    public class GeotaggingUnitTests
    {
        private string samplesFolder = @"..\..\..\~Sample Files\GpxTracks\";

        public GeotaggingUnitTests()
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
			GlobalUnitTests.InitializeCulture();
        }
        #endregion

        /// <summary>
        /// Check test photo can be read and metadata loaded into memory
        /// </summary>
        [TestMethod]
        public void ReadGpxFile()
        {
            ////GpsTrackLookup gpsTrackManager = new GpsTrackLookup();
            ////gpsTrackManager.Add(GpseXchangeFormat.GpxFileManager.Read(this.samplesFolder + "GarminDakota20.gpx"));
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
