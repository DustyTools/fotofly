// <copyright file="XmpRightsQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>Xmp Rights Queries</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public static class XmpRightsQueries
    {
        // XMP Rights Management Schema
        // This schema includes properties related to rights management. These properties specify
        // information regarding the legal restrictions associated with a resource.
        //  - The schema namespace URI is http://ns.adobe.com/xap/1.0/rights/
        //  - The preferred schema namespace prefix is xmpRights

        // URL External Online rights management certificate.
        public static readonly MetdataQuery<string, string> Certificate = new MetdataQuery<string, string>("/xmp/xmpRights:Certifcate");

        // Boolean External Indicates that this is a rights-managed resource.
        public static readonly MetdataQuery<string, string> Marked = new MetdataQuery<string, string>("/xmp/xmpRights:Marked");

        // bag ProperName External An unordered array specifying the legal owner(s) of a resource.
        public static readonly MetdataQuery<string, string> Owner = new MetdataQuery<string, string>("/xmp/xmpRights:Owner");

        // Lang Alt External Text instructions on how a resource can be legally used.
        public static readonly MetdataQuery<string, string> UsageTerms = new MetdataQuery<string, string>("/xmp/xmpRights:UsageTerms");

        // URL External The location of a web page describing the owner and/or rights statement for this resource.
        public static readonly MetdataQuery<string, string> WebStatement = new MetdataQuery<string, string>("/xmp/xmpRights:WebStatement");
    }
}
