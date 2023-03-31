using dotnetopentelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Severity Code	Description	Project	File	Line	Suppression State
//Warning	CS0618	'ApplicationInsightsExtensions.AddApplicationInsightsTelemetry(IServiceCollection, string)' is obsolete: 'InstrumentationKey based global ingestion is being deprecated. Use the AddApplicationInsightsTelemetry() overload which accepts Action<ApplicationInsightsServiceOptions> and set ApplicationInsightsServiceOptions.ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.'  dotnetopentelemetry C:\Users\v-hadakushal\source\repos\dotnetopentelemetry\dotnetopentelemetry\Program.cs	5	Active

builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource(DiagnosticsConfig.ActivitySource.Name)
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter())
       .WithMetrics(metricsProviderBuilder =>
        metricsProviderBuilder
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter());


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

app.UseAuthorization();

app.MapRazorPages();

await app.RunAsync();
