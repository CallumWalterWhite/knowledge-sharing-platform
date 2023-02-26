using KnowledgeShare.Core.Enitites.Tags;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KnowlodgeShare.Pages
{
    public class IndexModel : PageModel
    {
        public string Test { get; set; }

        private readonly ILogger<IndexModel> _logger;

        private readonly ITagContext _tagContext;

        public IndexModel(
            ILogger<IndexModel> logger,
            ITagContext tagContext)
        {
            _logger = logger;
            _tagContext = tagContext;
        }

        public async void OnGet()
        {
        }
    }
}