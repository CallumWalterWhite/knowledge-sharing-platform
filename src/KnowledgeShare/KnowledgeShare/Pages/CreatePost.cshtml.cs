using KnowledgeShare.Core.Enitites.Tags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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
        public List<TagViewModel> Tags { get; set; }

        private ITagContext _tagContext;

        public CreatePostModel(ITagContext tagContext)
        {
            Tags = new List<TagViewModel>();
            _tagContext = tagContext;
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
            
            await _tagContext.AddAsync(new Tag(Title));
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
