using WordlessApi;
using WordlessApi.Cors;
using WordlessApi.Config;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;

const string HTTP_VER_HEADER = "X-wordless-api-version";
const string CORS_CONFIG_PATH = "Kestrel:Cors";
const string API_CONFIG_PATH = "WordlessApi";

// get version string for http header and OpenAPI headers
string assemblyVersionString = WordlessApi.WordlessApi.GetAssemblyVersionString("0.0.0.0");

var builder = ApiSettingsExtensions.CreateCustomApiBuilder( args, API_CONFIG_PATH );

builder.Services.AddTransient<IWordlessApi, WordlessApi.WordlessApi>();
builder.Services.AddConfiguredCors( builder, CORS_CONFIG_PATH );
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Wordless API", 
            Version = assemblyVersionString,
            Description = "Backend functions for Bill's Wordless game"
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.ConfigureApiPathBase();

app.UseCors();

if ( app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}



// add the version header to every response
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers[HTTP_VER_HEADER] = assemblyVersionString;
        return Task.CompletedTask;
    });

    await next();
});

var routeGroup = app.CreateApiRouteGroup();

routeGroup.MapGet("/healthcheck",
                (HttpContext context, IWordlessApi apiService) => apiService.HealthCheck())
            .WithName("HealthCheck")
            .WithSummary("Returns TRUE if server/API are working.")
            .WithOpenApi();

routeGroup.MapGet("/randomword", (HttpContext context, IWordlessApi apiService) => apiService.RandomWord())
            .WithName("RandomWord")
            .WithSummary("Returns a random word from the dictionary.")
            .WithOpenApi();

routeGroup.MapGet( "/checkword/{word}", ( HttpContext context, IWordlessApi apiService, string word ) => apiService.WordExists( word ))
            .WithName("CheckWord")
            .WithSummary("Returns TRUE if {word} is found in the dictionary word list.")
            .WithOpenApi();

routeGroup.MapGet( "/getword/{daysago}",  ( HttpContext context, IWordlessApi apiService, int daysago ) => apiService.TodaysWord( daysago ))
            .WithName("GetWord")
            .WithSummary("Returns the random word-of-the-day for the day {daysago} in the past (zero returns today's word). Negative values return a random word.")
            .WithOpenApi();

routeGroup.MapPost("/querymatchcount", (HttpContext context, IWordlessApi apiService, QueryMatchCountRequest request) => apiService.CountMatches(request))
            .WithName("QueryMatchCount")
            .WithSummary("Given the list of guesses and the actual answer, determines each guess letter color-code and determines how many remaining dictionary words are compatible with the clues.")
            .WithOpenApi();

app.UseDefaultFiles();
app.UseStaticFiles();
app.Run();

