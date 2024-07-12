using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmartSchool.WebAPI.Data;
using System.Reflection;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SmartContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Default"),
    new MySqlServerVersion(new Version(8, 0, 21)),
    mysqlOptions =>
    {
        mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var apiProviderDesciption = builder.Services.BuildServiceProvider()
                                                .GetService<IApiVersionDescriptionProvider>();

builder.Services.AddControllers()
                .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    foreach (var description in apiProviderDesciption.ApiVersionDescriptions)
    {
        options.SwaggerDoc(
            description.GroupName,
            new OpenApiInfo()
            {
                Title = "SmartScool API",
                Version = description.ApiVersion.ToString(),
                TermsOfService = new Uri("https://github.com/rfagner"),
                Description = "A descri��o da WebAPI do SmartSchool",
                License = new OpenApiLicense
                {
                    Name = "SmartSchool License",
                    Url = new Uri("https://mit.com")
                },
                Contact = new OpenApiContact
                {
                    Name = "Renildo Fagner",
                    Url = new Uri("https://www.linkedin.com/in/rfagner/")
                }
            }

        );

    }

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI(options =>
        {
            foreach (var description in apiProviderDesciption.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant()
                );
            }

            options.RoutePrefix = string.Empty;
        });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
