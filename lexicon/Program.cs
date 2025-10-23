using lexicon.Components;
using dotenv.net;
using lexicon.Components.Data;
using lexicon.Components.Services;


var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

var apiKey = Environment.GetEnvironmentVariable("DL_KEY");
var mongoUri = Environment.GetEnvironmentVariable("MONGO_DB_URI");


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<DetectLanguageService>(sp => {
    return new DetectLanguageService(apiKey);
});

builder.Services.AddSingleton<ITranslationRepo>(sp => {
    return new MongoTranslationRepo(mongoUri);
});

builder.Services.AddScoped<TranslationService>(sp => {
    var repo = sp.GetRequiredService<ITranslationRepo>();
    return new TranslationService(repo);
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