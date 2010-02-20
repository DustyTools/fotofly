// <copyright file="XmpTiffQueries.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-10</date>
// <summary>XmpTiffQueries</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class XmpTiffQueries
    {
        // TIFF Schema
        // - The schema name is http://ns.adobe.com/tiff/1.0/
        // - The preferred schema namespace prefix is tiff
        private static string schema = @"/xmp/tiff:";

        // make (LPWSTR) ["http://ns.adobe.com/tiff/1.0/" (LPWSTR)]
        public static readonly MetdataQuery<string, string> Make = new MetdataQuery<string, string>(XmpTiffQueries.schema, "make");

        // model (LPWSTR) ["http://ns.adobe.com/tiff/1.0/" (LPWSTR)]
        public static readonly MetdataQuery<string, string> Model = new MetdataQuery<string, string>(XmpTiffQueries.schema, "model");

        // Orientation (LPWSTR) ["http://ns.adobe.com/tiff/1.0/" (LPWSTR)]
        public static readonly MetdataQuery<string, string> Orientation = new MetdataQuery<string, string>(XmpTiffQueries.schema, "Orientation");

        // software (LPWSTR) ["http://ns.adobe.com/tiff/1.0/" (LPWSTR)]
        public static readonly MetdataQuery<string, string> Software = new MetdataQuery<string, string>(XmpTiffQueries.schema, "software");
    }
}
