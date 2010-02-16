// <copyright file="BaseProvider.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2010-02-15</date>
// <summary>BaseProvider Abstract Class</summary>
namespace Fotofly.MetadataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Imaging;

    public abstract class BaseProvider : IDisposable
    {
        public BaseProvider(BitmapMetadata bitmapMetadata)
        {
            this.BitmapMetadata = bitmapMetadata;
        }

        protected BitmapMetadata BitmapMetadata
        {
            get;
            set;
        }

        public void Dispose()
        {
            this.Dispose(true);

            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Force Garbage ObjCollection
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
