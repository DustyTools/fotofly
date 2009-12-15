// <copyright file="XmpIptcQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>XmpIptcQueries Queries</summary>
namespace FotoFly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public static class XmpIptcQueries
    {
        // The Queries are taken from the IPTC Standards Document
        // “IPTC Core” Schema for XMP Version 1.0 Revision 8

        // Creator Contact Info
        public static readonly MetdataQuery CreatorContactInfo = new MetdataQuery("/xmp/Iptc4xmpCore:CreatorContactInfo", typeof(string));

        public static readonly MetdataQuery ContactInfoCiAdrCity = new MetdataQuery("/xmp/Iptc4xmpCore:CiAdrCity", typeof(string));
        public static readonly MetdataQuery ContactInfoAdrCtry = new MetdataQuery("/xmp/Iptc4xmpCore:CiAdrCtry", typeof(string));
        public static readonly MetdataQuery ContactInfoAdrExtadr = new MetdataQuery("/xmp/Iptc4xmpCore:CiAdrExtadr", typeof(string));
        public static readonly MetdataQuery ContactInfoAdrPcode = new MetdataQuery("/xmp/Iptc4xmpCore:CiAdrPcode", typeof(string));
        public static readonly MetdataQuery ContactInfoAdrRegion = new MetdataQuery("/xmp/Iptc4xmpCore:CiAdrRegion", typeof(string));

        public static readonly MetdataQuery ContactInfoTelWork = new MetdataQuery("/xmp/Iptc4xmpCore:CiTelWork", typeof(string));
        public static readonly MetdataQuery ContactInfoEmailWork = new MetdataQuery("/xmp/Iptc4xmpCore:CiEmailWork", typeof(string));
        public static readonly MetdataQuery ContactInfoUrlWork = new MetdataQuery("/xmp/Iptc4xmpCore:CiUrlWork", typeof(string));

        public static readonly MetdataQuery AuthorsPosition = new MetdataQuery("/xmp/Iptc4xmpCore:AuthorsPosition", typeof(string));
        
        public static readonly MetdataQuery IntellectualGenre = new MetdataQuery("/xmp/Iptc4xmpCore:IntellectualGenre", typeof(string));
        public static readonly MetdataQuery Location = new MetdataQuery("/xmp/Iptc4xmpCore:Location", typeof(string));
        public static readonly MetdataQuery Scene = new MetdataQuery("/xmp/Iptc4xmpCore:Scene", typeof(string));
        public static readonly MetdataQuery SubjectCode = new MetdataQuery("/xmp/Iptc4xmpCore:SubjectCode", typeof(string));

        // Address
        public static readonly MetdataQuery LocationCreatedCountryName = new MetdataQuery("/xmp/Iptc4xmpExt:LocationCreatedCountryName", typeof(string));
        public static readonly MetdataQuery LocationCreatedProvinceState = new MetdataQuery("/xmp/Iptc4xmpExt:LocationCreatedCountryName", typeof(string));
        public static readonly MetdataQuery LocationCreatedCity = new MetdataQuery("/xmp/Iptc4xmpExt:LocationCreatedCountryName", typeof(string));
    }
}
