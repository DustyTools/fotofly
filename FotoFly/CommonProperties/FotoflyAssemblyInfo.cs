// <copyright file="FotoflyAssemblyInfo.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-03-02</date>
// <summary>FotoflyAssemblyInfo Class</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;

    public static class FotoflyAssemblyInfo
    {
        public static string ShortBuildVersion
        {
            get
            {
                string versionNumber = string.Empty;

                string[] name = Assembly.GetExecutingAssembly().FullName.Split(',');

                if (name.Length == 4)
                {
                    versionNumber = name[1].ToLower().Replace("version=", "v");

                    name = versionNumber.Split('.');

                    return "Fotofly" + name[0] + "." + name[1];
                }

                throw new Exception("Failed to get build number");
            }
        }

        public static string BuildVersion
        {
            get
            {
                string versionNumber = string.Empty;

                string[] name = Assembly.GetExecutingAssembly().FullName.Split(',');

                if (name.Length == 4)
                {
                    versionNumber = name[1].ToLower();
                    versionNumber = versionNumber.Replace("version=", "v");
                }
                else
                {
                    throw new Exception("Failed to get build number");
                }

                return "Fotofly" + versionNumber;
            }
        }
    }
}
