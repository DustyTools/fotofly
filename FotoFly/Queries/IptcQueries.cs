// <copyright file="IptcQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>IptcQueries</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class IptcQueries
    {
        // IPTC Fields
        // See http://www.ap.org/apserver/userguide/codes.htm

        // Padding
        public static readonly MetdataQuery Padding = new MetdataQuery("/app1/ifd/PaddingSchema:Padding", typeof(Int32));

        // Identifies city of object data origin according to guidelines established by the provider.
        public static readonly MetdataQuery City = new MetdataQuery("/app13/irb/8bimiptc/iptc/City", typeof(string));

        // Identifies the location within a city from which the object data originates
        public static readonly MetdataQuery Country = new MetdataQuery(@"/app13/irb/8bimiptc/iptc/Country\/Primary Location Name", typeof(string));
        
        // Identifies Province/State/County of origin according to guidelines established by the provider.
        public static readonly MetdataQuery Region = new MetdataQuery(@"/app13/irb/8bimiptc/iptc/Province\/State", typeof(string));

        // Identifies the location within a city from which the object data originates
        public static readonly MetdataQuery SubLocation = new MetdataQuery("/app13/irb/8bimiptc/iptc/Sub-location", typeof(string));

        // Copyright notice.
        public static readonly MetdataQuery Copyright = new MetdataQuery(@"/ifd/iptc/copyright notice", typeof(string));

        // Identification of the name of the person involved in the writing
        public static readonly MetdataQuery WriterEditor = new MetdataQuery(@"/ifd/iptc/writer\/editor", typeof(string));

        // Unused
        public static readonly MetdataQuery Credit = new MetdataQuery(@"/app13/irb/8bimiptc/iptc/Credit", typeof(string));
        public static readonly MetdataQuery Urgency = new MetdataQuery(@"/app13/irb/8bimiptc/iptc/Urgency", typeof(string));
        public static readonly MetdataQuery Sublocation = new MetdataQuery(@"/app13/irb/8bimiptc/iptc/Sub-location", typeof(string));
        public static readonly MetdataQuery CopyrightNotice = new MetdataQuery(@"/app13/irb/8bimiptc/iptc/Copyright Notice", typeof(string));
        public static readonly MetdataQuery Category = new MetdataQuery(@"/app13/irb/8bimiptc/iptc/Category", typeof(string));
        public static readonly MetdataQuery SupplementalCategory = new MetdataQuery(@"/app13/irb/8bimiptc/iptc/Supplemental Category", typeof(string));
        public static readonly MetdataQuery BylineTitle = new MetdataQuery(@"/app13/irb/8bimiptc/iptc/By-line Title", typeof(string));
    }
}
