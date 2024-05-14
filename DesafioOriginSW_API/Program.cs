using DesafioOriginSW_API;
using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Handlers;
using DesafioOriginSW_API.Repository;
using DesafioOriginSW_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrSQLServer"), x => x.UseDateOnlyTimeOnly());
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.SetIsOriginAllowed(_ => true)
                                                .WithOrigins("+")
                                                .AllowAnyMethod()
                                                .AllowAnyHeader()
                                                .AllowCredentials()
                                                );
});
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IBankCardRepository, BankCardRepository>();
builder.Services.AddScoped<IOperationRepository, OperationRepository>();
builder.Services.AddScoped<IOperationTypeRepository, OperationTypeRepository>();
builder.Services.AddScoped<ICardStateRepository, CardStateRepository>();

builder.Services.AddScoped<IAccountHandler, AccountHandler>();
builder.Services.AddScoped<IOperationHandler, OperationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCors("AllowSpecificOrigin");
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
