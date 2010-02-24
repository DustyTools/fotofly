// <copyright file="TagList.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>TagList</summary>
namespace Fotofly
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class TagList : IEnumerable<Tag>, ICloneable
    {
        private List<Tag> tags;

        public TagList()
        {
            this.tags = new List<Tag>();
        }

        public TagList(TagList tagList)
        {
            this.tags = new List<Tag>();

            if (tagList != null)
            {
                this.AddRange(tagList);
            }
        }

        public TagList(string[] stringArray)
        {
            this.tags = new List<Tag>();

            if (stringArray != null)
            {
                foreach (string text in stringArray)
                {
                    this.Add(Regex.Replace(text.TrimEnd('/'), "//", "/"));
                }
            }
        }

        public TagList(string tagString)
        {
            this.tags = new List<Tag>();

            if (!string.IsNullOrEmpty(tagString))
            {
                this.Add(Regex.Replace(tagString.TrimEnd('/'), "//", "/"));
            }
        }

        public TagList(ReadOnlyCollection<string> readOnlyCollection)
        {
            this.tags = new List<Tag>();

            if (readOnlyCollection != null)
            {
                foreach (string text in readOnlyCollection)
                {
                    this.Add(Regex.Replace(text.TrimEnd('/'), "//", "/"));
                }
            }
        }

        public int Count
        {
            get
            {
                return this.tags.Count;
            }
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return this.tags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Clear()
        {
            this.tags = new List<Tag>();
        }

        public void Add(object tagList)
        {
            if (tagList is TagList)
            {
                this.Add(tagList as TagList);
            }
            else
            {
                throw new Exception("object is not of type TagList");
            }
        }

        public void Add(string rootTag, string tag)
        {
            this.tags.Add(new Tag(rootTag.TrimEnd('/') + "/" + tag.TrimStart('/')));
        }

        public void Add(string tag)
        {
            this.tags.Add(new Tag(tag));
        }

        public void Add(Tag tag)
        {
            this.tags.Add(tag);
        }

        public void AddRange(List<string> tagList)
        {
            foreach (string tagName in tagList)
            {
                this.Add(tagName);
            }
        }

        public void AddRange(TagList tagList)
        {
            foreach (Tag tag in tagList)
            {
                this.Add(tag);
            }
        }

        public void Remove(Tag tag)
        {
            this.Remove(tag.FullName);
        }

        public void Remove(string tag)
        {
            this.tags.Remove(new Tag(tag));
        }

        public void RemoveAllByStartName(string tagName)
        {
            // Loop deleting all matching Tags
            while (true)
            {
                Tag tag = this.FindByStartName(tagName);

                if (tag == null)
                {
                    break;
                }
                else
                {
                    this.Remove(tag);
                }
            }
        }

        public Tag Find(Tag tag)
        {
            return this.Find(tag.FullName);
        }

        public Tag Find(string tag)
        {
            var query = from x in this.tags
                        where x.ToString().ToLower() == tag.ToLower()
                        select x;

            return query.FirstOrDefault();
        }

        public bool Contains(string tag)
        {
            return this.Find(tag) != null;
        }

        public void ReplaceByStartName(string currentTag, string newTag)
        {
            this.RemoveAllByStartName(currentTag);

            this.Add(newTag);
        }

        public Tag FindByStartName(string tag)
        {
            var query = from x in this.tags
                        where x.ToString().ToLower().StartsWith(tag.ToLower())
                        select x;

            return query.FirstOrDefault();
        }

        public List<Tag> FindAllByStartName(string tag)
        {
            var query = from x in this.tags
                        where x.ToString().ToLower().StartsWith(tag.ToLower())
                        select x;

            return query.ToList<Tag>();
        }

        public override bool Equals(object unknownObject)
        {
            if (unknownObject is TagList)
            {
                TagList compareTagList = unknownObject as TagList;

                // Strip off all duplicates
                this.RemoveDuplicates();
                compareTagList.RemoveDuplicates();

                // Do the comparison
                if (this.tags.Count != compareTagList.Count)
                {
                    return false;
                }
                else
                {
                    foreach (Tag tag in compareTagList)
                    {
                        if (this.Find(tag) == null)
                        {
                            return false;
                        }
                    }

                    // Count is the same and didn't not find any Tags, must be equal
                    return true;
                }
            }

            return false;
        }

        public ReadOnlyCollection<string> ToReadOnlyCollection()
        {
            List<string> tagsAsStrings = new List<string>();

            foreach (Tag tag in this.tags)
            {
                tagsAsStrings.Add(tag.FullName);
            }

            return new ReadOnlyCollection<string>(tagsAsStrings);
        }

        public string[] ToStringArray()
        {
            string[] stringArray = new string[this.tags.Count];

            for(int i = 0; i < this.tags.Count ; i ++)
            {
                stringArray[i] = this.tags[i].FullName;
            }

            return stringArray;
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();

            foreach (Tag tag in this)
            {
                toString.Append(tag.FullName);
                toString.Append(", ");
            }

            return toString.ToString().Trim().TrimEnd(',');
        }

        public object Clone()
        {
            // Remove duplicate Tags
            this.RemoveDuplicates();

            TagList cloneTagList = new TagList();

            foreach (Tag tag in this.tags)
            {
                cloneTagList.Add((Tag)tag.Clone());
            }

            return cloneTagList;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private void RemoveDuplicates()
        {
            if (this.tags.Count > 0)
            {
                List<string> oldTags = new List<string>();

                // Grab copy of tags
                foreach (Tag tag in this.tags)
                {
                    oldTags.Add(tag.FullName);
                }

                // Clear Existing data
                this.Clear();

                // Add all distinct tags
                foreach (string tag in oldTags.Distinct().ToList())
                {
                    this.tags.Add(new Tag(tag));
                }
            }
        }
    }
}
