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
