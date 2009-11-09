// <copyright file="BitmapMetadataHelper.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>Static Class that provides extension methods to BitmapMetadata</summary>
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
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static class BitmapMetadataHelper
    {
        public static int? GetQueryAsInt(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQueryAsObject(query);

            if (string.IsNullOrEmpty(unknownObject.ToString()))
            {
                return null;
            }
            else
            {
                int returnValue;

                if (int.TryParse(unknownObject.ToString(), out returnValue))
                {
                    return returnValue;
                }
                else
                {
                    return null;
                }
            }
        }

        public static uint? GetQueryAsUint(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQueryAsObject(query);

            if (string.IsNullOrEmpty(unknownObject.ToString()))
            {
                return null;
            }
            else if (unknownObject is ushort[])
            {
                return null;
            }
            else
            {
                try
                {
                    return Convert.ToUInt32(unknownObject);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static string GetQueryAsString(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQueryAsObject(query);

            if (string.IsNullOrEmpty(unknownObject.ToString()))
            {
                return String.Empty;
            }
            else
            {
                try
                {
                    return unknownObject.ToString();
                }
                catch
                {
                    return String.Empty;
                }
            }
        }
        
        public static object GetQueryAsObject(this BitmapMetadata bitmapMetadata, string query)
        {
            try
            {
                if (bitmapMetadata.ContainsQuery(query))
                {
                    return bitmapMetadata.GetQuery(query);
                }
                else
                {
                    return String.Empty;
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public static DateTime? GetQueryAsDateTime(this BitmapMetadata bitmapMetadata, string query)
        {
            // ToDo try unknownObject is string?
            object unknownObject = bitmapMetadata.GetQueryAsObject(query);

            if (string.IsNullOrEmpty(unknownObject.ToString()))
            {
                return null;
            }
            else
            {
                try
                {
                    string date = (string)unknownObject;

                    date = date.TrimEnd('Z');

                    DateTime newDateTime = DateTime.Parse(date);

                    newDateTime = new DateTime(newDateTime.Ticks, DateTimeKind.Local);

                    return newDateTime;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static DateTime? GetQueryAsExifDateTime(this BitmapMetadata bitmapMetadata, string query)
        {
            // ToDo try unknownObject is string?
            object unknownObject = bitmapMetadata.GetQueryAsObject(query);

            if (string.IsNullOrEmpty(unknownObject.ToString()))
            {
                return null;
            }
            else
            {
                try
                {
                    string date = (string)unknownObject;

                    StringBuilder formattedDate = new StringBuilder();
                    formattedDate.Append(date.Substring(0, 4)); // year
                    formattedDate.Append(@"/");
                    formattedDate.Append(date.Substring(5, 2)); // month
                    formattedDate.Append(@"/");
                    formattedDate.Append(date.Substring(8, 2)); // day
                    formattedDate.Append("T");
                    formattedDate.Append(date.Substring(11, 2)); // hour
                    formattedDate.Append(":");
                    formattedDate.Append(date.Substring(14, 2)); // minute
                    formattedDate.Append(":");
                    formattedDate.Append(date.Substring(17, 2)); // second

                    DateTime newDateTime = DateTime.Parse(formattedDate.ToString());

                    newDateTime = new DateTime(newDateTime.Ticks, DateTimeKind.Local);

                    return newDateTime;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static Rational GetQueryAsRational(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQueryAsObject(query);

            if (string.IsNullOrEmpty(unknownObject.ToString()) || unknownObject is string)
            {
                return null;
            }
            else
            {
                ulong numeric;

                if (ulong.TryParse(unknownObject.ToString(), out numeric))
                {
                    return new Rational(numeric);
                }
                else
                {
                    return null;
                }
            }
        }

        public static URational GetQueryAsURational(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQueryAsObject(query);

            if (string.IsNullOrEmpty(unknownObject.ToString()) || unknownObject is string)
            {
                return null;
            }
            else
            {
                try
                {
                    return new URational((ulong)unknownObject);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static GpsRational GetQueryAsGpsRational(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQueryAsObject(query);

            if (string.IsNullOrEmpty(unknownObject.ToString()) || unknownObject is string)
            {
                return null;
            }
            else
            {
                try
                {
                    return new GpsRational((ulong[])unknownObject);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static BitmapMetadata GetQueryAsBitmapMetadata(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQueryAsObject(query);

            if (string.IsNullOrEmpty(unknownObject.ToString()))
            {
                return null;
            }
            else if (unknownObject is BitmapMetadata)
            {
                return (BitmapMetadata)unknownObject;
            }
            else
            {
                return null;
            }
        }

        public static List<string> ConvertReadOnlyCollectionToList(this BitmapMetadata bitmapMetadata, ReadOnlyCollection<string> readyOnlyCollection)
        {
            List<string> returnValue = new List<string>();

            if (readyOnlyCollection != null)
            {
                foreach (string tag in readyOnlyCollection)
                {
                    returnValue.Add(tag);
                }
            }

            return returnValue;
        }
    }
}
