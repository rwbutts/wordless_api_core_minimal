using WordlessApi;
using System.Reflection;

const string HTTP_VER_HEADER = "X-wordless-api-version";
const string CORS_CONFIG_PATH = "Kestrel:Cors";
const string API_CONFIG_PATH = "WordlessApi";

// get version string for http header
Version? apiVersion = Assembly.GetExecutingAssembly().GetName().Version;
string verHeaderValue = apiVersion?.ToString() ?? "unknown";

var builder = WebApplication.CreateBuilder( args );

ApiConfig ApiSettings = builder.Configuration.GetSection( API_CONFIG_PATH ).Get<ApiConfig>() ?? ApiConfig.Default; 

builder.Services.AddConfiguredCors( builder, CORS_CONFIG_PATH );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors();

if ( app.Environment.IsDevelopment() )
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

// add the version header to every response
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers[HTTP_VER_HEADER] = verHeaderValue;
        return Task.CompletedTask;
    });

    await next();
});

IWordlessApi apiService = new WordlessApi.WordlessApi();

var apiRoutes = app.MapGroup(ApiSettings.ApiRootUri);

apiRoutes.MapGet( "/healthcheck", (HttpContext context) => {

          return apiService.HealthCheck();
     }
);

apiRoutes.MapGet( "/randomword",  ( HttpContext context ) => { 

          return apiService.RandomWord();
     }
);

apiRoutes.MapGet( "/checkword/{word}",  ( HttpContext context, string word ) => {
 
          return apiService.WordExists( word );
     }
);

apiRoutes.MapGet( "/getword/{daysago}",  ( HttpContext context, int daysago ) => { 

          return apiService.TodaysWord( daysago );
     }
);

apiRoutes.MapPost( "/querymatchcount",  ( HttpContext context, QueryMatchCountRequest request) => {
     
          return apiService.CountMatches(request.answer, request.guesses );
     }
);

app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();
app.Run();

public record QueryMatchCountRequest( string answer, string[] guesses );
