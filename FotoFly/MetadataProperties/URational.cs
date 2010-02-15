// <copyright file="URational.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>URational</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class URational : AbstractRational
    {
        public URational(double numerator, int accuracy)
            : base(numerator, accuracy)
        {
        }

        public URational(UInt64 data)
        {
            this.Numerator = (int)(data & 0xFFFFFFFFL);
            this.Denominator = (int)((data & 0xFFFFFFFF00000000L) >> 32);
        }
    }
}
