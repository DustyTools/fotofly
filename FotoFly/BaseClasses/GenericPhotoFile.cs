// <copyright file="GenericPhotoFile.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>GenericPhotoFile Abstract Class</summary>
namespace Fotofly
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Media.Imaging;

    public abstract class GenericPhotoFile
    {
        private string fileName = string.Empty;
        private string fileExtension = string.Empty;
        private DateTime fileLastModified = new DateTime();

        public bool HandleExceptions
        {
            get;
            set;
        }

        /// <summary>
        /// The full file name of the file, including path
        /// </summary>
        public string FileFullName
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the file, excluding the extension and path
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        /// <summary>
        /// The Last Modified date of the file
        /// </summary>
        public DateTime FileLastModified
        {
            get
            {
                return this.fileLastModified;
            }
        }

        /// <summary>
        /// The Extension of the file with preceeding full stop
        /// </summary>
        public string FileExtension
        {
            get
            {
                return this.fileExtension;
            }
        }

        public bool IsFileNameValid
        {
            get
            {
                return !string.IsNullOrEmpty(this.FileFullName);
            }
        }

        public bool FileExists
        {
            get
            {
                return File.Exists(this.FileFullName);
            }
        }

        public List<string> SecondaryFiles
        {
            get;
            set;
        }

        public GenericPhotoEnums.ImageTypes ImageType
        {
            get
            {
                switch (this.FileExtension)
                {
                    case ".jpeg":
                    case ".jpg":
                        return GenericPhotoEnums.ImageTypes.Jpeg;

                    case ".tif":
                    case ".tiff":
                        return GenericPhotoEnums.ImageTypes.Jpeg;

                    default:
                        return GenericPhotoEnums.ImageTypes.Unknown;
                }
            }
        }

        public GenericPhotoFile()
        {
            this.SecondaryFiles = new List<string>();
        }

        protected PhotoMetadata InternalPhotoMetadata
        {
            get;
            set;
        }

        protected PhotoMetadata InternalPhotoMetadataInFile
        {
            get;
            set;
        }

        public void MoveTo(string newDirectory)
        {
            this.MoveTo(newDirectory, false);
        }

        public void MoveTo(string newDirectory, bool overWriteExistingFile)
        {
            if (this.IsFileNameValid)
            {
                FileInfo currentFile = new FileInfo(this.FileFullName);
                FileInfo newName = new FileInfo(newDirectory + currentFile.Name);

                if (newName.Exists && overWriteExistingFile)
                {
                    newName.Delete();
                }

                currentFile.MoveTo(newName.FullName);
            }
            else
            {
                throw new Exception("Filename is not valid");
            }
        }

        public void RenameFile(string newFileName, bool renameSecondary)
        {
            // Ensure new name parameter type is correct
            if (newFileName.Contains(@"/"))
            {
                throw new ArgumentException("New filename shouldn't contain a path");
            }
            else if (newFileName.EndsWith(this.FileExtension))
            {
                throw new ArgumentException("New filename shouldn't include an extension");
            }
            else if (newFileName == this.FileName)
            {
                // Nothing to do
                return;
            }

            if (this.IsFileNameValid && this.FileExists)
            {
                // Build list of files to rename (old name, new name)
                Dictionary<string, string> filesToRename = new Dictionary<string, string>();

                // Add the base file name
                filesToRename.Add(this.FileFullName, Regex.Replace(this.FileFullName, Regex.Escape(this.FileName), newFileName, RegexOptions.IgnoreCase));
                
                // Add all secondary filenames
                foreach (string file in this.SecondaryFiles)
                {
                    filesToRename.Add(file, Regex.Replace(file, Regex.Escape(this.FileName), newFileName, RegexOptions.IgnoreCase));
                }

                // Check source files don't already exist
                foreach (KeyValuePair<string, string> file in filesToRename)
                {
                    if (File.Exists(file.Value))
                    {
                        throw new Exception("File already exists");
                    }
                }

                // Flag to determine if all files copied successfully
                bool success = true;

                // Try to rename all files
                foreach (KeyValuePair<string, string> file in filesToRename)
                {
                    try
                    {
                        File.Move(file.Key, file.Value);
                    }
                    catch
                    {
                        success = false;
                        break;
                    }
                }

                // If not successful rewind
                if (!success)
                {
                    // Try and rename all files back to thier original name
                    foreach (KeyValuePair<string, string> file in filesToRename)
                    {
                        if (File.Exists(file.Value))
                        {
                            try
                            {
                                File.Move(file.Value, file.Key);
                            }
                            catch
                            {
                                throw new Exception("Unable to rollback from failed rename");
                            }
                        }
                    }

                    throw new Exception("Unable to rename, rollback successful");
                }

                // Update all metadata
                this.FileFullName = filesToRename.First().Value;

                this.SecondaryFiles.Clear();

                // Skip the first file (it's the name file)
                foreach (KeyValuePair<string, string> file in filesToRename.Skip(1))
                {
                    this.SecondaryFiles.Add(file.Value);
                }
            }
            else
            {
                throw new Exception("Filename is not valid");
            }
        }

        public void FindSecondaryFiles(string directoryName)
        {
            if (!Directory.Exists(directoryName))
            {
                throw new Exception("Directory not found: " + directoryName);
            }

            // Only search for files if the main photo is valid
            if (this.IsFileNameValid)
            {
                foreach (string file in Directory.GetFiles(directoryName, this.FileName + ".*"))
                {
                    this.SecondaryFiles.Add(file);
                }
            }
        }

        public void Delete()
        {
            if (this.IsFileNameValid)
            {
                FileInfo currentFile = new FileInfo(this.FileFullName);

                currentFile.Delete();
            }
            else
            {
                throw new Exception("Filename is not valid");
            }
        }

        public bool IsFileNameCorrect(GenericPhotoEnums.FilenameFormats fileFormat)
        {
            return this.IsFileNameCorrect(fileFormat, string.Empty);
        }

        public bool IsFileNameCorrect(GenericPhotoEnums.FilenameFormats fileFormat, string fileNamePrefix)
        {
            // Throw Exception if date is not read
            if (this.InternalPhotoMetadata == null || this.InternalPhotoMetadata.DateTaken == new DateTime())
            {
                throw new Exception("Metadata has not been read or it is invalid");
            }

            string fileDatePart = this.InternalPhotoMetadata.DateTaken.ToString("yyyyMMdd");
            string fileSequencePart = string.Empty;

            // Calculate yyyymmddSequence
            if (fileFormat == GenericPhotoEnums.FilenameFormats.yyyymmddSequence)
            {
                // Add Prefix if one is defined
                // Return true of the date part is correct
                // TODO Should also try and validate the last part is a number
                if (string.IsNullOrEmpty(fileNamePrefix))
                {
                    return this.FileName.StartsWith(fileDatePart + "_");
                }
                else
                {
                    return this.FileName.StartsWith(fileNamePrefix + "_" + fileDatePart + "_");
                }
            }
            
            if (fileFormat == GenericPhotoEnums.FilenameFormats.yyyymmddHoursMinutesSeconds)
            {
                // Generate fileSequencePart based on HHMMss
                fileSequencePart = this.InternalPhotoMetadata.DateTaken.ToString("HHmmss");
            }
            else if (fileFormat == GenericPhotoEnums.FilenameFormats.yyyymmddSecondsSinceMidnight)
            {
                // Generate fileSequencePart based on seconds part
                // Pad to 5 digits
                fileSequencePart = this.InternalPhotoMetadata.DateTaken.TimeOfDay.TotalSeconds.ToString().PadLeft(5, '0');
            }

            // Check filename starts with the the correct pattern
            if (string.IsNullOrEmpty(fileNamePrefix))
            {
                return this.FileName.StartsWith(fileDatePart + "_" + fileSequencePart);
            }
            else
            {
                return this.FileName.StartsWith(fileNamePrefix + "_" + fileDatePart + "_" + fileSequencePart);
            }
        }

        public string RecommendedFileName(GenericPhotoEnums.FilenameFormats fileFormat)
        {
            return this.RecommendedFileName(fileFormat);
        }

        public string RecommendedFileName(GenericPhotoEnums.FilenameFormats fileFormat, string fileNamePrefix)
        {
            // Throw Exception if date is not read
            if (this.InternalPhotoMetadata == null || this.InternalPhotoMetadata.DateTaken == null || this.InternalPhotoMetadata.DateTaken == new DateTime())
            {
                throw new Exception("Metadata has not been read or it is invalid");
            }

            string fileDatePart = this.InternalPhotoMetadata.DateTaken.ToString("yyyyMMdd");
            string fileSequencePart = string.Empty;
            int fileIncrement = 0;

            // File name is blank which should fail the first Exists check
            string newFileName = string.Empty;

            // Calculate yyyymmddSequence
            if (fileFormat == GenericPhotoEnums.FilenameFormats.yyyymmddSequence)
            {
                // Generate fileSequencePart based on sequence, try looping until filename doesn't exist
                while (true)
                {
                    fileIncrement++;

                    // Add Prefix if one is defined
                    if (string.IsNullOrEmpty(fileNamePrefix))
                    {
                        newFileName = fileDatePart + "_" + fileIncrement;
                    }
                    else
                    {
                        newFileName = fileNamePrefix + "_" + fileDatePart + "_" + fileIncrement;
                    }

                    if (!File.Exists(this.FileFullName.Replace(this.FileName, newFileName)))
                    {
                        break;
                    }
                }

                return newFileName;
            }

            // Set the right SequencePart
            if (fileFormat == GenericPhotoEnums.FilenameFormats.yyyymmddHoursMinutesSeconds)
            {
                // Generate fileSequencePart based on HHMMss
                fileSequencePart = this.InternalPhotoMetadata.DateTaken.ToString("HHmmss");
            }
            else if (fileFormat == GenericPhotoEnums.FilenameFormats.yyyymmddSecondsSinceMidnight)
            {
                // Generate fileSequencePart based on seconds part
                // Pad to 5 digits
                fileSequencePart = this.InternalPhotoMetadata.DateTaken.TimeOfDay.TotalSeconds.ToString().PadLeft(5, '0');
            }

            // Generate filename and loop until no file exists with the same name
            while (true)
            {
                // Add Prefix if one is defined
                if (string.IsNullOrEmpty(fileNamePrefix))
                {
                    newFileName = fileDatePart + "_" + fileSequencePart + fileIncrement;
                }
                else
                {
                    newFileName = fileNamePrefix + "_" + fileDatePart + "_" + fileSequencePart + fileIncrement;
                }

                if (!File.Exists(this.FileFullName.Replace(this.FileName, newFileName)))
                {
                    break;
                }

                fileIncrement++;
            }

            return newFileName;
        }

        /// <summary>
        /// Sets filename, ensuring it has a full path, defaulting to the working directory
        /// </summary>
        /// <param name="fileName"></param>
        protected void SetFileName(string fileName)
        {
            // Save the filename
            this.FileFullName = this.GetFullFileName(fileName);

            // Get and save File data (using FileInfo realtime creates locks on the file)
            FileInfo file = new FileInfo(this.FileFullName);
            this.fileLastModified = file.LastWriteTime;
            this.fileExtension = file.Extension.ToLower();
            this.fileName = Regex.Replace(file.Name, this.fileExtension, string.Empty, RegexOptions.IgnoreCase);
        }

        protected string GetFullFileName(string fileName)
        {
            string fullFileName = fileName;

            // Regex for a valid file path
            Regex validPath = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");

            // Check filename includes a path
            if (!validPath.IsMatch(fullFileName))
            {
                fullFileName = Directory.GetCurrentDirectory() + "\\" + fullFileName.TrimStart('\\');
            }

            return fullFileName;
        }
    }
}
