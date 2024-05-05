namespace VacdmDataFaker.FlowMeasures
{
    public static partial class FlowMeasureFaker
    {
        private static (MeasureType MeasureType, string MeasureTypeString) GetMeasureType(string measureTypeRaw) =>
            measureTypeRaw switch
            {
                "minimum_departure_interval" => (MeasureType.Mdi, "MDI"),
                "average_departure_interval" => (MeasureType.Adi, "ADI"),
                "per_hour" => (MeasureType.FlightsPerHour, "Flights per Hour"),
                "miles_in_trail" => (MeasureType.Mit, "Miles in Trail"),
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
