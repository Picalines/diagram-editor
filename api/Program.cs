using DiagramEditor.Configuration;
using DiagramEditor.Database;
using DiagramEditor.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json");

builder.Services.Scan(selector => selector.FromCallingAssembly().InjectableAttributes());

builder.Services.AddControllers();

builder.Services.UseSwagger();
builder.Services.UseDistributedCache(builder.Configuration);
builder.Services.UseJwtAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/* app.UseHttpsRedirection(); */
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.MapControllers();

var database = app.Services.GetRequiredService<ApplicationContext>().Database;
database.EnsureCreated();

app.Run();
