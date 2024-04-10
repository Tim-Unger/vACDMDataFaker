namespace VacdmDataFaker.FlowMeasures
{
    public static partial class FlowMeasureFaker
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
            for (int i = 0; i < count; i++)
            {
                var flowMeasure = FakeMeasure();

                FlowMeasures.Add(flowMeasure);
            }
        }
    }
}
