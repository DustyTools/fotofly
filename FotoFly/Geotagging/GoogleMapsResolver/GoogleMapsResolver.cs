// <copyright file="GoogleMapsResolver.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>Class to retrieve Addresses from Google using GPS Position</summary>
namespace FotoFly.Geotagging.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    public class GoogleMapsResolver : IResolverCache
    {
        public static readonly string SourceName = "Google Maps";

        // http://code.google.com/apis/maps/documentation/geocoding/index.html
        private readonly string googleUrl = "http://maps.google.com/maps/geo?q={0},{1}&output=xml&sensor=true_or_false&key=your_api_key";

        private ResolverCache resolverCache;

        public void ConfigResolverCache(string cacheDirectory, string cacheName)
        {
            this.resolverCache = new ResolverCache(cacheDirectory, cacheName);
        }

        public Address FindAddress(GpsPosition gpsPosition)
        {
            Address returnValue = new Address();

            if (gpsPosition.IsValidCoordinate == true)
            {
                string lat = gpsPosition.Latitude.ToString();
                string lon = gpsPosition.Longitude.ToString();
                string url = String.Format(this.googleUrl, lat, lon);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; 50iso)";
                webRequest.Method = "GET";
                webRequest.Accept = "image/jpeg, image/gif, */*";
                webRequest.AllowAutoRedirect = false;
                webRequest.Timeout = 5000;

                WebResponse webResponse = null;
                string jsonResponse = null;

                try
                {
                    webResponse = webRequest.GetResponse();

                    using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        jsonResponse = streamReader.ReadToEnd();

                        streamReader.Close();
                    }
                }
                catch
                {
                }

                if (!String.IsNullOrEmpty(jsonResponse))
                {
                    ////JObject o = JObject.Parse(gpsjson);
                    ////JArray placemark = (JArray)o["Placemark"];

                    ////foreach (JObject instance in placemark)
                    ////{
                    ////    string address = address ?? (string)instance["address"];

                    ////    JObject ad = (JObject)instance["AddressDetails"];

                    ////    if (ad == null)
                    ////    {
                    ////        continue;
                    ////    }

                    ////    JArray aline = (JArray)ad["AddressLine"];

                    ////    if (aline != null)
                    ////    {
                    ////        returnValue.AddressLine = returnValue.AddressLine ?? (string)aline[0];
                    ////    }

                    ////    JObject cc = (JObject)ad["Country"];

                    ////    if (cc == null)
                    ////    {
                    ////        continue;
                    ////    }

                    ////    returnValue.Country = returnValue.Country ?? (string)cc["CountryName"];

                    ////    JObject aa = (JObject)cc["AdministrativeArea"];

                    ////    if (aa == null)
                    ////    {
                    ////        continue;
                    ////    }

                    ////    returnValue.Region = returnValue.Region ?? (string)aa["AdministrativeAreaName"];

                    ////    JObject saa = (JObject)aa["SubAdministrativeArea"];

                    ////    if (saa == null)
                    ////    {
                    ////        continue;
                    ////    }

                    ////    JObject locality = (JObject)saa["Locality"];

                    ////    if (locality == null)
                    ////    {
                    ////        continue;
                    ////    }

                    ////    returnValue.City = returnValue.City ?? (string)locality["LocalityName"];
                    ////}

                    ////// override city with address line if not set
                    ////returnValue.City = returnValue.City ?? returnValue.AddressLine;
                }
            }

            return returnValue;
        }
    }
}
