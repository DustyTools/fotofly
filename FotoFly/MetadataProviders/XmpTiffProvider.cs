// <copyright file="XmpTiffProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-19</date>
// <summary>XmpTiffProvider Class</summary>
namespace Fotofly.MetadataProviders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Media.Imaging;

    using Fotofly.BitmapMetadataTools;
    using Fotofly.MetadataQueries;

    public class XmpTiffProvider : BaseProvider
    {
        public XmpTiffProvider(BitmapMetadata bitmapMetadata)
            : base(bitmapMetadata)
        {
        }

        /// <summary>
        /// Make
        /// </summary>
        public string Make
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpTiffQueries.Make.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Make))
                {
                    this.BitmapMetadata.SetQueryOrRemove(XmpTiffQueries.Make.Query, value, string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// Model
        /// </summary>
        public string Model
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpTiffQueries.Model.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Model))
                {
                    this.BitmapMetadata.SetQueryOrRemove(XmpTiffQueries.Model.Query, value, string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// Orientation
        /// </summary>
        public string Orientation
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpTiffQueries.Orientation.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Orientation))
                {
                    this.BitmapMetadata.SetQueryOrRemove(XmpTiffQueries.Orientation.Query, value, string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// Software
        /// </summary>
        public string Software
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpTiffQueries.Software.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Software))
                {
                    this.BitmapMetadata.SetQueryOrRemove(XmpTiffQueries.Software.Query, value, string.IsNullOrEmpty(value));
                }
            }
        }
    }
}