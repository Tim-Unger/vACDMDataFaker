using System.Text.Json;
using VacdmDataFaker;

var jsonOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

var config = JsonSerializer.Deserialize<Config>($"{Environment.CurrentDirectory}/config.json");

var client = 
