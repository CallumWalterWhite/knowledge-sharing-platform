using KnowledgeShare.Core.Context;
using KnowledgeShare.Persistence.Content;
using KnowledgeShare.Persistence.Tags;
using KnowledgeShare.Web.Data;
using Radzen;

namespace KnowledgeShare.Web.Setup.Ioc;

public class CoreInstaller
{
    public static void Install(IServiceCollection service)
    {
        service.AddScoped(typeof(ITagContext), typeof(TagContext));
        service.AddScoped(typeof(IArticleSummaryContext), typeof(ArticleSummaryContext));
        service.AddScoped<DialogService>();
        service.AddScoped<NotificationService>();
        service.AddScoped<TooltipService>();
        service.AddScoped<ContextMenuService>();
        
        //TODO: REMOVE
        service.AddSingleton<WeatherForecastService>();
    }
}