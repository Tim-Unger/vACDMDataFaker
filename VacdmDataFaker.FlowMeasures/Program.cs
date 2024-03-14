using VacdmDataFaker.FlowMeasures;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.WebHost.UseUrls("http://localhost:6000");

builder.Services.AddControllersWithViews(options =>
{
    options.AllowEmptyInputInBodyModelBinding = true;
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Start();

await TaskRunner.Run();
