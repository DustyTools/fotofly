// <copyright file="BitmapMetadataHelper.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>Static Class that provides extension methods to BitmapMetadata</summary>
namespace Fotofly.BitmapMetadataTools
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
            // Return default if the BitmapMetadata doesn't contain the query
            // Would prefer to return null
            if (!bitmapMetadata.ContainsQuery(query))
            {
                return default(T);
            }

            // Grab object
            object unknownObject = bitmapMetadata.GetQuery(query);

            if (unknownObject == null)
            {
                return default(T);
            }
            else if (typeof(T) == typeof(SRational))
            {
                if (unknownObject.GetType() == typeof(Int64))
                {
                    // Create new Rational, casting the unknownobject as an Int64
                    SRational rational = new SRational((Int64)unknownObject);

                    // Convert back to typeof(T)
                    return (T)Convert.ChangeType(rational, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            else if (typeof(T) == typeof(URational))
            {
                if (unknownObject.GetType() == typeof(UInt64))
                {
                    // Create new URational, casting the unknownobject as an UInt64
                    URational urational = new URational((UInt64)unknownObject);

                    // Convert back to typeof(T)
                    return (T)Convert.ChangeType(urational, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            else if (typeof(T) == typeof(URationalTriplet))
            {
                if (unknownObject.GetType() == typeof(UInt64[]))
                {
                    // Create new GpsRational, casting the unknownobject as an Int64[]
                    URationalTriplet gpsRational = new URationalTriplet((UInt64[])unknownObject);

                    // Convert back to typeof(T)
                    return (T)Convert.ChangeType(gpsRational, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            else if (typeof(T) == typeof(ExifDateTime))
            {
                // Create new ExifDateTime, casting the unknownobject as a string
                ExifDateTime exifDateTime = new ExifDateTime(unknownObject.ToString());

                // Convert back to typeof(T)
                return (T)Convert.ChangeType(exifDateTime, typeof(T));
            }
            else if (typeof(T) == typeof(DateTime))
            {
                // Split the string into date & time
                // Convert T to a space
                string[] dateTimeString = (unknownObject as string).Replace("T", " ").Split(' ');

                if (dateTimeString.Length != 2)
                {
                    return default(T);
                }

                // Ensure seperate is dash for Date
                dateTimeString[0] = dateTimeString[0].Replace(":", "-");

                // Strip the Z from the Time
                dateTimeString[1] = dateTimeString[1].TrimEnd('Z');
                
                DateTime dateTime = DateTime.Parse(dateTimeString[0] + " " + dateTimeString[1]);

                // Parse as local time
                DateTime localDateTime = new DateTime(dateTime.Ticks, DateTimeKind.Local);

                // Convert back to typeof(T)
                return (T)Convert.ChangeType(localDateTime, typeof(T));
            }
            else if (typeof(T) == typeof(string))
            {
                // Trim the string
                return (T)Convert.ChangeType(unknownObject.ToString().Trim(), typeof(T));
            }
            else if (!typeof(T).IsAssignableFrom(unknownObject.GetType()))
            {
                // Throw exception if the object is the wrong type
                throw new System.ArgumentException("Query \"" + query + "\" has type \"" + unknownObject.GetType().ToString() + "\" not expected type \"" + typeof(T).ToString() + "\"");
            }

            return (T)unknownObject;
        }

        public static void MoveQuery(this BitmapMetadata bitmapMetadata, string sourceQuery, string destinationQuery)
        {
            // Check Source Query exists
            if (bitmapMetadata.ContainsQuery(sourceQuery))
            {
                // Copy the Query
                bitmapMetadata.CopyQuery(sourceQuery, destinationQuery);

                // Finally remove old query
                bitmapMetadata.RemoveQuery(sourceQuery);

                // Ensure old query has been removed
                if (!bitmapMetadata.ContainsQuery(destinationQuery))
                {
                    throw new Exception("Move Query Failed");
                }
            }
        }

        public static void CopyQuery(this BitmapMetadata bitmapMetadata, string sourceQuery, string destinationQuery)
        {
            // Check Source Query exists
            if (bitmapMetadata.ContainsQuery(sourceQuery))
            {
                // Garb source object
                object sourceObject = bitmapMetadata.GetQuery(sourceQuery);

                // Remove destination Query
                bitmapMetadata.RemoveQuery(sourceQuery);

                // Set destination Query
                bitmapMetadata.SetQuery(destinationQuery, sourceObject.ToString());

                // Grab Destination Query
                object destinationObject = bitmapMetadata.GetQuery(destinationQuery);

                // Check Type and value.ToString() match
                if (sourceObject.GetType() != destinationObject.GetType() || sourceObject.ToString() != destinationObject.ToString())
                {
                    throw new Exception("Copy Query Failed");
                }
            }
        }

        public static bool IsQueryOfType(this BitmapMetadata bitmapMetadata, string query, Type type)
        {
            return bitmapMetadata.GetQueryType(query) == type;
        }

        public static Type GetQueryType(this BitmapMetadata bitmapMetadata, string query)
        {
            // Return null if no query value
            if (!bitmapMetadata.ContainsQuery(query))
            {
                return null;
            }

            // Return Object Type
            return bitmapMetadata.GetQuery(query).GetType();
        }

        public static void SetQueryOrRemove(this BitmapMetadata bitmapMetadata, string query, object value, bool whenTrueRemoveQuery)
        {
            if (whenTrueRemoveQuery)
            {
                bitmapMetadata.RemoveQuery(query);
            }
            else
            {
                bitmapMetadata.SetQuery(query, value);
            }
        }
    }
}
