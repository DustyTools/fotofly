// <copyright file="XmpRightsProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>XmpRightsProvider Class</summary>
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

    public class XmpRightsProvider : BaseProvider
    {
        public XmpRightsProvider(BitmapMetadata bitmapMetadata)
            : base(bitmapMetadata)
        {}

        /// <summary>
        /// Copyright owner of the photo
        /// </summary>
        public string Copyright
        {
            get
            {
                string formattedString = string.Empty;

                try
                {
                    formattedString = this.BitmapMetadata.Copyright;
                }
                catch
                {
                    return String.Empty;
                }

                return formattedString;
            }

            set
            {
                if (this.ValueHasChanged(value, this.Copyright))
                {
                    if (value == null)
                    {
                        this.BitmapMetadata.Copyright = string.Empty;
                    }
                    else
                    {
                        this.BitmapMetadata.Copyright = value;
                    }
                }
            }
        }
    }
}
