using VintageCashCow.Application;
using VintageCashCow.WebUI.Server.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

// Configure HttpClient for ProductService with base address from configuration
builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    var baseAddress = builder.Configuration.GetValue<string>("ApiClient:BaseAddress");
    if (string.IsNullOrEmpty(baseAddress))
    {
        throw new InvalidOperationException("ApiClient:BaseAddress is not configured in appsettings.json");
    }
    client.BaseAddress = new Uri(baseAddress);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
