// <copyright file="GenericPhotoFile.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-11-04</date>
// <summary>GenericPhotoFile Abstract Class</summary>
namespace FotoFly
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public abstract class GenericPhotoFile
    {
        public bool HandleExceptions
        {
            get;
            set;
        }

        public string FileName
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

        public string FileExtension
        {
            get
            {
                if (this.IsFileValid)
                {
                    return new FileInfo(this.FileName).Extension.ToLower();
                }
                else
                {
                    throw new Exception("Filename is not valid");
                }
            }
        }

        public bool IsFileValid
        {
            get
            {
                return !string.IsNullOrEmpty(this.FileName) && File.Exists(this.FileName);
            }
        }

        protected PhotoMetadata PhotoMetadata
        {
            get;
            set;
        }

        protected FotoFlyMetadata FotoFlyMetadata
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
            if (this.IsFileValid)
            {
                FileInfo currentFile = new FileInfo(this.FileName);
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

        public void RenameFile(string newFileName)
        {
            this.RenameFile(newFileName, false);
        }

        public void RenameFile(string newFileName, bool overWriteExistingFile)
        {
            if (this.IsFileValid)
            {
                FileInfo currentFile = new FileInfo(this.FileName);

                newFileName = currentFile.DirectoryName + newFileName;

                if (File.Exists(newFileName) && overWriteExistingFile)
                {
                    File.Delete(newFileName);
                }

                currentFile.MoveTo(newFileName);
            }
            else
            {
                throw new Exception("Filename is not valid");
            }
        }

        public void Delete()
        {
            if (this.IsFileValid)
            {
                FileInfo currentFile = new FileInfo(this.FileName);

                currentFile.Delete();
            }
            else
            {
                throw new Exception("Filename is not valid");
            }
        }

        public string RecommendedFileName(GenericPhotoEnums.FilenameFormats fileFormat)
        {
            return this.RecommendedFileName(fileFormat);
        }

        public string RecommendedFileName(GenericPhotoEnums.FilenameFormats fileFormat, string fileNamePrefix)
        {
            // Throw Exception if date is not read
            if (this.PhotoMetadata == null || this.PhotoMetadata.DateTaken == new DateTime())
            {
                throw new Exception("Metadata has not been read or it is invalid");
            }

            string fileDatePart = this.PhotoMetadata.DateTaken.ToString("yyyymmdd");
            string fileSequencePart = string.Empty;
            string newFileName = string.Empty;

            // Set the right SequencePart
            if (fileFormat == GenericPhotoEnums.FilenameFormats.yyyymmddHoursMinutesSeconds)
            {
                // Generate fileSequencePart based on HHMMss
                fileSequencePart = this.PhotoMetadata.DateTaken.ToString("HHmmss");
            }
            else if (fileFormat == GenericPhotoEnums.FilenameFormats.yyyymmddSecondsSinceMidnight)
            {
                // Generate fileSequencePart based on seconds part
                fileSequencePart = this.PhotoMetadata.DateTaken.TimeOfDay.TotalSeconds.ToString();
            }
            else if (fileFormat == GenericPhotoEnums.FilenameFormats.yyyymmddSequence)
            {
                int increment = 0;

                // Generate fileSequencePart based on sequence, try looping until filename doesn't exist
                while (File.Exists(newFileName))
                {
                    increment++;
                }
                
                fileSequencePart = increment.ToString();

                newFileName = fileNamePrefix + "_" + fileDatePart + "_" + increment + this.FileExtension;
            }

            // Add Prefix if one is defined
            if (string.IsNullOrEmpty(fileNamePrefix))
            {
                newFileName = fileDatePart + "_" + fileSequencePart + this.FileExtension;
            }
            else
            {
                newFileName = fileNamePrefix + "_" + fileDatePart + "_" + fileSequencePart + this.FileExtension;
            }

            return newFileName;
        }
    }
}
