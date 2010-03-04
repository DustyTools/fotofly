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


        public Address AddressOfLocationCreated
        {
            get
            {
                Address address = new Address();
                address.Country = this.LocationCreatedCountry;
                address.City = this.LocationCreatedCity;
                address.Region = this.LocationCreatedRegion;
                address.AddressLine = this.LocationCreatedSubLocation;

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
                this.LocationCreatedCountry = value.Country;
                this.LocationCreatedCity = value.City;
                this.LocationCreatedRegion = value.Region;
                this.LocationCreatedSubLocation = value.AddressLine;
            }
        }

        /// <summary>
        /// City
        /// </summary>
        public string LocationCreatedCity
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.City.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.LocationCreatedCity))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.City.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// County
        /// </summary>
        public string LocationCreatedCountry
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.Country.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.LocationCreatedCountry))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.Country.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// Region, also used for State, County or Province
        /// </summary>
        public string LocationCreatedRegion
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.Region.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.LocationCreatedRegion))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.Region.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// SubLocation
        /// </summary>
        public string LocationCreatedSubLocation
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.SubLocation.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.LocationCreatedSubLocation))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.SubLocation.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        public Address AddressOfLocationShown
        {
            get
            {
                Address address = new Address();
                address.Country = this.LocationShownCountry;
                address.City = this.LocationShownCity;
                address.Region = this.LocationShownRegion;
                address.AddressLine = this.LocationShownSubLocation;

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
                this.LocationShownCountry = value.Country;
                this.LocationShownCity = value.City;
                this.LocationShownRegion = value.Region;
                this.LocationShownSubLocation = value.AddressLine;
            }
        }

        /// <summary>
        /// City
        /// </summary>
        public string LocationShownCity
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.City.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.LocationShownCity))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.City.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// County
        /// </summary>
        public string LocationShownCountry
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.Country.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.LocationShownCountry))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.Country.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// Region, also used for State, County or Province
        /// </summary>
        public string LocationShownRegion
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.Region.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.LocationShownRegion))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.Region.Query, value.ToString(), string.IsNullOrEmpty(value));
                }
            }
        }

        /// <summary>
        /// SubLocation
        /// </summary>
        public string LocationShownSubLocation
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.SubLocation.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.LocationShownSubLocation))
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
        /// Datetime Created
        /// </summary>
        public DateTime DateTimeCreated
        {
            get
            {
                return new DateTime(this.DateCreated.Year, this.DateCreated.Month, this.DateCreated.Day, this.TimeCreated.Hours, this.TimeCreated.Minutes, this.TimeCreated.Seconds, this.TimeCreated.Milliseconds); 
            }

            set
            {
                this.DateCreated = value.Date;
                this.TimeCreated = value.TimeOfDay;
            }
        }

        /// <summary>
        /// DateCreated
        /// </summary>
        public DateTime DateCreated
        {
            get
            {
                return this.BitmapMetadata.GetQuery<DateTime>(IptcQueries.DateCreated.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateCreated))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.DateCreated.Query, value.ToString(), value == new DateTime());
                }
            }
        }

        /// <summary>
        /// TimeCreated
        /// </summary>
        public TimeSpan TimeCreated
        {
            get
            {
                return this.BitmapMetadata.GetQuery<TimeSpan>(IptcQueries.TimeCreated.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.TimeCreated))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.TimeCreated.Query, value.ToString(), value == new TimeSpan());
                }
            }
        }

        /// <summary>
        /// Date Time Digitised
        /// </summary>
        public DateTime DateTimeDigitised
        {
            get
            {
                return new DateTime(this.DateDigitised.Year, this.DateDigitised.Month, this.DateDigitised.Day, this.TimeDigitised.Hours, this.TimeDigitised.Minutes, this.TimeDigitised.Seconds, this.TimeDigitised.Milliseconds);
            }

            set
            {
                this.DateDigitised = value.Date;
                this.TimeDigitised = value.TimeOfDay;
            }
        }

        /// <summary>
        /// DateDigitised
        /// </summary>
        public DateTime DateDigitised 
        {
            get
            {
                return this.BitmapMetadata.GetQuery<DateTime>(IptcQueries.DateDigitised.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateDigitised))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.DateDigitised.Query, value.ToString(), value == new DateTime());
                }
            }
        }

        /// <summary>
        /// TimeDigitised
        /// </summary>
        public TimeSpan TimeDigitised 
        {
            get
            {
                return this.BitmapMetadata.GetQuery<TimeSpan>(IptcQueries.TimeDigitised.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.TimeDigitised))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.TimeDigitised.Query, value.ToString(), value == new TimeSpan());
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
                if (this.BitmapMetadata.IsQueryValidAndOfType(IptcQueries.Keywords.Query, typeof(string)))
                {
                    string stringTag = this.BitmapMetadata.GetQuery<string>(IptcQueries.Keywords.Query);

                    return new TagList(stringTag);
                }
                else if (this.BitmapMetadata.IsQueryValidAndOfType(IptcQueries.Keywords.Query, typeof(string[])))
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
        /// Caption
        /// </summary>
        public string Caption
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(IptcQueries.Caption.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.Caption))
                {
                    this.BitmapMetadata.SetQueryOrRemove(IptcQueries.Caption.Query, value.ToString(), string.IsNullOrEmpty(value));
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