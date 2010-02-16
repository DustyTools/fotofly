// <copyright file="FileMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>FileMetadata Class</summary>
namespace Fotofly.MetadataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public class FileMetadata
    {
        private ExifProvider exifProvider;
        private GpsProvider gpsProvider;
        private IptcProvider iptcProvider;
        private XmpCoreProvider xmpCoreProvider;
        private XmpExifProvider xmpExifProvider;
        private XmpFotoflyProvider xmpFotoflyProvider;
        private XmpMicrosoftProvider xmpMicrosoftProvider;
        private XmpRightsProvider xmpRightsProvider;
        private BitmapMetadata bitmapMetadata;

        public FileMetadata(BitmapMetadata bitmapMetadata)
        {
            this.exifProvider = new ExifProvider(bitmapMetadata);
            this.gpsProvider = new GpsProvider(bitmapMetadata);
            this.iptcProvider = new IptcProvider(bitmapMetadata);
            this.xmpCoreProvider = new XmpCoreProvider(bitmapMetadata);
            this.xmpExifProvider = new XmpExifProvider(bitmapMetadata);
            this.xmpFotoflyProvider = new XmpFotoflyProvider(bitmapMetadata);
            this.xmpMicrosoftProvider = new XmpMicrosoftProvider(bitmapMetadata);
            this.xmpRightsProvider = new XmpRightsProvider(bitmapMetadata);
            this.bitmapMetadata = bitmapMetadata;
        }

        /// <summary>
        /// Data stored in Exif IFD
        /// </summary>
        public ExifProvider ExifProvider
        {
            get { return this.exifProvider; }
        }

        /// <summary>
        /// Data stored in Gps IFD
        /// </summary>
        public GpsProvider GpsProvider
        {
            get { return this.gpsProvider; }
        }

        /// <summary>
        /// Data stored in IPTC (International Press Telecommunications Council) IFD
        /// </summary>
        public IptcProvider IptcProvider
        {
            get { return this.iptcProvider; }
        }

        /// <summary>
        /// Data stored in xmp
        /// </summary>
        public XmpCoreProvider XmpCoreProvider
        {
            get { return this.xmpCoreProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Fotofly Schema
        /// </summary>
        public XmpFotoflyProvider XmpFotoflyProvider
        {
            get { return this.xmpFotoflyProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Microsoft Schema
        /// </summary>
        public XmpMicrosoftProvider XmpMicrosoftProvider
        {
            get { return this.xmpMicrosoftProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Exif for XMP Schema
        /// </summary>
        public XmpExifProvider XmpExifProvider
        {
            get { return this.xmpExifProvider; }
        }

        /// <summary>
        /// Data stored in xmp using Rights Schema
        /// </summary>
        public XmpRightsProvider XmpRightsProvider
        {
            get { return this.xmpRightsProvider; }
        }

        public BitmapMetadata BitmapMetadata
        {
            get { return this.bitmapMetadata; }
        }
    }
}
