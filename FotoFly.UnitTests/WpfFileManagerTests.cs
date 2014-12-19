namespace Fotofly.UnitTests
{
    using System.IO;
    using System.Linq;

    using Fotofly.BitmapMetadataTools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WpfFileManagerTests
    {
        private static readonly string FileName = TestPhotos.PhotosFolder + TestPhotos.UnitTest1;

        /// <summary>
        /// Reads the metadata from stream.
        /// </summary>
        [TestMethod]
        public void ReadMetadataFromStream()
        {
            using (var stream = File.OpenRead(FileName))
            {
                var bitmapMetadata = WpfFileManager.ReadMetadata(stream);
                Assert.IsNotNull(bitmapMetadata);
            }
        }

        /// <summary>
        /// Reads the tags from stream.
        /// </summary>
        [TestMethod]
        public void ReadTagsFromStream()
        {
            using (var stream = File.OpenRead(FileName))
            {
                var bitmapMetadata = WpfFileManager.ReadMetadata(stream);
                var metadata = PhotoMetadataTools.ReadBitmapMetadata(bitmapMetadata);
                Assert.AreEqual("Test Tag 3,Test Tag 1,Test Tag 2,Test Tag Î", string.Join(",", metadata.Tags.Select(t => t.ToString())));
            }
        }
    }
}