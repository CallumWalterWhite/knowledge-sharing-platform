using KnowledgeShare.Web.Setup.Auth;
using KnowledgeShare.Web.Setup.Ioc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
AuthenticationInstaller.Install(builder);
CoreInstaller.Install(builder.Services);
GraphInstaller.Install(builder.Services);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
