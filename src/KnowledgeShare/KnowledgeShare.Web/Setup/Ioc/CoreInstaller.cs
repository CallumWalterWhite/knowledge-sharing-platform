using System.Reflection;
using KnowledgeShare.Core.Authentication;
using KnowledgeShare.Core.People;
using KnowledgeShare.Core.Posts;
using KnowledgeShare.Core.Posts.Types;
using KnowledgeShare.Core.Social;
using KnowledgeShare.Core.Tags;
using KnowledgeShare.Persistence;
using KnowledgeShare.Persistence.People;
using KnowledgeShare.Persistence.Posts;
using KnowledgeShare.Persistence.Social;
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
        services.AddScoped(typeof(IPostRepository<ArticlePost>), typeof(ArticlePostRepository));
        services.AddScoped(typeof(IPostRepository<BookPost>), typeof(BookPostRepository));
        services.AddScoped(typeof(IPostRepository<FreeFormPost>), typeof(FreeFormPostRepository));
        services.AddScoped(typeof(IPostRepository<Post>), typeof(PostRepository));
        services.AddScoped(typeof(ILikeRepository), typeof(LikeRepository));
        services.AddScoped(typeof(IPostCommentRepository), typeof(PostCommentRepository));
        var assembly = typeof(IGetArticlePostService).Assembly;
        AddScopedByConvention(services, assembly, x => x.Name.EndsWith("Service"));
    }

    private static void AddScopedByConvention(IServiceCollection services, Assembly assembly,
        Func<Type, bool> predicate)
    {
        var interfaces = assembly.ExportedTypes
            .Where(x => x.IsInterface && predicate(x))
            .ToList();
        var implementations = assembly.ExportedTypes
            .Where(x => x is { IsInterface: false, IsAbstract: false } && predicate(x))
            .ToList();
        foreach (var @interface in interfaces)
        {
            var implementation = implementations.FirstOrDefault(x => @interface.IsAssignableFrom(x));
            if (implementation == null) continue;
            services.AddScoped(@interface, implementation);
        }
    }
}