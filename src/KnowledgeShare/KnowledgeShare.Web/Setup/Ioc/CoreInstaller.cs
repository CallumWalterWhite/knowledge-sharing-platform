using KnowledgeShare.Core.Context;
using KnowledgeShare.Core.Context.Posts;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using KnowledgeShare.Persistence.Posts;
using KnowledgeShare.Persistence.Tags;
using Radzen;

namespace KnowledgeShare.Web.Setup.Ioc;

public class CoreInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddScoped(typeof(ITagContext), typeof(TagContext));
        services.AddScoped<DialogService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<TooltipService>();
        services.AddScoped<ContextMenuService>();
        services.AddScoped(typeof(ICreatePostService), typeof(CreatePostService));
        services.AddScoped(typeof(IGetAllTagsService), typeof(GetAllTagsService));
        services.AddScoped(typeof(ISearchPostService), typeof(SearchPostService));
        services.AddScoped(typeof(IPostContextProvider), typeof(PostContextProvider));
        services.AddScoped(typeof(IPostFactory), typeof(PostFactory));
        services.AddScoped(typeof(ISearchPostQuery), typeof(SearchPostQuery));
        
        services.AddScoped(typeof(IPostContextProvider), typeof(PostContextProvider));
        services.AddScoped(typeof(IPostContext<ArticlePost>), typeof(ArticlePostContext<ArticlePost>));
        services.AddScoped(typeof(IPostContext<BookPost>), typeof(BookPostContext<BookPost>));
    }
}