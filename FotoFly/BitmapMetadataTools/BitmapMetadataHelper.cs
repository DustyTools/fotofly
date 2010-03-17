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
                if (unknownObject.GetType() == bitmapMetadata.GetStorageType(typeof(SRational)))
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
                if (unknownObject.GetType() == bitmapMetadata.GetStorageType(typeof(URational)))
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
                if (unknownObject.GetType() == bitmapMetadata.GetStorageType(typeof(URationalTriplet)))
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
            else if (typeof(T) == typeof(TimeSpan))
            {
                string timespanString = (unknownObject as string);

                if (!timespanString.EndsWith("+0000"))
                {
                    throw new NotImplementedException("Timespan contains timezone, need to implement the right code");
                }
                else if (timespanString.Length > 6)
                {
                    int hour = Convert.ToInt32(timespanString.Substring(0, 2));
                    int minute = Convert.ToInt32(timespanString.Substring(2, 2));
                    int second = Convert.ToInt32(timespanString.Substring(4, 2));

                    TimeSpan timeSpan = new TimeSpan(hour, minute, second);

                    return (T)Convert.ChangeType(timeSpan, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            else if (typeof(T) == typeof(DateTime))
            {
                // Split the string into date & time
                // Convert T to a space
                string[] dateTimeString = (unknownObject as string).Replace("T", " ").Split(' ');

                if (dateTimeString.Length == 1 && dateTimeString[0].Length == 8)
                {
                    int year = Convert.ToInt32(dateTimeString[0].Substring(0, 4));
                    int month = Convert.ToInt32(dateTimeString[0].Substring(4, 2));
                    int day = Convert.ToInt32(dateTimeString[0].Substring(6, 2));

                    DateTime dateTime = new DateTime(year, month, day);

                    return (T)Convert.ChangeType(dateTime, typeof(T));
                }
                else if (dateTimeString.Length == 2)
                {
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

                return default(T);
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

        public static void MoveQuery(this BitmapMetadata bitmapMetadata, string sourceQuery, string destinationQuery, bool overWriteExistingData)
        {
            // Check Source Query exists
            if (bitmapMetadata.ContainsQuery(sourceQuery))
            {
                bool existingData = bitmapMetadata.ContainsQuery(destinationQuery);

                if (existingData && !overWriteExistingData)
                {
                    // Skip the copy and source the value
                    bitmapMetadata.RemoveQuery(sourceQuery);
                }
                else
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
        }

        public static void MoveStruct(this BitmapMetadata bitmapMetadata, string sourceNameSpace, string sourceStructName, string destinationNameSpace, string destinationStructName, bool overWriteExistingData, bool ignoreInvalidSource)
        {
            // Rewrite Structnames
            string sourceStruct = @"/xmp/" + sourceNameSpace + sourceStructName;
            string sourceQuery = sourceStruct + "/" + sourceNameSpace;

            string destinationStruct = @"/xmp/" + destinationNameSpace + destinationStructName;
            string destinationQuery = destinationStruct + "/" + destinationNameSpace;

            // Check Source exists
            bool sourceExists = bitmapMetadata.ContainsQuery(sourceStruct);

            if (sourceExists)
            {
                // Create a new Struct, if it doesn't exist
                if (!bitmapMetadata.ContainsQuery(destinationStruct))
                {
                    bitmapMetadata.SetQuery(destinationStruct, new BitmapMetadata("xmpstruct"));
                }

                List<string> sourceSubQueries = bitmapMetadata.GetSubQueries(sourceStruct);

                // Loop through all data in the struct
                foreach (string sourceProperty in sourceSubQueries)
                {
                    // Construct the full query
                    string destinationProperty = sourceProperty.Replace(sourceQuery, destinationQuery);

                    bitmapMetadata.MoveQuery(sourceProperty, destinationProperty, overWriteExistingData);
                }

                // Remove the old struct
                bitmapMetadata.RemoveQuery(sourceStruct);

                // Get Destination Queries
                List<string> destinationSubQueries = bitmapMetadata.GetSubQueries(destinationStruct);

                // Check the destination has at least as many subqueries as the source
                if (destinationSubQueries.Count < sourceSubQueries.Count)
                {
                    throw new Exception("Source Struct was not fully moved to Destination");
                }
            }
            else if (!sourceExists && !ignoreInvalidSource)
            {
                throw new Exception("Source Struct does not exist:" + sourceStruct);
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

        public static List<string> GetSubQueries(this BitmapMetadata bitmapMetadata, string query)
        {
            BitmapMetadata subBitmapMetadata = bitmapMetadata.GetQuery<BitmapMetadata>(query);

            if (subBitmapMetadata == null)
            {
                throw new Exception("Query does not return a BitmapMetadata object");
            }

            List<string> subQueries = new List<string>();

            // Loop through all data in the struct
            foreach (string subQuery in subBitmapMetadata)
            {
                subQueries.Add(query + subQuery);
            }

            return subQueries;
        }

        public static bool IsQueryValidAndOfType(this BitmapMetadata bitmapMetadata, string query, Type type)
        {
            // Check there's a query value
            if (bitmapMetadata.ContainsQuery(query))
            {
                type = bitmapMetadata.GetStorageType(type);
                
                // Get object
                Object obj = bitmapMetadata.GetQuery(query).GetType();

                if (obj != null)
                {
                    // Special case for empty strings, return true if set
                    // Otherwise check for null and then type
                    if (type == typeof(string))
                    {
                        return true;
                    }
                    else if (obj == type)
                    {
                        return true;
                    }
                }
            }

            return false;
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

        private static Type GetStorageType(this BitmapMetadata bitmapMetadata, Type type)
        {
            // Flip type to the expected storage type
            if (type == typeof(SRational))
            {
                type = typeof(Int64);
            }
            else if (type == typeof(URational))
            {
                type = typeof(UInt64);
            }
            else if (type == typeof(URationalTriplet))
            {
                type = typeof(UInt64[]);
            }
            else if (type == typeof(ExifDateTime))
            {
                type = typeof(string);
            }
            else if (type == typeof(TimeSpan))
            {
                type = typeof(string);
            }
            else if (type == typeof(DateTime))
            {
                type = typeof(string);
            }
            
            return type;
        }
    }
}
