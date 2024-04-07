using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourNamespace; // Replace YourNamespace with the actual namespace of your NewsService

public class HomeController : Controller
{
    private readonly NewsService _newsService;

    public HomeController(NewsService newsService)
    {
        _newsService = newsService; // Injecting the NewsService dependency into the controller
    }

    // Action method for the default Index route
    public async Task<IActionResult> Index()
    {
        // Call the GetGoogleNewsTitlesAsync method to fetch news titles asynchronously
        List<(string Title, string Link)> newsTitles = await _newsService.GetGoogleNewsTitlesAsync();

        // Return the Index view passing the fetched news titles as the model
        return View("Index", newsTitles);
    }
}
