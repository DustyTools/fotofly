namespace FotoFly.XmlTools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class GenericSerialiser
    {
        private static Exception saveException;
        private static bool saveSucceeded;

        public static T Read<T>(string fileName) where T : new ()
        {
            return Read<T>(fileName, false);
        }

        public static T Read<T>(string fileName, bool createObjectAsNewIfNotFound) where T : new()
        {
            T returnValue = new T();

            if (File.Exists(fileName))
            {
                try
                {
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                            returnValue = (T)xmlSerializer.Deserialize(reader);
                        }

                        // Try and force the file lock to be released
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to read the file: " + fileName, e);
                }
            }
            else if (!createObjectAsNewIfNotFound)
            {
                throw new Exception("File not found: " + fileName);
            }

            if (returnValue == null && createObjectAsNewIfNotFound)
            {
                returnValue = new T();
            }

            return returnValue;
        }

        public static void Write<T>(T objectToSerialise, string fileName)
        {
            GenericSerialiser.Write<T>(objectToSerialise, fileName, 3);
        }

        public static void Write<T>(T objectToSerialise, string fileName, int retryCount)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("FileName is NULL");
            }

            GenericSerialiser.saveSucceeded = false;

            for (int i = 0; i < retryCount; i++)
            {
                // Try saving the file
                GenericSerialiser.TryWrite<T>(objectToSerialise, fileName);

                if (GenericSerialiser.saveSucceeded)
                {
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(5000);
                }
            }

            if (GenericSerialiser.saveSucceeded == false)
            {
                throw new Exception("Unable to save the file: " + fileName + "\n" + GenericSerialiser.saveException.Message);
            }
        }

        private static void TryWrite<T>(T objectToSerialise, string fileName)
        {
            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                        xmlSerializer.Serialize(writer, objectToSerialise);
                    }

                    // Try and force the file lock to be released
                    fileStream.Close();
                    fileStream.Dispose();
                }

                GenericSerialiser.saveSucceeded = true;
            }
            catch (Exception e)
            {
                GenericSerialiser.saveException = e;

                GenericSerialiser.saveSucceeded = false;
            }
        }
    }
}
