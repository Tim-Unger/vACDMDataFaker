using Bogus;
using System.Diagnostics;
using System.Text;
using VacdmDataFaker.Shared;

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
                var randomFilterIndex = random.Next(_filters.Count);
                var randomFilterType = _filters[randomFilterIndex];

                var randomFilter = GetRandomFilter(randomFilterType);

                randomFilters.Add(randomFilter);
            }

            randomFilters = randomFilters.DistinctBy(x => x.Type).ToList();

            //TODO make sure DEP and ARR only exist once
            //DEP or ARR are always required
            if(!randomFilters.Any(x => x.Type == "ADEP"))
            {
                var randomAirportIndex = random.Next(_airports.Count);
                var randomAirport = _airports[randomAirportIndex];

                randomFilters.Add(new Filter() { Type = "ADEP", Value = randomAirport });
            }

            if (!randomFilters.Any(x => x.Type == "ADES"))
            {
                var randomAirportIndex = random.Next(_airports.Count);
                var randomAirport = _airports[randomAirportIndex];

                randomFilters.Add(new Filter() { Type = "ADES", Value = randomAirport });
            }

            var randomIdentBuilder = new StringBuilder();

            var randomIdentNameIndex = random.Next(_idents.Count);
            var randomIdentName = _idents[randomIdentNameIndex];
            randomIdentBuilder.Append(randomIdentName);

            var randomIndentCount = random.Next(0, 9);
            var randomIndent = $"0{randomIndentCount}";
            randomIdentBuilder.Append(randomIndent);

            var randomLetterIdentifierIndex = random.Next(_letters.Count);
            var randomLetterIdentifier = _letters[randomLetterIdentifierIndex];
            randomIdentBuilder.Append(randomLetterIdentifier);
            

            var flowMeasureFaker = new Faker<FlowMeasure>()
                .RuleFor(x => x.Id, y => y.Random.Int(0, 9999))
                .RuleFor(x => x.Ident, y => randomIdentBuilder.ToString())
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

            TaskRunner.LogMessages.Add(Logger.LogInfo($"Added Measure {fakeMeasure.Ident}"));

            return fakeMeasure;
        }

        private static Filter GetRandomFilter(string filterType)
        {
            var filter = new Filter() { Type = filterType };

            var random = new Random();

            if (filterType == "ADEP" || filterType == "ADES")
            {
                var randomIcaoIndex = random.Next(_airports.Count);

                var randomIcao = _airports[randomIcaoIndex];

                filter.Value = randomIcao;

                return filter;
            }

            if(filterType == "level_above" || filterType == "level_below" || filterType == "level")
            {
                var randomLevelIndex = random.Next(_flightlevels.Count);

                var randomLevel = _flightlevels[randomLevelIndex];

                filter.Value = randomLevel;

                return filter;
            }

            if(filterType == "waypoint")
            {
                var randomWaypointIndex = random.Next(_waypoints.Count);

                var randomWaypoint = _waypoints[randomWaypointIndex];

                filter.Value = randomWaypoint;

                return filter;
            }

            throw new UnreachableException("filterType was not a value that exists");
        }
    }
}
