// <copyright file="RectangleCoordinates.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>RectangleCoordinates</summary>
namespace Fotofly
{
    using System;
    using System.Xml.Serialization;

    public class RectangleCoordinates
    {
        [XmlAttribute]
        public double Left
        {
            get;
            set;
        }

        [XmlAttribute]
        public double Top
        {
            get;
            set;
        }

        [XmlAttribute]
        public double Width
        {
            get;
            set;
        }

        [XmlAttribute]
        public double Height
        {
            get;
            set;
        }

        [XmlAttribute]
        public double Right
        {
            get { return this.Left + this.Width; }
        }

        [XmlAttribute]
        public double Bottom
        {
            get { return this.Top + this.Height; }
        }
    }
}
