// <copyright file="GpxFile.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-05</date>
// <summary>Class that represents a Gpx File</summary>
namespace Fotofly.GpseXchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class GpxFile
    {
        public string Filename
        {
            get;
            set;
        }

        public GpxRootNode RootNode
        {
            get;
            set;
        }
    }
}
