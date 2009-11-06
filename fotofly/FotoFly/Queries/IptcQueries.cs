namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class IptcQueries
    {
        // Padding
        public static readonly string Padding = "/app1/ifd/PaddingSchema:Padding";

        // Place Fields
        public static readonly string City = "/app13/irb/8bimiptc/iptc/City";
        public static readonly string Country = @"/app13/irb/8bimiptc/iptc/Country\/Primary Location Name";
        public static readonly string Region = @"/app13/irb/8bimiptc/iptc/Province\/State";
        public static readonly string SubLocation = "/app13/irb/8bimiptc/iptc/Sub-location";

        // IPTC Fields
        // See http://www.ap.org/apserver/userguide/codes.htm
        public static readonly string Copyright = @"/ifd/iptc/copyright notice";
        public static readonly string WriterEditor = @"/ifd/iptc/writer\/editor";
    }
}
