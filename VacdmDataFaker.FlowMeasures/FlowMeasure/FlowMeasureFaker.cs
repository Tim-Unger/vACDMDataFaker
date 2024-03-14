using Bogus;
using System.Security.Cryptography.Xml;

namespace VacdmDataFaker.FlowMeasures
{
    public static class FlowMeasureFaker
    {
        internal static List<FlowMeasure> FlowMeasures = new();

        private static readonly List<string> _measureTypes = new()
        {
            "minimum_departure_interval",
            "average_departure_interval",
            "per_hour",
            "miles_in_trail",
            "max_ias",
            "max_mach",
            "ias_reduction",
            "prohibit",
            "ground_stop",
            "mandatory_route"
        };

        private static readonly List<Filter> _filters = new()
        {
            new Filter()
            {
                Type = "ADES",
                Value = "****"
            },
            new Filter()
            {
                Type = "waypoint",
                Value = "AAAAA"
            },
            new Filter()
            {
                Type = "ADEP",
                Value = "****"
            },
            new Filter()
            {
                Type = "ADEP",
                Value = "EDDM"
            },
            new Filter()
            {
                Type = "waypoint",
                Value = "BBBBB"
            },
            new Filter()
            {
                Type = "ADES",
                Value = "EG**"
            }
        };

        private static readonly List<string> _reasons = new()
        {
            "TMA Capacity",
            "Airport Capacity",
            "Staffing",
            "ATC Capacity",
            "Event Traffic",
            "RWY Capacity",
            "Holding Capacity"
        };

        internal static void FakeMeasures(int count)
        {
            var flowMeasures = new List<FlowMeasure>();

            for (int i = 0; i < count; i++)
            {
                var flowMeasure = FakeMeasure();

                flowMeasures.Add(flowMeasure);
            }

            int index = 0;
            while (FlowMeasures.Count < 10 && index < flowMeasures.Count)
            {
                FlowMeasures.Add(flowMeasures[index]);

                index++;
            }
        }

        private static FlowMeasure FakeMeasure()
        {
            var random = new Random();

            var randomMeasureTypeIndex = random.Next(0, 10);
            var randomMeasureType = _measureTypes[randomMeasureTypeIndex];

            var measureType = GetMeasureType(randomMeasureType);

            var measure = new Measure()
            {
                TypeRaw = randomMeasureType,
                MeasureType = measureType.MeasureType,
                MeasureTypeString = measureType.MeasureTypeString,
                Value = GetRandomMeasureValue(measureType.MeasureType)
            };

            var randomNotifiedAmount = random.Next(1, 5);
            var randomNotifiedFirs = new int[randomNotifiedAmount];

            for (var i = 0; i < randomNotifiedAmount; i++)
            {
                randomNotifiedFirs[i] = random.Next(1, 15);
            }

            var randomFilterAmount = random.Next(0, 5);
            var randomFilters = new List<Filter>();

            for (var i = 0; i < randomFilterAmount; i++)
            {
                randomFilters.Add(_filters[random.Next(0, _filters.Count)]);
            }

            randomFilters = randomFilters.DistinctBy(x => x.Type).ToList();

            var flowMeasureFaker = new Faker<FlowMeasure>()
                .RuleFor(x => x.Id, y => y.Random.Int(0, 9999))
                .RuleFor(x => x.Ident, y => $"{y.Random.String2(4).ToUpper()}{y.Random.Int(1,6)}{y.Random.String2(1).ToUpper()}")
                .RuleFor(x => x.Measure, y => measure)
                .RuleFor(x => x.MeasureStatus, y => (MeasureStatus)y.Random.Int(0, 3));

            var fakeMeasure = flowMeasureFaker.Generate();

            var startTimeFaker = new Faker().Date.Soon();

            var randomEndMinutes = random.Next(30, 300);

            fakeMeasure.StartTime = $"{startTimeFaker:s}Z";
            fakeMeasure.EndTime = $"{startTimeFaker.AddMinutes(randomEndMinutes):s}Z";

            var dateTimeFaker = new Faker().Date.Soon();

            if (fakeMeasure.MeasureStatus == MeasureStatus.Withdrawn)
            {
                fakeMeasure.IsWithdrawn = true;
                fakeMeasure.WithdrawnAt = $"{dateTimeFaker:s}Z";
            }

            var randomReasonIndex = random.Next(0, _reasons.Count);
            var randomReason = _reasons[randomReasonIndex];

            fakeMeasure.Reason = randomReason;

            fakeMeasure.NotifiedFlightInformationRegions = randomNotifiedFirs;
            fakeMeasure.Filters = randomFilters;

            Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] Added Measure {fakeMeasure.Ident}");

            return fakeMeasure;
        }

        private static (MeasureType MeasureType, string MeasureTypeString) GetMeasureType(string measureTypeRaw) =>
            measureTypeRaw switch
            {
                "minimum_departure_interval" => (MeasureType.MDI, "MDI"),
                "average_departure_interval" => (MeasureType.ADI, "ADI"),
                "per_hour" => (MeasureType.FlightsPerHour, "Flights per Hour"),
                "miles_in_trail" => (MeasureType.MIT, "Miles in Trail"),
                "max_ias" => (MeasureType.MaxIas, "Max Indicated Airspeed"),
                "max_mach" => (MeasureType.MaxMach, "Max Mach Number"),
                "ias_reduction" => (MeasureType.IasReduction, "Reduce IAS by"),
                "mach_reduction" => (MeasureType.MachReduction, "Reduce Mach Number by"),
                "prohibit" => (MeasureType.Prohibit, "Prohibit"),
                "ground_stop" => (MeasureType.GroundStop, "Ground Stop"),
                "mandatory_route" => (MeasureType.MandatoryRoute, "Mandatory Route"),
                _ => throw new ArgumentOutOfRangeException(nameof(measureTypeRaw))
            };

        private static object? GetRandomMeasureValue(MeasureType measureType)
        {
            if(measureType == MeasureType.GroundStop)
            {
                return null;
            }

            if(measureType == MeasureType.MandatoryRoute)
            {
                return "AAAAA DCT BBBBB A69 CCCCC";
            }
            
            if(measureType == MeasureType.Prohibit)
            {
                return "ACTYP = A388";
            }

            var random = new Random();

            return measureType switch
            {
                MeasureType.MDI => random.Next(30, 600),
                MeasureType.ADI => random.Next(30, 300),
                MeasureType.FlightsPerHour => random.Next(2, 20),
                MeasureType.MIT => random.Next(10, 50),
                MeasureType.MaxIas => random.Next(230, 320),
                MeasureType.MaxMach => random.Next(73, 82),
                MeasureType.IasReduction => random.Next(20, 80),
                MeasureType.MachReduction => random.Next(2, 10),
                _ => throw new ArgumentOutOfRangeException(nameof(measureType))
            };
        }
    }
}
