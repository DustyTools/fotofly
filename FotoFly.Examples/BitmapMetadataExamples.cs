namespace Fotofly.Examples
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    using Fotofly.BitmapMetadataTools;
    using Fotofly.MetadataProviders;
    using Fotofly.MetadataQueries;

    public static class BitmapMetadataExamples
    {
        public static void ReadMetadata(string inputFile)
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(inputFile))
            {
                FileMetadata wpfMetadata = new FileMetadata(wpfFileManager.BitmapMetadata);

                Debug.WriteLine(wpfMetadata.ExifProvider.Aperture);
            }
        }

        public static void WriteMetadata(string inputFile)
        {
            File.Copy(inputFile, "BitmapMetadataExamples.WriteMetadata.jpg", true);

            using (WpfFileManager wpfFileManager = new WpfFileManager(inputFile, true))
            {
                FileMetadata wpfMetadata = new FileMetadata(wpfFileManager.BitmapMetadata);
                wpfMetadata.IptcProvider.LocationCreatedCountry = "United States";

                wpfFileManager.WriteMetadata();
            }
        }

        public static void ReadIPTCAddress(string inputFile)
        {
            // Queries for the IPTC Address fields
            // Note: Region is normally the State or County
            // iptcCountry = "/app13/irb/8bimiptc/iptc/Country\/Primary Location Name";
            // iptcRegion = "/app13/irb/8bimiptc/iptc/Province\/State";
            // iptcCity = "/app13/irb/8bimiptc/iptc/City";
            // iptcSubLocation = "/app13/irb/8bimiptc/iptc/Sub-location";
            string iptcCity = @"/app13/irb/8bimiptc/iptc/City";

            using (WpfFileManager wpfFileManager = new WpfFileManager(inputFile))
            {
                // Check there's city data
                if (wpfFileManager.BitmapMetadata.ContainsQuery(iptcCity))
                {
                    // Dump it to the console
                    Debug.WriteLine(wpfFileManager.BitmapMetadata.GetQuery(iptcCity));
                }
            }
        }

        public static void ReadGpsAltitude(string inputFile)
        {
            // Grab copy of BitmapMetadata
            using (WpfFileManager wpfFileManager = new WpfFileManager(inputFile))
            {
                // Grab the GpsAltitudeRef
                string altitudeRef = wpfFileManager.BitmapMetadata.GetQuery("/app1/ifd/Gps/subifd:{uint=5}").ToString();

                // Grab GpsAltitude as a ulong
                ulong rational = (ulong)wpfFileManager.BitmapMetadata.GetQuery("/app1/ifd/Gps/subifd:{uint=6}");

                // Now shift & mask out the upper and lower parts to get the numerator and the denominator
                uint numerator = (uint)(rational & 0xFFFFFFFFL);
                uint denominator = (uint)((rational & 0xFFFFFFFF00000000L) >> 32);

                // And finally turn it into a double
                double altitude = Math.Round(Convert.ToDouble(numerator) / Convert.ToDouble(denominator), 3);
            }
        }

        public static void ReadGpsLatitude(string inputFile)
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(inputFile))
            {
                // Grab the GpsLatitudeRef
                // 'N' indicates north latitude, and 'S' is south latitude
                string latitudeRef = wpfFileManager.BitmapMetadata.GetQuery("/app1/ifd/Gps/subifd:{uint=1}").ToString();

                // Grab GpsLatitude as a ulong array
                ulong[] rational = (ulong[])wpfFileManager.BitmapMetadata.GetQuery("/app1/ifd/Gps/subifd:{uint=2}");
                double[] latitude = new double[3];

                // Read and convert each of the rationals into a double
                for (int i = 0; i < 3; i++)
                {
                    // Now shift & mask out the upper and lower parts to get the numerator and the denominator
                    uint numerator = (uint)(rational[i] & 0xFFFFFFFFL);
                    uint denominator = (uint)((rational[i] & 0xFFFFFFFF00000000L) >> 32);

                    latitude[i] = Math.Round(Convert.ToDouble(numerator) / Convert.ToDouble(denominator), 3);
                }
            }
        }

        public static void WriteIPTCAddres(string inputFile)
        {
            File.Copy(inputFile, "BitmapMetadataExamples.WriteIPTCAddres.jpg", true);

            // Queries for the IPTC Address fields
            // Note: Region is normally the State or County
            string iptcCountry = @"/app13/irb/8bimiptc/iptc/Country\/Primary Location Name";
            string iptcRegion = @"/app13/irb/8bimiptc/iptc/Province\/State";
            string iptcCity = @"/app13/irb/8bimiptc/iptc/City";
            string iptcSubLocation = @"/app13/irb/8bimiptc/iptc/Sub-location";

            using (WpfFileManager wpfFileManager = new WpfFileManager(inputFile, true))
            {
                // Use SetQuery to store the IPTC Address fields
                wpfFileManager.BitmapMetadata.SetQuery(iptcCity, "IPTC City");
                wpfFileManager.BitmapMetadata.SetQuery(iptcCountry, "IPTC Country");
                wpfFileManager.BitmapMetadata.SetQuery(iptcRegion, "IPTC Region");
                wpfFileManager.BitmapMetadata.SetQuery(iptcSubLocation, "IPTC SubLocation");

                wpfFileManager.WriteMetadata();
            }
        }

        public static void RemoveIPTCAddres(string inputFile)
        {
            File.Copy(inputFile, "BitmapMetadataExamples.RemoveIPTCAddres.jpg", true);

            // Queries for the IPTC Address fields
            // Note: Region is normally the State or County
            string iptcCountry = @"/app13/irb/8bimiptc/iptc/Country\/Primary Location Name";
            string iptcRegion = @"/app13/irb/8bimiptc/iptc/Province\/State";
            string iptcCity = @"/app13/irb/8bimiptc/iptc/City";
            string iptcSubLocation = @"/app13/irb/8bimiptc/iptc/Sub-location";

            using (WpfFileManager wpfFileManager = new WpfFileManager(inputFile, true))
            {
                // Remove IPTC Address fields
                wpfFileManager.BitmapMetadata.RemoveQuery(iptcCity);
                wpfFileManager.BitmapMetadata.RemoveQuery(iptcCountry);
                wpfFileManager.BitmapMetadata.RemoveQuery(iptcRegion);
                wpfFileManager.BitmapMetadata.RemoveQuery(iptcSubLocation);

                wpfFileManager.WriteMetadata();
            }
        }

        public static void ReadWLPGRegions(string inputFile)
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(inputFile))
            {
                // Declare a bunch of XMP paths (see my last blog for details)
                string microsoftRegions = @"/xmp/MP:RegionInfo/MPRI:Regions";
                string microsoftPersonDisplayName = @"/MPReg:PersonDisplayName";
                string microsoftRectangle = @"/MPReg:Rectangle";

                // Check there is a RegionInfo
                if (wpfFileManager.BitmapMetadata.ContainsQuery(microsoftRegions))
                {
                    BitmapMetadata regionsMetadata = wpfFileManager.BitmapMetadata.GetQuery(microsoftRegions) as BitmapMetadata;

                    // Loop through each Region
                    foreach (string regionQuery in regionsMetadata)
                    {
                        string regionFullQuery = microsoftRegions + regionQuery;

                        // Query for all the data for this region
                        BitmapMetadata regionMetadata = wpfFileManager.BitmapMetadata.GetQuery(regionFullQuery) as BitmapMetadata;

                        if (regionMetadata != null)
                        {
                            if (regionMetadata.ContainsQuery(microsoftPersonDisplayName))
                            {
                                Console.WriteLine("PersonDisplayName:\t"
                                   + regionMetadata.GetQuery(XmpMicrosoftQueries.RegionPersonDisplayName.Query).ToString());
                            }

                            if (regionMetadata.ContainsQuery(microsoftRectangle))
                            {
                                Console.WriteLine("Rectangle:\t\t"
                                   + regionMetadata.GetQuery(XmpMicrosoftQueries.RegionRectangle.Query).ToString());
                            }
                        }
                    }
                }
            }
        }

        public static void CreateWLPGRegions(string inputFile)
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(inputFile, true))
            {
                // Declare a bunch of XMP paths (see my last blog for details)
                string microsoftRegionInfo = @"/xmp/MP:RegionInfo";
                string microsoftRegions = @"/xmp/MP:RegionInfo/MPRI:Regions";
                string microsoftPersonDisplayName = @"/MPReg:PersonDisplayName";

                wpfFileManager.BitmapMetadata.SetQuery(microsoftRegionInfo, new BitmapMetadata("xmpstruct"));

                wpfFileManager.BitmapMetadata.SetQuery(microsoftRegions, new BitmapMetadata("xmpbag"));

                wpfFileManager.BitmapMetadata.SetQuery(microsoftRegions + "/{ulong=0}", new BitmapMetadata("xmpstruct"));

                wpfFileManager.BitmapMetadata.SetQuery(microsoftRegions + "/{ulong=0}" + microsoftPersonDisplayName, "Test " + DateTime.Now.ToString());

                wpfFileManager.BitmapMetadata.SetQuery(microsoftRegions + "/{ulong=1}", new BitmapMetadata("xmpstruct"));

                wpfFileManager.BitmapMetadata.SetQuery(microsoftRegions + "/{ulong=1}" + microsoftPersonDisplayName, "Test " + DateTime.Now.ToString());

                wpfFileManager.WriteMetadata();
            }
        }

        public static void UpdateWLPGRegions(string inputFile)
        {
            using (WpfFileManager wpfFileManager = new WpfFileManager(inputFile, true))
            {
                // Declare a bunch of XMP paths (see my last blog for details)
                string microsoftRegions = @"/xmp/MP:RegionInfo/MPRI:Regions";
                string microsoftPersonDisplayName = @"/MPReg:PersonDisplayName";
                string microsoftRectangle = @"/MPReg:Rectangle";

                // Check the sourceMetadata contains a Region
                if (wpfFileManager.BitmapMetadata.ContainsQuery(microsoftRegions))
                {
                    // Get Region Data as BitmapMetadata
                    BitmapMetadata regionsMetadata = wpfFileManager.BitmapMetadata.GetQuery(microsoftRegions) as BitmapMetadata;

                    // Loop through each Region
                    foreach (string regionQuery in regionsMetadata)
                    {
                        string regionFullQuery = microsoftRegions + regionQuery;

                        // Grab Region metadata as BitmapMetadata
                        BitmapMetadata regionMetadata = wpfFileManager.BitmapMetadata.GetQuery(regionFullQuery) as BitmapMetadata;

                        // Change Rectangle & DisplayName to test values
                        if (regionMetadata != null)
                        {
                            // If the region has a DisplayName, change the value
                            if (regionMetadata.ContainsQuery(microsoftPersonDisplayName))
                            {
                                regionMetadata.SetQuery(XmpMicrosoftQueries.RegionPersonDisplayName.Query, "test");
                            }

                            // If the region has a DisplayName, change the value
                            if (regionMetadata.ContainsQuery(microsoftRectangle))
                            {
                                regionMetadata.SetQuery(XmpMicrosoftQueries.RegionRectangle.Query, "test");
                            }
                        }

                        // Write the Region back to Regions
                        wpfFileManager.BitmapMetadata.SetQuery(regionFullQuery, regionMetadata);
                    }

                    // Write the Regions back to Root Metadata
                    wpfFileManager.BitmapMetadata.SetQuery(microsoftRegions, regionsMetadata);
                }

                wpfFileManager.WriteMetadata();
            }
        }
    }
}
