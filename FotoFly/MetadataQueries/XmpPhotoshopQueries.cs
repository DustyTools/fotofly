// <copyright file="XmpPhotoshopQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>Xmp Photoshop Queries</summary>
namespace FotoFly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public static class XmpPhotoshopQueries
    {
        // Photoshop Schema
        // This schema specifies properties used by Adobe Photoshop.
        //  - The schema namespace URI is http://ns.adobe.com/photoshop/1.0/
        //  - The preferred schema namespace prefix is photoshop

        // Text: By-line title.
        public static readonly MetdataQuery AuthorsPosition = new MetdataQuery("/xmp/photoshop:AuthorsPosition", typeof(string));

        // ProperName: Writer/editor
        public static readonly MetdataQuery CaptionWriter = new MetdataQuery("/xmp/photoshop:CaptionWriter", typeof(string));

        // Text : Category. Limited to 3 7-bit ASCII characters.
        public static readonly MetdataQuery Category = new MetdataQuery("/xmp/photoshop:Category", typeof(string));

        // Text : City.
        public static readonly MetdataQuery City = new MetdataQuery("/xmp/photoshop:City", typeof(string));

        // Text : Country/primary location.
        public static readonly MetdataQuery Country = new MetdataQuery("/xmp/photoshop:Country", typeof(string));

        // Text : Credit.
        public static readonly MetdataQuery Credit = new MetdataQuery("/xmp/photoshop:Credit", typeof(string));

        // Date: The date the intellectual content of the document was created (rather than the creation date of the
        // physical representation), following IIM conventions. For example, a photo taken during the American
        // Civil War would have a creation date during that epoch (1861-1865) rather than the date the photo
        // was digitized for archiving.
        public static readonly MetdataQuery DateCreated = new MetdataQuery("/xmp/photoshop:DateCreated", typeof(string));

        // Text : Headline.
        public static readonly MetdataQuery Headline = new MetdataQuery("/xmp/photoshop:Headline", typeof(string));

        // Text : Special instructions.
        public static readonly MetdataQuery Instructions = new MetdataQuery("/xmp/photoshop:Instructions", typeof(string));

        // Text : Source.
        public static readonly MetdataQuery Source = new MetdataQuery("/xmp/photoshop:Source", typeof(string));

        // Text : Province/state.
        public static readonly MetdataQuery State = new MetdataQuery("/xmp/photoshop:State", typeof(string));

        // Bag : Supplemental category
        public static readonly MetdataQuery SupplementalCategories = new MetdataQuery("/xmp/photoshop:SupplementalCategories", typeof(string));

        // Text: External Original transmission reference.
        public static readonly MetdataQuery TransmissionReference = new MetdataQuery("/xmp/photoshop:TransmissionReferenceText", typeof(string));

        // Integer : Urgency. Valid range is 1-8.
        public static readonly MetdataQuery Urgency = new MetdataQuery("/xmp/photoshop:Urgency", typeof(string));
    }
}
