// <copyright file="SRational.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>SRational</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SRational : AbstractRational
    {
        public SRational(double numerator, int accuracy)
            : base(numerator, accuracy)
        {
        }

        /// <summary>
        /// Creates an Exif Rational
        /// </summary>
        /// <param name="data">A ulong typically read from exif metadata</param>
        public SRational(Int64 data)
        {
            this.Numerator = (int)(data & 0xFFFFFFFFL);
            this.Denominator = (int)(((ulong)data & 0xFFFFFFFF00000000L) >> 32);
        }
    }
}
