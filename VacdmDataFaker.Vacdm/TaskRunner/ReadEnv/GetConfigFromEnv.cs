namespace VacdmDataFaker.Vacdm
{
    internal static class ReadConfig
    {
        private static readonly List<int> _devCids =
            new()
            {
                10000000,
                10000001,
                10000002,
                10000003,
                10000004,
                10000005,
                10000006,
                10000007,
                10000008,
                10000009,
                100000010
            };

        internal static Config FromEnv()
        {
            var config = new Config();

            var envCid = Environment.GetEnvironmentVariable("VACDM_CID");

            if (envCid is null)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable VACDM_CID was not provided"
                );

                throw new MissingFieldException();
            }

            if (!int.TryParse(envCid, out var envCidParsed))
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable VACDM_CID was not an int"
                );

                throw new InvalidDataException();
            }

            if (!_devCids.Any(x => x == envCidParsed))
            {
                Console.WriteLine(
                    $"\r\n[{DateTime.UtcNow:s}Z] [WARN] VACDM_CID is not any of the DEV-CIDs. This program should not be run on a Prod Server\r\n"
                );
            }

            config.Cid = envCidParsed;

            var envPassword = Environment.GetEnvironmentVariable("VACDM_PASSWORD");

            if (envPassword is null)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable VACDM_PASSWORD was not provided"
                );

                throw new MissingFieldException();
            }

            config.Password = envPassword;

            var envMinimumAmount = Environment.GetEnvironmentVariable("MINIMUM_AMOUNT");

            var minimumAmount = 10;

            if (envMinimumAmount is not null)
            {
                if (!int.TryParse(envMinimumAmount, out var mimimumAmountParsed))
                {
                    Console.WriteLine(
                        $"[{DateTime.UtcNow:s}Z] [FATAL] Variable MIMIMUM_AMOUNT was not an int"
                    );

                    throw new InvalidDataException();
                }

                if (mimimumAmountParsed < 0)
                {
                    Console.WriteLine(
                        $"[{DateTime.UtcNow:s}Z] [FATAL] Variable MIMIMUM_AMOUNT was smaller than 0"
                    );

                    throw new InvalidDataException();
                }

                minimumAmount = mimimumAmountParsed;
            }

            config.MinimumAmount = minimumAmount;

            var envMaximumAmount = Environment.GetEnvironmentVariable("MAXIMUM_AMOUNT");

            var maximumAmount = 50;

            if (envMaximumAmount is not null)
            {
                if (!int.TryParse(envMaximumAmount, out var maximumAmountParsed))
                {
                    Console.WriteLine(
                        $"[{DateTime.UtcNow:s}Z] [FATAL] Variable MAXIMUM_AMOUNT was not an int"
                    );

                    throw new InvalidDataException();
                }

                if (maximumAmountParsed < minimumAmount)
                {
                    Console.WriteLine(
                        $"[{DateTime.UtcNow:s}Z] [FATAL] MAXIMUM_AMOUNT is smaller than MINIMUM_AMOUNT"
                    );

                    throw new InvalidDataException();
                }

                maximumAmount = maximumAmountParsed;
            }

            config.MaximumAmount = maximumAmount;

            var envUpdateAutomcatically = Environment.GetEnvironmentVariable(
                "UPDATE_AUTOMATICALLY"
            );

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

            var envDeleteOnStartup = Environment.GetEnvironmentVariable("DELETE_ALL_ON_STARTUP");

            var deleteAllOnStartup = true;

            if (envDeleteOnStartup is not null)
            {
                if(!bool.TryParse(envDeleteOnStartup, out var deleteAllOnStartupParsed))
                {
                    Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable DELETE_ALL_ON_STARTUP was not a bool"
                );

                    throw new InvalidDataException();
                }

                deleteAllOnStartup = deleteAllOnStartupParsed;
            }

            config.DeleteAllOnStartup = deleteAllOnStartup;

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
