using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.Persons;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Tags;
using KnowledgeShare.Persistence;
using KnowledgeShare.Persistence.Persons;
using KnowledgeShare.Persistence.Posts;
using KnowledgeShare.Persistence.Tags;
using Radzen;

namespace KnowledgeShare.Web.Setup.Ioc;

public class CoreInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddScoped(typeof(ITagRepository), typeof(TagRepository));
        services.AddScoped<DialogService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<TooltipService>();
        services.AddScoped<ContextMenuService>();
        services.AddScoped(typeof(ICreatePostService), typeof(CreatePostService));
        services.AddScoped(typeof(IGetAllTagsService), typeof(GetAllTagsService));
        services.AddScoped(typeof(ISearchPostService), typeof(SearchPostService));
        services.AddScoped(typeof(IPostRepositoryProvider), typeof(PostRepositoryProvider));
        services.AddScoped(typeof(IPostFactory), typeof(PostFactory));
        services.AddScoped(typeof(ISearchPostQuery), typeof(SearchPostQuery));
        services.AddScoped(typeof(ICurrentAuthUser), typeof(CurrentAuthUser));
        
        services.AddScoped(typeof(IPersonService), typeof(PersonService));
        services.AddScoped(typeof(IPersonRepository), typeof(PersonRepository));
        services.AddScoped(typeof(INeo4jDataAccess), typeof(Neo4jDataAccess));

        services.AddScoped(typeof(IPostRepositoryProvider), typeof(PostRepositoryProvider));
        services.AddScoped(typeof(IPostRepository<ArticlePost>), typeof(ArticlePostRepository<ArticlePost>));
        services.AddScoped(typeof(IPostRepository<BookPost>), typeof(BookPostRepository<BookPost>));
    }
}