// <copyright file="Tag.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>Tag</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    [XmlRootAttribute("Tag", Namespace = "http://www.tassography.com/fotofly")]
    public class Tag : ICloneable
    {
        // ToDo This is crap
        // Consider deriving from string
        // The main problem is the fact it's difficult to use for comparisons etc
        // Would be nice if it could flag a change itself
        public Tag()
        {
        }

        public Tag(string fullname)
        {
            this.FullName = Regex.Replace(fullname.TrimEnd('/'), "//", "/");
        }

        [XmlAttribute]
        public string FullName
        {
            get;
            set;
        }

        [XmlIgnore]
        public string Name
        {
            get
            {
                if (this.Tree.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return this.FullName.Split('/').Last();
                }
            }
        }

        [XmlIgnore]
        public int Length
        {
            get
            {
                return this.FullName.TrimEnd('/').TrimStart('/').Split('/').Length;
            }
        }

        [XmlIgnore]
        public string RootTag
        {
            get
            {
                if (this.Tree.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return this.Tree[0];
                }
            }
        }

        public Tag SubTag(int startTag)
        {
            return this.SubTag(startTag, this.Tree.Count);
        }

        public Tag SubTag(int startTag, int length)
        {
            // Build the Tree
            string parts = string.Empty;

            string[] splitString = this.FullName.TrimEnd('/').Split('/');

            for (int i = startTag; i < length; i++ )
            {
                parts = parts + "/" + splitString[i];
            }

            return new Tag(parts.TrimStart('/'));
        }

        [XmlIgnore]
        public List<string> Tree
        {
            get
            {
                // Build the Tree
                List<string> returnValue = new List<string>();

                string previousTag = string.Empty;

                string[] splitString = this.FullName.TrimEnd('/').Split('/');

                foreach (string tagPart in splitString)
                {
                    previousTag = previousTag + tagPart;

                    returnValue.Add(previousTag.TrimEnd('/'));

                    previousTag = previousTag + "/";
                }

                return returnValue;
            }
        }

        public bool StartsWith(string compareString)
        {
            return this.FullName.StartsWith(compareString);
        }

        public override bool Equals(object unknownObject)
        {
            if (unknownObject is Tag)
            {
                Tag compareTag = unknownObject as Tag;

                if (compareTag.FullName == this.FullName)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return this.FullName;
        }

        public object Clone()
        {
            return new Tag(this.FullName);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
