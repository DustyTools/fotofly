// <copyright file="ExifDateTime.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-17</date>
// <summary>ExifDateTime</summary>
namespace FotoFly
{
    using System;
    using System.Text;

    public class ExifDateTime
    {
        private DateTime dateTime;

        public ExifDateTime(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        public ExifDateTime(string exifString)
        {
            // Default to new Datetime
            this.dateTime = new DateTime();

            // Check the string is valid in the format "YYYY:MM:DD HH:mm:ss"
            if (!string.IsNullOrEmpty(exifString) && exifString.Length == 19 && exifString != "0000:00:00 00:00:00")
            {
                StringBuilder formattedDate = new StringBuilder();
                formattedDate.Append(exifString.Substring(0, 4)); // year
                formattedDate.Append(@"/");
                formattedDate.Append(exifString.Substring(5, 2)); // month
                formattedDate.Append(@"/");
                formattedDate.Append(exifString.Substring(8, 2)); // day
                formattedDate.Append("T");
                formattedDate.Append(exifString.Substring(11, 2)); // hour
                formattedDate.Append(":");
                formattedDate.Append(exifString.Substring(14, 2)); // minute
                formattedDate.Append(":");
                formattedDate.Append(exifString.Substring(17, 2)); // second

                DateTime newDateTime;

                // Try parsing as DateTime
                if (DateTime.TryParse(formattedDate.ToString(), out newDateTime))
                {
                    this.dateTime = new DateTime(newDateTime.Ticks, DateTimeKind.Local);
                }
            }
        }

        public DateTime ToDateTime()
        {
            return this.dateTime;
        }

        public string ToExifString()
        {
            // Convert to sortable date and convert to EXIF format
            // 2008:01:01 10:00:00
            string exifDateTime = this.dateTime.ToString("s");

            exifDateTime = exifDateTime.Replace("-", ":");
            exifDateTime = exifDateTime.Replace("T", " ");

            return exifDateTime;
        }

        public new string ToString()
        {
            return this.dateTime.ToString();
        }
    }
}
