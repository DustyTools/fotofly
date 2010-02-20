// <copyright file="XmpXapProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-19</date>
// <summary>XmpXapProvider Class</summary>
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

    public class XmpXapProvider : BaseProvider
    {
        public XmpXapProvider(BitmapMetadata bitmapMetadata)
            : base(bitmapMetadata)
        { }

        /// <summary>
        /// Rating
        /// </summary>
        public string Rating
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpXapQueries.Rating.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Rating))
                {
                    this.BitmapMetadata.SetQueryOrRemove(XmpXapQueries.Rating.Query, value, string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// CreatorTool
        /// </summary>
        public string CreatorTool
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpXapQueries.CreatorTool.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.CreatorTool))
                {
                    this.BitmapMetadata.SetQueryOrRemove(XmpXapQueries.CreatorTool.Query, value, string.IsNullOrEmpty(value));
                }
            }
        }
    }
}