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
    using System.Windows.Media.Imaging;

    public static class XmpQueries
    {
        // Padding used when adding properties to the File
        public static readonly MetdataQuery Padding = new MetdataQuery("/xmp/PaddingSchema:Padding", typeof(Int32));

        // XMP Block Types
        public static readonly string StructBlock = "xmpstruct";

        // XMP Alt has a default value x-default where you can set the value
        public static readonly string AltBlock = "xmpalt";

        // XMP Seq/XMP Bag are indexed.  You can set multiple values using {ulong=<offset>}
        public static readonly string BagBlock = "xmpbag";
        public static readonly string SeqBlock = "xmpseq";

        // "DateAcquired" (LPWSTR) ["http://ns.microsoft.com/photo/1.0/" (LPWSTR)]
        public static readonly MetdataQuery DateAcquired = new MetdataQuery("/xmp/MicrosoftPhoto:DateAcquired", typeof(DateTime));

        // People
        public static readonly MetdataQuery People = new MetdataQuery("/xmp/mediapro:People", typeof(string));

        // "RegionInfo" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/" (LPWSTR)]
        public static readonly MetdataQuery MicrosoftRegionInfo = new MetdataQuery(@"/xmp/MP:RegionInfo", typeof(BitmapMetadata));

        // MPRI:DateRegionsValid
        public static readonly MetdataQuery MicrosoftRegionsLastUpdate = new MetdataQuery(@"/xmp/MP:RegionInfo/MPRI:DateRegionsValid", typeof(DateTime));

        // "Regions" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/RegionInfo#" (LPWSTR)]
        public static readonly MetdataQuery MicrosoftRegions = new MetdataQuery(@"/xmp/MP:RegionInfo/MPRI:Regions", typeof(BitmapMetadata));

        // Region query, meant to be used with String.Format to replace {0} with the appropriate region
        public static readonly MetdataQuery MicrosoftRegion = new MetdataQuery(@"/xmp/MP:RegionInfo/MPRI:Regions/{{ulong={0}}}", typeof(BitmapMetadata));

        // "PersonDisplayName" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        public static readonly MetdataQuery MicrosoftPersonDisplayName = new MetdataQuery(@"/MPReg:PersonDisplayName", typeof(string));

        // "Rectangle" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        public static readonly MetdataQuery MicrosoftRectangle = new MetdataQuery(@"/MPReg:Rectangle", typeof(string));

        // "PersonEmailDigest" (LPWSTR) ["http://ns.microsoft.com/photo/1.2/t/Region#" (LPWSTR)]
        public static readonly MetdataQuery MicrosoftPersonEmailDigest = new MetdataQuery(@"/MPReg:PersonEmailDigest", typeof(string));
        public static readonly MetdataQuery MicrosoftPersonLiveIdCID = new MetdataQuery(@"/MPReg:PersonLiveIdCID", typeof(string));
    
        // Unused Queries
        public static readonly MetdataQuery AuthorsPosition = new MetdataQuery("/xmp/AuthorsPosition", typeof(string));
        public static readonly MetdataQuery Category = new MetdataQuery("/xmp/Category", typeof(string));
        public static readonly MetdataQuery State = new MetdataQuery("/xmp/State", typeof(string));
        public static readonly MetdataQuery Country = new MetdataQuery("/xmp/Country", typeof(string));
        public static readonly MetdataQuery Credit = new MetdataQuery("/xmp/Credit", typeof(string));
        public static readonly MetdataQuery Urgency = new MetdataQuery("/xmp/Urgency", typeof(string));
        public static readonly MetdataQuery Location = new MetdataQuery("/xmp/Location", typeof(string));
        public static readonly MetdataQuery Creators = new MetdataQuery("/xmp/creator/{ulong=0}", typeof(string));
        public static readonly MetdataQuery Rights = new MetdataQuery("/xmp/rights/x-default", typeof(string));
        public static readonly MetdataQuery Marked = new MetdataQuery("/xmp/Marked", typeof(string));
        public static readonly MetdataQuery WebStatement = new MetdataQuery("/xmp/WebStatement", typeof(string));
        public static readonly MetdataQuery IntellectualGenre = new MetdataQuery("/xmp/IntellectualGenre", typeof(string));
        public static readonly MetdataQuery Scenes = new MetdataQuery("/xmp/Scene/{ulong=0}", typeof(string));
        public static readonly MetdataQuery SubjectCodes = new MetdataQuery("/xmp/SubjectCode/{ulong=0}", typeof(string));

        // CreatorContactInfo
        public static readonly MetdataQuery CiAdrExtadr = new MetdataQuery("/xmp/CreatorContactInfo/CiAdrExtadr", typeof(string));
        public static readonly MetdataQuery CiAdrCity = new MetdataQuery("/xmp/CreatorContactInfo/CiAdrCity", typeof(string));
        public static readonly MetdataQuery CiAdrRegion = new MetdataQuery("/xmp/CreatorContactInfo/CiAdrRegion", typeof(string));
        public static readonly MetdataQuery CiAdrPcode = new MetdataQuery("/xmp/CreatorContactInfo/CiAdrPcode", typeof(string));
        public static readonly MetdataQuery CiAdrCtry = new MetdataQuery("/xmp/CreatorContactInfo/CiAdrCtry", typeof(string));
        public static readonly MetdataQuery CiTelWork = new MetdataQuery("/xmp/CreatorContactInfo/CiTelWork", typeof(string));
        public static readonly MetdataQuery CiEmailWork = new MetdataQuery("/xmp/CreatorContactInfo/CiEmailWork", typeof(string));
        public static readonly MetdataQuery CiUrlWork = new MetdataQuery("/xmp/CreatorContactInfo/CiUrlWork", typeof(string));
    }
}
