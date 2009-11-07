// <copyright file="XmpQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>XmpQueries</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class XmpQueries
    {
        // Padding used when adding properties to the File
        public static readonly string Padding = "/xmp/PaddingSchema:Padding";

        // Data types
        public static readonly string Struct = "xmpstruct";
        public static readonly string Bag = "xmpbag";

        // XMP People
        public static readonly string People = "/xmp/mediapro:People";

        // "RegionInfo" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/" (LPWSTR)]
        public static readonly string MicrosoftRegionInfo = @"/xmp/MP:RegionInfo";

        // MPRI:DateRegionsValid
        public static readonly string MicrosoftRegionsLastUpdate = @"/xmp/MP:RegionInfo/MPRI:DateRegionsValid";

        // "Regions" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/RegionInfo#" (LPWSTR)]
        public static readonly string MicrosoftRegions = @"/xmp/MP:RegionInfo/MPRI:Regions";

        // Region query, meant to be used with String.Format to replace {0} with the appropriate region
        public static readonly string MicrosoftRegion = @"/xmp/MP:RegionInfo/MPRI:Regions/{{ulong={0}}}";

        // "PersonDisplayName" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        public static readonly string MicrosoftPersonDisplayName = @"/MPReg:PersonDisplayName";

        // "Rectangle" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        public static readonly string MicrosoftRectangle = @"/MPReg:Rectangle";

        // "PersonEmailDigest" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        public static readonly string MicrosoftPersonEmailDigest = @"/MPReg:PersonEmailDigest";
        public static readonly string MicrosoftPersonLiveIdCID = @"/MPReg:PersonLiveIdCID";
    }
}
