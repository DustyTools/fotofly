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
        public static readonly string QuerySchema = @"http\:\/\/ns.fotofly:";
        public static readonly string QueryPrefix = @"/xmp/" + XmpFotoflyQueries.QuerySchema + "Fotofly/" + XmpFotoflyQueries.QuerySchema;

        public static readonly MetdataQuery<BitmapMetadata, BitmapMetadata> FotoflyStruct = new MetdataQuery<BitmapMetadata, BitmapMetadata>(@"/xmp/" + QuerySchema + "Fotofly");

        public static readonly MetdataQuery<string, DateTime> UtcDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryPrefix, "UtcDate");

        public static readonly MetdataQuery<string, double?> UtcOffset = new MetdataQuery<string, double?>(XmpFotoflyQueries.QueryPrefix, "UtcOffset");

        public static readonly MetdataQuery<string, DateTime> LastEditDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryPrefix, "LastEditDate");

        public static readonly MetdataQuery<string, string> OriginalCameraFilename = new MetdataQuery<string, string>(XmpFotoflyQueries.QueryPrefix, "OriginalCameraFilename");

        public static readonly MetdataQuery<string, DateTime> OriginalCameraDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryPrefix, "OriginalCameraDate");

        public static readonly MetdataQuery<string, DateTime> AddressOfGpsLookupDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.QueryPrefix, "AddressOfGpsLookupDate");

        public static readonly MetdataQuery<string, string> AddressOfGpsSource = new MetdataQuery<string, string>(XmpFotoflyQueries.QueryPrefix, "AddressOfGpsSource");

        public static readonly MetdataQuery<string, Address> AddressOfGps = new MetdataQuery<string, Address>(XmpFotoflyQueries.QueryPrefix, "AddressOfGps");

        // Old no longer used
        public static readonly MetdataQuery<string, Address> Address = new MetdataQuery<string, Address>(XmpFotoflyQueries.QueryPrefix, "Address");
        public static readonly MetdataQuery<string, string> AccuracyOfGps = new MetdataQuery<string, string>(XmpFotoflyQueries.QueryPrefix, "AccuracyOfGps");
    }
}
