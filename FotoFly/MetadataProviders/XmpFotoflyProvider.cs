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
            string query = XmpFotoflyQueries.FotoflyStruct.Query.Replace(XmpFotoflyQueries.QueryStruct, XmpFotoflyQueries.OldQueryStruct);

            if (this.BitmapMetadata.ContainsQuery(query))
            {
                this.MigrateXmpNamespace();
            }
        }

        public DateTime UtcDate
        {
            get
            {
                DateTime utcDate = this.BitmapMetadata.GetQuery<DateTime>(XmpFotoflyQueries.UtcDate.Query);

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
                if (this.ValueHasChanged(value, this.UtcDate))
                {
                    this.CreateFotoflyStruct();

                    if (value == new DateTime())
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.UtcDate.Query);
                    }
                    else
                    {
                        string utcDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.UtcDate.Query, utcDate);
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

        public DateTime LastEditDate
        {
            get
            {
                string lastEditDate = this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.LastEditDate.Query);

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
                if (this.ValueHasChanged(value, this.LastEditDate))
                {
                    this.CreateFotoflyStruct();

                    if (value == new DateTime())
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.LastEditDate.Query);
                    }
                    else
                    {
                        string lastEditDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.LastEditDate.Query, lastEditDate);
                    }

                    // Remove Address
                    this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.Address.Query);
                }
            }
        }

        public DateTime AddressOfGpsLookupDate
        {
            get
            {
                string addressOfGpsLookupDate = this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.AddressOfGpsLookupDate.Query);

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
                if (this.ValueHasChanged(value, this.AddressOfGpsLookupDate))
                {
                    this.CreateFotoflyStruct();

                    if (value == new DateTime())
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.AddressOfGpsLookupDate.Query);
                    }
                    else
                    {
                        string addressOfGpsLookupDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.AddressOfGpsLookupDate.Query, addressOfGpsLookupDate);
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

        public Address AddressOfGps
        {
            get
            {
                string address = this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.AddressOfGps.Query);

                if (string.IsNullOrEmpty(address))
                {
                    return new Address();
                }
                else
                {
                    return new Address(address);
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.AddressOfGps))
                {
                    this.CreateFotoflyStruct();

                    if (value == null || !value.IsValidAddress)
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.AddressOfGps.Query);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.AddressOfGps.Query, value.HierarchicalName);
                    }
                }
            }
        }

        public string AddressOfGpsSource
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.AddressOfGpsSource.Query);
            }

            set
            {
                if (this.ValueHasChanged(value, this.AddressOfGpsSource))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.AddressOfGpsSource.Query);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.AddressOfGpsSource.Query, value);
                    }
                }
            }
        }

        public GpsPosition.Accuracies AccuracyOfGps
        {
            get
            {
                string accuracyOfGpsString = this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.AccuracyOfGps.Query);

                if (!string.IsNullOrEmpty(accuracyOfGpsString))
                {
                    int accuracyOfGps;

                    if (Int32.TryParse(accuracyOfGpsString[0].ToString(), out accuracyOfGps) && accuracyOfGps < 9 && accuracyOfGps > 0)
                    {
                        return (GpsPosition.Accuracies)accuracyOfGps;
                    }
                }

                return GpsPosition.Accuracies.Unknown;
            }

            set
            {
                if (this.ValueHasChanged(value, this.AccuracyOfGps))
                {
                    this.CreateFotoflyStruct();

                    if (value == GpsPosition.Accuracies.Unknown)
                    {
                        this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.AccuracyOfGps.Query);
                    }
                    else
                    {
                        this.BitmapMetadata.SetQuery(XmpFotoflyQueries.AccuracyOfGps.Query, (int)value + "-" + value.ToString());
                    }
                }
            }
        }

        private void MigrateXmpNamespace()
        {
            if (this.BitmapMetadata.IsFrozen)
            {
                throw new Exception("This file has been saved with a previous version of Fotofly (v0.5 or earlier) and the data needs to be migrated. Use the XmpFotoflyProvider.MigrateXmpNamespace() Method to migrate data.");
            }
            else
            {
                // Migrate older naming schema to new
                // Create a new Struct
                this.BitmapMetadata.SetQuery(XmpFotoflyQueries.FotoflyStruct.Query, new BitmapMetadata(XmpCoreQueries.StructBlock));

                // Move all Metadata
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.AccuracyOfGps.Query.Replace(XmpFotoflyQueries.QueryRoot, XmpFotoflyQueries.OldQueryRoot), XmpFotoflyQueries.AccuracyOfGps.Query);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.AddressOfGps.Query.Replace(XmpFotoflyQueries.QueryRoot, XmpFotoflyQueries.OldQueryRoot), XmpFotoflyQueries.AddressOfGps.Query);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.AddressOfGpsLookupDate.Query.Replace(XmpFotoflyQueries.QueryRoot, XmpFotoflyQueries.OldQueryRoot), XmpFotoflyQueries.AddressOfGpsLookupDate.Query);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.AddressOfGpsSource.Query.Replace(XmpFotoflyQueries.QueryRoot, XmpFotoflyQueries.OldQueryRoot), XmpFotoflyQueries.AddressOfGpsSource.Query);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.LastEditDate.Query.Replace(XmpFotoflyQueries.QueryRoot, XmpFotoflyQueries.OldQueryRoot), XmpFotoflyQueries.LastEditDate.Query);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.OriginalCameraDate.Query.Replace(XmpFotoflyQueries.QueryRoot, XmpFotoflyQueries.OldQueryRoot), XmpFotoflyQueries.OriginalCameraDate.Query);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.OriginalCameraFilename.Query.Replace(XmpFotoflyQueries.QueryRoot, XmpFotoflyQueries.OldQueryRoot), XmpFotoflyQueries.OriginalCameraFilename.Query);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.UtcDate.Query.Replace(XmpFotoflyQueries.QueryRoot, XmpFotoflyQueries.OldQueryRoot), XmpFotoflyQueries.UtcDate.Query);
                this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.UtcOffset.Query.Replace(XmpFotoflyQueries.QueryRoot, XmpFotoflyQueries.OldQueryRoot), XmpFotoflyQueries.UtcOffset.Query);
                this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.Address.Query.Replace(XmpFotoflyQueries.QueryRoot, XmpFotoflyQueries.OldQueryRoot));

                // Remove the old struct
                this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.QueryStruct.Replace(XmpFotoflyQueries.QueryStruct, XmpFotoflyQueries.OldQueryStruct));
            }
        }

        private void CreateFotoflyStruct()
        {
            if (!this.BitmapMetadata.ContainsQuery(XmpFotoflyQueries.FotoflyStruct.Query))
            {
                this.BitmapMetadata.SetQuery(XmpFotoflyQueries.FotoflyStruct.Query, new BitmapMetadata(XmpCoreQueries.StructBlock));
            }
        }
    }
}
