// <copyright file="XmpXapQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-19</date>
// <summary>XmpXapQueries</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class XmpXapQueries
    {
        // Xap Schema
        // - The schema name is http://ns.adobe.com/xap/1.0/
        // - The preferred schema namespace prefix is xap
        public static readonly string QueryPrefix = @"/xmp/http\:\/\/ns.adobe.com\/xap\/1.0\/:";

        // creatortool (LPWSTR) ["http://ns.adobe.com/xap/1.0/" (LPWSTR)]
        public static readonly MetdataQuery<string, string> CreatorTool = new MetdataQuery<string, string>(XmpXapQueries.QueryPrefix, "creatortool");

        // Rating (LPWSTR) ["http://ns.adobe.com/xap/1.0/" (LPWSTR)]
        public static readonly MetdataQuery<string, double> Rating = new MetdataQuery<string, double>(XmpXapQueries.QueryPrefix, "Rating");
    }
}
