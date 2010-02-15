// <copyright file="GeotaggingManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>Class for Geotagging Photos</summary>
namespace Fotofly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Fotofly.Geotagging.Resolvers;
    using Fotofly.GpseXchangeFormat;

    public class Geotagger
    {
        // All Resolvers
        private BingMapsResolver bingMapsResolver;
        private GoogleMapsResolver googleMapsResolver;
        private GpsTrackResolver gpsTrackResolver;
        private ResolverCache manualCache;

        public Geotagger()
        {
        }

        public bool IsManualCacheConfigured
        {
            get { return this.manualCache != null; }
        }

        public bool IsBingMapsResolverConfigured
        {
            get { return this.bingMapsResolver != null; }
        }

        public bool IsGoogleMapsResolverConfigured
        {
            get { return this.googleMapsResolver != null; }
        }

        public bool IsGpsTracksResolverConfigured
        {
            get { return this.gpsTrackResolver != null; }
        }

        public void ConfigureManualCache(string cacheDirectory)
        {
            this.manualCache = new ResolverCache(cacheDirectory, "Manual");
        }

        public void ConfigureBingMapsResolver(string bingUsername, string bingPassword, string cacheDirectory)
        {
            this.bingMapsResolver = new BingMapsResolver(bingUsername, bingPassword);
            this.bingMapsResolver.ConfigResolverCache(cacheDirectory, "BingMaps");
        }

        public void ConfigureGoogleMapsResolver(string cacheDirectory)
        {
            this.googleMapsResolver = new GoogleMapsResolver();
            this.googleMapsResolver.ConfigResolverCache(cacheDirectory, "GoogleMaps");
        }

        public void ConfigureGpsTracksResolver(List<string> gpxTrackFiles, string trackSource, int toleranceInMeters, int toleranceInMinutes)
        {
            // Only setup if we have valid tracks
            if (gpxTrackFiles != null && gpxTrackFiles.Count != 0)
            {
                this.gpsTrackResolver = new GpsTrackResolver();
                this.gpsTrackResolver.ToleranceInMeters = toleranceInMeters;
                this.gpsTrackResolver.ToleranceInMinutes = toleranceInMinutes;
                this.gpsTrackResolver.GpsTrackSource = trackSource;

                foreach (string gpxTrackFile in gpxTrackFiles)
                {
                    this.gpsTrackResolver.Add(GpxFileManager.Read(gpxTrackFile));
                }
            }
        }

        public GpsPosition FindGpsPosition(DateTime utcDate)
        {
            if (this.IsGpsTracksResolverConfigured)
            {
                if (utcDate == null || utcDate == new DateTime())
                {
                    throw new Exception("FotoflyMetadata.UtcDate must be set Gps Tracks are UTC based");
                }

                GpsPosition gpsPosition = this.gpsTrackResolver.FindGpsPosition(utcDate);

                if (gpsPosition != null && gpsPosition.IsValidCoordinate)
                {
                    return gpsPosition;
                }
            }

            return new GpsPosition();
        }

        public void FindGpsPosition(JpgPhoto photo)
        {
            // Order is 1) Gps Track 2) Bing Maps 4) Manual Cache

            // Use Gps Tracks if Configured
            if (!photo.Metadata.GpsPosition.IsValidCoordinate && this.IsGpsTracksResolverConfigured)
            {
                if (photo.FotoflyMetadata.UtcDate == null || photo.FotoflyMetadata.UtcDate == new DateTime())
                {
                    throw new Exception("FotoflyMetadata.UtcDate must be set Gps Tracks are UTC based");
                }

                GpsPosition gpsPosition = this.gpsTrackResolver.FindGpsPosition(photo.FotoflyMetadata.UtcDate);

                if (gpsPosition != null && gpsPosition.IsValidCoordinate)
                {
                    photo.Metadata.GpsPosition = gpsPosition;
                }
            }

            // If we have no valid result try alternate methods
            if (!photo.Metadata.GpsPosition.IsValidCoordinate)
            {
                // Retrieve result from Bing & Cache and choose the most accurate
                GpsPosition bingResult = new GpsPosition();
                GpsPosition manualResult = new GpsPosition();

                // Use Bing if Configured
                if (this.IsBingMapsResolverConfigured)
                {
                    // Check Bing
                    // Cycle through the address, start as accurate as possible
                    for (int i = photo.Metadata.IptcAddress.HierarchicalNameLength; i > 0; i--)
                    {
                        // Query Bing
                        bingResult = this.bingMapsResolver.FindGpsPosition(photo.Metadata.IptcAddress.AddressTruncated(i));

                        if (bingResult.IsValidCoordinate)
                        {
                            bingResult.Accuracy = i;
                            break;
                        }
                        else
                        {
                            bingResult = new GpsPosition();
                        }
                    }
                }

                // Use Cache if Configured
                if (this.IsManualCacheConfigured)
                {
                    // Check Cache
                    // Cycle through the address, start as accurate as possible
                    for (int i = photo.Metadata.IptcAddress.HierarchicalNameLength; i > 0; i--)
                    {
                        // Query Cache
                        manualResult = this.manualCache.FindGpsPosition(photo.Metadata.IptcAddress.AddressTruncated(i));

                        if (manualResult != null && manualResult.IsValidCoordinate)
                        {
                            manualResult.Accuracy = i;
                            break;
                        }
                        else
                        {
                            manualResult = new GpsPosition();
                        }
                    }
                }

                if (manualResult.Accuracy == 0 && bingResult.Accuracy == 0)
                {
                    photo.Metadata.GpsPosition = new GpsPosition();
                }
                else if (manualResult.Accuracy > bingResult.Accuracy)
                {
                    photo.Metadata.GpsPosition = manualResult;
                }
                else
                {
                    photo.Metadata.GpsPosition = bingResult;
                }
            }
        }

        public void FindAddress(JpgPhoto photo)
        {
            // Should only be run if the GPS Position was found using a GPS
            // The is determined by knowing the altitude, because that's not normally returned from Lookups
            // Order is: 1) Bing Maps 2) Google Maps
            if (photo.Metadata != null && photo.Metadata.GpsPosition.Dimension == GpsPosition.Dimensions.ThreeDimensional)
            {
                // Use Bing if configured
                if (!photo.FotoflyMetadata.AddressOfGps.IsValidAddress && this.IsBingMapsResolverConfigured)
                {
                    photo.FotoflyMetadata.AddressOfGps = this.bingMapsResolver.FindAddress(photo.Metadata.GpsPosition, photo.Metadata.IptcAddress.Country);
                    photo.FotoflyMetadata.AddressOfGpsLookupDate = DateTime.Now;
                    photo.FotoflyMetadata.AddressOfGpsSource = BingMapsResolver.SourceName;
                }

                // Use Google if configured
                if (!photo.FotoflyMetadata.AddressOfGps.IsValidAddress && this.IsGoogleMapsResolverConfigured)
                {
                    photo.FotoflyMetadata.AddressOfGps = this.googleMapsResolver.FindAddress(photo.Metadata.GpsPosition);
                    photo.FotoflyMetadata.AddressOfGpsLookupDate = DateTime.Now;
                    photo.FotoflyMetadata.AddressOfGpsSource = GoogleMapsResolver.SourceName;
                }

                if (!photo.FotoflyMetadata.AddressOfGps.IsValidAddress)
                {
                    photo.FotoflyMetadata.AddressOfGps = new Address();
                    photo.FotoflyMetadata.AddressOfGpsLookupDate = new DateTime();
                    photo.FotoflyMetadata.AddressOfGpsSource = null;
                }
            }
        }
    }
}
