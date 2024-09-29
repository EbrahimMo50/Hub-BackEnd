using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using User_managment_system.Data;
using User_managment_system.Models;
using User_managment_system.Policies;
using User_managment_system.Policies.PolicyForGet;
using User_managment_system.Policies.PolicyForPost;
using User_managment_system.Repositories;
using User_managment_system.Repositories.User;
using User_managment_system.Repositories.UserTask;

//https://isriramkumarm.medium.com/create-a-custom-authorization-policy-in-asp-net-core-in-3-steps-1488b51264d0
//perfect site for walking through the custom policy

//to make a custom policy we first create a class for the requirment
//then create a handler that validates the user with our custom logic
//add the service to our services using singleton as best practice (no need for more instances one is enough)
//add the policy to the application then call it in annotation on the endpoint
//note we can add a string in the requirment it will be defined as a claim the user must have to pass the validation (claim type)

//good note to self learned while building this app that singleton service lifetime is bigger than transient thus singleton services can not depend on transient :)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(
    options=>options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Ebrahim.Mohsen\\Documents\\UserDb.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True")
    );

builder.Services.AddScoped<IUserRepo,UserRepo>();
builder.Services.AddScoped<ITaskRepo, TaskRepo>();

builder.Services.AddScoped<Auth>();
builder.Services.AddScoped<IAuthorizationHandler, GetPolicyHandler>();
builder.Services.AddScoped<IAuthorizationHandler, PostPolicyHandler>();
builder.Services.AddScoped<IAuthorizationHandler, PutPolicyHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DeletePolicyHandler>();
//swapped auth policies service type to scoped since context is defined as scoped and singleton can not depend on lower level lifetime service

builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
        var PrivateKey = config.GetSection("PrivateKey").Value;

        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    //added all the custom policies for the endpoints access 

    options.AddPolicy("GetPolicy", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new GetPermission());
    });

    options.AddPolicy("PostPolicy", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new PostPermission());
    });

    options.AddPolicy("PutPolicy", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new PutPermission());
    });

    options.AddPolicy("DeletePolicy", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new DeletePermission());
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var initializer = new InitDb(dbContext);
        initializer.Init();
    }
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
