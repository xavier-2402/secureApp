using ISTA.SecureApp.Api.Middlewares;
using ISTA.SecureApp.Repositories;
using ISTA.SecureApp.Repositories.DbContext.Mongo;
using ISTA.SecureApp.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<MongoSetting>(builder.Configuration.GetSection(nameof(MongoSetting)));

builder.Services.AddSingleton<IMongoSetting>
    (d => d.GetRequiredService<IOptions<MongoSetting>>().Value);

builder.Services.AddSingleton<MongoContext>();

#region Repositories
builder.Services.AddScoped<UserRepository>();
#endregion

#region Services
builder.Services.AddScoped<UserService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<Middleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
