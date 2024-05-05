namespace VacdmDataFaker.Shared
{
    public static partial class ConfigReader
    {
        private static EcfmpConfig ReadEcfmp()
        {
            var config = new EcfmpConfig();

            var envUsername = Environment.GetEnvironmentVariable("ECFMP_USER");

            if (envUsername is null)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable ECFMP_USER was not provided"
                );

                throw new MissingFieldException();
            }

            config.Username = envUsername;

            var envPassword = Environment.GetEnvironmentVariable("ECFMP_PASSWORD");

            if (envPassword is null)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable ECFMP_PASSWORD was not provided"
                );

                throw new MissingFieldException();
            }

            config.Password = envPassword;

            var envInitialAmount = Environment.GetEnvironmentVariable("INITIAL_AMOUNT");

            var initialAmount = 10;

            if(envInitialAmount is not null) 
            {
                if(!int.TryParse(envInitialAmount, out var initialAmountParsed))
                {
                    Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable INITIAL_AMOUNT was not an int"
                );

                    throw new InvalidDataException();
                }

                initialAmount = initialAmountParsed;
            }

            config.InitialAmount = initialAmount;

            var envRequireAuthForLogs = Environment.GetEnvironmentVariable("REQUIRE_AUTH_FOR_LOGS");

            if (envRequireAuthForLogs is not null)
            {

                if (!bool.TryParse(envRequireAuthForLogs, out var requireAuthForLogs))
                {
                    Console.WriteLine(
                        $"[{DateTime.UtcNow:s}Z] [FATAL] Variable REQUIRE_AUTH_FOR_LOGS was not a bool"
                    );

                    throw new InvalidDataException();
                }

                config.RequireAuthentificationForLogs = requireAuthForLogs;
            }

            var envUpdateAutomcatically = Environment.GetEnvironmentVariable("UPDATE_AUTOMATICALLY");

            if (envUpdateAutomcatically is null)
            {
                config.UpdateAutomatically = true;
                config.UpdateInterval = 10;
                return config;
            }

            if (!bool.TryParse(envUpdateAutomcatically, out var shouldUpdateAutomatically))
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable UPDATE_AUTOMATICALLY was not a bool"
                );

                throw new InvalidDataException();
            }

            config.UpdateAutomatically = shouldUpdateAutomatically;

            var envUpdateInterval = Environment.GetEnvironmentVariable("UPDATE_INTERVAL_MIN");

            if (envUpdateInterval is null)
            {
                config.UpdateInterval = 10;
                return config;
            }

            if (!int.TryParse(envUpdateInterval, out var updateInterval))
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable UPDATE_INTERVAL was not an int"
                );

                throw new InvalidDataException();
            }

            config.UpdateInterval = updateInterval;

            return config;
        }
    }
}
