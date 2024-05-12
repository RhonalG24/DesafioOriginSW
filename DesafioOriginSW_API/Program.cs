using DesafioOriginSW_API;
using DesafioOriginSW_API.Data;
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

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IBankCardRepository, BankCardRepository>();
builder.Services.AddScoped<IOperationRepository, OperationRepository>();
builder.Services.AddScoped<IOperationTypeRepository, OperationTypeRepository>();
builder.Services.AddScoped<ICardStateRepository, CardStateRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
