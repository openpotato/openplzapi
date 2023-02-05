#region OpenPLZ API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
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
using Microsoft.OpenApi.Models;
using OpenPlzApi;
using OpenPlzApi.DataLayer;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration
var appConfiguration = builder.Configuration.Get<AppConfiguration>();

// Enable cross-origin resource sharing 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET");
    });
});

// Add controller support
builder.Services
    .AddControllers()
    .AddJsonOptions(setup =>
    {
        setup.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        setup.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "OpenPLZ API v1",
            Version = "v1",
            Description = "Open Data API for street and postal code directories of Germany, Austria and Switzerland",
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
    setup.CustomSchemaIds(x => x.FullName);
    setup.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "OpenPlzApi.WebService.xml"));
    setup.OrderActionsBy((apiDesc) => apiDesc.RelativePath);
    setup.UseDateOnlyTimeOnlyStringConverters();
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
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "OpenPLZ API";
    options.SwaggerEndpoint("v1/swagger.json", "OpenPLZ API v1");
});

app.UseCors();
app.MapControllers();
app.Run();
