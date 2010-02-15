// <copyright file="IResolverCache.cs" company="Taasss">Copyright (c) 2009 All Right Reserved</copyright>
// <author>Ben Vincent</author>
// <date>2009-12-06</date>
// <summary>Interface to enforce consistent use of ResolverCache</summary>
namespace Fotofly.Geotagging.Resolvers
{
    public interface IResolverCache
    {
        void ConfigResolverCache(string cacheDirectory, string cacheName);
    }
}
