// <copyright file="WpfFotoflyMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>WpfFotoflyMetadata Class</summary>
namespace Fotofly.WpfTools
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Fotofly.MetadataQueries;

    public class WpfFotoflyMetadata : IFotoflyMetadata, IDisposable
    {
        public WpfFotoflyMetadata()
        {
        }

        public WpfFotoflyMetadata(BitmapMetadata bitmapMetadata)
        {
            this.BitmapMetadata = bitmapMetadata;

            if (!this.BitmapMetadata.IsFrozen && this.ContainsOldNamespaceMetadata)
            {
                this.MigrateXmpNamespace();
            }
        }

        public BitmapMetadata BitmapMetadata
        {
            get;
            set;
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

        public string OriginalCameraFilename
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.OriginalCameraFilename.Query);
            }

            set
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

        public Address Address
        {
            get
            {
                string address = this.BitmapMetadata.GetQuery<string>(XmpFotoflyQueries.Address.Query);

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
                this.CreateFotoflyStruct();

                if (value == null || !value.IsValidAddress)
                {
                    this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.Address.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(XmpFotoflyQueries.Address.Query, value.HierarchicalName);
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

        public bool ContainsOldNamespaceMetadata
        {
            get
            {
                return this.BitmapMetadata.ContainsQuery(XmpFotoflyQueries.FotoflyStruct.Query.Replace(":Fotofly", ":FotoFly"));
            }
        }

        public void Dispose()
        {
            this.Dispose(true);

            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Force Garbage ObjCollection
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void MigrateXmpNamespace()
        {
            // Migrate older naming schema to new
            // Create a new Struct
            this.BitmapMetadata.SetQuery(XmpFotoflyQueries.FotoflyStruct.Query, new BitmapMetadata(XmpCoreQueries.StructBlock));

            // Move all Metadata
            this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.AccuracyOfGps.Query.Replace(":Fotofly", ":FotoFly"), XmpFotoflyQueries.AccuracyOfGps.Query);
            this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.Address.Query.Replace(":Fotofly", ":FotoFly"), XmpFotoflyQueries.Address.Query);
            this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.AddressOfGps.Query.Replace(":Fotofly", ":FotoFly"), XmpFotoflyQueries.AddressOfGps.Query);
            this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.AddressOfGpsLookupDate.Query.Replace(":Fotofly", ":FotoFly"), XmpFotoflyQueries.AddressOfGpsLookupDate.Query);
            this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.AddressOfGpsSource.Query.Replace(":Fotofly", ":FotoFly"), XmpFotoflyQueries.AddressOfGpsSource.Query);
            this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.LastEditDate.Query.Replace(":Fotofly", ":FotoFly"), XmpFotoflyQueries.LastEditDate.Query);
            this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.OriginalCameraDate.Query.Replace(":Fotofly", ":FotoFly"), XmpFotoflyQueries.OriginalCameraDate.Query);
            this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.OriginalCameraFilename.Query.Replace(":Fotofly", ":FotoFly"), XmpFotoflyQueries.OriginalCameraFilename.Query);
            this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.UtcDate.Query.Replace(":Fotofly", ":FotoFly"), XmpFotoflyQueries.UtcDate.Query);
            this.BitmapMetadata.MoveQuery(XmpFotoflyQueries.UtcOffset.Query.Replace(":Fotofly", ":FotoFly"), XmpFotoflyQueries.UtcOffset.Query);

            // Remove the old struct
            this.BitmapMetadata.RemoveQuery(XmpFotoflyQueries.FotoflyStruct.Query.Replace(":Fotofly", ":FotoFly"));
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
