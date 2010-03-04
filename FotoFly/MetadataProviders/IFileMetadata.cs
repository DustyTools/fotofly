namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IFileMetadata
    {
        #region Readonly Properties (e.g. Camera Settings)

        Aperture Aperture { get; }

        ShutterSpeed ShutterSpeed { get; }

        ExposureBias ExposureBias { get; }

        string FocalLength { get; }

        MetadataEnums.MeteringModes MeteringMode { get; }

        double? DigitalZoomRatio { get; }

        IsoSpeed IsoSpeed { get; }

        int ImageWidth { get; }

        int ImageHeight { get; }

        int VerticalResolution { get; }

        int HorizontalResolution { get; }

        #endregion

        #region Read & Write Properties

        string CreationSoftware { get; set; }

        string Copyright { get; set; }

        string CameraModel { get; set; }

        string CameraManufacturer { get; set; }

        MicrosoftImageRegionInfo MicrosoftRegionInfo { get; set; }

        TagList Tags { get; set; }

        GpsPosition GpsPositionOfLocationCreated { get; set; }

        GpsPosition GpsPositionOfLocationShown { get; set; }

        DateTime DateModified { get; set; }

        DateTime DateAquired { get; set; }

        DateTime DateTaken { get; set; }

        Rating Rating { get; set; }

        string Description { get; set; }

        PeopleList Authors { get; set; }

        string Comment { get; set; }

        string Subject { get; set; }

        DateTime DateDigitised { get; set; }

        DateTime DateUtc { get; set; }

        double? UtcOffset { get; set; }

        DateTime DateLastFotoflySave { get; set; }

        DateTime AddressOfGpsLookupDate { get; set; }

        DateTime OriginalCameraDate { get; set; }

        string OriginalCameraFilename { get; set; }

        Address AddressOfLocationCreated { get; set; }

        Address AddressOfLocationShown { get; set; }

        string AddressOfGpsSource { get; set; }

        #endregion
    }
}
