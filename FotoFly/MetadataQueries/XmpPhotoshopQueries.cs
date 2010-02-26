// <copyright file="XmpPhotoshopQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>Xmp Photoshop Queries</summary>
namespace Fotofly.MetadataQueries
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
        public static readonly MetdataQuery<string, string> AuthorsPosition = new MetdataQuery<string, string>("/xmp/photoshop:AuthorsPosition");

        // ProperName: Writer/editor
        public static readonly MetdataQuery<string, string> CaptionWriter = new MetdataQuery<string, string>("/xmp/photoshop:CaptionWriter");

        // Text : Category. Limited to 3 7-bit ASCII characters.
        public static readonly MetdataQuery<string, string> Category = new MetdataQuery<string, string>("/xmp/photoshop:Category");

        // Text : City.
        public static readonly MetdataQuery<string, string> City = new MetdataQuery<string, string>("/xmp/photoshop:City");

        // Text : Country/primary location.
        public static readonly MetdataQuery<string, string> Country = new MetdataQuery<string, string>("/xmp/photoshop:Country");

        // Text : Credit.
        public static readonly MetdataQuery<string, string> Credit = new MetdataQuery<string, string>("/xmp/photoshop:Credit");

        // Date: The date the intellectual content of the document was created (rather than the creation date of the
        // physical representation), following IIM conventions. For example, a photo taken during the American
        // Civil War would have a creation date during that epoch (1861-1865) rather than the date the photo
        // was digitized for archiving.
        public static readonly MetdataQuery<string, string> DateTimeCreated = new MetdataQuery<string, string>("/xmp/photoshop:DateCreated");

        // Text : Headline.
        public static readonly MetdataQuery<string, string> Headline = new MetdataQuery<string, string>("/xmp/photoshop:Headline");

        // Text : Special instructions.
        public static readonly MetdataQuery<string, string> Instructions = new MetdataQuery<string, string>("/xmp/photoshop:Instructions");

        // Text : Source.
        public static readonly MetdataQuery<string, string> Source = new MetdataQuery<string, string>("/xmp/photoshop:Source");

        // Text : Province/state.
        public static readonly MetdataQuery<string, string> State = new MetdataQuery<string, string>("/xmp/photoshop:State");

        // Bag : Supplemental category
        public static readonly MetdataQuery<string, string> SupplementalCategories = new MetdataQuery<string, string>("/xmp/photoshop:SupplementalCategories");

        // Text: External Original transmission reference.
        public static readonly MetdataQuery<string, string> TransmissionReference = new MetdataQuery<string, string>("/xmp/photoshop:TransmissionReferenceText");

        // Integer : Urgency. Valid range is 1-8.
        public static readonly MetdataQuery<string, string> Urgency = new MetdataQuery<string, string>("/xmp/photoshop:Urgency");
    }
}
