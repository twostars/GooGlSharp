using System;
using System.Collections.Generic;

namespace GooGlSharp
{
	public class GooGl
	{
        private readonly GooGlService _gooGlService;
        private readonly Dictionary<string, Uri> _urlCache;

        public GooGl()
        {
            _gooGlService = new GooGlService();
            _urlCache = new Dictionary<string, Uri>();
        }

        private Uri AttemptLookup(string url, string apiKey)
        {
            var key = url;
            if (_urlCache.ContainsKey(key))
                return _urlCache[key];

            try
            {
                var shortUri = _gooGlService.Execute(url, apiKey);
                _urlCache.Add(key, shortUri);
                return shortUri;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Uri Shorten(Uri url, string apiKey)
        {
            return AttemptLookup(url.ToString(), apiKey);
        }
	}
}