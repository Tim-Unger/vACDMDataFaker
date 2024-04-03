using System;

namespace VacdmDataFaker.Vacdm
{
    public partial class VacdmPilotFaker
    {
        internal static VacdmPilot AddDataToFakePilot(Pilot vatsimPilot, VacdmPilot fakePilot)
        {
            var random = new Random();

            var now = DateTime.UtcNow;

            var possibleHours = new[]
            {
                    now.AddHours(-1).Hour,
                    now.Hour,
                    now.AddHours(1).Hour
                };

            possibleHours = possibleHours.Order().ToArray();

            var randomHour = random.Next(possibleHours.First(), possibleHours.Last());

            var randomMinute = random.Next(0, 59);

            var eobt = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                randomHour,
                randomMinute,
                00,
                DateTimeKind.Utc
            );
            fakePilot.Vacdm.Eobt = eobt;

            var randomTobtOffset = random.Next(0, 5);

            var tsat = eobt.AddMinutes(randomTobtOffset);
            fakePilot.Vacdm.Tobt = eobt;
            fakePilot.Vacdm.Tsat = tsat;
            fakePilot.Vacdm.Ctot = tsat.AddMinutes(fakePilot.Vacdm.Exot);
            fakePilot.Vacdm.Ttot = tsat.AddMinutes(fakePilot.Vacdm.Exot);

            fakePilot.FlightPlan.Departure = vatsimPilot.flight_plan.departure;
            fakePilot.FlightPlan.Arrival = vatsimPilot.flight_plan.arrival;

            fakePilot.Callsign = vatsimPilot.callsign;

            fakePilot.Clearance.CurrentSquawk = vatsimPilot.transponder;

            return fakePilot;
        }
    }
}
