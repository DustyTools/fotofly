// <copyright file="XmpIptcExtQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2011-03-11</date>
// <summary>XmpIptcExtQueries Queries</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public static class XmpIptcExtQueries
    {
        // The Queries are taken from the IPTC Photo Metadata 2008
        // The IPTC Extension schema extends and complements the IPTC Core schema
        // http://www.exiv2.org/tags-xmp-iptcExt.html

        public static readonly MetdataQuery<string, string> AddlModelInfo = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:AddlModelInfo");
        public static readonly MetdataQuery<string, string> OrganisationInImageCode = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:OrganisationInImageCode");
        public static readonly MetdataQuery<string, string> CVterm = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:CVterm");
        public static readonly MetdataQuery<string, string> ModelAge = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:ModelAge");
        public static readonly MetdataQuery<string, string> OrganisationInImageName = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:OrganisationInImageName");
        public static readonly MetdataQuery<string, string> PersonInImage = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:PersonInImage");
        public static readonly MetdataQuery<string, string> DigImageGUID = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:DigImageGUID");
        public static readonly MetdataQuery<string, string> DigitalSourcefileType = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:DigitalSourcefileType");
        public static readonly MetdataQuery<string, string> Event = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:Event");
        public static readonly MetdataQuery<string, string> MaxAvailHeight = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:MaxAvailHeight");
        public static readonly MetdataQuery<string, string> MaxAvailWidth = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:MaxAvailWidth");
        public static readonly MetdataQuery<string, string> RegistryId = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:RegistryId");
        public static readonly MetdataQuery<string, string> RegItemId = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:RegItemId");
        public static readonly MetdataQuery<string, string> RegOrgId = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:RegOrgId");
        public static readonly MetdataQuery<string, string> IptcLastEdited = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:IptcLastEdited");
        public static readonly MetdataQuery<string, string> LocationShown = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:LocationShown");
        public static readonly MetdataQuery<string, string> LocationCreated = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:LocationCreated");
        public static readonly MetdataQuery<string, string> City = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:City");
        public static readonly MetdataQuery<string, string> CountryCode = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:CountryCode");
        public static readonly MetdataQuery<string, string> CountryName = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:CountryName");
        public static readonly MetdataQuery<string, string> ProvinceState = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:ProvinceState");
        public static readonly MetdataQuery<string, string> Sublocation = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:Sublocation");
        public static readonly MetdataQuery<string, string> WorldRegion = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:WorldRegion");
        public static readonly MetdataQuery<string, string> ArtworkOrObject = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:ArtworkOrObject");
        public static readonly MetdataQuery<string, string> AOCopyrightNotice = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:AOCopyrightNotice");
        public static readonly MetdataQuery<string, string> AOCreator = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:AOCreator");
        public static readonly MetdataQuery<string, string> AODateCreated = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:AODateCreated");
        public static readonly MetdataQuery<string, string> AOSource = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:AOSource");
        public static readonly MetdataQuery<string, string> AOSourceInvNo = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:AOSourceInvNo");
        public static readonly MetdataQuery<string, string> AOTitle = new MetdataQuery<string, string>("/xmp/http\\:\\/\\/iptc.org\\/std\\/Iptc4xmpExt\\/2008-02-29\\/:AOTitle");
    }
}
