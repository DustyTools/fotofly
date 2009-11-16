// <copyright file="IImageMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>IPhotoMetadata Interface</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPhotoMetadata
    {
        /// <summary>
        /// Aperture
        /// </summary>
        string Aperture { get; }

        /// <summary>
        /// List of Authors, also known as Photographer
        /// </summary>
        PeopleList Authors { get; set; }

        /// <summary>
        /// Comment, also known as Description
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// Camera Manufacturer
        /// </summary>
        string CameraManufacturer { get; set; }
        
        /// <summary>
        /// Camera Model, normally includes camera Manufacturer
        /// </summary>
        string CameraModel { get; set; }
        
        /// <summary>
        /// Copyright owner of the photo
        /// </summary>
        string Copyright { get; set; }
        
        /// <summary>
        /// DateDigitized, recorded by the camera when the photo is taken
        /// </summary>
        DateTime DateDigitised { get; set; }
        
        /// <summary>
        /// DateTaken, recorded by the camera when the photo is taken
        /// </summary>
        DateTime DateTaken { get; set; }

        /// <summary>
        /// DateAquired, Microsoft Windows7 Property field
        /// </summary>
        DateTime DateAquired { get; set; }

        /// <summary>
        /// DigitalZoomRatio
        /// </summary>
        double DigitalZoomRatio { get; }

        /// <summary>
        /// Exposure Bias
        /// </summary>
        string ExposureBias { get; }

        /// <summary>
        /// Focal Length
        /// </summary>
        string FocalLength { get; }

        /// <summary>
        /// Image Height measured in Pixels
        /// </summary>
        int ImageHeight { get; }

        /// <summary>
        /// Image Width measured in Pixels
        /// </summary>
        int ImageWidth { get; }

        /// <summary>
        /// Horizontal Resolution of main photo
        /// </summary>
        int HorizontalResolution { get; }

        /// <summary>
        /// ISO Speed rating 
        /// </summary>
        string Iso { get; }

        /// <summary>
        /// Metering Mode
        /// </summary>
        PhotoMetadataEnums.MeteringModes MeteringMode { get; }

        /// <summary>
        /// Rating (Ranging 0-5)
        /// </summary>
        int Rating { get; set; }

        /// <summary>
        /// Microsoft Region Info extension which provides data on regions in the photo
        /// </summary>
        XmpRegionInfo RegionInfo { get; set; }

        /// <summary>
        /// Shutter Speed
        /// </summary>
        string ShutterSpeed { get; }

        /// <summary>
        /// Subject, not often used by software, Title should be used in most cases
        /// </summary>
        string Subject { get; set; }

        /// <summary>
        /// List of Tags, sometimes known as Keywords
        /// </summary>
        TagList Tags { get; set; }

        /// <summary>
        /// Title, sometimes knows as Caption and Subject (there is also another attribute for Subject)
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Vertical Resolution of main photo
        /// </summary>
        int VerticalResolution { get; }

        /// <summary>
        /// Gps Position
        /// </summary>
        GpsPosition GpsPosition { get; set; }

        /// <summary>
        /// Iptc Address
        /// </summary>
        Address IptcAddress { get; set; }
    }
}
