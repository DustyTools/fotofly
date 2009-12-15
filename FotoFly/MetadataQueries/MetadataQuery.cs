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

    public class MetdataQuery
    {
        public MetdataQuery(string query, Type valueType)
        {
            this.Query = query;
            this.ValueType = valueType;
        }

        public string Query
        {
            get;
            set;
        }

        public Type ValueType
        {
            get;
            set;
        }
    }
}