// <copyright file="GenericPhotoEnums.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>GenericPhotoEnums Class</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GenericPhotoEnums
    {
        public enum ImageTypes
        {
            Unknown,
            Jpeg,
            Tiff
        }

        public enum FilenameFormats
        {
            yyyymmddSequence,
            yyyymmddSecondsSinceMidnight,
            yyyymmddHoursMinutesSeconds
        }
    }
}
