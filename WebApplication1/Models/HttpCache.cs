using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace YourNamespace
{

    public class HttpCache
    {
        private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();

        public bool TryGet(string key, out string value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void Set(string key, string value)
        {
            _cache[key] = value;
        }
    }
}