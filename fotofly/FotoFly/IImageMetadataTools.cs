﻿namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class IImageMetadataTools
    {
        public static void CompareMetadata(object source, object destination, out List<string> changes)
        {
            IImageMetadataTools.UseReflection(source, destination, false, out changes);
        }

        public static void CopyMetadata(object source, object destination, out List<string> changes)
        {
            IImageMetadataTools.UseReflection(source, destination, true, out changes);
        }

        public static void CopyMetadata(object source, object destination)
        {
            List<string> changes;

            IImageMetadataTools.UseReflection(source, destination, true, out changes);
        }

        private static void UseReflection(object source, object destination, bool applyChanges, out List<string> changes)
        {
            // Use Reflection to copy properties of the same name and type
            // This is done to reduce the risk of overwriting data in the file
            changes = new List<string>();

            // Loop through every property in the source
            foreach (PropertyInfo sourcePropertyInfo in source.GetType().GetProperties())
            {
                string sourceName = sourcePropertyInfo.Name;
                object sourceValue = sourcePropertyInfo.GetValue(source, null);
                Type sourceType = sourcePropertyInfo.PropertyType;

                // Look for a matching property in the destination
                var destinationProperty = from x in destination.GetType().GetProperties()
                                          where x.Name == sourceName
                                          && x.PropertyType == sourceType
                                          && x.CanWrite
                                          select x;

                PropertyInfo destinationPropertyInfo = destinationProperty.FirstOrDefault();

                // Check if there's a matching property in the destination
                if (destinationPropertyInfo != null)
                {
                    object destinationValue = destinationPropertyInfo.GetValue(destination, null);

                    if (destinationValue == null && sourceValue == null)
                    {
                        // Both null, do nothing
                    }
                    else if ((destinationValue == null && sourceValue != null) || !destinationValue.Equals(sourceValue))
                    {
                        if (applyChanges)
                        {
                            // Copy across the matching property
                            // Either as null, using cloning or a straight copy
                            if (sourceValue == null)
                            {
                                destinationPropertyInfo.SetValue(destination, null, null);
                            }
                            else if (sourceValue.GetType().GetInterface("ICloneable", true) == null)
                            {
                                destinationPropertyInfo.SetValue(destination, sourceValue, null);
                            }
                            else
                            {
                                destinationPropertyInfo.SetValue(destination, ((ICloneable)sourceValue).Clone(), null);
                            }
                        }

                        StringBuilder change = new StringBuilder();
                        change.Append(sourceName);
                        change.Append(" (From:'");
                        change.Append(destinationValue);
                        change.Append("' To: '");
                        change.Append(sourceValue); 
                        change.Append("')");

                        changes.Add(change.ToString());
                    }
                }
            }
        }
    }
}
