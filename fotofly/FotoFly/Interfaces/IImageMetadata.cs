namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IImageMetadata
    {
        string Aperture { get; }

        // / <summary>
        // / Author, also known as Photographer
        // / </summary>
        PeopleList Authors { get; set; }

        double Brightness { get; }

        // / <summary>
        // / Comment, also known as Description
        // / </summary>
        string Comment { get; set; }
        
        string CameraManufacturer { get; set; }
        
        string CameraModel { get; set; }
        
        string Copyright { get; set; }
        
        // dateUTC XMP private namespace value
        // dateTaken must be valid
        // dateDigitised can be null should be the same as dateTaken
        DateTime DateDigitised { get; set; }
        
        DateTime DateTaken { get; set; }

        string ExposureBias { get; }

        string FocalLength { get; }

        int Height { get; }

        Address IptcAddress { get; set; }

        string Iso { get; }

        /// <summary>
        /// Rating (Ranging 0-5)
        /// </summary>
        int Rating { get; set; }

        XmpRegionInfo RegionInfo { get; }

        // Readonly Properties
        string ShutterSpeed { get; }

        string Subject { get; set; }

        TagList Tags { get; set; }

        // / <summary>
        // / Title (sometimes knows as Subject but there is another attribute for that!)
        // / </summary>
        string Title { get; set; }

        int Width { get; }
    }
}
