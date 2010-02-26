// <copyright file="XmpMicrosoftProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>XmpMicrosoftProvider Class</summary>
namespace Fotofly.MetadataProviders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Text;
    using System.Windows.Media.Imaging;

    using Fotofly.BitmapMetadataTools;
    using Fotofly.MetadataQueries;

    public class XmpMicrosoftProvider : BaseProvider
    {
        public XmpMicrosoftProvider(BitmapMetadata bitmapMetadata)
            : base(bitmapMetadata)
        {
        }

        /// <summary>
        /// DateAquired, Microsoft Windows 7 Property
        /// </summary>
        public DateTime DateAquired
        {
            get
            {
                DateTime dateAquired = this.BitmapMetadata.GetQuery<DateTime>(XmpMicrosoftQueries.DateAcquired.Query);

                if (dateAquired == null)
                {
                    return new DateTime();
                }
                else
                {
                    return dateAquired;
                }
            }

            set
            {
                if (this.ValueHasChanged(value, this.DateAquired))
                {
                    string dateAquired = value.ToString("yyyy-MM-ddTHH:mm:ss");

                    this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.DateAcquired.Query, dateAquired);
                }
            }
        }

        /// <summary>
        /// Microsoft Region Info extension which provides data on regions in the photo
        /// </summary>
        public ImageRegionInfo RegionInfo
        {
            get
            {
                ImageRegionInfo regionInfo = new ImageRegionInfo();

                // Read Each Region
                BitmapMetadata regionsMetadata = this.BitmapMetadata.GetQuery<BitmapMetadata>(XmpMicrosoftQueries.Regions.Query);

                if (regionsMetadata != null)
                {
                    foreach (string regionQuery in regionsMetadata)
                    {
                        string regionFullQuery = XmpMicrosoftQueries.Regions.Query + regionQuery;

                        BitmapMetadata regionMetadata = this.BitmapMetadata.GetQuery<BitmapMetadata>(regionFullQuery);

                        if (regionMetadata != null)
                        {
                            ImageRegion imageRegion = new ImageRegion();

                            if (regionMetadata.ContainsQuery(XmpMicrosoftQueries.RegionRectangle.Query))
                            {
                                imageRegion.RectangleString = regionMetadata.GetQuery(XmpMicrosoftQueries.RegionRectangle.Query).ToString();
                            }

                            if (regionMetadata.ContainsQuery(XmpMicrosoftQueries.RegionPersonDisplayName.Query))
                            {
                                imageRegion.PersonDisplayName = regionMetadata.GetQuery(XmpMicrosoftQueries.RegionPersonDisplayName.Query).ToString();
                            }

                            if (regionMetadata.ContainsQuery(XmpMicrosoftQueries.RegionPersonEmailDigest.Query))
                            {
                                imageRegion.PersonEmailDigest = regionMetadata.GetQuery(XmpMicrosoftQueries.RegionPersonEmailDigest.Query).ToString();
                            }

                            if (regionMetadata.ContainsQuery(XmpMicrosoftQueries.RegionPersonLiveIdCID.Query))
                            {
                                imageRegion.PersonLiveIdCID = regionMetadata.GetQuery(XmpMicrosoftQueries.RegionPersonLiveIdCID.Query).ToString();
                            }

                            regionInfo.Regions.Add(imageRegion);
                        }
                    }
                }

                return regionInfo;
            }

            set
            {
                if (this.ValueHasChanged(value, this.RegionInfo))
                {
                    // This method is distructive, it deletes all existing regions
                    // In place editing would be complicated because each region doesn't have a unique IDs
                    // In theory the new data is based on the existing data so nothing should be lost
                    // Except where new properties exist that aren't in the XMPRegion class

                    // Delete RegionInfo if there's no Regions
                    if (value != null && value.Regions.Count > 0)
                    {
                        // Check for RegionInfo Struct, if none exists, create it
                        if (!this.BitmapMetadata.ContainsQuery(XmpMicrosoftQueries.RegionInfo.Query))
                        {
                            this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.RegionInfo.Query, new BitmapMetadata(XmpCoreQueries.StructBlock));
                        }

                        // Check for Regions Bag, if none exists, create it
                        if (!this.BitmapMetadata.ContainsQuery(XmpMicrosoftQueries.Regions.Query))
                        {
                            this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.Regions.Query, new BitmapMetadata(XmpCoreQueries.BagBlock));
                        }

                        // If Region count has changed, clear our existing regions and create empty regions
                        if (value.Regions.Count != this.RegionInfo.Regions.Count)
                        {
                            // Delete any 'extra' regions
                            for (int i = value.Regions.Count; i < this.RegionInfo.Regions.Count; i++)
                            {
                                // Build query for current region
                                string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, XmpMicrosoftQueries.Region.Query, i.ToString());

                                this.BitmapMetadata.RemoveQuery(currentRegionQuery);
                            }

                            // Add any additional regions
                            for (int i = this.RegionInfo.Regions.Count; i < value.Regions.Count; i++)
                            {
                                // Build query for current region
                                string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, XmpMicrosoftQueries.Region.Query, i.ToString());

                                this.BitmapMetadata.SetQuery(currentRegionQuery, new BitmapMetadata(XmpCoreQueries.StructBlock));
                            }
                        }

                        int currentRegion = 0;

                        // Loop through each Region
                        foreach (ImageRegion region in value.Regions)
                        {
                            // Build query for current region
                            string currentRegionQuery = String.Format(CultureInfo.InvariantCulture, XmpMicrosoftQueries.Region.Query, currentRegion);

                            // Update or clear PersonDisplayName
                            if (string.IsNullOrEmpty(region.PersonDisplayName))
                            {
                                this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonDisplayName.Query);
                            }
                            else
                            {
                                this.BitmapMetadata.SetQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonDisplayName.Query, region.PersonDisplayName);
                            }

                            // Update or clear PersonEmailDigest
                            if (string.IsNullOrEmpty(region.PersonEmailDigest))
                            {
                                this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonEmailDigest.Query);
                            }
                            else
                            {
                                this.BitmapMetadata.SetQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonEmailDigest.Query, region.PersonEmailDigest);
                            }

                            // Update or clear PersonLiveIdCID
                            if (string.IsNullOrEmpty(region.PersonLiveIdCID))
                            {
                                this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonLiveIdCID.Query);
                            }
                            else
                            {
                                this.BitmapMetadata.SetQuery(currentRegionQuery + XmpMicrosoftQueries.RegionPersonLiveIdCID.Query, region.PersonLiveIdCID);
                            }

                            // Update or clear RectangleString
                            if (string.IsNullOrEmpty(region.RectangleString))
                            {
                                this.BitmapMetadata.RemoveQuery(currentRegionQuery + XmpMicrosoftQueries.RegionRectangle.Query);
                            }
                            else
                            {
                                this.BitmapMetadata.SetQuery(currentRegionQuery + XmpMicrosoftQueries.RegionRectangle.Query, region.RectangleString);
                            }

                            currentRegion++;
                        }
                    }
                    else
                    {
                        // Delete existing RegionInfos, deletes all child data
                        if (this.BitmapMetadata.ContainsQuery(XmpMicrosoftQueries.RegionInfo.Query))
                        {
                            this.BitmapMetadata.RemoveQuery(XmpMicrosoftQueries.RegionInfo.Query);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Rating (Range -1.0 to 5.0)
        /// </summary>
        public Rating Rating
        {
            // 1 Star - 1
            // 2 Star - 25
            // 3 Star - 50
            // 4 Star - 75
            // 5 Star - 99
            get
            {
                string rating = this.BitmapMetadata.GetQuery<string>(XmpMicrosoftQueries.Rating.Query);

                if (String.IsNullOrEmpty(rating))
                {
                    return new Rating();
                }
                else if (rating == "1")
                {
                    return new Rating(1);
                }
                else if (rating == "25")
                {
                    return new Rating(2);
                }
                else if (rating == "50")
                {
                    return new Rating(3);
                }
                else if (rating == "75")
                {
                    return new Rating(4);
                }
                else if (rating == "99")
                {
                    return new Rating(5);
                }

                return new Rating();
            }

            set
            {
                if (this.ValueHasChanged(value, this.Rating))
                {
                    switch (value.AsEnum)
                    {
                        case Rating.Ratings.OneStar:
                            this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.Rating.Query, "1");
                            break;

                        case Rating.Ratings.TwoStar:
                            this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.Rating.Query, "25");
                            break;

                        case Rating.Ratings.ThreeStar:
                            this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.Rating.Query, "50");
                            break;

                        case Rating.Ratings.FourStar:
                            this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.Rating.Query, "75");
                            break;

                        case Rating.Ratings.FiveStar:
                            this.BitmapMetadata.SetQuery(XmpMicrosoftQueries.Rating.Query, "99");
                            break;

                        case Rating.Ratings.Rejected:
                        case Rating.Ratings.NoRating:
                        default:
                            this.BitmapMetadata.RemoveQuery(XmpMicrosoftQueries.Rating.Query);
                            break;
                    }
                }
            }
        }
    }
}
