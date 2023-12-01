using DiagramEditor.Configuration;
using DiagramEditor.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Secret.json");
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddHttpContextAccessor();
builder.Services.Scan(selector => selector.FromCallingAssembly().InjectableAttributes());

builder.Services.AddControllers();

builder.Services.UseSwagger();
builder.Services.UseDistributedCache(builder.Configuration);
builder.Services.UseJwtAuthentication(builder.Configuration);

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
