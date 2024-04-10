using Bogus;

namespace VacdmDataFaker.FlowMeasures
{
    public static partial class FlowMeasureFaker
    {
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
                .RuleFor(x => x.Ident, y => $"{y.Random.String2(4).ToUpper()}{y.Random.Int(1, 6)}{y.Random.String2(1).ToUpper()}")
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

            fakeMeasure.NotifiedFlightInformationRegions = randomNotifiedFirs.Distinct().ToArray();
            fakeMeasure.Filters = randomFilters;

            Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] Added Measure {fakeMeasure.Ident}");

            return fakeMeasure;
        }
    }
}
