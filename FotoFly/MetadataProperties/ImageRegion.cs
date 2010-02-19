// <copyright file="ImageRegion.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>ImageRegion</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class ImageRegion : ICloneable
    {
        public ImageRegion()
        {
        }

        public ImageRegion(string personDisplayName)
        {
            this.PersonDisplayName = personDisplayName;
        }

        public ImageRegion(string personDisplayName, string rectangleString, string personEmailDigest, string personLiveIdCID)
        {
            this.RectangleString = rectangleString;
            this.PersonDisplayName = personDisplayName;
            this.PersonEmailDigest = personEmailDigest;
            this.PersonLiveIdCID = personLiveIdCID;
        }

        [XmlAttribute]
        public string RectangleString
        {
            get;
            set;
        }

        [XmlAttribute]
        public string PersonDisplayName
        {
            get;
            set;
        }

        [XmlIgnore]
        public string PersonEmailDigest
        {
            get;
            set;
        }

        [XmlIgnore]
        public string PersonLiveIdCID
        {
            get;
            set;
        }

        [XmlElement]
        public RectangleCoordinates RegionRectangle
        {
            get
            {
                RectangleCoordinates regionRectangle = new RectangleCoordinates();

                if (string.IsNullOrEmpty(this.RectangleString))
                {
                    regionRectangle.Left = 0;
                    regionRectangle.Top = 0;
                    regionRectangle.Width = 0;
                    regionRectangle.Height = 0;
                }
                else
                {
                    string[] splitString = this.RectangleString.Split(',');

                    if (splitString.Length == 4)
                    {
                        regionRectangle.Left = Convert.ToDouble(splitString[0]);
                        regionRectangle.Top = Convert.ToDouble(splitString[1]);
                        regionRectangle.Width = Convert.ToDouble(splitString[2]);
                        regionRectangle.Height = Convert.ToDouble(splitString[3]);
                    }
                    else
                    {
                        throw new Exception("Invalid Rectangle Value: " + this.RectangleString);
                    }
                }

                return regionRectangle;
            }

            set
            {
                // For Serialisation
            }
        }

        public override bool Equals(object unknownObject)
        {
            if (unknownObject is ImageRegion)
            {
                ImageRegion compareRegion = unknownObject as ImageRegion;

                if (compareRegion.PersonDisplayName == this.PersonDisplayName
                    && compareRegion.PersonEmailDigest == this.PersonEmailDigest
                    && compareRegion.PersonLiveIdCID == this.PersonLiveIdCID
                    && compareRegion.RectangleString == this.RectangleString)
                {
                    return true;
                }
            }

            return false;
        }

        public object Clone()
        {
            ImageRegion cloneRegion = new ImageRegion();
            cloneRegion.RectangleString = this.RectangleString;
            cloneRegion.PersonLiveIdCID = this.PersonLiveIdCID;
            cloneRegion.PersonEmailDigest = this.PersonEmailDigest;
            cloneRegion.PersonDisplayName = this.PersonDisplayName;

            return cloneRegion;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
