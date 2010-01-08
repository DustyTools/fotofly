// <copyright file="ImageRegionInfo.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>ImageRegionInfo</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class ImageRegionInfo : ICloneable
    {
        private List<ImageRegion> regions;

        [XmlArrayItem("Region")]
        public List<ImageRegion> Regions
        {
            get
            {
                if (this.regions == null)
                {
                    this.regions = new List<ImageRegion>();
                }

                return this.regions;
            }
            
            set
            {
                this.regions = value;
            }
        }

        public override bool Equals(object unknownObject)
        {
            if (unknownObject is ImageRegionInfo)
            {
                ImageRegionInfo compareRegionInfo = unknownObject as ImageRegionInfo;

                if (compareRegionInfo.Regions.Count == this.Regions.Count)
                {
                    for (int i = 0; i < this.Regions.Count; i++)
                    {
                        // If region is not the same, return false
                        if (!this.Regions[i].Equals(compareRegionInfo.Regions[i]))
                        {
                            return false;
                        }
                    }

                    // All regions must be the same
                    return true;
                }
            }

            return false;
        }

        public object Clone()
        {
            ImageRegionInfo cloneRegionInfo = new ImageRegionInfo();
            cloneRegionInfo.Regions = new List<ImageRegion>();

            foreach (ImageRegion region in this.Regions)
            {
                cloneRegionInfo.Regions.Add(region.Clone() as ImageRegion);
            }

            return cloneRegionInfo;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
