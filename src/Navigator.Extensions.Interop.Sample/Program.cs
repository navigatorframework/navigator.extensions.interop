using Navigator;
using Navigator.Configuration;
using Navigator.Extensions.Interop;
using Navigator.Extensions.Interop.Sample.Actions;
using Navigator.Providers.Telegram;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", true);

// Add services to the container.
builder.Services
    .AddNavigator(options =>
    {
        options.SetWebHookBaseUrl(builder.Configuration["BASE_WEBHOOK_URL"]);
        options.RegisterActionsFromAssemblies(typeof(EchoAction).Assembly);
    })
    .WithExtension.Interop()
    .WithProvider.Telegram(options => { options.SetTelegramToken(builder.Configuration["BOT_TOKEN"]); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

// app.UseHttpsRedirection();

app.MapNavigator()
    .ForProvider.Telegram();

app.Run();