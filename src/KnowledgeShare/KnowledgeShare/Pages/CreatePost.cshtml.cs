using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using KnowledgeShare.Core.Context;
using KnowledgeShare.Core.Entities.Content;

namespace KnowledgeShare.Pages
{
    public class CreatePostModel : PageModel
    {
        [BindProperty]
        [Required]
        public string Title { get; set; }

        [BindProperty]
        [Required]
        public string Body { get; set; }
        
        [BindProperty]
        [Required]
        public string Link { get; set; }

        [BindProperty]
        public List<TagViewModel> Tags { get; set; }

        private ITagContext _tagContext;

        private IArticleSummaryContext _articleSummaryContext;

        public CreatePostModel(
            ITagContext tagContext,
            IArticleSummaryContext articleSummaryContext)
        {
            Tags = new List<TagViewModel>();
            _tagContext = tagContext;
            _articleSummaryContext = articleSummaryContext;
        }

        public async Task OnGetAsync()
        {
            IEnumerable<string> tags = await _tagContext.GetAllTags();

            Tags = tags.Select(x => new TagViewModel()
            {
                Id = tags.ToList().IndexOf(x),
                Name = x,
                IsSelected = false
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            await _articleSummaryContext.AddAsync(ArticleSummary.Create(Title, Body, Link));
            return Page();
        }
    }
}

public class TagViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsSelected { get; set; }
}
