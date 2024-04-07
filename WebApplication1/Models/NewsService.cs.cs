using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace YourNamespace
{
    public class NewsService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpCache _httpCache;

        // Constructor to inject HttpClient and HttpCache dependencies
        public NewsService(HttpClient httpClient, HttpCache httpCache)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpCache = httpCache ?? throw new ArgumentNullException(nameof(httpCache));
        }

        // Method to fetch Google News titles asynchronously
        public async Task<List<(string Title, string Link)>> GetGoogleNewsTitlesAsync()
        {
            string rssUrl = "https://www.kore.co.il/category/all";

            // Check if the response is already cached
            if (_httpCache.TryGet(rssUrl, out string cachedContent))
            {
                return ExtractTitlesAndLinksFromHtml(cachedContent);
            }

            // Send request to the API and get the response
            HttpResponseMessage response = await _httpClient.GetAsync(rssUrl);
            if (response.IsSuccessStatusCode)
            {
                // Read the HTML content from the response
                string htmlContent = await response.Content.ReadAsStringAsync();

                // Store the response in cache
                _httpCache.Set(rssUrl, htmlContent);

                return ExtractTitlesAndLinksFromHtml(htmlContent);
            }
            else
            {
                // Handle unsuccessful response
                throw new HttpRequestException($"Failed to retrieve data from {rssUrl}. Status code: {response.StatusCode}");
            }
        }

        // Method to extract titles and links from HTML content
        private List<(string Title, string Url)> ExtractTitlesAndLinksFromHtml(string htmlContent)
        {
            var titlesAndLinks = new List<(string Title, string Url)>();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            // Select all article tags
            var articleNodes = htmlDocument.DocumentNode.SelectNodes("//article");
            if (articleNodes != null)
            {
                foreach (var articleNode in articleNodes)
                {
                    // Extract title and link
                    var titleNode = articleNode.SelectSingleNode(".//h4");
                    var linkNode = articleNode.SelectSingleNode(".//a[@aria-label]");

                    if (titleNode != null && linkNode != null)
                    {
                        string title = titleNode.InnerText.Trim();
                        string url = linkNode.GetAttributeValue("href", "");

                        titlesAndLinks.Add((title, url));
                    }
                }
            }

            return titlesAndLinks;
        }
    }
}
