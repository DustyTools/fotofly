// <copyright file="WpfBitmapMetadataExtender.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>Class that extends BitmapMetadata with additional methods</summary>
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

    public class WpfBitmapMetadataExtender
    {
        public WpfBitmapMetadataExtender(BitmapMetadata bitmapMetadata)
        {
            this.BitmapMetadata = bitmapMetadata;
        }

        public BitmapMetadata BitmapMetadata
        {
            get;
            set;
        }

        public Dictionary<string, object> MetadataDictionary
        {
            get
            {
                return this.GenerateMetadataDictionary(this.BitmapMetadata, string.Empty);
            }
        }

        public void RemoveProperty(string query)
        {
            try
            {
                this.BitmapMetadata.RemoveQuery(query);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to Remove Property", e);
            }
        }

        public void ClearProperty(string query)
        {
            try
            {
                this.BitmapMetadata.SetQuery(query, String.Empty);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to Clear Property", e);
            }
        }

        public void SetProperty(string query, object value)
        {
            this.SetProperty(query, value, false);
        }

        public void SetProperty(string query, object value, bool ignoreException)
        {
            try
            {
                if (value == null)
                {
                    this.ClearProperty(query);
                }
                else
                {
                    this.BitmapMetadata.SetQuery(query, value);
                }
            }
            catch (Exception e)
            {
                if (!ignoreException)
                {
                    throw new Exception("Unable to set property.", e);
                }
            }
        }

        public object QueryMetadataForObject(string query)
        {
            try
            {
                if (this.BitmapMetadata.ContainsQuery(query))
                {
                    return this.BitmapMetadata.GetQuery(query);
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

        public BitmapMetadata QueryMetadataForBitmapMetadata(string query)
        {
            object unknownObject = this.QueryMetadataForObject(query);

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

        public int? QueryMetadataForInt(string query)
        {
            object unknownObject = this.QueryMetadataForObject(query);

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

        public DateTime? QueryMetadataForDateTime(string query)
        {
            // ToDo try unknownObject is string?
            object unknownObject = this.QueryMetadataForObject(query);

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

        public DateTime? QueryMetadataForExifDateTime(string query)
        {
            // ToDo try unknownObject is string?
            object unknownObject = this.QueryMetadataForObject(query);

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

        public Rational QueryMetadataForRational(string query)
        {
            object unknownObject = this.QueryMetadataForObject(query);

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

        public URational QueryMetadataForURational(string query)
        {
            object unknownObject = this.QueryMetadataForObject(query);

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

        public GpsRational QueryMetadataForGpsRational(string query)
        {
            object unknownObject = this.QueryMetadataForObject(query);

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

        public string QueryMetadataForString(string query)
        {
            object unknownObject = this.QueryMetadataForObject(query);

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

        public uint? QueryMetadataForUint(string query)
        {
            object unknownObject = this.QueryMetadataForObject(query);

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

        public List<string> ConvertReadOnlyCollectionToList(ReadOnlyCollection<string> readyOnlyCollection)
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

        public Dictionary<string, object> GenerateMetadataDictionary(BitmapMetadata metadata)
        {
            return this.GenerateMetadataDictionary(metadata, string.Empty);
        }

        public Dictionary<string, object> GenerateMetadataDictionary(BitmapMetadata metadata, string fullQuery)
        {
            Dictionary<string, object> returnValue = new Dictionary<string, object>();

            if (metadata != null)
            {
                foreach (string query in metadata)
                {
                    string tempQuery = fullQuery + query;

                    try
                    {
                        // query string here is relative to the previous metadata reader.
                        object unknownObject = metadata.GetQuery(query);

                        BitmapMetadata moreMetadata = unknownObject as BitmapMetadata;

                        if (moreMetadata != null)
                        {
                            // Add all sub values
                            foreach (KeyValuePair<string, object> keyValuePair in this.GenerateMetadataDictionary(moreMetadata, tempQuery))
                            {
                                returnValue.Add(keyValuePair.Key, keyValuePair.Value);
                            }
                        }
                        else
                        {
                            returnValue.Add(tempQuery, unknownObject);
                        }
                    }
                    catch
                    {
                        // Swallow it
                    }
                }
            }

            return returnValue;
        }
    }
}
