// <copyright file="XmpDublinCoreQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-12</date>
// <summary>Xmp Dublin Core Queries</summary>
namespace FotoFly.MetadataQueries
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
        public static readonly MetdataQuery Contributor = new MetdataQuery("/xmp/dc:contributor", typeof(string));

        // Text: The extent or scope of the resource.
        public static readonly MetdataQuery Coverage = new MetdataQuery("/xmp/dc:coverage", typeof(string));

        // Seq: The authors of the resource (listed in order of precedence, if significant).
        public static readonly MetdataQuery Creator = new MetdataQuery("/xmp/dc:creator/", typeof(string[]));

        // Seq: Date(s) that something interesting happened to the resource
        public static readonly MetdataQuery Date = new MetdataQuery("/xmp/dc:date", typeof(string));

        // Lang Alt: A textual description of the content of the resource. Multiple values may be present for different languages.
        public static readonly MetdataQuery Description = new MetdataQuery("/xmp/dc:description", typeof(string));

        // MIMEType: The file format used when saving the resource. Tools an applications should set this property to the save format of the data. It may include appropriate qualifiers.
        public static readonly MetdataQuery Format = new MetdataQuery("/xmp/dc:format/", typeof(string));

        // Text: Unique identifier of the resource.
        public static readonly MetdataQuery Identifier = new MetdataQuery("/xmp/dc:identifier/", typeof(string));

        // Bag: An unordered array specifying the languages used in the resource.
        public static readonly MetdataQuery Language = new MetdataQuery("/xmp/dc:language/", typeof(string));

        // Bag: Publishers.
        public static readonly MetdataQuery Publisher = new MetdataQuery("/xmp/dc:publisher/", typeof(string));

        // Text: Relationships to other documents.
        public static readonly MetdataQuery Relation = new MetdataQuery("/xmp/dc:relation/", typeof(string));

        // Lang Alt: Informal rights statement, selected by language.
        public static readonly MetdataQuery Rights = new MetdataQuery("/xmp/dc:rights", typeof(string));

        // Text: Unique identifier of the work from which this resource was derived.
        public static readonly MetdataQuery Source = new MetdataQuery("/xmp/dc:source", typeof(string));

        // Bag: An unordered array of descriptive phrases or keywords that specify the topic of the content of the resource.
        public static readonly MetdataQuery Subject = new MetdataQuery("/xmp/dc:subject", typeof(string));

        // Lang Alt: The title of the document, or the name given to the resource. Typically, it will be a name by which the resource is formally known.
        public static readonly MetdataQuery Title = new MetdataQuery("/xmp/dc:title", typeof(string));

        // Bag: A document type; for example, novel, poem, or working paper.
        public static readonly MetdataQuery Type = new MetdataQuery("/xmp/dc:type", typeof(string));
    }
}
