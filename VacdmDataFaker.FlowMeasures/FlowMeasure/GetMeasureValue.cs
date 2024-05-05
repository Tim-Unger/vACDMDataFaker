namespace VacdmDataFaker.FlowMeasures
{
    public static partial class FlowMeasureFaker
    {
        private static object? GetRandomMeasureValue(MeasureType measureType)
        {
            if (measureType == MeasureType.GroundStop)
            {
                return null;
            }

            var random = new Random();

            if (measureType == MeasureType.MandatoryRoute)
            {
                var randomIndex = random.Next(_routes.Count);

                return _routes[randomIndex];
            }

            if (measureType == MeasureType.Prohibit)
            {
                var randomIndex = random.Next(_aircraftTypes.Count);

                return _aircraftTypes[randomIndex];
            }

            return measureType switch
            {
                MeasureType.Mdi => GetRandomValue(_mdiAdiValues),
                MeasureType.Adi => GetRandomValue(_mdiAdiValues),
                MeasureType.FlightsPerHour => GetRandomValue(_fphValues),
                MeasureType.Mit => GetRandomValue(_mitValues),
                MeasureType.MaxIas => GetRandomValue(_iasValues),
                MeasureType.MaxMach => GetRandomValue(_machValues),
                MeasureType.IasReduction => GetRandomValue(_iasRedValues),
                MeasureType.MachReduction => GetRandomValue(_machRedValues),
                _ => throw new ArgumentOutOfRangeException(nameof(measureType))
            } ;
        }

        private static double GetRandomValue(List<double> values)
        {
            var random = new Random();

            var randomIndex = random.Next(values.Count);

            return values[randomIndex];
        }

        private static int GetRandomValue(List<int> values)
        {
            var random = new Random();

            var randomIndex = random.Next(values.Count);

            return values[randomIndex];
        }
    }
}
