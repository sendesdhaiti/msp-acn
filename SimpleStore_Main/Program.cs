using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.FileProviders;
using SimpleStore_Main;
// using REPO;
// using LOGIC;
const string MyAllowAllOrigins = "MyAllowAllOrigins";
const string tokenScheme = nameof(tokenScheme);

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory())
//            .UseWebRoot("wwwroot");
var config = SimpleStore_Main.ConfigManager.AppSetting;
Console.ForegroundColor
            = ConsoleColor.Red;




// Add services to the container.
builder.Services.AddScoped<actions.Imsactions, actions.all.msactions>();
builder.Services.AddScoped<ACTIONS.convert.IFormConversions, ACTIONS.convert.msactions>();

builder.Services.AddScoped<LOGIC.EMAIL.IEMAIL_LOGIC, LOGIC.EMAIL.EMAIL_LOGIC>();
builder.Services.AddScoped<REPO.EMAIL.IEMAIL, REPO.EMAIL.EMAIL>();

builder.Services.AddScoped<REPO.IAccount_REPO, REPO.Account_REPO>();
builder.Services.AddScoped<LOGIC.IAccountLogic, LOGIC.AccountLogic>();

builder.Services.AddScoped<REPO.IProduct_REPO, REPO.Product_REPO>();
builder.Services.AddScoped<LOGIC.IProductLogic, LOGIC.ProductLogic>();

builder.Services.AddScoped<REPO.IOrder_REPO, REPO.Order_REPO>();
builder.Services.AddScoped<LOGIC.IOrderLogic, LOGIC.OrderLogic>();


builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowAllOrigins,
    builder =>
    {
        builder
             //.AllowAnyOrigin()
             .WithOrigins(config["ConnectionStrings:Localhost"], config["ConnectionStrings:SimpleStoreFE"])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        // .SetIsOriginAllowed(origin => true);
    });
});

builder.Services.AddAuthentication(tokenScheme)
.AddJwtBearer(tokenScheme, options =>
{
    options.Events = new JwtBearerEvents()
    {
        OnMessageReceived = (context) =>
        {
            var path = context.HttpContext.Request.Path;
            Console.WriteLine("Bearer Authentication on " + ACTIONS.all.msactions._ToString(path));
            if (path.StartsWithSegments("/Contact/portal ") || path.StartsWithSegments("/Authenitcation/portal"))
            {
                Microsoft.Extensions.Primitives.StringValues accessToken = context.Request.Query["access_token"];
                Console.WriteLine("User's access token is: ", ACTIONS.all.msactions._ToString(accessToken));

                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    var claims = new Claim[]{
                        new Claim("user_id", accessToken ),
                        new Claim("token", "token_claim"),
                      };

                    var identity = new ClaimsIdentity(claims);
                    context.Principal = new ClaimsPrincipal(identity);
                    context.Success();
                }
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    // var builder = new AuthorizationPolicyBuilder("signalr-auth-cookie");
    // builder.RequireClaim("user_id");
    // options.DefaultPolicy = builder.Build();
    options.AddPolicy("Token", policy => policy.AddAuthenticationSchemes(tokenScheme).RequireAuthenticatedUser());

    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
        JwtBearerDefaults.AuthenticationScheme);
    defaultAuthorizationPolicyBuilder =
        defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowAllOrigins);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Environment.CurrentDirectory),
    //RequestPath = new PathString("/Content")
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
