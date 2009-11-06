namespace FotoFly
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
