// <copyright file="XmpPhotoshopProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-25</date>
// <summary>XmpPhotoshopProvider Class</summary>
namespace Fotofly.MetadataProviders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using System.Windows.Media.Imaging;

    using Fotofly.BitmapMetadataTools;
    using Fotofly.MetadataQueries;

    public class XmpPhotoshopProvider : BaseProvider
    {
        public XmpPhotoshopProvider(BitmapMetadata bitmapMetadata)
            : base(bitmapMetadata)
        {
        }

        public DateTime DateTimeOriginal
        {
            get
            {
                ExifDateTime exifDateTime = this.BitmapMetadata.GetQuery<ExifDateTime>(XmpPhotoshopQueries.DateTimeCreated.Query);

                if (exifDateTime == null)
                {
                    return new DateTime();
                }
                else
                {
                    return exifDateTime.ToDateTime();
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateTimeOriginal))
                {
                    this.BitmapMetadata.SetQuery(XmpPhotoshopQueries.DateTimeCreated.Query, new ExifDateTime(value).ToExifString());
                }
            }
        }
    }
}
