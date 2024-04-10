namespace VacdmDataFaker.Vacdm
{
    public partial class VacdmPilotFaker
    {
        internal static async Task DeleteCountAsync(int count)
        {
            Client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(
                TaskRunner.Config.Cid.ToString(),
                TaskRunner.Config.Password.ToString()
            );

            var currentPilots = await GetCurrentPilots();

            int deleteCount = 0;
            for(var i = 0; i < count; i++)
            {
                var currentPilot = currentPilots.ToArray()[i];

                await DeletePilotAsync(currentPilot);

                deleteCount++;
            }

            if(deleteCount != count)
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}Z] [WARN] Tried to delete all, but only deleted {deleteCount} out of {count} pilots");
            }
        }
    }
}
