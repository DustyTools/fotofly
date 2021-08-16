// <copyright file="Aperture.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-25</date>
// <summary>Aperture</summary>
namespace Fotofly
{
    using System;
    using System.Text;
    using System.Xml.Serialization;
	using System.Globalization;

    [XmlRootAttribute("Aperture", Namespace = "http://www.tassography.com/fotofly")]
    public class Aperture
    {
        URational uRational;

        public Aperture()
        {
            this.uRational = new URational(0, 0);
        }

        public Aperture(URational uRational)
        {
            this.uRational = new URational(uRational);

            ////URational urational = this.BitmapMetadata.GetQuery<URational>(ExifQueries.Aperture.Query);
            ////return "f/" + urational.ToDouble().ToString();
        }

        public Aperture(double numerator)
        {
            this.uRational = new URational(numerator, 1);
        }

        public Aperture(int numerator, int denominator)
        {
            this.uRational = new URational(numerator, denominator);
        }

        public Aperture(string aperture)
        {
            // Expected format is {numerator}/{denominator}
            string[] splitString = aperture.Split('/');

            if (splitString.Length == 2)
            {
                int numerator = Convert.ToInt32(splitString[0]);
                int denominator = Convert.ToInt32(splitString[1]);

                this.uRational = new URational(numerator, denominator);
            }
            else
            {
                throw new ArgumentException("Aperture was not of expected format:" + aperture);
            }
        }

        [XmlAttribute]
        public double Numeric
        {
            get
            {
                if (this.uRational != null)
                {
                    return this.uRational.Numerator;
                }

                return 0;
            }

            set
            {
                this.uRational = new URational(value, 1);
            }
        }

        public bool IsValid
        {
            get
            {
                return this.uRational.Numerator != 0;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Aperture)
            {
                if ((obj as Aperture).ToString() == this.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            if (this.uRational.ToDouble() == 0)
            {
                return string.Empty;
            }
            else
            {
				return "f/" + this.uRational.ToDouble().ToString(NumberFormatInfo.InvariantInfo);
            }
        }
    }
}
