using AutoMapper;
using ISTA.SecureApp.Api;
using ISTA.SecureApp.Api.Middlewares;
using ISTA.SecureApp.Repositories;
using ISTA.SecureApp.Repositories.DbContext.Mongo;
using ISTA.SecureApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = "ISTA",
                      ValidAudience = "ISTA",
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("tokenkey").Value!))
                  };
              });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.Configure<MongoSetting>(builder.Configuration.GetSection(nameof(MongoSetting)));

builder.Services.AddSingleton<IMongoSetting>
    (d => d.GetRequiredService<IOptions<MongoSetting>>().Value);

builder.Services.AddSingleton<MongoContext>();

#region Repositories
builder.Services.AddScoped<UserRepository>();
#endregion

#region Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
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
