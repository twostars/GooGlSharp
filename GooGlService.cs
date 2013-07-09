using System;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GooGlSharp
{
	public class GooGlService
	{
        private const int RequestTimeout = 30 * 1000; // 30 seconds

        public Uri Execute(string url, string key)
        {
            var request = HttpWebRequest.Create(string.Format("https://www.googleapis.com/urlshortener/v1/url?key={0}", key)) as HttpWebRequest;

            request.UserAgent = "GooGlSharp";
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Timeout = RequestTimeout;

            // Shouldn't need a full Json formatter for this.
            var json = "{\"longUrl\": \"" + url + "\"}";
            var bytes = Encoding.ASCII.GetBytes(json);
            request.ContentLength = bytes.Length;

            using (var os = request.GetRequestStream())
                os.Write(bytes, 0, bytes.Length);

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                using (var responseStream = response.GetResponseStream())
                using (var responseReader = new StreamReader(responseStream))
                {
                    json = responseReader.ReadToEnd();
                    return new Uri(Regex.Match(json, @"""id"": ?""(?<id>.+)""").Groups["id"].Value);
                }
            }
        }
	}
}