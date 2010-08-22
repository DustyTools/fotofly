// <copyright file="XmpFotoflyProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>XmpFotoflyProvider Class</summary>
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

    public class XmpFotoflyProvider : BaseProvider, IDisposable
    {
        public XmpFotoflyProvider(BitmapMetadata bitmapMetadata)
            : base(bitmapMetadata)
        {
            if (!bitmapMetadata.IsFrozen)
            {
                // Migrate Old Namespace data
                this.BitmapMetadata.MoveStruct(@"http\:\/\/ns.fotofly.com:", "Fotofly", XmpFotoflyQueries.QueryNamespace, XmpFotoflyQueries.QueryStructName, false, true);
                this.BitmapMetadata.MoveStruct(@"http\:\/\/ns.fotofly:", "Fotofly", XmpFotoflyQueries.QueryNamespace, XmpFotoflyQueries.QueryStructName, false, true);
                this.BitmapMetadata.MoveStruct(@"http\:\/\/ns.fotofly:", "FotoFly", XmpFotoflyQueries.QueryNamespace, XmpFotoflyQueries.QueryStructName, false, true);

                // Move old values
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.QueryRoot + "UtcDate", XmpFotoflyQueries.DateUtc.Query, false);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.QueryRoot + "AddressOfGpsLookupDate", XmpFotoflyQueries.AddressOfLocationCreatedLookupDate.Query, false);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.QueryRoot + "AddressOfGpsSource", XmpFotoflyQueries.AddressOfLocationCreatedSource.Query, false);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.QueryRoot + "LastEditDate", XmpFotoflyQueries.DateLastSave.Query, false);

                // Remove unused queries
                this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.QueryRoot + "Address");
                this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.QueryRoot + "AccuracyOfGps");
                this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.QueryRoot + "AddressOfGps");
            }
        }

        public DateTime DateUtc
        {
            get
            {
                DateTime utcDate = this.BitmapMetadata.GetQuery<DateTime>(XmpFotoflyQueries.DateUtc.Query);

                if (utcDate == null)
                {
                    return new DateTime();
                }
                else
                {
                    return utcDate;
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateUtc))
                {
                    this.CreateFotoflyStruct();

                    if (value == new DateTime())
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.DateUtc.Query);
                    }
                    else
                    {
                        string utcDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.DateUtc.Query, utcDate);
                    }
                }
            }
        }

        public double? UtcOffset
        {
            get
            {
                string utcOffsetString = this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.UtcOffset.Query);

                if (string.IsNullOrEmpty(utcOffsetString))
                {
                    return null;
                }
                else
                {
                    double utcOffset = Convert.ToDouble(utcOffsetString);

                    // Max range is -12 to +14
                    // UTC-12 = Baker Island, Howland Island
                    // UTC+14 = Kiribati
                    if (utcOffset > 14 || utcOffset < -12)
                    {
                        return null;
                    }
                    else
                    {
                        return utcOffset;
                    }
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.UtcOffset))
                {
                    this.CreateFotoflyStruct();

                    // Max range is -12 to +14
                    // UTC-12 = Baker Island, Howland Island
                    // UTC+14 = Kiribati
                    if (value > 14 || value < -12)
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.UtcOffset.Query);
                    }
                    else if (value > 0)
                    {
                        // Prepend postive values with a plus sign
                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.UtcOffset.Query, "+" + value.ToString());
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.UtcOffset.Query, value.ToString());
                    }
                }
            }
        }

        public DateTime DateLastSave
        {
            get
            {
                string lastEditDate = this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.DateLastSave.Query);

                if (string.IsNullOrEmpty(lastEditDate))
                {
                    return new DateTime();
                }
                else
                {
                    // Parse the object as a DateTime, stripping out the Z
                    DateTime dateTime = DateTime.Parse(lastEditDate.TrimEnd('Z'));

                    // Parse as local time
                    DateTime localDateTime = new DateTime(dateTime.Ticks, DateTimeKind.Local);

                    return localDateTime;
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateLastSave))
                {
                    this.CreateFotoflyStruct();

                    if (value == new DateTime())
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.DateLastSave.Query);
                    }
                    else
                    {
                        string lastEditDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.DateLastSave.Query, lastEditDate);
                    }
                }
            }
        }

        public DateTime OriginalCameraDate
        {
            get
            {
                string originalCameraDate = this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.OriginalCameraDate.Query);

                if (originalCameraDate == null)
                {
                    return new DateTime();
                }
                else
                {
                    // Parse the object as a DateTime, stripping out the Z
                    DateTime dateTime = DateTime.Parse(originalCameraDate.TrimEnd('Z'));

                    // Parse as local time
                    DateTime localDateTime = new DateTime(dateTime.Ticks, DateTimeKind.Local);

                    return localDateTime;
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.OriginalCameraDate))
                {
                    this.CreateFotoflyStruct();

                    if (value == new DateTime())
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.OriginalCameraDate.Query);
                    }
                    else
                    {
                        string originalCameraDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.OriginalCameraDate.Query, originalCameraDate);
                    }
                }
            }
        }

        public string OriginalCameraFilename
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.OriginalCameraFilename.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.OriginalCameraFilename))
                {
                    this.CreateFotoflyStruct();

                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.OriginalCameraFilename.Query);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.OriginalCameraFilename.Query, value);
                    }
                }
            }
        }

        public DateTime AddressCreatedLookupDate
        {
            get
            {
                string addressOfGpsLookupDate = this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.AddressOfLocationCreatedLookupDate.Query);

                if (string.IsNullOrEmpty(addressOfGpsLookupDate))
                {
                    return new DateTime();
                }
                else
                {
                    // Parse the object as a DateTime, stripping out the Z
                    DateTime dateTime = DateTime.Parse(addressOfGpsLookupDate.TrimEnd('Z'));

                    // Parse as local time
                    DateTime localDateTime = new DateTime(dateTime.Ticks, DateTimeKind.Local);

                    return localDateTime;
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.AddressCreatedLookupDate))
                {
                    this.CreateFotoflyStruct();

                    if (value == new DateTime())
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.AddressOfLocationCreatedLookupDate.Query);
                    }
                    else
                    {
                        string addressOfGpsLookupDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.AddressOfLocationCreatedLookupDate.Query, addressOfGpsLookupDate);
                    }
                }
            }
        }

        public string AddressCreatedSource
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.AddressOfLocationCreatedSource.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.AddressCreatedSource))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.AddressOfLocationCreatedSource.Query);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.AddressOfLocationCreatedSource.Query, value);
                    }
                }
            }
        }

        public string GpsPositionShownSource
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.GpsPositionOfLocationShownSource.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.GpsPositionShownSource))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.GpsPositionOfLocationShownSource.Query);
                    }
                    else
                    {
                        this.CreateFotoflyStruct();

                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.GpsPositionOfLocationShownSource.Query, value);
                    }
                }
            }
        }

        private void CreateFotoflyStruct()
        {
            if (!this.BitmapMetadata.ContainsQuery(XmpFotoflyQueries.FotoflyStruct.Query))
            {
                this.BitmapMetadata.SetQuery(XmpFotoflyQueries.FotoflyStruct.Query, new BitmapMetadata(XmpCoreQueries.StructBlock));
            }
        }

        private Dictionary<string, string> AllFotoflyProperties
        {
            get
            {
                Dictionary<string, string> allFotoflyProperties = new Dictionary<string, string>();

                // Loop through all data in the struct
                foreach (string query in base.BitmapMetadata.GetQuery<BitmapMetadata>(XmpFotoflyQueries.FotoflyStruct.Query))
                {
                    allFotoflyProperties.Add(XmpFotoflyQueries.FotoflyStruct.Query + query, base.BitmapMetadata.GetQuery<string>(query));
                }

                return allFotoflyProperties;
            }
        }
    }
}
