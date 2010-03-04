// <copyright file="ExposureBias.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-25</date>
// <summary>ExposureBias</summary>
namespace Fotofly
{
    using System;
    using System.Text;
    using System.Xml.Serialization;

    [XmlRootAttribute("ExposureBias", Namespace = "http://www.tassography.com/fotofly")]
    public class ExposureBias
    {
        SRational srational;

        public ExposureBias()
        {
            this.srational = new SRational();
        }

        public ExposureBias(SRational srational)
        {
            if (srational != null && srational.ToInt() != 0)
            {
                this.srational = srational;
            }
            else
            {
                this.srational = new SRational();
            }
        }

        public ExposureBias(string exposureBias)
        {
            if (string.IsNullOrEmpty(exposureBias))
            {
                this.srational = new SRational();
            }
            else
            {
                // Expected format is {numerator}/{denominator}
                string[] splitString = exposureBias.Split('/');

                if (splitString.Length == 2)
                {
                    int numerator = Convert.ToInt32(splitString[0]);
                    int denominator = Convert.ToInt32(splitString[1]);

                    this.srational = new SRational(numerator, denominator);
                }
                else
                {
                    throw new ArgumentException("Aperture was not of expected format:" + exposureBias);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ExposureBias)
            {
                if ((obj as ExposureBias).ToString() == this.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            if (this.srational.ToInt() > 0)
            {
                return "+" + Math.Round(this.srational.ToDouble(), 1) + " step";
            }
            else
            {
                return Math.Round(this.srational.ToDouble(), 1) + " step";
            }

            return "0 step";
        }
    }
}