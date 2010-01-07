// <copyright file="IFotoFlyMetadata.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>IFotoFlyMetadata Interface</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IFotoFlyMetadata
    {
        DateTime UtcDate { get; set; }

        double? UtcOffset { get; set; }

        DateTime LastEditDate { get; set; }

        DateTime OriginalCameraDate { get; set; }

        string OriginalCameraFilename { get; set; }

        DateTime AddressOfGpsLookupDate { get; set; }

        Address AddressOfGps { get; set; }

        Address Address { get; set; }

        string AddressOfGpsSource { get; set; }

        GpsPosition.Accuracies AccuracyOfGps { get; set; }
    }
}
