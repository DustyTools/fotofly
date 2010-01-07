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
        public static readonly MetdataQuery FotoFlyStruct = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly", typeof(BitmapMetadata));

        public static readonly MetdataQuery UtcDate = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:UtcDate", typeof(DateTime));

        public static readonly MetdataQuery UtcOffset = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:UtcOffset", typeof(int));

        public static readonly MetdataQuery LastEditDate = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:LastEditDate", typeof(DateTime));

        public static readonly MetdataQuery OriginalCameraFilename = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:OriginalCameraFilename", typeof(string));

        public static readonly MetdataQuery OriginalCameraDate = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:OriginalCameraDate", typeof(DateTime));

        public static readonly MetdataQuery AccuracyOfGps = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:AccuracyOfGps", typeof(int));

        public static readonly MetdataQuery AddressOfGpsLookupDate = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:AddressOfGpsLookupDate", typeof(DateTime));

        public static readonly MetdataQuery AddressOfGpsSource = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:AddressOfGpsSource", typeof(string));

        public static readonly MetdataQuery AddressOfGps = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:AddressOfGps", typeof(string));

        public static readonly MetdataQuery Address = new MetdataQuery(@"/xmp/http\:\/\/ns.fotofly:FotoFly/http\:\/\/ns.fotofly:Address", typeof(string));
    }
}
