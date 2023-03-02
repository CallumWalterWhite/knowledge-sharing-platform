using KnowledgeShare.Core.Context;
using KnowledgeShare.Persistence.Content;
using KnowledgeShare.Persistence.Tags;
using KnowledgeShare.Web.Data;
using Radzen;

namespace KnowledgeShare.Web.Setup.Ioc;

public class CoreInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddScoped(typeof(ITagContext), typeof(TagContext));
        services.AddScoped(typeof(IArticleSummaryContext), typeof(ArticleSummaryContext));
        services.AddScoped<DialogService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<TooltipService>();
        services.AddScoped<ContextMenuService>();
        
        //TODO: REMOVE
        services.AddSingleton<WeatherForecastService>();
    }
}