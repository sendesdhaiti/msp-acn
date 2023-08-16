using Microsoft.AspNetCore.Cors;
using SimpleStore_Main;
// using REPO;
// using LOGIC;
const string MyAllowAllOrigins = "MyAllowAllOrigins";

var builder = WebApplication.CreateBuilder(args);
var config = ConfigManager.AppSetting_Dev;





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


builder.Services.AddControllers();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();
app.UseCors(MyAllowAllOrigins);

app.MapControllers();

app.Run();
