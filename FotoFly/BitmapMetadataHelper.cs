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
                // Parse the object as a DateTime, stripping out the Z
                DateTime dateTime = DateTime.Parse(((string)unknownObject).TrimEnd('Z'));

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
    }
}
