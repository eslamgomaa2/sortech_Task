using Countriestask.Repository.Blockcountriesrepo;
using Countriestask.Repository.logAttemptRepo;
using Countriestask.Services.BlockCountry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


builder.Services.AddControllers();
builder.Services.AddSingleton<IBlockedCountryRepo, BlockedCountryRepo>();
builder.Services.AddScoped<IBlockCountryService, BlockCountryService>();
builder.Services.AddSingleton<ILogRepo,LogRepo>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
