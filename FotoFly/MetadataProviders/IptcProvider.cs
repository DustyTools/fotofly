// <copyright file="IptcProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>IptcProvider Class</summary>
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

    public class IptcProvider : BaseProvider
    {
        public IptcProvider(BitmapMetadata bitmapMetadata)
            : base(bitmapMetadata)
        {}

        /// <summary>
        /// City
        /// </summary>
        public string City
        {
            get
            {
                string returnValue = this.BitmapMetadata.GetQuery<string>(IptcQueries.City.Query);

                if (string.IsNullOrEmpty(returnValue))
                {
                    return string.Empty;
                }
                else
                {
                    return returnValue;
                }
            }

            set
            {
                if (!value.Equals(this.City))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.SetQuery(IptcQueries.City.Query, string.Empty);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(IptcQueries.City.Query, value);
                    }
                }
            }
        }

        /// <summary>
        /// County
        /// </summary>
        public string Country
        {
            get
            {
                string returnValue = this.BitmapMetadata.GetQuery<string>(IptcQueries.Country.Query);

                if (string.IsNullOrEmpty(returnValue))
                {
                    return string.Empty;
                }
                else
                {
                    return returnValue;
                }
            }

            set
            {
                if (!value.Equals(this.Country))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.SetQuery(IptcQueries.Country.Query, string.Empty);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(IptcQueries.Country.Query, value);
                    }
                }
            }
        }

        /// <summary>
        /// Region, also used for State, County or Province
        /// </summary>
        public string Region
        {
            get
            {
                string returnValue = this.BitmapMetadata.GetQuery<string>(IptcQueries.Region.Query);

                if (string.IsNullOrEmpty(returnValue))
                {
                    return string.Empty;
                }
                else
                {
                    return returnValue;
                }
            }

            set
            {
                if (!value.Equals(this.Region))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.SetQuery(IptcQueries.Region.Query, string.Empty);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(IptcQueries.Region.Query, value);
                    }
                }
            }
        }

        /// <summary>
        /// Sublocation
        /// </summary>
        public string SubLocation
        {
            get
            {
                string returnValue = this.BitmapMetadata.GetQuery<string>(IptcQueries.SubLocation.Query);

                if (string.IsNullOrEmpty(returnValue))
                {
                    return string.Empty;
                }
                else
                {
                    return returnValue;
                }
            }

            set
            {
                if (!value.Equals(this.SubLocation))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.SetQuery(IptcQueries.SubLocation.Query, string.Empty);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(IptcQueries.SubLocation.Query, value);
                    }
                }
            }
        }
    }
}
