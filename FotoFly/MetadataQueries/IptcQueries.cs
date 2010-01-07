// <copyright file="IptcQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>IptcQueries</summary>
namespace FotoFly.MetadataQueries
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
        public static readonly MetdataQuery<Int32, int> Padding = new MetdataQuery<Int32, int>("/app1/ifd/PaddingSchema:Padding");

        // Identifies city of object data origin according to guidelines established by the provider.
        public static readonly MetdataQuery<string, string> City = new MetdataQuery<string, string>("/app13/irb/8bimiptc/iptc/City");

        // Identifies the location within a city from which the object data originates
        public static readonly MetdataQuery<string, string> Country = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/Country\/Primary Location Name");
        
        // Identifies Province/State/County of origin according to guidelines established by the provider.
        public static readonly MetdataQuery<string, string> Region = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/Province\/State");

        // Identifies the location within a city from which the object data originates
        public static readonly MetdataQuery<string, string> SubLocation = new MetdataQuery<string, string>("/app13/irb/8bimiptc/iptc/Sub-location");

        // Copyright notice.
        public static readonly MetdataQuery<string, string> Copyright = new MetdataQuery<string, string>(@"/ifd/iptc/copyright notice");

        // Identification of the name of the person involved in the writing
        public static readonly MetdataQuery<string, string> WriterEditor = new MetdataQuery<string, string>(@"/ifd/iptc/writer\/editor");

        // Unused
        public static readonly MetdataQuery<string, string> Credit = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/Credit");
        public static readonly MetdataQuery<string, string> Urgency = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/Urgency");
        public static readonly MetdataQuery<string, string> Sublocation = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/Sub-location");
        public static readonly MetdataQuery<string, string> CopyrightNotice = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/Copyright Notice");
        public static readonly MetdataQuery<string, string> Category = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/Category");
        public static readonly MetdataQuery<string, string> SupplementalCategory = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/Supplemental Category");
        public static readonly MetdataQuery<string, string> BylineTitle = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/By-line Title");
        public static readonly MetdataQuery<string, string> Byline = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/By-line");
        public static readonly MetdataQuery<string, string> Caption = new MetdataQuery<string, string>(@"/app13/irb/8bimiptc/iptc/Caption");

        public static readonly MetdataQuery<string[], TagList> Keywords = new MetdataQuery<string[], TagList>(@"/app13/irb/8bimiptc/iptc/Caption");
    }
}
