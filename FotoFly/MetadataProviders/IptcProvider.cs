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
        {
        }

        public Address Address
        {
            get
            {
                Address address = new Address();
                address.Country = this.Country;
                address.City = this.City;
                address.Region = this.Region;
                address.AddressLine = this.SubLocation;

                if (address.IsValidAddress)
                {
                    return address;
                }
                else
                {
                    return new Address();
                }
            }

            set
            {
                this.Country = value.Country;
                this.City = value.City;
                this.Region = value.Region;
                this.SubLocation = value.AddressLine;
            }
        }

        /// <summary>
        /// City
        /// </summary>
        public string City
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.City.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.City))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.City.Query, value.ToString(), string.IsNullOrEmpty(value));
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
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.Country.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Country))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.Country.Query, value.ToString(), string.IsNullOrEmpty(value));
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
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.Region.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Region))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.Region.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// SubLocation
        /// </summary>
        public string SubLocation
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.SubLocation.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.SubLocation))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.SubLocation.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// CodeCharacterSet
        /// </summary>
        public string CodeCharacterSet
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.CodeCharacterSet.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.CodeCharacterSet))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.CodeCharacterSet.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// DateCreated
        /// </summary>
        public string DateCreated
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.DateCreated.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateCreated))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.DateCreated.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// TimeCreated
        /// </summary>
        public string TimeCreated
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.TimeCreated.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.TimeCreated))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.TimeCreated.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// OriginatingProgram
        /// </summary>
        public string OriginatingProgram
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.OriginatingProgram.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.OriginatingProgram))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.OriginatingProgram.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// Byline
        /// </summary>
        public string Byline
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.Byline.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Byline))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.Byline.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// Keywords
        /// </summary>
        public TagList Keywords
        {
            get
            {
                if (this.BitmapMetadata.IsQueryOfType(IptcQueries.Keywords.Query, typeof(string)))
                {
                    string stringTag = this.BitmapMetadata.GetQuery<string>(IptcQueries.Keywords.Query);

                    return new TagList(stringTag);
                }
                else if (this.BitmapMetadata.IsQueryOfType(IptcQueries.Keywords.Query, typeof(string[])))
                {
                    string[] stringArray = this.BitmapMetadata.GetQuery<string[]>(IptcQueries.Keywords.Query);

                    return new TagList(stringArray);
                }

                return new TagList();
            }

            set
            {
                if (this.ValueHasChanged(value, this.Keywords))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.Keywords.Query, value.ToStringArray(), value.Count == 0);
                }
            }
        }

        /// <summary>
        /// ObjectName
        /// </summary>
        public string ObjectName
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.ObjectName.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.ObjectName))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.ObjectName.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// CopyrightNotice
        /// </summary>
        public string CopyrightNotice
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.CopyrightNotice.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.CopyrightNotice))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.CopyrightNotice.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }
    }
}