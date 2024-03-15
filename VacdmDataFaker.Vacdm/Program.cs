using VacdmDataFaker.Vacdm;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

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