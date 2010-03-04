// <copyright file="Address.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>Address Class</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    [XmlRootAttribute("Address", Namespace = "http://www.tassography.com/fotofly")]
    public class Address : ICloneable
    {
        public Address()
        {
            this.Reset();
        }

        public Address(string hierarchicalName)
        {
            this.HierarchicalName = hierarchicalName;
        }

        /// <summary>
        /// Region, also known as State or County
        /// </summary>
        [XmlIgnore]
        public string Region
        {
            get;
            set;
        }

        /// <summary>
        /// AddressLine, also known Sublocation in IPTC
        /// </summary>
        [XmlIgnore]
        public string AddressLine
        {
            get;
            set;
        }

        [XmlIgnore]
        public string City
        {
            get;
            set;
        }

        [XmlIgnore]
        public string Country
        {
            get;
            set;
        }

        [XmlIgnore]
        public string FormattedString
        {
            get
            {
                StringBuilder returnValue = new StringBuilder();

                for (int i = this.Parts.Count - 1; i > -1; i--)
                {
                    returnValue.Append(this.Parts[i]);
                    returnValue.Append(", ");
                }

                if (returnValue.Length > 0)
                {
                    returnValue = returnValue.Remove(returnValue.Length - 2, 2);
                }

                return returnValue.ToString();
            }
        }

        [XmlAttribute]
        public string HierarchicalName
        {
            get
            {
                // Must create using Country, Region, City & Addressline fields
                // Everything else uses this as the base value

                // Country\Region\City is straight forward concatenation
                StringBuilder hierarchicalName = new StringBuilder();
                hierarchicalName.Append(this.Country);
                hierarchicalName.Append("/");
                hierarchicalName.Append(this.Region);
                hierarchicalName.Append("/");
                hierarchicalName.Append(this.City);
                hierarchicalName.Append("/");

                // Now parse addressline, using comma as a delimiter
                if (!string.IsNullOrEmpty(this.AddressLine))
                {
                    string[] addressArray = this.AddressLine.Split(',');

                    for (int i = addressArray.Length - 1; i >= 0; i--)
                    {
                        // Trim address to remove white space and add to hierarchicalName
                        hierarchicalName.Append(addressArray[i].Trim());
                        hierarchicalName.Append("/");
                    }
                }

                // Tidy up the formatted address
                hierarchicalName = new StringBuilder(hierarchicalName.ToString().TrimEnd('/'));
                hierarchicalName = new StringBuilder(hierarchicalName.ToString().Replace(@"//", @"/"));

                return hierarchicalName.ToString();
            }

            set
            {
                this.Reset();

                string newAddress = value;

                if (!string.IsNullOrEmpty(newAddress))
                {
                    // Ensure correct format of the hierarchicalName
                    newAddress = newAddress.Trim();
                    newAddress = newAddress.Replace(@" /", @"/");
                    newAddress = newAddress.Replace(@"/ ", @"/");
                    newAddress = newAddress.TrimStart('/');

                    // Split the string up
                    string[] keywordArray = newAddress.Split('/');

                    // Set Country
                    if (keywordArray.Length >= 1)
                    {
                        this.Country = keywordArray[0];
                    }

                    // Set Region
                    if (keywordArray.Length >= 2)
                    {
                        this.Region = keywordArray[1];
                    }

                    // Set City
                    if (keywordArray.Length >= 3)
                    {
                        this.City = keywordArray[2];
                    }

                    // Set AddressLine
                    if (keywordArray.Length >= 4)
                    {
                        // Read each part of the rest of the address
                        for (int i = 3; i < keywordArray.Length; i++)
                        {
                            this.AddressLine = keywordArray[i] + ", " + this.AddressLine;
                        }

                        this.AddressLine = this.AddressLine.Remove(this.AddressLine.Length - 2, 2);
                    }
                }
            }
        }

        [XmlIgnore]
        public string HierarchicalCountryRegionCity
        {
            get
            {
                // Return address truncated to Country\Region\City
                return this.HierarchicalNameTruncated(3);
            }
        }

        [XmlIgnore]
        public bool IsCompleteAddress
        {
            get
            {
                return !string.IsNullOrEmpty(this.Country) && !string.IsNullOrEmpty(this.Region) && !string.IsNullOrEmpty(this.City);
            }
        }

        [XmlIgnore]
        public bool IsValidAddress
        {
            get
            {
                return !string.IsNullOrEmpty(this.Country);
            }
        }

        [XmlIgnore]
        public string ShortName
        {
            get
            {
                // Return the lowest part of the address
                return this.HierarchicalName.Split('/').Last();
            }
        }

        [XmlIgnore]
        public List<string> Parts
        {
            get
            {
                return new List<string>(this.HierarchicalName.Split('/'));
            }
        }

        public int HierarchicalNameLength
        {
            get
            {
                return this.HierarchicalName.Split('/').Length;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Address)
            {
                return (obj as Address).HierarchicalName == this.HierarchicalName;
            }

            return false;
        }

        public override string ToString()
        {
            return this.HierarchicalName;
        }

        public object Clone()
        {
            return new Address(this.HierarchicalName);
        }

        public Address AddressTruncated(int length)
        {
            return new Address(this.HierarchicalNameTruncated(length));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private string HierarchicalNameTruncated(int length)
        {
            StringBuilder returnName = new StringBuilder();

            foreach (string part in this.Parts)
            {
                returnName.Append(part + "/");

                length--;

                if (length == 0)
                {
                    break;
                }
            }

            return returnName.ToString().TrimEnd('/');
        }

        private void Reset()
        {
            // Reset all the values
            this.City = string.Empty;
            this.AddressLine = string.Empty;
            this.Region = string.Empty;
            this.Country = string.Empty;
        }
    }
}
