namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class XmpQueries
    {
        public static readonly string Struct = "xmpstruct";
        public static readonly string Bag = "xmpbag";

        // Padding used when adding properties to the File
        public static readonly string Padding = "/xmp/PaddingSchema:Padding";

        // XMP People
        public static readonly string People = "/xmp/mediapro:People";

        // "RegionInfo" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/" (LPWSTR)]
        // "Regions" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/RegionInfo#" (LPWSTR)]
        // "Rectangle" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        // "PersonDisplayName" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        // "PersonEmailDigest" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        // MPRI:DateRegionsValid
        public static readonly string MicrosoftRegionInfo = @"/xmp/MP:RegionInfo";
        public static readonly string MicrosoftRegionsLastUpdate = @"/xmp/MP:RegionInfo/MPRI:DateRegionsValid";

        public static readonly string MicrosoftRegions = @"/xmp/MP:RegionInfo/MPRI:Regions";
        public static readonly string MicrosoftRegion = @"/xmp/MP:RegionInfo/MPRI:Regions/{{ulong={0}}}";

        public static readonly string MicrosoftPersonDisplayName = @"/MPReg:PersonDisplayName";
        public static readonly string MicrosoftRectangle = @"/MPReg:Rectangle";
        public static readonly string MicrosoftPersonEmailDigest = @"/MPReg:PersonEmailDigest";
        public static readonly string MicrosoftPersonLiveIdCID = @"/MPReg:PersonLiveIdCID";
    }
}
