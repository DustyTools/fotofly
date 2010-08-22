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
        public static readonly string QueryNamespace = @"http\:\/\/ns.tassography.com\/fotofly:";
        public static readonly string QueryStructName = @"Fotofly";
        public static readonly string QueryStruct = @"/xmp/" + XmpFotoflyQueries.QueryNamespace + XmpFotoflyQueries.QueryStructName;
        public static readonly string QueryRoot = XmpFotoflyQueries.QueryStruct + "/" + XmpFotoflyQueries.QueryNamespace;

        public static readonly MetdataQuery<BitmapMetadata, BitmapMetadata> FotoflyStruct = new MetdataQuery<BitmapMetadata, BitmapMetadata>(XmpFotoflyQueries.QueryStruct);

        public static readonly MetdataQuery<string, DateTime> DateUtc = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryRoot, "DateUtc");

        public static readonly MetdataQuery<string, double?> UtcOffset = new MetdataQuery<string, double?>(XmpFotoflyQueries.QueryRoot, "UtcOffset");

        public static readonly MetdataQuery<string, DateTime> DateLastSave = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryRoot, "DateLastSave");

        public static readonly MetdataQuery<string, string> OriginalCameraFilename = new MetdataQuery<string, string>(XmpFotoflyQueries.QueryRoot, "OriginalCameraFilename");

        public static readonly MetdataQuery<string, DateTime> OriginalCameraDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryRoot, "OriginalCameraDate");

        public static readonly MetdataQuery<string, DateTime> AddressOfLocationCreatedLookupDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryRoot, "AddressOfLocationCreatedLookupDate");

        public static readonly MetdataQuery<string, string> AddressOfLocationCreatedSource = new MetdataQuery<string, string>(XmpFotoflyQueries.QueryRoot, "AddressOfLocationCreatedSource");

        public static readonly MetdataQuery<string, string> GpsPositionOfLocationShownSource = new MetdataQuery<string, string>(XmpFotoflyQueries.QueryRoot, "GpsPositionOfLocationShownSource");
    }
}
