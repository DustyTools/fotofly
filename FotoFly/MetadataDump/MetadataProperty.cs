// <copyright file="MetadataProperty.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-17</date>
// <summary>MetadataProperty</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;

    public class MetadataProperty
    {
        public MetadataProperty()
        {
            this.Children = new List<MetadataProperty>();
        }

        public MetadataProperty(string name, object value)
        {
            this.Query = name;
            this.ValueType = value.GetType();
            this.Children = new List<MetadataProperty>();

            if (value is BitmapMetadataBlob)
            {
                this.Value = "[" + (value as BitmapMetadataBlob).GetBlobValue().Length + " bytes]";
            }
            else if (value is UInt64)
            {
                this.Value = new URational((UInt64)value).ToFractionString();
                this.ValueType = typeof(URational);
            }
            else if (value is Int64)
            {
                this.Value = new SRational((Int64)value).ToFractionString();
                this.ValueType = typeof(SRational);
            }
            else if (value is UInt64[])
            {
                this.Value = new URationalTriplet((UInt64[])value).ToFractionString();
                this.ValueType = typeof(URationalTriplet);
            }
            else if (value is string)
            {
                this.Value = "\"" + value.ToString() + "\"";
            }
            else if (value is string[])
            {
                string newValue = string.Empty;

                foreach (string str in value as string[])
                {
                    newValue += "\"" + str + "\", ";
                }

                this.Value = newValue.TrimEnd(' ').TrimEnd(',');
            }
            else
            {
                this.Value = value;
            }
        }

        public string Query
        {
            get; 
            set;
        }

        public Type ValueType
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }

        public List<MetadataProperty> Children
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Query + "\t" + this.Value + " (" + this.ValueType + ")";
        }
    }
}
