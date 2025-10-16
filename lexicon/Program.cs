using lexicon.Components;
using dotenv.net;
using lexicon.Components.Services;


var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

var apiKey = Environment.GetEnvironmentVariable("DL_KEY");

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<DetectLanguageService>(sp =>
{
    return new DetectLanguageService(apiKey);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();