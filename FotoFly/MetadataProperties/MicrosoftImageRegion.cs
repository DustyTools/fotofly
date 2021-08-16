// <copyright file="MicrosoftImageRegion.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>MicrosoftImageRegion</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
	using System.Globalization;

    [XmlRootAttribute("MicrosoftImageRegion", Namespace = "http://www.tassography.com/fotofly")]
    public class MicrosoftImageRegion : RectangleCoordinates, ICloneable
    {
        public MicrosoftImageRegion()
        {
        }

        public MicrosoftImageRegion(string personDisplayName)
        {
            this.PersonDisplayName = personDisplayName;
        }

        public MicrosoftImageRegion(string personDisplayName, string rectangleString, string personEmailDigest, string personLiveIdCID)
        {
            this.PersonDisplayName = personDisplayName;
            this.PersonEmailDigest = personEmailDigest;
            this.PersonLiveIdCID = personLiveIdCID;
            this.SetRectangle(rectangleString);
        }

        [XmlAttribute]
        public string PersonDisplayName
        {
            get;
            set;
        }

        [XmlAttribute]
        public string PersonEmailDigest
        {
            get;
            set;
        }

        [XmlAttribute]
        public string PersonLiveIdCID
        {
            get;
            set;
        }

        [XmlIgnore]
        public string RectangleString
        {
            get
            {
				return string.Format("{0}, {1}, {2}, {3}", this.Left.ToString(NumberFormatInfo.InvariantInfo).TrimEnd('0'), this.Top.ToString(NumberFormatInfo.InvariantInfo).TrimEnd('0'), this.Width.ToString(NumberFormatInfo.InvariantInfo).TrimEnd('0'), this.Height.ToString(NumberFormatInfo.InvariantInfo).TrimEnd('0'));
            }

            set
            {
                this.SetRectangle(value);
            }
        }

        [XmlAttribute]
        public bool HasValidDimensions
        {
            get
            {
                if (this.Width == 0 || this.Height == 0 || this.Top == 0 || this.Right == 0)
                {
                    return false;
                }

                return true;
            }

            set
            {
                // For Seralization
            }
        }

        public override bool Equals(object unknownObject)
        {
            if (unknownObject is MicrosoftImageRegion)
            {
                MicrosoftImageRegion compareRegion = unknownObject as MicrosoftImageRegion;

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            MicrosoftImageRegion cloneRegion = new MicrosoftImageRegion();
            cloneRegion.RectangleString = this.RectangleString;
            cloneRegion.PersonLiveIdCID = this.PersonLiveIdCID;
            cloneRegion.PersonEmailDigest = this.PersonEmailDigest;
            cloneRegion.PersonDisplayName = this.PersonDisplayName;

            return cloneRegion;
        }

        private void SetRectangle(string rectangleString)
        {
            if (!string.IsNullOrEmpty(rectangleString))
            {
                // Split the string in four and strip out all white space
                string[] splitString = rectangleString.Replace(" ", string.Empty).Split(',');

                if (splitString.Length == 4)
                {
                    if (!string.IsNullOrEmpty(splitString[0]))
                    {
                        this.Left = Convert.ToDouble(splitString[0], NumberFormatInfo.InvariantInfo);
                    }

                    if (!string.IsNullOrEmpty(splitString[1]))
                    {
						this.Top = Convert.ToDouble(splitString[1], NumberFormatInfo.InvariantInfo);
                    }

                    if (!string.IsNullOrEmpty(splitString[2]))
                    {
						this.Width = Convert.ToDouble(splitString[2], NumberFormatInfo.InvariantInfo);
                    }

                    if (!string.IsNullOrEmpty(splitString[3]))
                    {
						this.Height = Convert.ToDouble(splitString[3], NumberFormatInfo.InvariantInfo);
                    }
                }
            }
        }
    }
}
