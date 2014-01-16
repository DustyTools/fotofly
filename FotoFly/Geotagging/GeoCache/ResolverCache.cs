// <copyright file="ResolverCache.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>Class that manages cached GPS results</summary>
namespace Fotofly.Geotagging.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
	using System.Globalization;

    public class ResolverCache
    {
        private readonly string filePrefix = "FotoflyGeoCache-";

        private string cacheDirectory;
        private string cacheSuffixName;

        // Forward is Address to GpsPosition
        private string cacheForwardFileName;
        private List<GeoCacheRecord> cachedForwardRecords;

        private string failedForwardFileName;
        private List<Address> failedForwardRecords;

        // Reverse is GpsPosition to Address
        private string cacheReverseFileName;
        private List<GeoCacheRecord> cachedReverseRecords;

        private string failedReverseFileName;
        private List<GpsPosition> failedReverseRecords;

        public ResolverCache(string cacheDirectory, string cacheName)
        {
            this.cacheDirectory = cacheDirectory.TrimEnd('\\') + "\\";
            this.cacheSuffixName = cacheName;

            if (!Directory.Exists(this.cacheDirectory))
            {
                throw new Exception("CacheManager.Directory does not exist: " + this.cacheDirectory);
            }

            // Declare all filenames
            this.cacheForwardFileName = this.cacheDirectory + this.filePrefix + this.cacheSuffixName + "_Forward.log";
            this.cacheReverseFileName = this.cacheDirectory + this.filePrefix + this.cacheSuffixName + "_Reverse.log";
            this.failedReverseFileName = this.cacheDirectory + this.filePrefix + this.cacheSuffixName + "_ReverseFailed.log";
            this.failedForwardFileName = this.cacheDirectory + this.filePrefix + this.cacheSuffixName + "_ForwardFailed.log";

            // Read Records Files
            this.cachedForwardRecords = this.ReadCacheRecords(this.cacheForwardFileName);
            this.cachedReverseRecords = this.ReadCacheRecords(this.cacheReverseFileName);

            // Read Failed Files
            this.failedForwardRecords = this.ReadFailedAddresses(this.failedForwardFileName);
            this.failedReverseRecords = this.ReadFailedGpsPosition(this.failedReverseFileName);
        }

        /// <summary>
        /// Check Reverse Lookup cache
        /// </summary>
        /// <param name="gpsPosition">Gps Position to Lookup</param>
        /// <returns>Address matching the Gps Position</returns>
        public Address FindAddress(GpsPosition gpsPosition)
        {
            var query = from x in this.cachedReverseRecords
                        where x.GpsPosition.Latitude.Numeric == gpsPosition.Latitude.Numeric
                        && x.GpsPosition.Longitude.Numeric == gpsPosition.Longitude.Numeric
                        select x.Address;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Check Forward Lookup cache
        /// </summary>
        /// <param name="address">Address to Lookup</param>
        /// <returns>GpsPosition matching the Address</returns>
        public GpsPosition FindGpsPosition(Address address)
        {
            return this.FindGpsPosition(address, address.HierarchicalNameLength);
        }

        /// <summary>
        /// Check Forward Lookup cache
        /// </summary>
        /// <param name="address">Address to Lookup</param>
        /// <param name="accuracy">Total parts of the address to use (1 = Country, 2 = Region etc)</param>
        /// <returns>GpsPosition matching the Address</returns>
        public GpsPosition FindGpsPosition(Address address, int accuracy)
        {
            var query = from x in this.cachedForwardRecords
                        where x.Address.AddressTruncated(accuracy).HierarchicalName.ToLower() == address.AddressTruncated(accuracy).HierarchicalName.ToLower()
                        select x.GpsPosition;

            return query.FirstOrDefault();
        }

        public void AddToReverseFailedRecords(GpsPosition gpsPosition)
        {
            var query = from x in this.failedReverseRecords
                        where x.ToString() == gpsPosition.ToString()
                        select x;

            if (query.FirstOrDefault() == null)
            {
                this.failedReverseRecords.Add(gpsPosition);

                this.WriteFailedRecords(this.failedReverseFileName, this.failedReverseRecords);
            }
        }

        public void AddToForwardFailedRecords(Address address)
        {
            var query = from x in this.failedForwardRecords
                        where x.HierarchicalName.ToLower() == address.HierarchicalName.ToLower()
                        select x;

            if (query.FirstOrDefault() == null)
            {
                this.failedForwardRecords.Add(address);

                this.WriteFailedRecords(this.failedForwardFileName, this.failedForwardRecords);
            }
        }

        public void AddToForwardCacheRecords(Address address, GpsPosition gpsPosition, DateTime date)
        {
            GpsPosition existingGpsPosition = this.FindGpsPosition(address);

            // Remove existing entry
            if (existingGpsPosition != null)
            {
                var query = from x in this.cachedForwardRecords
                            where x.Address.HierarchicalName.ToLower() == address.HierarchicalName.ToLower()
                            select x;

                this.cachedForwardRecords.Remove(query.First());
            }

            GeoCacheRecord cachedResult = new GeoCacheRecord();
            cachedResult.Address = address.Clone() as Address;
            cachedResult.Date = date;
            cachedResult.GpsPosition = gpsPosition.Clone() as GpsPosition;

            this.cachedForwardRecords.Add(cachedResult);

            this.WriteCachedRecords(this.cacheForwardFileName, this.cachedForwardRecords, true);
        }

        public void AddToReverseCacheRecords(Address address, GpsPosition gpsPosition, DateTime date)
        {
            Address existingAddress = this.FindAddress(gpsPosition);

            // Remove existing entry
            if (existingAddress != null)
            {
                var query = from x in this.cachedReverseRecords
                            where x.GpsPosition.Latitude.Numeric == gpsPosition.Latitude.Numeric
                            && x.GpsPosition.Longitude.Numeric == gpsPosition.Longitude.Numeric
                            select x;
                
                this.cachedReverseRecords.Remove(query.First());
            }

            GeoCacheRecord cachedResult = new GeoCacheRecord();
            cachedResult.Address = address.Clone() as Address;
            cachedResult.Date = date;
            cachedResult.GpsPosition = gpsPosition.Clone() as GpsPosition;

            this.cachedReverseRecords.Add(cachedResult);

            this.WriteCachedRecords(this.cacheReverseFileName, this.cachedReverseRecords, false);
        }

        private List<Address> ReadFailedAddresses(string filename)
        {
            List<Address> returnValue = new List<Address>();

            if (File.Exists(filename))
            {
                // Read the file line by line.
                using (StreamReader file = new StreamReader(filename))
                {
                    while (!file.EndOfStream)
                    {
                        string newData = file.ReadLine();

                        // Skip any line starting with a comment
                        if (!newData.StartsWith("##"))
                        {
                            returnValue.Add(new Address(newData));
                        }
                    }

                    file.Close();
                }
            }

            return returnValue;
        }

        private List<GpsPosition> ReadFailedGpsPosition(string filename)
        {
            List<GpsPosition> returnValue = new List<GpsPosition>();

            if (File.Exists(filename))
            {
                // Read the file line by line.
                using (StreamReader file = new StreamReader(filename))
                {
                    while (!file.EndOfStream)
                    {
                        string newData = file.ReadLine();

                        // Skip any line starting with a comment
                        if (!newData.StartsWith("##"))
                        {
                            string[] data = newData.Split(',');

							returnValue.Add(new GpsPosition(Convert.ToDouble(data[0], NumberFormatInfo.InvariantInfo), Convert.ToDouble(data[1], NumberFormatInfo.InvariantInfo)));
                        }
                    }

                    file.Close();
                }
            }

            return returnValue;
        }

        private List<GeoCacheRecord> ReadCacheRecords(string filename)
        {
            List<GeoCacheRecord> returnValue = new List<GeoCacheRecord>();

            if (File.Exists(filename))
            {
                // Read the file line by line.
                using (StreamReader file = new StreamReader(filename))
                {
                    while (!file.EndOfStream)
                    {
                        string newData = file.ReadLine();

                        // Skip any line starting with a comment
                        if (!newData.StartsWith("##"))
                        {
                            string[] data = newData.Split(',');

                            returnValue.Add(new GeoCacheRecord(data[0], data[1], data[2], data[3]));
                        }
                    }

                    file.Close();
                }
            }

            return returnValue;
        }

        private void WriteFailedRecords(string filename, List<Address> records)
        {
            List<string> recordsToWrite = new List<string>();

            foreach (Address address in records)
            {
                recordsToWrite.Add(address.HierarchicalName);
            }

            this.WriteToFile(filename, recordsToWrite, "Failed Forward lookups for Address to GpsPosition resolution");
        }

        private void WriteFailedRecords(string filename, List<GpsPosition> records)
        {
            List<string> recordsToWrite = new List<string>();

            foreach (GpsPosition gpsPosition in records)
            {
                StringBuilder newLine = new StringBuilder();

                newLine.Append(gpsPosition.Latitude.Numeric);
                newLine.Append(", ");
                newLine.Append(gpsPosition.Longitude.Numeric);

                recordsToWrite.Add(newLine.ToString());
            }

            this.WriteToFile(filename, recordsToWrite, "Failed Reverse lookups for GpsPosition to Address resolution");
        }

        private void WriteCachedRecords(string filename, List<GeoCacheRecord> cachedResults, bool forwardLookup)
        {
            List<string> recordsToWrite = new List<string>();

            foreach (GeoCacheRecord results in cachedResults)
            {
                StringBuilder newLine = new StringBuilder();

                newLine.Append(results.Address.HierarchicalName);
                newLine.Append(", ");
                newLine.Append(results.GpsPosition.Latitude.Numeric.ToString());
                newLine.Append(", ");
                newLine.Append(results.GpsPosition.Longitude.Numeric.ToString());
                newLine.Append(", ");
                newLine.Append(results.Date.ToString());

                recordsToWrite.Add(newLine.ToString());
            }

            if (forwardLookup)
            {
                this.WriteToFile(filename, recordsToWrite, "Forward lookups for Address to GpsPosition resolution");
            }
            else
            {
                this.WriteToFile(filename, recordsToWrite, "Reverse lookups for GpsPosition to Address resolution");
            }
        }

        private void WriteToFile(string filename, List<string> recordsToWrite, string fileHeader)
        {
            if (recordsToWrite.Count > 0)
            {
                // Write the current data into the file
                using (StreamWriter file = new StreamWriter(filename, false))
                {
                    file.WriteLine("## " + fileHeader);
                    file.WriteLine("## Last saved " + DateTime.Now.ToString());
                    file.WriteLine("##########################################################################################");

                    // Select distinct rows and order the results
                    foreach (string record in recordsToWrite.OrderBy(x => x).Distinct())
                    {
                        file.WriteLine(record);
                    }

                    file.Close();
                }
            }
        }
    }
}
