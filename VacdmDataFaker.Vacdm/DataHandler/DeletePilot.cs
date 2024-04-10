namespace VacdmDataFaker.Vacdm
{
    public partial class VacdmPilotFaker
    {
        internal static async Task DeletePilotAsync(string callsign)
        {
            var url = TaskRunner.Config.Url;

            var deleteUrl = $"{url}/{callsign}";

            var response = await Client.DeleteAsync(deleteUrl);

            if (response.Content != null)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [INFO] Deleted fake pilot {callsign}"
                );
            }
        }
    }
}
