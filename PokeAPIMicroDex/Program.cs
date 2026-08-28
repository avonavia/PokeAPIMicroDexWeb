using DNBigHelpfulLib;
using MudBlazor.Services;
using Poke.Redis;
using PokeAPIAgent;
using PokeAPIMicroDex.Components;
using PokeApiNet;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

ILogger mainLogger = SimpleLogger.InitLogger();

builder.Services.AddSingleton<PokeApiClient>();
builder.Services.AddSingleton(mainLogger);

builder.Services.AddSingleton<Connection>(sp => 
{
    var client = sp.GetRequiredService<PokeApiClient>();
    var logger = sp.GetRequiredService<ILogger>();
    return new Connection(client, logger);
});

builder.Services.AddSingleton<RedisAgent>();

builder.Services.AddSingleton<PokemonCardRedisRepository>(pcr => 
{
    var agent = pcr.GetRequiredService<RedisAgent>();
    var logger = pcr.GetRequiredService<ILogger>();
    return new PokemonCardRedisRepository(agent, logger);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Urls.Add("http://0.0.0.0:5279");
app.Run();
