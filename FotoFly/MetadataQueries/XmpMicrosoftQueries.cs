// <copyright file="XmpMicrosoftQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>Xmp Microsoft Queries</summary>
namespace FotoFly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public static class XmpMicrosoftQueries
    {
        public static readonly MetdataQuery Rating = new MetdataQuery("/xmp/MicrosoftPhoto:Rating", typeof(string));

        // "DateAcquired" (LPWSTR) ["http://ns.microsoft.com/photo/1.0/" (LPWSTR)]
        public static readonly MetdataQuery DateAcquired = new MetdataQuery("/xmp/MicrosoftPhoto:DateAcquired", typeof(DateTime));

        // "RegionInfo" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/" (LPWSTR)]
        public static readonly MetdataQuery RegionInfo = new MetdataQuery(@"/xmp/MP:RegionInfo", typeof(BitmapMetadata));

        // MPRI:DateRegionsValid
        // Does not appear to be ever used
        public static readonly MetdataQuery RegionsLastUpdate = new MetdataQuery(@"/xmp/MP:RegionInfo/MPRI:DateRegionsValid", typeof(DateTime));

        // "Regions" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/RegionInfo#" (LPWSTR)]
        public static readonly MetdataQuery Regions = new MetdataQuery(@"/xmp/MP:RegionInfo/MPRI:Regions", typeof(BitmapMetadata));

        // Region query, meant to be used with String.Format to replace {0} with the appropriate region
        public static readonly MetdataQuery Region = new MetdataQuery(@"/xmp/MP:RegionInfo/MPRI:Regions/{{ulong={0}}}", typeof(BitmapMetadata));

        // "PersonDisplayName" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        public static readonly MetdataQuery RegionPersonDisplayName = new MetdataQuery(@"/MPReg:PersonDisplayName", typeof(string));

        // "Rectangle" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        public static readonly MetdataQuery RegionRectangle = new MetdataQuery(@"/MPReg:Rectangle", typeof(string));

        // "PersonEmailDigest" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        public static readonly MetdataQuery RegionPersonEmailDigest = new MetdataQuery(@"/MPReg:PersonEmailDigest", typeof(string));
        public static readonly MetdataQuery RegionPersonLiveIdCID = new MetdataQuery(@"/MPReg:PersonLiveIdCID", typeof(string));
    }
}
