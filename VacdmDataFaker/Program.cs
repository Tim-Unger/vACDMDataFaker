using System.Net.Http;
using System.Text;
using System.Text.Json;
using VACDMApp.VACDMData;
using VacdmDataFaker;

var jsonOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

var config = JsonSerializer.Deserialize<Config>(
    File.ReadAllText($"{Environment.CurrentDirectory}/config.json"),
    jsonOptions
);

var client = new HttpClient();

while (true)
{
    Console.Clear();

    var vatsimPilotsRaw = await client.GetStringAsync(
        "https://data.vatsim.net/v3/vatsim-data.json"
    );

    var vatsimPilots = JsonSerializer.Deserialize<VatsimData>(vatsimPilotsRaw)!.pilots;

    var aerodromes = new string[] { "EDDF", "EDDS", "EDDK", "EDDM", "EDDL", "EDDH", "EDDB" };

    var concernedPilots = vatsimPilots
        .Where(x => x.flight_plan is not null)
        .Where(x => aerodromes.Any(y => y == x.flight_plan.departure))
        //.Where(x => x.groundspeed < 50)
        .ToList();

    var random = new Random();

    var randomIndices = new List<int>();

    for (var i = 0; i < 20; i++)
    {
        var randomNumber = random.Next(0, concernedPilots.Count - 1);
        randomIndices.Add(randomNumber);
    }

    randomIndices = randomIndices.Distinct().ToList();

    var randomPilots = randomIndices.Select(x => concernedPilots[x]).ToList();

    var fakePilots = PilotFaker.FakePilots(randomPilots.Count);

    int index = 0;

    var updatedPilots = new List<VACDMPilot>();

    foreach (var fakePilot in fakePilots)
    {
        var vatsimPilot = randomPilots[index];

        //var deptime = vatsimPilot.flight_plan.deptime;

        //(var depHour, var depMinute) = (int.Parse(deptime[..2]), int.Parse(deptime[3..]));

        var now = DateTime.UtcNow;

        //We are weighting the current Hour double to get more pilots with a possible TSAT
        var possibleHours = new[] { now.AddHours(-1).Hour, now.Hour, now.Hour, now.AddHours(1).Hour};

        var randomHour = random.Next(possibleHours.First(), possibleHours.Last());

        var randomMinute = random.Next(0, 59);


        var eobt = new DateTime(
            now.Year,
            now.Month,
            now.Day,
            randomHour,
            randomMinute,
            00,
            DateTimeKind.Utc
        );
        fakePilot.Vacdm.Eobt = eobt;

        var randomTobtOffset = random.Next(0, 5);

        var tsat = eobt.AddMinutes(randomTobtOffset);
        fakePilot.Vacdm.Tobt = eobt;
        fakePilot.Vacdm.Tsat = tsat;
        fakePilot.Vacdm.Ctot = tsat.AddMinutes(fakePilot.Vacdm.Exot);
        fakePilot.Vacdm.Ttot = tsat.AddMinutes(fakePilot.Vacdm.Exot);

        fakePilot.FlightPlan.Departure = vatsimPilot.flight_plan.departure;
        fakePilot.FlightPlan.Arrival = vatsimPilot.flight_plan.arrival;

        fakePilot.Callsign = vatsimPilot.callsign;

        fakePilot.Clearance.CurrentSquawk = vatsimPilot.transponder;

        updatedPilots.Add(fakePilot);

        index++;
    }

    var currentPilotsRaw = await client.GetStringAsync("https://vacdm.tim-u.me/api/v1/pilots");

    var currentCallsigns = JsonSerializer
        .Deserialize<List<VACDMPilot>>(currentPilotsRaw)
        .Select(x => x.Callsign);

    client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(
        config!.Cid.ToString(),
        config!.Password.ToString()
    );

    if (!currentCallsigns.Any())
    {
        Console.WriteLine("No pilots active at the moment");
    }

    int remainingCallsigns = currentCallsigns.Count();

    foreach (var currentCallsign in currentCallsigns)
    {
        //TODO Delete pilots that are not connected to Vatsim any more

        if(currentCallsigns.Count() < 15)
        {
            break;
        }

        if(remainingCallsigns > 14)
        {
            remainingCallsigns--;
            break;
        }

        var deleteUrl = $"https://vacdm.tim-u.me/api/v1/pilots/{currentCallsign}";

        var response = await client.DeleteAsync(deleteUrl);

        if (response.Content != null)
        {
            var messageRaw = await response.Content.ReadAsStringAsync();

            var message = JsonSerializer.Deserialize<ApiStatus>(messageRaw);

            Console.WriteLine($"{currentCallsign} -- Deleted, bye bye");
        }

        await Task.Delay(100);
    }

    foreach (var pilot in updatedPilots)
    {
        if (currentCallsigns.Any(x => x == pilot.Callsign))
        {
            continue;
        }

        var postUrl = $"https://vacdm.tim-u.me/api/v1/pilots";

        var contentRaw = JsonSerializer.Serialize<VACDMPilot>(pilot);

        var content = new StringContent(contentRaw, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(postUrl, content);

        if (response.Content != null)
        {
            var messageRaw = await response.Content.ReadAsStringAsync();

            var message = JsonSerializer.Deserialize<VACDMPilot>(messageRaw);

            if (message is not null)
            {
                Console.WriteLine($"{pilot.Callsign} -- Great Success");
            }
        }

        await Task.Delay(100);
    }

    Console.WriteLine("--");
    Console.WriteLine($"Next Update at: {DateTime.UtcNow.AddMinutes(10).ToLongTimeString()}Z");

    await Task.Delay(TimeSpan.FromMinutes(10));
}
