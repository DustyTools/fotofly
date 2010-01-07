// <copyright file="XmpFotoFlyQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>XmpFotoFlyQueries</summary>
namespace FotoFly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public static class XmpFotoFlyQueries
    {
        public static readonly MetdataQuery<BitmapMetadata, FotoFlyMetadata> FotoFlyStruct = new MetdataQuery<BitmapMetadata, FotoFlyMetadata>(@"/xmp/http\:\/\/ns.fotofly:FotoFly");

        public static readonly MetdataQuery<string, DateTime> UtcDate = new MetdataQuery<string, DateTime>(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:UtcDate");

        public static readonly MetdataQuery<string, double?> UtcOffset = new MetdataQuery<string, double?>(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:UtcOffset");

        public static readonly MetdataQuery<string, DateTime> LastEditDate = new MetdataQuery<string, DateTime>(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:LastEditDate");

        public static readonly MetdataQuery<string, string> OriginalCameraFilename = new MetdataQuery<string, string>(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:OriginalCameraFilename");

        public static readonly MetdataQuery<string, DateTime> OriginalCameraDate = new MetdataQuery<string, DateTime>(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:OriginalCameraDate");

        public static readonly MetdataQuery<string, string> AccuracyOfGps = new MetdataQuery<string, string>(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:AccuracyOfGps");

        public static readonly MetdataQuery<string, DateTime> AddressOfGpsLookupDate = new MetdataQuery<string, DateTime>(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:AddressOfGpsLookupDate");

        public static readonly MetdataQuery<string, string> AddressOfGpsSource = new MetdataQuery<string, string>(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:AddressOfGpsSource");

        public static readonly MetdataQuery<string, Address> AddressOfGps = new MetdataQuery<string, Address>(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:AddressOfGps");

        public static readonly MetdataQuery<string, Address> Address = new MetdataQuery<string, Address>(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:Address");
    }
}
