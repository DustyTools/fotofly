// <copyright file="ResolverCache.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>Class that manages cached GPS results</summary>
namespace FotoFly.Geotagging.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class ResolverCache
    {
        private readonly string filePrefix = "FotoFlyGeoCache-";

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
            this.cacheForwardFileName = this.cacheDirectory + this.filePrefix + "_Forward.log";
            this.cacheReverseFileName = this.cacheDirectory + this.filePrefix + "_Reverse.log";
            this.failedReverseFileName = this.cacheDirectory + this.filePrefix + "_ReverseFailed.log";
            this.failedForwardFileName = this.cacheDirectory + this.filePrefix + "_FowardFailed.log";

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
                        where x.DegreesMinutesSecondsAltitude == gpsPosition.DegreesMinutesSecondsAltitude
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

                this.WriteFailedRecords(this.cacheReverseFileName, this.failedForwardRecords);
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
                        returnValue.Add(new Address(file.ReadLine()));
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

                        string[] data = newData.Split(',');

                        returnValue.Add(new GpsPosition(Convert.ToDouble(data[0]), Convert.ToDouble(data[1])));
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

                        string[] data = newData.Split(',');

                        returnValue.Add(new GeoCacheRecord(data[0], data[1], data[2], data[3]));
                    }

                    file.Close();
                }
            }

            return returnValue;
        }

        private void WriteFailedRecords(string filename, List<Address> records)
        {
            // Delete the existing File
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            if (records.Count > 0)
            {
                // Write the current data into the file
                using (StreamWriter file = new StreamWriter(filename))
                {
                    foreach (Address address in records)
                    {
                        file.WriteLine(address.HierarchicalName);
                    }

                    file.Close();
                }
            }
        }

        private void WriteFailedRecords(string filename, List<GpsPosition> records)
        {
            // Delete the existing File
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            if (records.Count > 0)
            {
                // Write the current data into the file
                using (StreamWriter file = new StreamWriter(filename))
                {
                    foreach (GpsPosition gpsPosition in records)
                    {
                        StringBuilder newLine = new StringBuilder();

                        newLine.Append(gpsPosition.Latitude.Numeric);
                        newLine.Append(", ");
                        newLine.Append(gpsPosition.Longitude.Numeric);

                        file.WriteLine(newLine.ToString());
                    }

                    file.Close();
                }
            }
        }

        private void WriteCachedRecords(string filename, List<GeoCacheRecord> cachedResults)
        {
            // Delete the existing File
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            if (cachedResults.Count > 0)
            {
                // Write the current data into the file
                using (StreamWriter file = new StreamWriter(filename))
                {
                    var resultsToSave = from x in cachedResults
                                        orderby x.Address.HierarchicalName
                                        select x;

                    foreach (GeoCacheRecord results in resultsToSave.ToList<GeoCacheRecord>())
                    {
                        StringBuilder newLine = new StringBuilder();

                        newLine.Append(results.Address.HierarchicalName);
                        newLine.Append(", ");
                        newLine.Append(results.GpsPosition.Latitude.Numeric.ToString());
                        newLine.Append(", ");
                        newLine.Append(results.GpsPosition.Longitude.Numeric.ToString());
                        newLine.Append(", ");
                        newLine.Append(results.Date.ToString());

                        file.WriteLine(newLine.ToString());
                    }

                    file.Close();
                }
            }
        }
    }
}
