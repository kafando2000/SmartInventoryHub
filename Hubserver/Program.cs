using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using HubServer;
using HubServer.Services;
using System.Text.Json;
using HubServer.Hubs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddControllers()
                .AddJsonOptions(options => {
                            options.JsonSerializerOptions.UnmappedMemberHandling = 
                            System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow;
                            options.JsonSerializerOptions.AllowTrailingCommas = true;
                            options.JsonSerializerOptions.PropertyNamingPolicy = 
                            JsonNamingPolicy.SnakeCaseLower;
                });
builder.Services.AddProblemDetails();
builder.Services.AddSignalR();
builder.Services.AddDbContext<InventoryHubDbContext>();
builder.Services.AddScoped<IProductsHandler,ProductsHandler>();
builder.Services.AddScoped<ICategoriesHandler,CategoriesHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseExceptionHandler();
app.UseCors();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<CategoryHub>("/populate-categories");
app.MapGet("/weather", () =>
{
    return Results.Ok("bonjour !");
});
app.MapControllers();
app.Run();
