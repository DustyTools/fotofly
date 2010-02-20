// <copyright file="XmpIptcProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-18</date>
// <summary>XmpIptcProvider Class</summary>
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

    public class XmpIptcProvider : BaseProvider
    {
        public XmpIptcProvider(BitmapMetadata bitmapMetadata)
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
                string returnValue = this.BitmapMetadata.GetQuery<string>(XmpIptcQueries.LocationCreatedCity.Query);

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
                if (this.ValueHasChanged(value, this.City))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.RemoveQuery(XmpIptcQueries.LocationCreatedCity.Query);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpIptcQueries.LocationCreatedCity.Query, value);
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
                string returnValue = this.BitmapMetadata.GetQuery<string>(XmpIptcQueries.LocationCreatedCountryName.Query);

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
                if (this.ValueHasChanged(value, this.Country))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.RemoveQuery(XmpIptcQueries.LocationCreatedCountryName.Query);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpIptcQueries.LocationCreatedCountryName.Query, value);
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
                string returnValue = this.BitmapMetadata.GetQuery<string>(XmpIptcQueries.LocationCreatedProvinceState.Query);

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
                if (this.ValueHasChanged(value, this.Region))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.RemoveQuery(XmpIptcQueries.LocationCreatedProvinceState.Query);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpIptcQueries.LocationCreatedProvinceState.Query, value);
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
                string returnValue = this.BitmapMetadata.GetQuery<string>(XmpIptcQueries.LocationCreatedSublocation.Query);

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
                if (this.ValueHasChanged(value, this.SubLocation))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.RemoveQuery(XmpIptcQueries.LocationCreatedSublocation.Query);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpIptcQueries.LocationCreatedSublocation.Query, value);
                    }
                }
            }
        }
    }
}
