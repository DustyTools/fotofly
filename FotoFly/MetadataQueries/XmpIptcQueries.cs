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
        public static readonly MetdataQuery<string, string> CreatorContactInfo = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:CreatorContactInfo");

        public static readonly MetdataQuery<string, string> ContactInfoCiAdrCity = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:CiAdrCity");
        public static readonly MetdataQuery<string, string> ContactInfoAdrCtry = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:CiAdrCtry");
        public static readonly MetdataQuery<string, string> ContactInfoAdrExtadr = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:CiAdrExtadr");
        public static readonly MetdataQuery<string, string> ContactInfoAdrPcode = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:CiAdrPcode");
        public static readonly MetdataQuery<string, string> ContactInfoAdrRegion = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:CiAdrRegion");

        public static readonly MetdataQuery<string, string> ContactInfoTelWork = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:CiTelWork");
        public static readonly MetdataQuery<string, string> ContactInfoEmailWork = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:CiEmailWork");
        public static readonly MetdataQuery<string, string> ContactInfoUrlWork = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:CiUrlWork");

        public static readonly MetdataQuery<string, string> AuthorsPosition = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:AuthorsPosition");
        
        public static readonly MetdataQuery<string, string> IntellectualGenre = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:IntellectualGenre");
        public static readonly MetdataQuery<string, string> Location = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:Location");
        public static readonly MetdataQuery<string, string> Scene = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:Scene");
        public static readonly MetdataQuery<string, string> SubjectCode = new MetdataQuery<string, string>("/xmp/Iptc4xmpCore:SubjectCode");

        // Address
        public static readonly MetdataQuery<string, string> LocationCreatedCountryName = new MetdataQuery<string, string>("/xmp/Iptc4xmpExt:LocationCreatedCountryName");
        public static readonly MetdataQuery<string, string> LocationCreatedProvinceState = new MetdataQuery<string, string>("/xmp/Iptc4xmpExt:LocationCreatedCountryName");
        public static readonly MetdataQuery<string, string> LocationCreatedCity = new MetdataQuery<string, string>("/xmp/Iptc4xmpExt:LocationCreatedCountryName");
    }
}
