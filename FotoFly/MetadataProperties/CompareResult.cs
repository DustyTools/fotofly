// <copyright file="CompareResult.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-03-02</date>
// <summary>CompareResult Class</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    public class CompareResult : IComparable
    {
        public CompareResult()
        {
        }

        public string PropertyName { get; set; }

        public object Source
        {
            get
            {
                return this.SourceObject == null ? "{null}" : (this.SourceObject.ToString() == string.Empty ? "{empty}" : this.SourceObject);
            }
        }

        public object Destination
        {
            get
            {
                return this.DestinationObject == null ? "{null}" : (this.DestinationObject.ToString() == string.Empty ? "{empty}" : this.DestinationObject);
            }
        }

        public object DestinationObject { get; set; }

        public object SourceObject { get; set; }

        public override string ToString()
        {
            StringBuilder change = new StringBuilder();
            change.Append(this.PropertyName);
            change.Append(" ('");
            change.Append(this.Source);
            change.Append("' vs '");
            change.Append(this.Destination);
            change.Append("')");

            return change.ToString();
        }

        public int CompareTo(object obj)
        {
            return this.PropertyName.CompareTo(obj.ToString());
        }
    }
}