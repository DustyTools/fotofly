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
        /// Rating (Range 0-5)
        /// </summary>
        public MetadataEnums.Rating Rating
        {
            get
            {
                string rating = this.BitmapMetadata.GetQuery<string>(XmpXapQueries.Rating.Query);

                if (String.IsNullOrEmpty(rating))
                {
                    return MetadataEnums.Rating.Unknown;
                }
                else if (rating == "1")
                {
                    return MetadataEnums.Rating.OneStar;
                }
                else if (rating == "2")
                {
                    return MetadataEnums.Rating.TwoStar;
                }
                else if (rating == "3")
                {
                    return MetadataEnums.Rating.ThreeStar;
                }
                else if (rating == "4")
                {
                    return MetadataEnums.Rating.FourStar;
                }
                else if (rating == "5")
                {
                    return MetadataEnums.Rating.FiveStar;
                }

                return MetadataEnums.Rating.Unknown;
            }

            set
            {
                if (this.ValueHasChanged(value, this.Rating))
                {
                    this.BitmapMetadata.SetQueryOrRemove(XmpXapQueries.Rating.Query, ((int)value).ToString(), value == MetadataEnums.Rating.Unknown);
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