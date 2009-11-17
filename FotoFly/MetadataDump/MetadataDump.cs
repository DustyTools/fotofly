namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
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

        private void GeneratePropertyList()
        {
            this.propertyList = new List<MetadataProperty>();

            this.propertyList.AddRange(this.GeneratePropertyList(bitmapMetadata, string.Empty));
        }

        private List<MetadataProperty> GeneratePropertyList(BitmapMetadata metadata, string fullQuery)
        {
            List<MetadataProperty> returnValue = new List<MetadataProperty>();

            if (metadata != null)
            {
                foreach (string query in metadata)
                {
                    string tempQuery = fullQuery + query;

                    try
                    {
                        // query string here is relative to the previous metadata reader.
                        object unknownObject = metadata.GetQuery(query);

                        MetadataProperty property = new MetadataProperty(tempQuery, unknownObject);

                        if (unknownObject is BitmapMetadata)
                        {
                            // Add all sub values
                            property.Children.AddRange(this.GeneratePropertyList(unknownObject as BitmapMetadata, tempQuery));
                        }

                        returnValue.Add(property);
                    }
                    catch
                    {
                        // Swallow it
                    }
                }
            }

            return returnValue;
        }

        private void GenerateStringList()
        {
            this.stringList = new List<string>();
            this.stringList.AddRange(this.GenerateStringList(this.PropertyList));
        }

        private List<string> GenerateStringList(List<MetadataProperty> properties)
        {
            List<string> returnValue = new List<string>();

            foreach (MetadataProperty property in properties)
            {
                if (property.Children.Count > 0)
                {
                    returnValue.Add(property.Name);
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
