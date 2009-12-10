// <copyright file="GeotaggingManager.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>Class for Geotagging Photos</summary>
namespace FotoFly.Geotagging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using FotoFly.Geotagging.Resolvers;
    using FotoFly.GpseXchangeFormat;

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
            this.gpsTrackResolver = new GpsTrackResolver();
            this.gpsTrackResolver.ToleranceInMeters = toleranceInMeters;
            this.gpsTrackResolver.ToleranceInMinutes = toleranceInMinutes;
            this.gpsTrackResolver.GpsTrackSource = trackSource;

            foreach (string gpxTrackFile in gpxTrackFiles)
            {
                this.gpsTrackResolver.Add(GpxFileManager.Read(gpxTrackFile));
            }
        }

        public void FindGpsPosition(JpgPhoto photo)
        {
            this.FindGpsPosition(photo, 0);
        }

        public void FindGpsPosition(JpgPhoto photo, int utcOffset)
        {
            // Order is 1) Gps Track 2) Bing Maps 4) Manual Cache

            // Use Gps Tracks if Configured
            if (!photo.Metadata.GpsPosition.IsValidCoordinate && this.gpsTrackResolver != null)
            {
                GpsPosition gpsPosition = this.gpsTrackResolver.FindGpsPosition(photo.Metadata.DateTaken.AddHours(utcOffset));

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
                if (this.bingMapsResolver != null)
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
                if (this.manualCache != null)
                {
                    // Check Cache
                    // Cycle through the address, start as accurate as possible
                    for (int i = photo.Metadata.IptcAddress.HierarchicalNameLength; i > 0; i--)
                    {
                        // Query Cache
                        manualResult = this.manualCache.FindGpsPosition(photo.Metadata.IptcAddress.AddressTruncated(i));

                        if (manualResult.IsValidCoordinate)
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
            if (photo.Metadata.GpsPosition.Dimension == GpsPosition.Dimensions.ThreeDimensional)
            {
                // Try both Bing & Google and see which is the most accurate
                Address address = new Address();

                // Use Bing if configured
                if (!address.IsValidAddress && this.bingMapsResolver != null)
                {
                    address = this.bingMapsResolver.FindAddress(photo.Metadata.GpsPosition, photo.Metadata.IptcAddress.Country);
                }

                // Use Google if configured
                if (!address.IsValidAddress && this.googleMapsResolver != null)
                {
                    address = this.googleMapsResolver.FindAddress(photo.Metadata.GpsPosition);
                }

                if (address.IsValidAddress)
                {
                }
            }
        }
    }
}
