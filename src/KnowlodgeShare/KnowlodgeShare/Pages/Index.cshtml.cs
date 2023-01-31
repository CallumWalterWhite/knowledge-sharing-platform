using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KnowlodgeShare.Pages
{
    public class IndexModel : PageModel
    {
        public string Test { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async void OnGet()
        {
        }
    }
}