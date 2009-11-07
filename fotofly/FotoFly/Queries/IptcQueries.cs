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
        public static readonly string Padding = "/app1/ifd/PaddingSchema:Padding";

        // Identifies city of object data origin according to guidelines established by the provider.
        public static readonly string City = "/app13/irb/8bimiptc/iptc/City";

        // Identifies the location within a city from which the object data originates
        public static readonly string Country = @"/app13/irb/8bimiptc/iptc/Country\/Primary Location Name";
        
        // Identifies Province/State/County of origin according to guidelines established by the provider.
        public static readonly string Region = @"/app13/irb/8bimiptc/iptc/Province\/State";

        // Identifies the location within a city from which the object data originates
        public static readonly string SubLocation = "/app13/irb/8bimiptc/iptc/Sub-location";

        // Copyright notice.
        public static readonly string Copyright = @"/ifd/iptc/copyright notice";

        // Identification of the name of the person involved in the writing
        public static readonly string WriterEditor = @"/ifd/iptc/writer\/editor";
    }
}
