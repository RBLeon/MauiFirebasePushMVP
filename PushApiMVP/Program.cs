using PushApiMVP;
using Shiny.Extensions.Push;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS policy
var corsPolicyName = "AllowBlazorApp";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        builder =>
        {
            builder.WithOrigins("https://localhost:7071")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Register FirebasePushSender
builder.Services.AddSingleton<IPushSender, FirebasePushSender>();
builder.Services.AddSingleton<IDeviceTokenService, DeviceTokenService>();

//// Configure the Push service
//var appleCfg = builder.Configuration.GetSection("Push:Apple");
//var googleCfg = builder.Configuration.GetSection("Push:Google");
//builder.Services.AddPushManagement(x => x
//    .AddApple(new AppleConfiguration
//    {
//        AppBundleIdentifier = appleCfg["AppBundleIdentifier"]!,
//        TeamId = appleCfg["TeamId"]!,
//        Key = appleCfg["Key"]!,
//        KeyId = appleCfg["KeyId"]!,
//        IsProduction = appleCfg["Production"] == "true"
//    })
//    .AddGoogleFirebase(new GoogleConfiguration
//    {
//        ServerKey = googleCfg["ServerKey"]!,
//        SenderId = googleCfg["SenderId"]!,
//        DefaultChannelId = googleCfg["DefaultChannelId"]!
//    })
//    .UseFileRepository()
//    .AddShinyAndroidClickAction()
//);

var app = builder.Build();

// Use the CORS policy
app.UseCors(corsPolicyName);

app.UseSwagger();
app.UseSwaggerUI();

//app.MapPushEndpoints("push", false);
//app.RegisterEndpoints();

app.MapControllers();

app.Run();