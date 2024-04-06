﻿using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace VacdmDataFaker.Vacdm
{
    internal class TaskRunner
    {
        private static bool _isInitialized = false;

        private static bool _isFirstLoad = true;

        internal static Config Config { get; set; }

        internal static async Task Run()
        {
            if (_isInitialized)
            {
                var nowErr = DateTime.UtcNow;

                Console.WriteLine(
                    $"[{nowErr:s}Z] [FATAL] TaskRunner was trying to be initialized but is already running"
                );

                throw new InvalidOperationException("TaskRunner is already running");
            }

            _isInitialized = true;

            var now = DateTime.UtcNow;

            Console.WriteLine($"[{now:s}Z] [INFO] TaskRunner initialized");

#if RELEASE
            Config = GetConfigFromEnv();
#else
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            Config =
                JsonSerializer.Deserialize<Config>(
                    File.ReadAllText($"{Environment.CurrentDirectory}/config.json"),
                    jsonOptions
                ) ?? throw new InvalidDataException();
#endif

            Console.WriteLine($"[{DateTime.UtcNow:s}Z] [INFO] Read Config");

            if(_isFirstLoad)
            {
                _isFirstLoad = false;

                await VacdmPilotFaker.DeleteAllAsync();

                Console.WriteLine(
                    $"[{now:s}Z] [INFO] First run, Deleted all old Pilots"
                );
            }

            while (true)
            {
                var nowRunner = DateTime.UtcNow;
                Console.WriteLine($"[{nowRunner:s}Z] [INFO] Running Tasks");

                await VacdmPilotFaker.RunUpdate();

                Console.WriteLine(
                    $"[{nowRunner:s}Z] [INFO] Success, updated pilots, Next update at: {nowRunner.AddMinutes(10):HH:mm}Z"
                );

                await Task.Delay(TimeSpan.FromMinutes(10));
            }
        }

        private static Config GetConfigFromEnv()
        {
            var config = new Config();

            var envCid = Environment.GetEnvironmentVariable("VACDM_CID");

            if(envCid is null)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable VACDM_CID was not provided"
                );

                throw new MissingFieldException();
            }

            if(!int.TryParse(envCid, out var envCidParsed))
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}Z] [FATAL] Variable VACDM_CID was not an int"
                );

                throw new InvalidDataException();
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

            return config;
        }
    }
}
