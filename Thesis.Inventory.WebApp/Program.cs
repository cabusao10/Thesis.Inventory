using Microsoft.OpenApi.Models;
using Thesis.Inventory.Authentication.Extensions;
using Thesis.Inventory.ItemManagement.Extensions;
using Thesis.Inventory.Infrastructure.Extensions;
using Thesis.Inventory.UserManagement.Extensions;
using Thesis.Inventory.Email.Extension;
using Thesis.Inventory.Messaging.ChatService;
using Thesis.Inventory.Messaging.Extensions;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add services to the container.
var config_sql = builder.Configuration.GetSection("SqlDatabase");
var config_token = builder.Configuration.GetSection("JsonWebTokenKeys");

//builder.Configuration.AddConfiguration(config);

builder.Services.AddInfrastructureLayer(config_sql);
builder.Services.AddUserManagementLayer();
builder.Services.AddProductManagementLayer();
builder.Services.AddEmailServiceLayer();
builder.Services.AddJWTTokenServices(config_token);
builder.Services.AddMessageLayer();

builder.Services.AddCors(x =>
{
    x.AddPolicy(name: MyAllowSpecificOrigins,
        z =>
        {
            z.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true).SetPreflightMaxAge(TimeSpan.FromSeconds(2520)); ;
        });
});

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Thesis.API", Version = "v1" });
    c.AddAuth();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.MapHub<ChatHub>("chat-hub");
app.Run();
