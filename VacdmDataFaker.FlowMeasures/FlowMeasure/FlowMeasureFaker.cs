namespace VacdmDataFaker.FlowMeasures
{
    public static partial class FlowMeasureFaker
    {
        internal static List<FlowMeasure> FlowMeasures = new();

        internal static void FakeMeasures(int count)
        {
            //Add measures until we have as many as we want
            for (int i = 0; i < count; i++)
            {
                var flowMeasure = FakeMeasure();

                FlowMeasures.Add(flowMeasure);
            }
        }
    }
}
