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
        private static string baseSchema = @"http\:\/\/ns.fotofly:";
        private static string schema = @"/xmp/" + XmpFotoflyQueries.baseSchema + "Fotofly/" + XmpFotoflyQueries.baseSchema;

        public static readonly MetdataQuery<BitmapMetadata, BitmapMetadata> FotoflyStruct = new MetdataQuery<BitmapMetadata, BitmapMetadata>(@"/xmp/" + baseSchema + "Fotofly");

        public static readonly MetdataQuery<string, DateTime> UtcDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.schema, "UtcDate");

        public static readonly MetdataQuery<string, double?> UtcOffset = new MetdataQuery<string, double?>(XmpFotoflyQueries.schema, "UtcOffset");

        public static readonly MetdataQuery<string, DateTime> LastEditDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.schema, "LastEditDate");

        public static readonly MetdataQuery<string, string> OriginalCameraFilename = new MetdataQuery<string, string>(XmpFotoflyQueries.schema, "OriginalCameraFilename");

        public static readonly MetdataQuery<string, DateTime> OriginalCameraDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.schema, "OriginalCameraDate");

        public static readonly MetdataQuery<string, DateTime> AddressOfGpsLookupDate = new MetdataQuery<string, DateTime>(XmpFotoflyQueries.schema, "AddressOfGpsLookupDate");

        public static readonly MetdataQuery<string, string> AddressOfGpsSource = new MetdataQuery<string, string>(XmpFotoflyQueries.schema, "AddressOfGpsSource");

        public static readonly MetdataQuery<string, Address> AddressOfGps = new MetdataQuery<string, Address>(XmpFotoflyQueries.schema, "AddressOfGps");

        // Old no longer used
        public static readonly MetdataQuery<string, Address> Address = new MetdataQuery<string, Address>(XmpFotoflyQueries.schema, "Address");
        public static readonly MetdataQuery<string, string> AccuracyOfGps = new MetdataQuery<string, string>(XmpFotoflyQueries.schema, "AccuracyOfGps");
    }
}
