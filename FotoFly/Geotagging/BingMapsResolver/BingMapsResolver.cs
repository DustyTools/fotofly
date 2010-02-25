// <copyright file="BingMapsResolver.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>Class to retrieve Addresses from Bing using GPS Position</summary>
namespace Fotofly.Geotagging.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Security.Principal;
    using System.ServiceModel.Security;
    using System.Text;
    using System.Web.Services.Protocols;
    using System.Xml;

    using Fotofly.BingMapsForEnterprise;
    using BingMaps = Fotofly.BingMapsForEnterprise;

    public class BingMapsResolver : IResolverCache
    {
        public static readonly string SourceName = "Bing Maps for Enterprise";
        private List<string> failedLookups;
        private FindServiceSoapClient findService;

        private StringDictionary usstateCodes;
        private List<string> apcountries;
        private List<string> eucountries;
        private List<string> nacountries;

        private ResolverCache resolverCache;

        public BingMapsResolver(string userName, string password)
        {
            // Create the find service, pointing at the correct place
            this.findService = new FindServiceSoapClient();

            // set the logon information
            this.findService.ClientCredentials.HttpDigest.ClientCredential = new NetworkCredential(userName, password);
            this.findService.ClientCredentials.HttpDigest.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

            // Build Region Data
            this.BuildLookups();

            // Setup the Failed Lookup List
            this.failedLookups = new List<string>();
        }

        private enum BingMapsDataSources
        {
            EU,
            NA,
            AP,
            BR,
            World
        }

        public void ConfigResolverCache(string cacheDirectory, string cacheName)
        {
            this.resolverCache = new ResolverCache(cacheDirectory, cacheName);
        }

        public Fotofly.Address FindAddress(GpsPosition gps, string country)
        {
            Fotofly.Address returnValue = new Fotofly.Address();

            // Check Resolver Cache
            if (this.resolverCache != null)
            {
                returnValue = this.resolverCache.FindAddress(gps);

                if (returnValue.IsValidAddress)
                {
                    return returnValue;
                }
            }

            // Set the LatLong
            LatLong latLong = new LatLong();
            latLong.Latitude = gps.Latitude.Numeric;
            latLong.Longitude = gps.Longitude.Numeric;

            // Set the datasource
            BingMapsDataSources dataSource = this.LiveMapsDataSource(country);

            GetInfoOptions options = new GetInfoOptions();
            options.IncludeAddresses = true;
            options.IncludeAllEntityTypes = false;

            GetLocationInfoRequest infoRequest = new GetLocationInfoRequest();
            infoRequest.dataSourceName = "MapPoint." + dataSource.ToString();
            infoRequest.options = options;
            infoRequest.location = latLong;

            Location[] places = null;

            try
            {
                places = this.findService.GetLocationInfo(null, null, latLong, infoRequest.dataSourceName, options);
            }
            catch
            {
                places = null;
            }

            if (places != null && places.Length > 0 && places[0].Address != null)
            {
                returnValue.Country = places[0].Address.CountryRegion;
                returnValue.Region = this.LookupRegionName(places[0].Address.Subdivision, returnValue.Country);
                returnValue.City = places[0].Address.PrimaryCity;
                returnValue.AddressLine = places[0].Address.AddressLine;
            }
            else
            {
                // Add to Failure to cache
                if (this.resolverCache != null)
                {
                    this.resolverCache.AddToReverseFailedRecords(gps);
                }
            }

            return returnValue;
        }

        public GpsPosition FindGpsPosition(Fotofly.Address addressToLookup)
        {
            GpsPosition returnValue = new GpsPosition();

            // Check Resolver Cache
            if (this.resolverCache != null)
            {
                returnValue = this.resolverCache.FindGpsPosition(addressToLookup);

                if (returnValue != null && returnValue.IsValidCoordinate)
                {
                    return returnValue;
                }
            }

            BingMapsDataSources dataSource = this.LiveMapsDataSource(addressToLookup.Country);

            if (this.failedLookups.Contains(addressToLookup.HierarchicalName))
            {
                Debug.WriteLine("BingMapsResolver: " + addressToLookup.HierarchicalName + ": Address skipped (Previous Failed Lookup)");
            }
            else if (dataSource == BingMapsDataSources.World)
            {
                Debug.WriteLine("BingMapsResolver: " + addressToLookup.HierarchicalName + ": Address skipped (World Maps does not support Reverse Geotagging)");
            }
            else
            {
                // Set up the address
                BingMaps.Address address = new BingMaps.Address();
                address.AddressLine = addressToLookup.AddressLine;
                address.PostalCode = string.Empty;
                address.PrimaryCity = addressToLookup.City;
                address.Subdivision = addressToLookup.Region;
                address.CountryRegion = addressToLookup.Country;

                if (string.IsNullOrEmpty(addressToLookup.City))
                {
                    address.PrimaryCity = addressToLookup.Region;
                    address.Subdivision = string.Empty;
                }

                // Set up the specification for the address
                // Set up the specification object.
                FindAddressSpecification findAddressSpec = new FindAddressSpecification();
                findAddressSpec.InputAddress = address;

                // More info: http://msdn2.microsoft.com/en-us/library/ms982198.aspx and http://msdn2.microsoft.com/en-us/library/aa493004.aspx
                findAddressSpec.DataSourceName = "MapPoint." + dataSource.ToString();

                // Set the find options. Allow more return values by decreasing
                // the count of the ThresholdScore option.
                // Also, limit the number of results returned to 20.
                FindOptions myFindOptions = new FindOptions();
                myFindOptions.ThresholdScore = 0.5;
                myFindOptions.Range = new FindRange();
                myFindOptions.Range.StartIndex = 0;
                myFindOptions.Range.Count = 1;
                findAddressSpec.Options = myFindOptions;

                FindAddressRequest addressRequest = new FindAddressRequest();
                addressRequest.specification = findAddressSpec;

                // Create a FindResults object to store the results of the FindAddress request.
                FindResults myFindResults;
                FindResult[] myResults;

                try
                {
                    // Get the results and return them if there are any. 
                    myFindResults = this.findService.FindAddress(null, null, findAddressSpec);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("BingMapsResolver: " + addressToLookup.HierarchicalName + ": Call failed, error = " + e.ToString());

                    returnValue = new GpsPosition();
                    myFindResults = null;
                }

                if (myFindResults != null)
                {
                    myResults = myFindResults.Results;

                    if (myResults != null && myResults.Length != 0)
                    {
                        returnValue = new GpsPosition();
                        returnValue.Latitude.Numeric = myResults[0].FoundLocation.LatLong.Latitude;
                        returnValue.Longitude.Numeric = myResults[0].FoundLocation.LatLong.Longitude;
                        returnValue.Source = BingMapsResolver.SourceName;

                        Debug.WriteLine("BingMapsResolver: " + addressToLookup.HierarchicalName + ": LatLong retrieved");
                    }
                    else
                    {
                        // Add to Failure to cache
                        if (this.resolverCache != null)
                        {
                            this.resolverCache.AddToForwardFailedRecords(addressToLookup);
                        }

                        // Add the failed resolution to the avoid list
                        this.failedLookups.Add(addressToLookup.HierarchicalName);

                        Debug.WriteLine("BingMapsResolver: " + addressToLookup.HierarchicalName + ": Address not found");

                        returnValue = new GpsPosition();
                    }
                }
            }

            return returnValue;
        }

        private void BuildLookups()
        {
            this.apcountries = new List<string>();
            this.apcountries.Add("australia");
            this.apcountries.Add("hong kong");
            this.apcountries.Add("new zealand");
            this.apcountries.Add("singapore");
            this.apcountries.Add("taiwan");

            this.nacountries = new List<string>();
            this.nacountries.Add("canada");
            this.nacountries.Add("puerto rico");
            this.nacountries.Add("united states");

            this.eucountries = new List<string>();
            this.eucountries.Add("austria");
            this.eucountries.Add("belgium");
            this.eucountries.Add("denmark");
            this.eucountries.Add("finland");
            this.eucountries.Add("france");
            this.eucountries.Add("germany");
            this.eucountries.Add("italy");
            this.eucountries.Add("luxembourg");
            this.eucountries.Add("the netherlands");
            this.eucountries.Add("holland");
            this.eucountries.Add("norway");
            this.eucountries.Add("portugal");
            this.eucountries.Add("spain");
            this.eucountries.Add("sweden");
            this.eucountries.Add("switzerland");
            this.eucountries.Add("greece");
            this.eucountries.Add("united kingdom");
            this.eucountries.Add("andorra");
            this.eucountries.Add("czech republic");
            this.eucountries.Add("ireland");
            this.eucountries.Add("liechtenstein");
            this.eucountries.Add("monaco");
            this.eucountries.Add("san marino");
            this.eucountries.Add("slovakia");
            this.eucountries.Add("vatican city");

            this.usstateCodes = new StringDictionary();
            this.usstateCodes.Add("AL", "Alabama");
            this.usstateCodes.Add("IL", "Illinois");
            this.usstateCodes.Add("MT", "Montana");
            this.usstateCodes.Add("RI", "Rhode Island");
            this.usstateCodes.Add("AK", "Alaska");
            this.usstateCodes.Add("IN", "Indiana");
            this.usstateCodes.Add("NE", "Nebraska");
            this.usstateCodes.Add("SC", "South Carolina");
            this.usstateCodes.Add("AZ", "Arizona");
            this.usstateCodes.Add("IA", "Iowa");
            this.usstateCodes.Add("NV", "Nevada");
            this.usstateCodes.Add("SD", "South Dakota");
            this.usstateCodes.Add("AR", "Arkansas");
            this.usstateCodes.Add("KS", "Kansas");
            this.usstateCodes.Add("NH", "New Hampshire");
            this.usstateCodes.Add("TN", "Tennessee");
            this.usstateCodes.Add("CA", "California");
            this.usstateCodes.Add("KY", "Kentucky");
            this.usstateCodes.Add("NJ", "New Jersey");
            this.usstateCodes.Add("TX", "Texas");
            this.usstateCodes.Add("CO", "Colorado");
            this.usstateCodes.Add("LA", "Louisiana");
            this.usstateCodes.Add("NM", "New Mexico");
            this.usstateCodes.Add("UT", "Utah");
            this.usstateCodes.Add("CT", "Connecticut");
            this.usstateCodes.Add("ME", "Maine");
            this.usstateCodes.Add("NY", "New York");
            this.usstateCodes.Add("VT", "Vermont");
            this.usstateCodes.Add("DE", "Delaware");
            this.usstateCodes.Add("MD", "Maryland");
            this.usstateCodes.Add("NC", "North Carolina");
            this.usstateCodes.Add("VA", "Virginia");
            this.usstateCodes.Add("DC", "District of Columbia");
            this.usstateCodes.Add("MA", "Massachusetts");
            this.usstateCodes.Add("ND", "NorthDakota");
            this.usstateCodes.Add("WA", "Washington");
            this.usstateCodes.Add("FL", "Florida");
            this.usstateCodes.Add("MI", "Michigan");
            this.usstateCodes.Add("OH", "Ohio");
            this.usstateCodes.Add("WV", "West Virginia");
            this.usstateCodes.Add("GA", "Georgia");
            this.usstateCodes.Add("MN", "Minnesota");
            this.usstateCodes.Add("OK", "Oklahoma");
            this.usstateCodes.Add("WI", "Wisconsin");
            this.usstateCodes.Add("HI", "Hawaii");
            this.usstateCodes.Add("MS", "Mississippi");
            this.usstateCodes.Add("OR", "Oregon");
            this.usstateCodes.Add("WY", "Wyoming");
            this.usstateCodes.Add("ID", "Idaho");
            this.usstateCodes.Add("MO", "Missouri");
            this.usstateCodes.Add("PA", "Pennsylvania");
        }

        private BingMapsDataSources LiveMapsDataSource(string country)
        {
            BingMapsDataSources bingMapsDataSource = BingMapsDataSources.World;

            if (string.IsNullOrEmpty(country))
            {
                // It happens
            }
            else if (this.eucountries.Contains(country.ToLower()))
            {
                bingMapsDataSource = BingMapsDataSources.EU;
            }
            else if (this.nacountries.Contains(country.ToLower()))
            {
                bingMapsDataSource = BingMapsDataSources.NA;
            }
            else if (this.apcountries.Contains(country.ToLower()))
            {
                bingMapsDataSource = BingMapsDataSources.AP;
            }
            else if (country.ToLower() == "brazil")
            {
                bingMapsDataSource = BingMapsDataSources.BR;
            }

            return bingMapsDataSource;
        }

        private string LookupRegionName(string shortCode, string country)
        {
            string returnValue = string.Empty;

            if (country.ToLower() == "united states")
            {
                returnValue = this.usstateCodes[shortCode];
            }

            if (string.IsNullOrEmpty(returnValue))
            {
                return shortCode;
            }
            else
            {
                return returnValue;
            }
        }
    }
}
