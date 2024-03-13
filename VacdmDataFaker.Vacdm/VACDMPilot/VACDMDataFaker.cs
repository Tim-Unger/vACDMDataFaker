using Bogus;

namespace VacdmDataFaker.Vacdm
{
    public partial class PilotFaker
    {
        private static readonly string[] _waypoints = new string[]
        {
            "VRYAN",
            "IAKEQ",
            "OCSNT",
            "QIXWI",
            "GLMBJ",
            "OOGFS",
            "IPBKD",
            "HLNSL",
            "NIGFQ",
            "CTRDL",
            "LXQYJ",
            "FXAVA",
            "CIVVJ",
            "VOCEV",
            "JMXBK",
            "FXANI",
            "VTWMW",
            "BRBHZ",
            "ZGQDV",
            "KQVPD"
        };

        private static readonly string[] _designators = new string[]
        {
            "A",
            "M",
            "W",
            "L",
            "S",
            "K",
            "E",
            "D",
            "P",
            "H"
        };

        public static List<VACDMPilot> FakePilots(int listCount)
        {
            var positionFaker = new Faker<Position>()
                .RuleFor(x => x.Latitude, y => float.Parse(y.Address.Latitude().ToString()))
                .RuleFor(x => x.Longitude, y => float.Parse(y.Address.Longitude().ToString()));

            var vacdmFaker = new Faker<Vacdm>()
                .RuleFor(x => x.Exot, y => y.Random.Int(0, 20))
                .RuleFor(x => x.TaxiZone, y => "Bogus Gate")
                .RuleFor(x => x.IsTaxizoneTaxiout, y => false)
                .RuleFor(x => x.TobtState, y => "GUESS");

            var flightPlanFaker = new Faker<FlightPlan>()
                .RuleFor(x => x.FlightRules, y => "I");

            var runways = new string[] { "07", "06", "25L", "25R", "16", "34R", "03" };

            var randomizer = new Randomizer();

            var sidFaker = $"{randomizer.ArrayElement(_waypoints)}{randomizer.Int(1,9)}{randomizer.ArrayElement(_designators)}";

            var clearanceFaker = new Faker<Clearance>()
                .RuleFor(x => x.DepRwy, y => y.Random.ArrayElement(runways))
                .RuleFor(x => x.Sid, y => sidFaker)
                .RuleFor(x => x.InitialClimb, y => "5000")
                .RuleFor(x => x.AssignedSquawk, y => y.Random.Int(2001, 2110).ToString());

            var pilotFaker = new Faker<VACDMPilot>()
                .RuleFor(x => x.Position, y => positionFaker.Generate())
                .RuleFor(x => x.Vacdm, y => vacdmFaker.Generate())
                .RuleFor(x => x.FlightPlan, y => flightPlanFaker.Generate())
                .RuleFor(x => x.Clearance, y => clearanceFaker.Generate())
                .RuleFor(x => x.HasBooking, y => false)
                .RuleFor(x => x.IsInactive, y => false)
                .RuleFor(x => x.Measures, y => Array.Empty<object>())
                .RuleFor(x => x.CreatedAt, y => DateTime.UtcNow)
                .RuleFor(x => x.UpdatedAt, y => DateTime.UtcNow)
                .RuleFor(x => x.V, y => 1);

            return pilotFaker.Generate(listCount);
        }
    }
}
