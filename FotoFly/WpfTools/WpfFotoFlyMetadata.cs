// <copyright file="WpfFotoFlyMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>WpfFotoFlyMetadata Class</summary>
namespace FotoFly.WpfTools
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

    using FotoFly.MetadataQueries;

    public class WpfFotoFlyMetadata : IFotoFlyMetadata, IDisposable
    {
        public WpfFotoFlyMetadata()
        {
        }

        public WpfFotoFlyMetadata(BitmapMetadata bitmapMetadata)
        {
            this.BitmapMetadata = bitmapMetadata;
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
                DateTime utcDate = this.BitmapMetadata.GetQuery<DateTime>(XmpFotoFlyQueries.UtcDate.Query);

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
                    this.BitmapMetadata.RemoveQuery(XmpFotoFlyQueries.UtcDate.Query);
                }
                else
                {
                    string utcDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                    this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.UtcDate.Query, utcDate);
                }
            }
        }

        public double? UtcOffset
        {
            get
            {
                string utcOffsetString = this.BitmapMetadata.GetQuery<string>(XmpFotoFlyQueries.UtcOffset.Query);

                if (string.IsNullOrEmpty(utcOffsetString))
                {
                    return null;
                }
                else
                {
                    double utcOffset = Convert.ToDouble(utcOffsetString);

                    if (utcOffset > 12 || utcOffset < -12)
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

                if (value > 12 || value < -12)
                {
                    this.BitmapMetadata.RemoveQuery(XmpFotoFlyQueries.UtcOffset.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.UtcOffset.Query, value.ToString());
                }
            }
        }

        public DateTime LastEditDate
        {
            get
            {
                string lastEditDate = this.BitmapMetadata.GetQuery<string>(XmpFotoFlyQueries.LastEditDate.Query);

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
                    this.BitmapMetadata.RemoveQuery(XmpFotoFlyQueries.LastEditDate.Query);
                }
                else
                {
                    string lastEditDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                    this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.LastEditDate.Query, lastEditDate);
                }
            }
        }

        public DateTime AddressOfGpsLookupDate
        {
            get
            {
                string addressOfGpsLookupDate = this.BitmapMetadata.GetQuery<string>(XmpFotoFlyQueries.AddressOfGpsLookupDate.Query);

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
                    this.BitmapMetadata.RemoveQuery(XmpFotoFlyQueries.AddressOfGpsLookupDate.Query);
                }
                else
                {
                    string addressOfGpsLookupDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                    this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.AddressOfGpsLookupDate.Query, addressOfGpsLookupDate);
                }
            }
        }

        public DateTime OriginalCameraDate
        {
            get
            {
                string originalCameraDate = this.BitmapMetadata.GetQuery<string>(XmpFotoFlyQueries.OriginalCameraDate.Query);

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
                    this.BitmapMetadata.RemoveQuery(XmpFotoFlyQueries.OriginalCameraDate.Query);
                }
                else
                {
                    string originalCameraDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                    this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.OriginalCameraDate.Query, originalCameraDate);
                }
            }
        }

        public string OriginalCameraFilename
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpFotoFlyQueries.OriginalCameraFilename.Query);
            }

            set
            {
                this.CreateFotoflyStruct();

                if (string.IsNullOrEmpty(value))
                {
                    this.BitmapMetadata.RemoveQuery(XmpFotoFlyQueries.OriginalCameraFilename.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.OriginalCameraFilename.Query, value);
                }
            }
        }

        public Address AddressOfGps
        {
            get
            {
                string address = this.BitmapMetadata.GetQuery<string>(XmpFotoFlyQueries.AddressOfGps.Query);

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
                    this.BitmapMetadata.RemoveQuery(XmpFotoFlyQueries.AddressOfGps.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.AddressOfGps.Query, value.HierarchicalName);
                }
            }
        }

        public Address Address
        {
            get
            {
                string address = this.BitmapMetadata.GetQuery<string>(XmpFotoFlyQueries.Address.Query);

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
                    this.BitmapMetadata.RemoveQuery(XmpFotoFlyQueries.Address.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.Address.Query, value.HierarchicalName);
                }
            }
        }

        public string AddressOfGpsSource
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(XmpFotoFlyQueries.AddressOfGpsSource.Query);
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.BitmapMetadata.RemoveQuery(XmpFotoFlyQueries.AddressOfGpsSource.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.AddressOfGpsSource.Query, value);
                }
            }
        }

        public GpsPosition.Accuracies AccuracyOfGps
        {
            get
            {
                string accuracyOfGpsString = this.BitmapMetadata.GetQuery<string>(XmpFotoFlyQueries.AccuracyOfGps.Query);

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
                    this.BitmapMetadata.RemoveQuery(XmpFotoFlyQueries.AccuracyOfGps.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.AccuracyOfGps.Query, (int)value + "-" + value.ToString());
                }
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

        private void CreateFotoflyStruct()
        {
            if (!this.BitmapMetadata.ContainsQuery(XmpFotoFlyQueries.FotoFlyStruct.Query))
            {
                this.BitmapMetadata.SetQuery(XmpFotoFlyQueries.FotoFlyStruct.Query, new BitmapMetadata(XmpCoreQueries.StructBlock));
            }
        }
    }
}
