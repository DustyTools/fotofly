// <copyright file="MetadataQuery.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-17</date>
// <summary>MetadataQuery</summary>
namespace FotoFly.MetadataQueries
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

        public Type FotoFlyType
        {
            get
            {
                return typeof(F);
            }
        }
    }
}