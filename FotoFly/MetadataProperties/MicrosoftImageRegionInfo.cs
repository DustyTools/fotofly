// <copyright file="MicrosoftImageRegionInfo.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>MicrosoftImageRegionInfo</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [XmlRootAttribute("GpsCoordinate", Namespace = "http://www.tassography.com/fotofly")]
    public class MicrosoftImageRegionInfo : ICloneable
    {
        private List<MicrosoftImageRegion> regions;

        [XmlArrayItem("Region")]
        public List<MicrosoftImageRegion> Regions
        {
            get
            {
                if (this.regions == null)
                {
                    this.regions = new List<MicrosoftImageRegion>();
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
            if (unknownObject is MicrosoftImageRegionInfo)
            {
                MicrosoftImageRegionInfo compareRegionInfo = unknownObject as MicrosoftImageRegionInfo;

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
            MicrosoftImageRegionInfo cloneRegionInfo = new MicrosoftImageRegionInfo();
            cloneRegionInfo.Regions = new List<MicrosoftImageRegion>();

            foreach (MicrosoftImageRegion region in this.Regions)
            {
                cloneRegionInfo.Regions.Add(region.Clone() as MicrosoftImageRegion);
            }

            return cloneRegionInfo;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
