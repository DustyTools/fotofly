// <copyright file="MetadataDump.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-17</date>
// <summary>MetadataDump</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public class MetadataDump
    {
        private BitmapMetadata bitmapMetadata;
        private List<MetadataProperty> propertyList;
        private List<string> stringList;

        public MetadataDump(BitmapMetadata bitmapMetadata)
        {
            this.bitmapMetadata = bitmapMetadata;
        }

        public List<MetadataProperty> PropertyList
        {
            get
            {
                if (this.propertyList == null)
                {
                    this.GeneratePropertyList();
                }

                return this.propertyList;
            }
        }

        public List<string> StringList
        {
            get
            {
                if (this.stringList == null)
                {
                    this.GenerateStringList();
                }

                return this.stringList;
            }
        }

        public void WriteListToFile(string fileName)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName, false))
            {
                using (TextWriter textWriter = TextWriter.Synchronized(streamWriter))
                {
                    foreach (string property in this.StringList)
                    {
                        textWriter.WriteLine(property);
                    }
                }
            }
        }

        public void GeneratePropertyList()
        {
            this.propertyList = new List<MetadataProperty>();

            this.propertyList.AddRange(this.GeneratePropertyList(this.bitmapMetadata, string.Empty));
        }

        public void GenerateStringList()
        {
            this.stringList = new List<string>();

            this.stringList.AddRange(this.GenerateStringList(this.PropertyList));
        }

        private List<MetadataProperty> GeneratePropertyList(BitmapMetadata metadata, string rootQuery)
        {
            List<MetadataProperty> returnValue = new List<MetadataProperty>();

            if (metadata != null)
            {
                foreach (string query in metadata)
                {
                    if (metadata.ContainsQuery(query))
                    {
                        try
                        {
                            // query string here is relative to the previous metadata reader.
                            object unknownObject = metadata.GetQuery(query);

                            if (unknownObject != null)
                            {
                                MetadataProperty property = new MetadataProperty(rootQuery + query, unknownObject);

                                if (unknownObject is BitmapMetadata)
                                {
                                    // Add all sub values
                                    property.Children.AddRange(this.GeneratePropertyList(unknownObject as BitmapMetadata, rootQuery + query));
                                }

                                returnValue.Add(property);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return returnValue;
        }

        private List<string> GenerateStringList(List<MetadataProperty> properties)
        {
            List<string> returnValue = new List<string>();

            foreach (MetadataProperty property in properties)
            {
                if (property.Children.Count > 0)
                {
                    returnValue.Add(property.Query);
                    returnValue.AddRange(this.GenerateStringList(property.Children));
                }
                else
                {
                    returnValue.Add(property.ToString());
                }
            }

            return returnValue;
        }
    }
}
