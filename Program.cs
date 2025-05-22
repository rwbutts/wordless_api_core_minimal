using WordlessApi;
using WordlessApi.Cors;
using System.Reflection;
using WordlessApi.Config;

const string HTTP_VER_HEADER = "X-wordless-api-version";
const string CORS_CONFIG_PATH = "Kestrel:Cors";

// get version string for http header
Version? apiVersion = Assembly.GetExecutingAssembly().GetName().Version;
string verHeaderValue = apiVersion?.ToString() ?? "unknown";

var builder = ApiSettingsExtensions.CreateCustomApiBuilder( args );

builder.Services.AddScoped<IWordlessApi, WordlessApi.WordlessApi>();
builder.Services.AddConfiguredCors( builder, CORS_CONFIG_PATH );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.ConfigureApiPathBase();

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

var routeGroup = app.CreateApiRouteGroup();

routeGroup.MapGet( "/healthcheck", (HttpContext context, IWordlessApi apiService) => {

          return apiService.HealthCheck();
     }
);

routeGroup.MapGet( "/randomword",  ( HttpContext context, IWordlessApi apiService ) => { 

          return apiService.RandomWord();
     }
);

routeGroup.MapGet( "/checkword/{word}",  ( HttpContext context, IWordlessApi apiService, string word ) => {
 
          return apiService.WordExists( word );
     }
);

routeGroup.MapGet( "/getword/{daysago}",  ( HttpContext context, IWordlessApi apiService, int daysago ) => { 

          return apiService.TodaysWord( daysago );
     }
);

routeGroup.MapPost( "/querymatchcount",  ( HttpContext context, IWordlessApi apiService, QueryMatchCountRequest request) => {
     
          return apiService.CountMatches(request);
     }
);

app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();
app.Run();

