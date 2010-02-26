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
                if (this.ValueHasChanged(value, this.LocationShownCity))
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
        public string LocationShownCountry
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
                if (this.ValueHasChanged(value, this.LocationShownCountry))
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
        public string LocationShownRegion
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
                if (this.ValueHasChanged(value, this.LocationShownRegion))
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
        public string LocationShownSubLocation
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
                if (this.ValueHasChanged(value, this.LocationShownSubLocation))
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
                if (this.ValueHasChanged(value, this.LocationCreatedCity))
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
        public string LocationCreatedCountry
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
                if (this.ValueHasChanged(value, this.LocationCreatedCountry))
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
        public string LocationCreatedRegion
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
                if (this.ValueHasChanged(value, this.LocationCreatedRegion))
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
        public string LocationCreatedSubLocation
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
                if (this.ValueHasChanged(value, this.LocationCreatedSubLocation))
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
