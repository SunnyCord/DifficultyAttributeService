using DifficultyAttributeService.Repositories;
using DifficultyAttributeService.Services;
using osu.Game.Beatmaps;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDatabase>(provider =>
{
    var redis =  ConnectionMultiplexer.Connect("localhost", options =>
    {
        options.AbortOnConnectFail = false;
    });
    return redis.GetDatabase();
});

builder.Services.AddScoped<RepositoryBase<Beatmap>, BeatmapRepository>();
    
builder.Services.AddScoped<BeatmapService>();

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
