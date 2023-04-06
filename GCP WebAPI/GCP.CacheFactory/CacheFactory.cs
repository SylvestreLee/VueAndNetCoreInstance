using GCP.CacheInterface;
using GCP.Memory;
using GCP.Redis;
using GCP.Util;
using System;

namespace GCP.CacheFactory
{
    public class CacheFactory
    {
        private static ICache cache = null;
        private static readonly object lockHelper = new object();

        public static ICache Cache
        {
            get
            {
                if (cache == null)
                {
                    lock (lockHelper)
                    {
                        if (cache == null)
                        {
                            switch (GlobalContext.SystemConfig.CacheProvider)
                            {
                                case "Redis": cache = new RedisCacheImp(); break;

                                default:
                                case "Memory": cache = new MemoryCacheImp(); break;
                            }
                        }
                    }
                }
                return cache;
            }
        }
    }
}
