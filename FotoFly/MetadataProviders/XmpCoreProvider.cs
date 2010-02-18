// <copyright file="XmpCoreProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>XmpCoreProvider Class</summary>
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

    public class XmpCoreProvider : BaseProvider
    {
        public XmpCoreProvider(BitmapMetadata bitmapMetadata)
            : base(bitmapMetadata)
        {}

        /// <summary>
        /// Subject, not often used by software, Title should be used in most cases
        /// </summary>
        public string Subject
        {
            get
            {
                if (string.IsNullOrEmpty(this.BitmapMetadata.Subject))
                {
                    return string.Empty;
                }
                else
                {
                    return this.BitmapMetadata.Subject;
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.Subject))
                {
                    this.BitmapMetadata.Subject = value;
                }
            }
        }

        /// <summary>
        /// List of Tags, sometimes known as Keywords
        /// </summary>
        public TagList Tags
        {
            get
            {
                return new TagList(this.BitmapMetadata.Keywords);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Tags))
                {
                    this.BitmapMetadata.Keywords = new ReadOnlyCollection<string>(value.ToReadOnlyCollection());
                }
            }
        }

        /// <summary>
        /// Comment, also known as Description
        /// </summary>
        public string Comment
        {
            get
            {
                string formattedString = String.Empty;

                try
                {
                    formattedString = this.BitmapMetadata.Comment;
                }
                catch
                {
                    formattedString = String.Empty;
                }

                return formattedString;
            }

            set
            {
                if (this.ValueHasChanged(value, this.Comment))
                {
                    this.BitmapMetadata.Comment = value;
                }
            }
        }
    }
}