using Bogus;

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
            }
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
            };

            var flowMeasureFaker = new Faker<FlowMeasure>()
                .RuleFor(x => x.Id, y => y.Random.Int(0, 9999))
                .RuleFor(x => x.Ident, y => y.Random.String2(4).ToUpper())
                .RuleFor(x => x.Reason, y => "Capacity")
                .RuleFor(x => x.StartTime, y => y.Date.Soon())
                .RuleFor(x => x.Measure, y => measure)
                .RuleFor(x => x.Filters, y => _filters)
                .RuleFor(x => x.MeasureStatus, y => (MeasureStatus)y.Random.Int(0, 3));

            var fakeMeasure = flowMeasureFaker.Generate();

            fakeMeasure.EndTime = fakeMeasure.StartTime.AddHours(2);

            var dateTimeFaker = new Faker().Date.Soon();

            if (fakeMeasure.MeasureStatus == MeasureStatus.Withdrawn)
            {
                fakeMeasure.IsWithdrawn = true;
                fakeMeasure.WithdrawnAt = dateTimeFaker;
            }

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
    }
}
