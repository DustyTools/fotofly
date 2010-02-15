// <copyright file="XmpDublinCoreQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>Xmp Dublin Core Queries</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public static class XmpDublinCoreQueries
    {
        // Dublin Core Schema
        // The Dublin Core schema provides a set of commonly used properties.
        // The schema namespace URI is http://purl.org/dc/elements/1.1/
        // The preferred schema namespace prefix is dc

        // BAG: Contributors to the resource (other than the authors).
        public static readonly MetdataQuery<string, string> Contributor = new MetdataQuery<string, string>("/xmp/dc:contributor");

        // Text: The extent or scope of the resource.
        public static readonly MetdataQuery<string, string> Coverage = new MetdataQuery<string, string>("/xmp/dc:coverage");

        // Seq: The authors of the resource (listed in order of precedence, if significant).
        public static readonly MetdataQuery<string[], string> Creator = new MetdataQuery<string[], string>("/xmp/dc:creator/");

        // Seq: Date(s) that something interesting happened to the resource
        public static readonly MetdataQuery<string, string> Date = new MetdataQuery<string, string>("/xmp/dc:date");

        // Lang Alt: A textual description of the content of the resource. Multiple values may be present for different languages.
        public static readonly MetdataQuery<string, string> Description = new MetdataQuery<string, string>("/xmp/dc:description");

        // MIMEType: The file format used when saving the resource. Tools an applications should set this property to the save format of the data. It may include appropriate qualifiers.
        public static readonly MetdataQuery<string, string> Format = new MetdataQuery<string, string>("/xmp/dc:format/");

        // Text: Unique identifier of the resource.
        public static readonly MetdataQuery<string, string> Identifier = new MetdataQuery<string, string>("/xmp/dc:identifier/");

        // Bag: An unordered array specifying the languages used in the resource.
        public static readonly MetdataQuery<string, string> Language = new MetdataQuery<string, string>("/xmp/dc:language/");

        // Bag: Publishers.
        public static readonly MetdataQuery<string, string> Publisher = new MetdataQuery<string, string>("/xmp/dc:publisher/");

        // Text: Relationships to other documents.
        public static readonly MetdataQuery<string, string> Relation = new MetdataQuery<string, string>("/xmp/dc:relation/");

        // Lang Alt: Informal rights statement, selected by language.
        public static readonly MetdataQuery<string, string> Rights = new MetdataQuery<string, string>("/xmp/dc:rights");

        // Text: Unique identifier of the work from which this resource was derived.
        public static readonly MetdataQuery<string, string> Source = new MetdataQuery<string, string>("/xmp/dc:source");

        // Bag: An unordered array of descriptive phrases or keywords that specify the topic of the content of the resource.
        public static readonly MetdataQuery<string, string> Subject = new MetdataQuery<string, string>("/xmp/dc:subject");

        // Lang Alt: The title of the document, or the name given to the resource. Typically, it will be a name by which the resource is formally known.
        public static readonly MetdataQuery<string, string> Title = new MetdataQuery<string, string>("/xmp/dc:title");

        // Bag: A document type; for example, novel, poem, or working paper.
        public static readonly MetdataQuery<string, string> Type = new MetdataQuery<string, string>("/xmp/dc:type");
    }
}
