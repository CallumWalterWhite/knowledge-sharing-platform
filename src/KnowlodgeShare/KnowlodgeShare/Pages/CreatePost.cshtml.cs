using KnowledgeShare.Core.Enitites.Tags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace KnowlodgeShare.Pages
{
    public class CreatePostModel : PageModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public List<TagViewModel> Tags { get; set; }

        private ITagContext _tagContext;

        public CreatePostModel(ITagContext tagContext)
        {
            Tags = new List<TagViewModel>();
            _tagContext = tagContext;
        }

        public async Task OnGet()
        {
            IEnumerable<string> tags = await _tagContext.GetAllTags();

            Tags = tags.Select(x => new TagViewModel()
            {
                Id = tags.ToList().IndexOf(x),
                Name = x,
                IsSelected = false
            }).ToList();
        }

        public async Task OnPost()
        {
            await _tagContext.AddAsync(new Tag(Title));
        }
    }
}

public class TagViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsSelected { get; set; }
}
