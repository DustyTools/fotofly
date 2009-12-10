// <copyright file="FotoFlyMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>FotoFlyMetadata</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class FotoFlyMetadata : IFotoFlyMetadata
    {
        public FotoFlyMetadata()
        {
        }

        public DateTime UtcDate { get; set; }

        public double UtcOffset { get; set; }

        public DateTime LastEditDate { get; set; }
        
        public DateTime AddressOfGpsLookupDate { get; set; }

        public DateTime OriginalCameraDate { get; set; }

        public string OriginalCameraFilename { get; set; }

        public Address AddressOfGps { get; set; }

        public string AddressOfGpsSource { get; set; }

        public GpsPosition.Accuracies AccuracyOfGps { get; set; }
    }
}