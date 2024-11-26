#region OpenPLZ API - Copyright (c) STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (c) STÜBER SYSTEMS GmbH
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU Affero General Public License, version 3,
 *    as published by the Free Software Foundation.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *    GNU Affero General Public License for more details.
 *
 *    You should have received a copy of the GNU Affero General Public License
 *    along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 */
#endregion

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using OpenPlzApi;
using OpenPlzApi.DataLayer;
using System.Collections;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration
var appConfiguration = builder.Configuration.Get<AppConfiguration>();

// Enable cross-origin resource sharing 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .WithMethods(WebRequestMethods.Http.Get)
              .WithHeaders(HeaderNames.Accept);
    });
});

// Add controller support
builder.Services
    .AddControllers(setup =>
    {
        setup.OutputFormatters.Add(new CsvOutputFormatter());
    })
    .AddJsonOptions(setup =>
    {
        setup.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        setup.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        setup.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { (JsonTypeInfo type_info) =>
                {
                    foreach (var property in type_info.Properties)
                    {
                        if (typeof(ICollection).IsAssignableFrom(property.PropertyType))
                        {
                            property.ShouldSerialize = (_, val) => val is ICollection collection && collection.Count > 0;
                        }
                    }
                }
            }
        };
    });

// Exception handling
builder.Services.AddProblemDetails();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDateOnlyTimeOnlyStringConverters();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "OpenPLZ API v1",
            Version = "v1",
            Description = "Open Data API for street and postal code directories of Germany, Austria, Switzerland and Liechtenstein",
            Contact = new OpenApiContact
            {
                Name = "The OpenPLZ API Project",
                Url = new Uri("https://www.openplzapi.org")
            },
            License = new OpenApiLicense
            {
                Name = "License",
                Url = new Uri("https://github.com/openpotato/openplzapi/blob/main/LICENSE")
            }
        });
    setup.EnableAnnotations();
    setup.CustomSchemaIds(x => x.Namespace.StartsWith("OpenPlzApi") ? x.FullName : x.Name);
    setup.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "OpenPlzApi.WebService.xml"));
    setup.OrderActionsBy((apiDesc) => apiDesc.RelativePath);
});

// Add EF Core support
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.BuildDbContextOptions(appConfiguration.Database);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
    app.UseStatusCodePages();
    app.UseExceptionHandler();
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "OpenPLZ API";
    options.SwaggerEndpoint("v1/swagger.json", "OpenPLZ API v1");
    
    // Sorting of controllers
    options.ConfigObject.AdditionalItems["tagsSorter"] = "alpha";
});

app.UseCors();
app.MapControllers();
app.Run();
