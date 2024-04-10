namespace VacdmDataFaker.FlowMeasures
{
    public partial class FlowMeasureFaker
    {
        internal static void DeleteMeasures(int? count)
        {
            var deleteCount = count ?? FlowMeasures.Count;

            if (deleteCount > FlowMeasures.Count)
            {
                FlowMeasures.Clear();
                return;
            }

            for (int i = 0; i < deleteCount; i++)
            {
                //Delete the first measure, since everything moves up after deleting the first one, we can always delete the oldest
                FlowMeasures.RemoveAt(0);
            }
        }
    }
}
