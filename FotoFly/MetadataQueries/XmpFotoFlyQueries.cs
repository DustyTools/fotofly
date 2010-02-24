// <copyright file="XmpFotoflyQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>XmpFotoflyQueries</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public static class XmpFotoflyQueries
    {
        public static readonly string QueryNamespace = @"http\:\/\/ns.fotofly.com:";
        public static readonly string QueryStructName = @"Fotofly";
        public static readonly string QueryStruct = @"/xmp/" + XmpFotoflyQueries.QueryNamespace + XmpFotoflyQueries.QueryStructName;
        public static readonly string QueryRoot = XmpFotoflyQueries.QueryStruct + "/" + XmpFotoflyQueries.QueryNamespace;

        public static readonly string OldQueryNamespace = @"http\:\/\/ns.fotofly:";
        public static readonly string OldQueryStructName = "FotoFly";
        public static readonly string OldQueryStruct = @"/xmp/" + XmpFotoflyQueries.OldQueryNamespace + XmpFotoflyQueries.OldQueryStructName;
        public static readonly string OldQueryRoot = XmpFotoflyQueries.OldQueryStruct + "/" + XmpFotoflyQueries.OldQueryNamespace;

        public static readonly MetdataQuery<BitmapMetadata, BitmapMetadata> FotoflyStruct = new MetdataQuery<BitmapMetadata, BitmapMetadata>(XmpFotoflyQueries.QueryStruct);

        public static readonly MetdataQuery<string, DateTime> UtcDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryRoot, "UtcDate");

        public static readonly MetdataQuery<string, double?> UtcOffset = new MetdataQuery<string, double?>(XmpFotoflyQueries.QueryRoot, "UtcOffset");

        public static readonly MetdataQuery<string, DateTime> LastEditDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryRoot, "LastEditDate");

        public static readonly MetdataQuery<string, string> OriginalCameraFilename = new MetdataQuery<string, string>(XmpFotoflyQueries.QueryRoot, "OriginalCameraFilename");

        public static readonly MetdataQuery<string, DateTime> OriginalCameraDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryRoot, "OriginalCameraDate");

        public static readonly MetdataQuery<string, DateTime> AddressOfGpsLookupDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryRoot, "AddressOfGpsLookupDate");

        public static readonly MetdataQuery<string, string> AddressOfGpsSource = new MetdataQuery<string, string>(XmpFotoflyQueries.QueryRoot, "AddressOfGpsSource");

        public static readonly MetdataQuery<string, Address> AddressOfGps = new MetdataQuery<string, Address>(XmpFotoflyQueries.QueryRoot, "AddressOfGps");

        // Old no longer used
        public static readonly MetdataQuery<string, Address> Address = new MetdataQuery<string, Address>(XmpFotoflyQueries.QueryRoot, "Address");
        public static readonly MetdataQuery<string, string> AccuracyOfGps = new MetdataQuery<string, string>(XmpFotoflyQueries.QueryRoot, "AccuracyOfGps");
    }
}
