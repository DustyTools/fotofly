// <copyright file="XmpCoreQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>XmpCoreQueries</summary>
namespace FotoFly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public static class XmpCoreQueries
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

        // XMP Basic Schema
        // The XMP Basic Schema contains properties that provide basic descriptive information.
        // - The schema namespace URI is http://ns.adobe.com/xap/1.0/
        // - The preferred schema namespace prefix is xmp

        // People
        public static readonly MetdataQuery People = new MetdataQuery("/xmp/mediapro:People", typeof(string));

        public static readonly MetdataQuery State = new MetdataQuery("/xmp/State", typeof(string));
        public static readonly MetdataQuery Country = new MetdataQuery("/xmp/Country", typeof(string));

        // Unused Queries
        public static readonly MetdataQuery AuthorsPosition = new MetdataQuery("/xmp/AuthorsPosition", typeof(string));
        public static readonly MetdataQuery Category = new MetdataQuery("/xmp/Category", typeof(string));
        public static readonly MetdataQuery Credit = new MetdataQuery("/xmp/Credit", typeof(string));
        public static readonly MetdataQuery Urgency = new MetdataQuery("/xmp/Urgency", typeof(string));
        public static readonly MetdataQuery Location = new MetdataQuery("/xmp/Location", typeof(string));
        public static readonly MetdataQuery Creators = new MetdataQuery("/xmp/creator/{ulong=0}", typeof(string));
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
