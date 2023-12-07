using DiagramEditor.Application;
using DiagramEditor.Infrastructure;
using DiagramEditor.Web.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Secret.json");
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers().AddJsonOptions();

builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

builder.Services.AddSwagger();

builder
    .Services
    .AddCors(
        options =>
            options.AddPolicy(
                "AllowAll",
                builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
            )
    );

var app = builder.Build();

/* app.UseHttpsRedirection(); */
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
