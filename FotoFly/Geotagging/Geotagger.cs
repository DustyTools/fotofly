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
                    throw new Exception("Metadata.UtcDate must be set Gps Tracks are UTC based");
                }

                GpsPosition gpsPosition = this.gpsTrackResolver.FindGpsPosition(utcDate);

                if (gpsPosition != null && gpsPosition.IsValidCoordinate)
                {
                    return gpsPosition;
                }
            }

            return new GpsPosition();
        }

        public void FindGpsPositionCreated(JpgPhoto photo)
        {
            // Use Gps Tracks if Configured
            if (!photo.Metadata.GpsPositionOfLocationCreated.IsValidCoordinate && this.IsGpsTracksResolverConfigured)
            {
                if (photo.Metadata.DateUtc == null || photo.Metadata.DateUtc == new DateTime())
                {
                    throw new Exception("Metadata.UtcDate must be set Gps Tracks are UTC based");
                }

                GpsPosition gpsPosition = this.gpsTrackResolver.FindGpsPosition(photo.Metadata.DateUtc);

                if (gpsPosition != null && gpsPosition.IsValidCoordinate)
                {
                    photo.Metadata.GpsPositionOfLocationCreated = gpsPosition;
                }
            }
        }

        public void FindGpsPositionShown(JpgPhoto photo)
        {
            // If we have no valid result try alternate methods
            if (!photo.Metadata.GpsPositionOfLocationShown.IsValidCoordinate)
            {
                // Retrieve result from Bing & Cache and choose the most accurate
                GpsPosition bingResult = new GpsPosition();
                int bingAccuracy = 0;
                GpsPosition manualResult = new GpsPosition();
                int manualAccuracy = 0;

                // Use Bing if Configured
                if (this.IsBingMapsResolverConfigured)
                {
                    // Check Bing
                    // Cycle through the address, start as accurate as possible
                    for (int i = photo.Metadata.AddressOfLocationShown.HierarchicalNameLength; i > 0; i--)
                    {
                        // Query Bing
                        bingResult = this.bingMapsResolver.FindGpsPosition(photo.Metadata.AddressOfLocationShown.AddressTruncated(i));

                        if (bingResult != null && bingResult.IsValidCoordinate)
                        {
                            bingAccuracy = i;
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
                    for (int i = photo.Metadata.AddressOfLocationShown.HierarchicalNameLength; i > 0; i--)
                    {
                        // Query Cache
                        manualResult = this.manualCache.FindGpsPosition(photo.Metadata.AddressOfLocationShown.AddressTruncated(i));

                        if (manualResult != null && manualResult.IsValidCoordinate)
                        {
                            manualAccuracy = i;
                            break;
                        }
                        else
                        {
                            manualResult = new GpsPosition();
                        }
                    }
                }

                if (manualAccuracy == 0 && bingAccuracy == 0)
                {
                    photo.Metadata.GpsPositionOfLocationShown = new GpsPosition();
                }
                else if (manualAccuracy > bingAccuracy)
                {
                    photo.Metadata.GpsPositionOfLocationShown = manualResult;
                }
                else
                {
                    photo.Metadata.GpsPositionOfLocationShown = bingResult;
                }
            }
        }

        public void FindAddressCreated(JpgPhoto photo)
        {
            // Look up address for Gps Position recorded by a Tracker
            // Order is: 1) Bing Maps 2) Google Maps
            if (photo.Metadata.GpsPositionOfLocationCreated.IsValidCoordinate)
            {
                // Use Bing if configured
                if (!photo.Metadata.AddressOfLocationCreated.IsValidAddress && this.IsBingMapsResolverConfigured)
                {
                    photo.Metadata.AddressOfLocationCreated = this.bingMapsResolver.FindAddress(photo.Metadata.GpsPositionOfLocationCreated, photo.Metadata.AddressOfLocationShown.Country);
                    photo.Metadata.AddressOfGpsLookupDate = DateTime.Now;
                    photo.Metadata.AddressOfGpsSource = BingMapsResolver.SourceName;
                }

                // Use Google if configured
                if (!photo.Metadata.AddressOfLocationCreated.IsValidAddress && this.IsGoogleMapsResolverConfigured)
                {
                    photo.Metadata.AddressOfLocationCreated = this.googleMapsResolver.FindAddress(photo.Metadata.GpsPositionOfLocationCreated);
                    photo.Metadata.AddressOfGpsLookupDate = DateTime.Now;
                    photo.Metadata.AddressOfGpsSource = GoogleMapsResolver.SourceName;
                }

                if (!photo.Metadata.AddressOfLocationCreated.IsValidAddress)
                {
                    photo.Metadata.AddressOfLocationCreated = new Address();
                    photo.Metadata.AddressOfGpsLookupDate = new DateTime();
                    photo.Metadata.AddressOfGpsSource = null;
                }
            }
        }
    }
}
