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
        public static T GetQuery<T>(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQuery(query);

            if (unknownObject != null && !typeof(T).IsAssignableFrom(unknownObject.GetType()))
            {
                throw new System.ArgumentException("query has type " + unknownObject.GetType().ToString());
            }

            return (T)unknownObject;
        }

        public static UInt16? GetQueryAsUInt16(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQuery<object>(query);

            if (unknownObject != null && unknownObject is UInt16)
            {
                return (UInt16)unknownObject;
            }
            else
            {
                return null;
            }
        }

        public static UInt32? GetQueryAsUint32(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQuery<object>(query);

            if (unknownObject != null && unknownObject is UInt32)
            {
                return Convert.ToUInt32(unknownObject);
            }

            return null;
        }
        
        public static DateTime? GetQueryAsDateTime(this BitmapMetadata bitmapMetadata, string query)
        {
            // ToDo try unknownObject is string?
            object unknownObject = bitmapMetadata.GetQuery<object>(query);

            if (unknownObject == null || string.IsNullOrEmpty(unknownObject.ToString()))
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

        public static ExifDateTime GetQueryAsExifDateTime(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQuery<object>(query);

            if (string.IsNullOrEmpty(unknownObject.ToString()))
            {
                return new ExifDateTime(new DateTime());
            }
            else
            {
                return new ExifDateTime(unknownObject as string);
            }
        }

        public static Rational GetQueryAsRational(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQuery<object>(query);

            if (unknownObject != null && unknownObject is Int64)
            {
                return new Rational((Int64)unknownObject);
            }
            else
            {
                return null;
            }
        }

        public static URational GetQueryAsURational(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQuery<object>(query);

            if (unknownObject != null && unknownObject is UInt64)
            {
                return new URational((UInt64)unknownObject);
            }
            else
            {
                return null;
            }
        }

        public static GpsRational GetQueryAsGpsRational(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQuery<object>(query);

            if (unknownObject != null && unknownObject is UInt64[])
            {
                return new GpsRational((UInt64[])unknownObject);
            }
            else
            {
                return null;
            }
        }

        public static BitmapMetadata GetQueryAsBitmapMetadata(this BitmapMetadata bitmapMetadata, string query)
        {
            object unknownObject = bitmapMetadata.GetQuery<object>(query);

            if (unknownObject != null && unknownObject is BitmapMetadata)
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
