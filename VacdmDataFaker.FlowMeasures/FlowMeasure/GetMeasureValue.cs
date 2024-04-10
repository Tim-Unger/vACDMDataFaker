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

            if (measureType == MeasureType.MandatoryRoute)
            {
                return "AAAAA DCT BBBBB A69 CCCCC";
            }

            if (measureType == MeasureType.Prohibit)
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
