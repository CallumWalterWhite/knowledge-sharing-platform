using KnowledgeShare.Core.Context;
using KnowledgeShare.Core.Entities.Content;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KnowledgeShare.Pages
{
    public class IndexModel : PageModel
    {
        public List<ArticleSummaryViewModel> ArticleSummaryViewModels { get; set; }

        private readonly ILogger<IndexModel> _logger;

        private readonly IArticleSummaryContext _articleSummaryContext;

        public IndexModel(
            ILogger<IndexModel> logger
            //IArticleSummaryContext articleSummaryContext
            )
        {
            _logger = logger;
            //_articleSummaryContext = articleSummaryContext;
        }

        public async Task OnGetAsync()
        {
            //IEnumerable<ArticleSummary> articleSummaries = await _articleSummaryContext.GetAllAsync();
            //ArticleSummaryViewModels = articleSummaries.Select(
            //    x => new ArticleSummaryViewModel()
            //    {
            //        Title = x.Title,
            //        Summary = x.Summary,
            //        Link = x.Link
            //    }
            //).ToList();
        }
    }
    
    public class ArticleSummaryViewModel
    {
        public string Title { get; set; }
    
        public string Summary { get; set; }

        public string Link { get; set; }
    }
}