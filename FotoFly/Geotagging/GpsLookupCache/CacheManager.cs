// <copyright file="CacheManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that manages cached GPS results</summary>
namespace FotoFly.Geotagging.GpsLookupCache
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using FotoFly;

    public class GpsLookupCacheManager
    {
        private string cacheDirectory;

        public GpsLookupCacheManager(string cacheDirectory)
        {
            this.cacheDirectory = cacheDirectory.TrimEnd('\\') + "\\";

            if (!Directory.Exists(this.cacheDirectory))
            {
                throw new Exception("CacheManager.Directory does not exist: " + this.cacheDirectory);
            }

            this.ReadCacheFiles();
        }

        public List<CachedResult> CachedResults
        {
            get;
            set;
        }

        public List<string> FailedResults
        {
            get;
            set;
        }

        public CachedResult FindAddressByGpsPosition(GpsPosition gpsPosition)
        {
            var query = from x in this.CachedResults
                        where x.GpsPosition.Latitude.Numeric == gpsPosition.Latitude.Numeric
                        && x.GpsPosition.Longitude.Numeric == gpsPosition.Longitude.Numeric
                        && x.ResultType == CachedResult.ResultTypes.BingAddressLookup
                        select x;

            return query.FirstOrDefault();
        }

        public CachedResult FindCachedResultByFullName(string name)
        {
            var query = from x in this.CachedResults
                        where x.Address.HierarchicalName.ToLower() == name.ToLower()
                        select x;

            return query.FirstOrDefault();
        }

        public GpsPosition FindGpsPositionByFullName(string name)
        {
            var query = from x in this.CachedResults
                        where x.Address.HierarchicalName.ToLower() == name.ToLower()
                        select x.GpsPosition;

            return query.FirstOrDefault();
        }

        public GpsPosition FindGpsPositionByCountryRegionCity(string name)
        {
            var query = from x in this.CachedResults
                        where x.Address.HierarchicalCountryRegionCity.ToLower() == name.ToLower()
                        select x.GpsPosition;

            return query.FirstOrDefault();
        }

        public bool IsFailedLookup(Address address)
        {
            return this.FailedResults.Contains(address.HierarchicalName.ToLower());
        }

        public void AddToCache(Address address, GpsPosition gpsPosition, DateTime date, CachedResult.ResultTypes resultType)
        {
            bool addToCache = false;

            CachedResult previousResult = this.FindCachedResultByFullName(address.HierarchicalName);

            if (previousResult == null)
            {
                // No cached entry, add it
                addToCache = true;
            }
            else if (previousResult != null && previousResult.ResultType == CachedResult.ResultTypes.Manual && resultType == CachedResult.ResultTypes.BingGpsLookup)
            {
                // Replace a manual version with a better version
                this.CachedResults.Remove(previousResult);

                addToCache = true;
            }
            else if (previousResult != null && resultType == CachedResult.ResultTypes.BingAddressLookup && previousResult.GpsPosition.ToString() == gpsPosition.ToString())
            {
                // Duplicate Entry, don't add it
                addToCache = false;
            }

            // Only add if not in the cache
            if (addToCache)
            {
                CachedResult cachedResult = new CachedResult();
                cachedResult.Address = address.Clone() as Address;
                cachedResult.Date = date;
                cachedResult.GpsPosition = gpsPosition.Clone() as GpsPosition;
                cachedResult.ResultType = resultType;

                this.CachedResults.Add(cachedResult);
            }
        }

        public void RecordFailedLookup(Address address)
        {
            if (!this.IsFailedLookup(address))
            {
                this.FailedResults.Add(address.HierarchicalName.ToLower());
            }
        }

        public void WriteCacheFiles()
        {
            this.WriteCacheFiles(this.cacheDirectory + "Cache-BingAddressLookups.log", CachedResult.ResultTypes.BingAddressLookup);
            this.WriteCacheFiles(this.cacheDirectory + "Cache-BingGpsLookups.log", CachedResult.ResultTypes.BingGpsLookup);
            this.WriteCacheFiles(this.cacheDirectory + "Cache-GoogleAddressLookups.log", CachedResult.ResultTypes.GoogleAddressLookup);
            this.WriteCacheFiles(this.cacheDirectory + "Cache-Manual.log", CachedResult.ResultTypes.Manual);
            this.WriteFailedFile(this.cacheDirectory + "Failed-BingAddressLookup.log");
        }

        private void ReadCacheFiles()
        {
            // Create new cache
            this.CachedResults = new List<CachedResult>();

            this.ReadCacheFiles(this.cacheDirectory + "Cache-BingAddressLookups.log", CachedResult.ResultTypes.BingAddressLookup);
            this.ReadCacheFiles(this.cacheDirectory + "Cache-BingGpsLookups.log", CachedResult.ResultTypes.BingGpsLookup);
            this.ReadCacheFiles(this.cacheDirectory + "Cache-GoogleAddressLookups.log", CachedResult.ResultTypes.GoogleAddressLookup);
            this.ReadCacheFiles(this.cacheDirectory + "Cache-Manual.log", CachedResult.ResultTypes.Manual);
            this.ReadFailedFile(this.cacheDirectory + "Failed-BingAddressLookup.log");
        }

        private void ReadFailedFile(string failedFile)
        {
            this.FailedResults = new List<string>();

            if (File.Exists(failedFile))
            {
                // Read the file line by line.
                using (StreamReader file = new StreamReader(failedFile))
                {
                    while (!file.EndOfStream)
                    {
                        string newData = file.ReadLine().Trim();

                        if (!string.IsNullOrEmpty(newData))
                        {
                            this.FailedResults.Add(newData);
                        }
                    }

                    file.Close();
                }
            }
        }

        private void ReadCacheFiles(string cacheFile, CachedResult.ResultTypes resultType)
        {
            if (File.Exists(cacheFile))
            {
                // Read the file line by line.
                using (StreamReader file = new StreamReader(cacheFile))
                {
                    while (!file.EndOfStream)
                    {
                        string newData = file.ReadLine();

                        string[] data = newData.Split(',');

                        this.AddToCache(data[0], data[1], data[2], data[3], resultType);
                    }

                    file.Close();
                }
            }
        }

        private void WriteFailedFile(string cacheFile)
        {
            // Delete the existing File
            if (File.Exists(cacheFile))
            {
                File.Delete(cacheFile);
            }

            // Write the current data into the file
            using (StreamWriter file = new StreamWriter(cacheFile))
            {
                foreach (string failedResult in this.FailedResults)
                {
                    file.WriteLine(failedResult);
                }

                file.Close();
            }
        }

        private void WriteCacheFiles(string cacheFile, CachedResult.ResultTypes resultType)
        {
            // Delete the existing File
            if (File.Exists(cacheFile))
            {
                File.Delete(cacheFile);
            }

            // Write the current data into the file
            using (StreamWriter file = new StreamWriter(cacheFile))
            {
                var resultsToSave = from x in this.CachedResults
                                    where x.ResultType == resultType
                                    orderby x.Address.HierarchicalName
                                    select x;

                foreach (CachedResult results in resultsToSave.ToList<CachedResult>())
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

        private void AddToCache(string hierarchicalName, string latitude, string longitude, string date, CachedResult.ResultTypes resultType)
        {
            DateTime dateTime;

            if (!DateTime.TryParse(date, out dateTime))
            {
                dateTime = new DateTime();
            }

            // Create new address
            Address address = new Address(hierarchicalName.Trim());

            // Create a new GpsPosition
            GpsPosition gpsPosition = new GpsPosition(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            gpsPosition.SatelliteTime = dateTime;

            switch (resultType)
            {
                case CachedResult.ResultTypes.BingGpsLookup:
                case CachedResult.ResultTypes.BingAddressLookup:
                    gpsPosition.Source = "Bing Maps for Enterprise";
                    break;

                case CachedResult.ResultTypes.GoogleAddressLookup:
                    gpsPosition.Source = "Google Maps";
                    break;

                case CachedResult.ResultTypes.Manual:
                    gpsPosition.Source = "Manual";
                    break;
            }

            this.AddToCache(address, gpsPosition, dateTime, resultType);
        }
    }
}
