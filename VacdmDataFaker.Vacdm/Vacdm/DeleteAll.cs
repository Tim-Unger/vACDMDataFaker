namespace VacdmDataFaker.Vacdm
{
    public partial class VacdmPilotFaker
    {
        internal static async Task DeleteAllAsync()
        {
            Client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(
                TaskRunner.Config.Cid.ToString(),
                TaskRunner.Config.Password.ToString()
            );

            var currentPilots = await GetCurrentPilots();

            int deleteCount = 0;

            foreach ( var pilot in currentPilots )
            {
                await DeletePilotAsync(pilot);
                
                deleteCount++;
            }

            if(deleteCount != currentPilots.Count())
            {
                Console.WriteLine($"[{DateTime.UtcNow:s}Z] [WARN] Tried to delete all, but only deleted {deleteCount} out of {currentPilots.Count()} pilots");
            }

            Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] Deleted all ({deleteCount}) pilots");
        }
    }
}
