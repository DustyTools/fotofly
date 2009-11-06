namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class XmpRegionInfo : ICloneable
    {
        private List<XmpRegion> regions;

        [XmlArrayItem("Region")]
        public List<XmpRegion> Regions
        {
            get
            {
                if (this.regions == null)
                {
                    this.regions = new List<XmpRegion>();
                }

                return this.regions;
            }
            
            set
            {
                this.regions = value;
            }
        }

        [XmlAttribute]
        public DateTime LastUpdate
        {
            get;
            set;
        }

        public override bool Equals(object unknownObject)
        {
            if (unknownObject is XmpRegionInfo)
            {
                XmpRegionInfo compareRegionInfo = unknownObject as XmpRegionInfo;

                if (compareRegionInfo.LastUpdate == this.LastUpdate && compareRegionInfo.Regions.Count == this.Regions.Count)
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
            XmpRegionInfo cloneRegionInfo = new XmpRegionInfo();
            cloneRegionInfo.Regions = new List<XmpRegion>();
            cloneRegionInfo.LastUpdate = this.LastUpdate;

            foreach (XmpRegion region in this.Regions)
            {
                cloneRegionInfo.Regions.Add(region.Clone() as XmpRegion);
            }

            return cloneRegionInfo;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
