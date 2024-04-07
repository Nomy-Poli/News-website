// Add this code to your Startup.cs file

using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProxyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route("{*url}", Order = int.MaxValue)]
        public async Task<IActionResult> Get(string url)
        {
            var client = _httpClientFactory.CreateClient();
            var fullUrl = "https://www.kore.co.il/" + url;

            try
            {
                var response = await client.GetAsync(fullUrl);
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();
                // Load HTML content into HtmlDocument
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // Select the content inside the <article> tag
                var articleNode = doc.DocumentNode.SelectSingleNode("//article");
                var articleContent = articleNode?.InnerHtml ?? "Article not found";

                // Find the index of the specified strings
                var indices = new[]
                {
            articleContent.IndexOf("רוצים להצטרף לקבוצות הווטסאפ של כל רגע?"),
            articleContent.IndexOf("לכתבה המלאה")
        };

                // Filter out -1 (not found) indices and get the minimum (earliest) index
                var endIndex = indices.Where(index => index != -1).DefaultIfEmpty(-1).Min();

                if (endIndex != -1)
                {
                    // Extract the content up to the specified string
                    articleContent = articleContent.Substring(0, endIndex);

                    // Add a button to navigate to the original URL before the article content
                    var buttonHtml = "<br/><br/><button class=\"button\" style=\"background-color: #007bff; color: #fff; border: none; border-radius: 5px; padding: 10px 20px; cursor: pointer; text-align: center;  font-size: 16px;\" onclick=\"window.location.href='"
                        + fullUrl + "'\">Read More</button>";

                    articleContent = "<div dir=\"rtl\">"+ buttonHtml + "<br/>" + articleContent + "</div>";
                }
                else
                {
                    // Handle case where specified strings are not found
                    articleContent = "Specified strings not found in the article content.";
                }

                return Content(articleContent, "text/html");
            }
            catch (HttpRequestException)
            {
                return StatusCode(500, "Error fetching data from the server.");
            }
        }

    }
}
