using System;
using System.Collections.Generic;
using System.Linq;

namespace PatternPA.Core.Model.Nhanes
{
    public class NhanesPersonActivityStatsGenerator
    {
        public const int MINUTES_IN_24HOURS = 60 * 24;

        public NhanesPersonActivityStats Generate(IEnumerable<NhanesRecord> records)
        {
            var stats = new NhanesPersonActivityStats();

            //time values counts

            var result = from r in records
                         group r by r.IntencityCode into intensityGroup
                         select new
                                    {
                                        IntensityCode = intensityGroup.Key,
                                        Count = intensityGroup.Count(),
                                    };

            foreach (var row in result)
            {
                switch (row.IntensityCode)
                {
                    case IntensityCodes.Sedentary:
                        stats.TotalSedentaryActivityMins = row.Count;
                        break;
                    case IntensityCodes.Low:
                        stats.TotalLowActivityActivityMins = row.Count;
                        break;
                    case IntensityCodes.Light:
                        stats.TotalLightActivityMins = row.Count;
                        break;
                    case IntensityCodes.Moderate:
                        stats.TotalModerateActivityMins = row.Count;
                        break;
                    case IntensityCodes.Vigorous:
                        stats.TotalVigorousActivityMins = row.Count;
                        break;
                    case IntensityCodes.ExtraVigorous:
                        stats.TotalExtraVigorousActivityMins = row.Count;
                        break;
                    default:
                        throw new NotSupportedException(
                            "intensity is not supported: " + row.IntensityCode);
                }
            }

            int totalTime = records.Count();
            stats.TotalDurationOfRecordingMins = totalTime;
            stats.TotalActigraphCount = records.Sum(r => r.DeviceIntensity);
            stats.NormalizedActigraphCountPerMin = (double)stats.TotalActigraphCount / totalTime;

            stats.NormalizedSedentaryActivityPerMin = (double)stats.TotalSedentaryActivityMins / totalTime;
            stats.NormalizedLowActivityActivityPerMin = (double)stats.TotalLowActivityActivityMins / totalTime;
            stats.NormalizedLightActivityPerMin = (double)stats.TotalLightActivityMins / totalTime;
            stats.NormalizedModerateActivityPerMin = (double)stats.TotalModerateActivityMins / totalTime;
            stats.NormalizedVigorousActivityPerMin = (double)stats.TotalVigorousActivityMins / totalTime;
            stats.NormalizedExtraVigorousActivityPerMin = (double)stats.TotalExtraVigorousActivityMins / totalTime;

            stats.AverageActigraphCountPer24Hour = (int)Math.Round(stats.NormalizedActigraphCountPerMin * MINUTES_IN_24HOURS, MidpointRounding.AwayFromZero);

            stats.AverageSedentaryActivityMinsPer24Hour = (int)Math.Round(stats.NormalizedSedentaryActivityPerMin * MINUTES_IN_24HOURS, MidpointRounding.AwayFromZero);
            stats.AverageLowActivityActivityMinsPer24Hour = (int)Math.Round(stats.NormalizedLowActivityActivityPerMin * MINUTES_IN_24HOURS, MidpointRounding.AwayFromZero);
            stats.AverageLightActivityMinsPer24Hour = (int)Math.Round(stats.NormalizedLightActivityPerMin * MINUTES_IN_24HOURS, MidpointRounding.AwayFromZero);
            stats.AverageModerateActivityMinsPer24Hour = (int)Math.Round(stats.NormalizedModerateActivityPerMin * MINUTES_IN_24HOURS, MidpointRounding.AwayFromZero);
            stats.AverageVigorousActivityMinsPer24Hour = (int)Math.Round(stats.NormalizedVigorousActivityPerMin * MINUTES_IN_24HOURS, MidpointRounding.AwayFromZero);
            stats.AverageExtraVigorousActivityMinsPer24Hour = (int)Math.Round(stats.NormalizedExtraVigorousActivityPerMin * MINUTES_IN_24HOURS, MidpointRounding.AwayFromZero);

            stats.AverageLightToModerateActivityMinsPer24Hour = stats.AverageLightActivityMinsPer24Hour + stats.AverageModerateActivityMinsPer24Hour;

            return stats;
        }
    }
}