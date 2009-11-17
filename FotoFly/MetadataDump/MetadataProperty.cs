namespace FotoFly
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
            this.Name = name;
            this.ValueType = value.GetType();
            this.Children = new List<MetadataProperty>();

            if (value is BitmapMetadataBlob)
            {
                this.Value = "[" + (value as BitmapMetadataBlob).GetBlobValue().Length + " bytes]";
            }
            else if (value is UInt64)
            {
                this.Value = new URational((UInt64)value).ToUnformattedString();
                this.ValueType = typeof(URational);
            }
            else if (value is Int64)
            {
                this.Value = new Rational((Int64)value).ToUnformattedString();
                this.ValueType = typeof(Rational);
            }
            else if (value is UInt64[])
            {
                this.Value = new GpsRational((UInt64[])value).ToUnformattedString();
                this.ValueType = typeof(GpsRational);
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

        public string Name
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
            return this.Name + "\t" + this.Value + " (" + this.ValueType + ")";
        }
    }
}
