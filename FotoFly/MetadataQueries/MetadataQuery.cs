// <copyright file="MetadataQuery.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-17</date>
// <summary>MetadataQuery</summary>
namespace Fotofly.MetadataQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MetdataQuery<B, F>
    {
        public MetdataQuery(string query)
        {
            this.Query = query;
        }

        public MetdataQuery(string schema, string query)
        {
            this.Query = schema.TrimEnd(':') + ":" + query.TrimStart(':');
        }

        public string Query
        {
            get;
            set;
        }

        public Type BitmapMetadataType
        {
            get
            {
                return typeof(B);
            }
        }

        public Type FotoflyType
        {
            get
            {
                return typeof(F);
            }
        }
    }
}