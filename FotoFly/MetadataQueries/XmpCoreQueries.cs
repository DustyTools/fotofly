// <copyright file="XmpCoreQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>XmpCoreQueries</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public static class XmpCoreQueries
    {
        // Padding used when adding properties to the File
        public static readonly MetdataQuery<Int32, int> Padding = new MetdataQuery<Int32, int>("/xmp/PaddingSchema:Padding");

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
        public static readonly MetdataQuery<string, string> People = new MetdataQuery<string, string>("/xmp/mediapro:People");

        public static readonly MetdataQuery<string, string> State = new MetdataQuery<string, string>("/xmp/State");
        public static readonly MetdataQuery<string, string> Country = new MetdataQuery<string, string>("/xmp/Country");

        // xmp:DateTimeModified
        public static readonly MetdataQuery<string, string> DateTimeModified = new MetdataQuery<string, string>("/xmp/ModifyDate");

        // xmp:DateTimeDigitised
        public static readonly MetdataQuery<string, string> DateTimeDigitised = new MetdataQuery<string, string>("/xmp/CreateDate");

        // Unused Queries
        public static readonly MetdataQuery<string, string> AuthorsPosition = new MetdataQuery<string, string>("/xmp/AuthorsPosition");
        public static readonly MetdataQuery<string, string> Category = new MetdataQuery<string, string>("/xmp/Category");
        public static readonly MetdataQuery<string, string> Credit = new MetdataQuery<string, string>("/xmp/Credit");
        public static readonly MetdataQuery<string, string> Urgency = new MetdataQuery<string, string>("/xmp/Urgency");
        public static readonly MetdataQuery<string, string> Location = new MetdataQuery<string, string>("/xmp/Location");
        public static readonly MetdataQuery<string, string> Creators = new MetdataQuery<string, string>("/xmp/creator/{ulong=0}");
        public static readonly MetdataQuery<string, string> Marked = new MetdataQuery<string, string>("/xmp/Marked");
        public static readonly MetdataQuery<string, string> WebStatement = new MetdataQuery<string, string>("/xmp/WebStatement");
        public static readonly MetdataQuery<string, string> IntellectualGenre = new MetdataQuery<string, string>("/xmp/IntellectualGenre");
        public static readonly MetdataQuery<string, string> Scenes = new MetdataQuery<string, string>("/xmp/Scene/{ulong=0}");
        public static readonly MetdataQuery<string, string> SubjectCodes = new MetdataQuery<string, string>("/xmp/SubjectCode/{ulong=0}");

        // CreatorContactInfo
        public static readonly MetdataQuery<string, string> CiAdrExtadr = new MetdataQuery<string, string>("/xmp/CreatorContactInfo/CiAdrExtadr");
        public static readonly MetdataQuery<string, string> CiAdrCity = new MetdataQuery<string, string>("/xmp/CreatorContactInfo/CiAdrCity");
        public static readonly MetdataQuery<string, string> CiAdrRegion = new MetdataQuery<string, string>("/xmp/CreatorContactInfo/CiAdrRegion");
        public static readonly MetdataQuery<string, string> CiAdrPcode = new MetdataQuery<string, string>("/xmp/CreatorContactInfo/CiAdrPcode");
        public static readonly MetdataQuery<string, string> CiAdrCtry = new MetdataQuery<string, string>("/xmp/CreatorContactInfo/CiAdrCtry");
        public static readonly MetdataQuery<string, string> CiTelWork = new MetdataQuery<string, string>("/xmp/CreatorContactInfo/CiTelWork");
        public static readonly MetdataQuery<string, string> CiEmailWork = new MetdataQuery<string, string>("/xmp/CreatorContactInfo/CiEmailWork");
        public static readonly MetdataQuery<string, string> CiUrlWork = new MetdataQuery<string, string>("/xmp/CreatorContactInfo/CiUrlWork");
    }
}
