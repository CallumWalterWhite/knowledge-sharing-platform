using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KnowlodgeShare.Pages
{
    public class FeedModel : PageModel
    {
        private readonly ILogger<FeedModel> _logger;

        public FeedModel(ILogger<FeedModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}