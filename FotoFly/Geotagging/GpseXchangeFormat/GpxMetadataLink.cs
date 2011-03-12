// <copyright file="GpxMetadataLink.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-08-24</date>
// <summary>Class that represents a Gpx Metadata Link</summary>
namespace Fotofly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
   
    public class GpxMetadataLink
    {
        [XmlAttribute("href")]
        public string Href
        {
            get;
            set;
        }

        [XmlElement("text")]
        public string Text
        {
            get;
            set;
        }
    }
}
