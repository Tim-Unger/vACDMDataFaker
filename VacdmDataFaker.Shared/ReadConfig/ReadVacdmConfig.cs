namespace VacdmDataFaker.Shared
{
    public static partial class ConfigReader
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

        private static VacdmConfig ReadVacdm()
        {
            var config = new VacdmConfig();

            var envCid = Environment.GetEnvironmentVariable("");

            if (envCid is null)
            {
                LogVariableInvalidType("VACDM_CID");

                throw new MissingFieldException();
            }

            if (!int.TryParse(envCid, out var envCidParsed))
            {
                LogVariableInvalidType("VACDM_CID");

                throw new InvalidDataException();
            }

            config.Cid = envCidParsed;

            var envPassword = Environment.GetEnvironmentVariable("VACDM_PASSWORD");

            if (envPassword is null)
            {
                LogVariableInvalidType("VACDM_PASSWORD");

                throw new MissingFieldException();
            }

            config.Password = envPassword;

            var envMinimumAmount = Environment.GetEnvironmentVariable("MINIMUM_AMOUNT");

            var minimumAmount = 10;

            if (envMinimumAmount is not null)
            {
                if (!int.TryParse(envMinimumAmount, out var mimimumAmountParsed))
                {
                    LogVariableInvalidType("MIMIMUM_AMOUNT");

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
                    LogVariableInvalidType("MAXIMUM_AMOUNT");

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

            var envRequireAuthForLogs = Environment.GetEnvironmentVariable("REQUIRE_AUTH_FOR_LOGS");

            if (envRequireAuthForLogs is not null)
            {
                if (!bool.TryParse(envRequireAuthForLogs, out var requireAuth))
                {
                    LogVariableInvalidType("REQUIRE_AUTH_FOR_LOGS");
                }

                config.RequireAuthenticationForLogs = requireAuth;
            }

            var envAllowNonDevCids = Environment.GetEnvironmentVariable("ALLOW_NON_DEV_CIDS");

            var allowNonDevCids = false;

            if (envAllowNonDevCids is not null)
            {
                if (!bool.TryParse(envAllowNonDevCids, out allowNonDevCids))
                {
                    LogVariableInvalidType("ALLOW_NON_DEV_CIDS");

                    throw new InvalidDataException();
                }

                config.AllowNonDevCids = allowNonDevCids;
            }

            if (!allowNonDevCids && !_devCids.Any(x => x == envCidParsed))
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Program is not allowed to run with a non-dev CID"
                );
                throw new InvalidDataException();
            }

            var envAirports = Environment.GetEnvironmentVariable("AIRPORTS");

            if (envAirports is not null)
            {
                var splitAirports = envAirports.Split(',');

                var formattedAirports = new List<string>();

                var allIcaosRaw = File.ReadAllText(
                    $"{Environment.CurrentDirectory}/Data/airports.txt"
                );

                //Only Airports that have an ICAO with the same criteria as below
                var allIcaosList = allIcaosRaw
                    .Split("\r\n")
                    .Where(x => x.Length == 4)
                    .Where(x => x.All(y => char.IsLetter(y)))
                    .ToList();

                foreach (var airport in splitAirports)
                {
                    var airportFormatted = airport.ToUpper().Trim();

                    //Check if the string is a valid ICAO
                    if (
                        airportFormatted.Length != 4 //4 Chars long
                        || airportFormatted.ToCharArray().Any(x => !char.IsLetter(x)) //only Letters allowed
                        || !allIcaosList.Any(x => x == airportFormatted) //has to be an actual real ICAO Code
                    )
                    {
                        //NORELEASE Log all throws
                        Console.WriteLine(
                            $"[{DateTime.UtcNow:s}Z] [FATAL] {airportFormatted} was not a valid Airport-ICAO"
                        );
                        throw new InvalidDataException();
                    }
                }

                config.Airports = formattedAirports.Distinct().ToList();
            }

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
                LogVariableInvalidType("UPDATE_AUTOMATICALLY");

                throw new InvalidDataException();
            }

            config.UpdateAutomatically = shouldUpdateAutomatically;

            var envDeleteOnStartup = Environment.GetEnvironmentVariable("DELETE_ALL_ON_STARTUP");

            var deleteAllOnStartup = true;

            if (envDeleteOnStartup is not null)
            {
                if (!bool.TryParse(envDeleteOnStartup, out var deleteAllOnStartupParsed))
                {
                    LogVariableInvalidType("DELETE_ALL_ON_STARTUP");

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
                LogVariableInvalidType("UPDATE_INTERVAL");

                throw new InvalidDataException();
            }

            config.UpdateInterval = updateInterval;

            return config;
        }

        private static void LogVariableInvalidType(string envVariable) =>
            Console.WriteLine(
                $"[{DateTime.UtcNow:s}Z] [FATAL] Variable {envVariable.ToUpper()} was not an int"
            );
    }
}
