// <copyright file="WpfFotoFlyMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>WpfFotoFlyMetadata Class</summary>
namespace FotoFly
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
                DateTime utcDate = this.BitmapMetadata.GetQuery<DateTime>(FotoFlyQueries.UtcDate.Query);

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
                    this.BitmapMetadata.RemoveQuery(FotoFlyQueries.UtcDate.Query);
                }
                else
                {
                    string utcDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                    this.BitmapMetadata.SetQuery(FotoFlyQueries.UtcDate.Query, utcDate);
                }
            }
        }

        public double UtcOffset
        {
            get
            {
                string utcOffsetString = this.BitmapMetadata.GetQuery<string>(FotoFlyQueries.UtcOffset.Query);

                if (string.IsNullOrEmpty(utcOffsetString))
                {
                    return 0;
                }
                else
                {
                    double utcOffset = Convert.ToDouble(utcOffsetString);

                    if (utcOffset > 12 || utcOffset < -12)
                    {
                        return 0;
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
                    this.BitmapMetadata.RemoveQuery(FotoFlyQueries.UtcOffset.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(FotoFlyQueries.UtcOffset.Query, value.ToString());
                }
            }
        }

        public DateTime LastEditDate
        {
            get
            {
                string lastEditDate = this.BitmapMetadata.GetQuery<string>(FotoFlyQueries.LastEditDate.Query);

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
                    this.BitmapMetadata.RemoveQuery(FotoFlyQueries.LastEditDate.Query);
                }
                else
                {
                    string lastEditDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                    this.BitmapMetadata.SetQuery(FotoFlyQueries.LastEditDate.Query, lastEditDate);
                }
            }
        }

        public DateTime AddressOfGpsLookupDate
        {
            get
            {
                string addressOfGpsLookupDate = this.BitmapMetadata.GetQuery<string>(FotoFlyQueries.AddressOfGpsLookupDate.Query);

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
                    this.BitmapMetadata.RemoveQuery(FotoFlyQueries.AddressOfGpsLookupDate.Query);
                }
                else
                {
                    string addressOfGpsLookupDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                    this.BitmapMetadata.SetQuery(FotoFlyQueries.AddressOfGpsLookupDate.Query, addressOfGpsLookupDate);
                }
            }
        }

        public DateTime OriginalCameraDate
        {
            get
            {
                string originalCameraDate = this.BitmapMetadata.GetQuery<string>(FotoFlyQueries.OriginalCameraDate.Query);

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
                    this.BitmapMetadata.RemoveQuery(FotoFlyQueries.OriginalCameraDate.Query);
                }
                else
                {
                    string originalCameraDate = value.ToString("yyyy-MM-ddTHH:mm:ss");

                    this.BitmapMetadata.SetQuery(FotoFlyQueries.OriginalCameraDate.Query, originalCameraDate);
                }
            }
        }

        public string OriginalCameraFilename
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(FotoFlyQueries.OriginalCameraFilename.Query);
            }

            set
            {
                this.CreateFotoflyStruct();

                if (string.IsNullOrEmpty(value))
                {
                    this.BitmapMetadata.RemoveQuery(FotoFlyQueries.OriginalCameraFilename.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(FotoFlyQueries.OriginalCameraFilename.Query, value);
                }
            }
        }

        public Address AddressOfGps
        {
            get
            {
                string address = this.BitmapMetadata.GetQuery<string>(FotoFlyQueries.AddressOfGps.Query);

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
                    this.BitmapMetadata.RemoveQuery(FotoFlyQueries.AddressOfGps.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(FotoFlyQueries.AddressOfGps.Query, value.HierarchicalName);
                }
            }
        }

        public string AddressOfGpsSource
        {
            get
            {
                return this.BitmapMetadata.GetQuery<string>(FotoFlyQueries.AddressOfGpsSource.Query);
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.BitmapMetadata.RemoveQuery(FotoFlyQueries.AddressOfGpsSource.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(FotoFlyQueries.AddressOfGpsSource.Query, value);
                }
            }
        }

        public GpsPosition.Accuracies AccuracyOfGps
        {
            get
            {
                string accuracyOfGpsString = this.BitmapMetadata.GetQuery<string>(FotoFlyQueries.AccuracyOfGps.Query);

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
                    this.BitmapMetadata.RemoveQuery(FotoFlyQueries.AccuracyOfGps.Query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(FotoFlyQueries.AccuracyOfGps.Query, (int)value + "-" + value.ToString());
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

        private void CreateFotoflyStruct()
        {
            if (!this.BitmapMetadata.ContainsQuery(FotoFlyQueries.FotoFlyStruct.Query))
            {
                this.BitmapMetadata.SetQuery(FotoFlyQueries.FotoFlyStruct.Query, new BitmapMetadata(XmpQueries.StructBlock));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            // Force Garbage ObjCollection
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
