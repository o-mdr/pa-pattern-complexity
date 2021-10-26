using System;

namespace PatternPA.Core.Model.Nhanes
{
    public class NhanesPersonActivityStats
    {
        #region Actigraph counts measures

        //sum per recording period
        public int TotalActigraphCount { get; set; }

        //this much counts per min
        public double NormalizedActigraphCountPerMin { get; set; }
        //on average this much counts per 24 hours
        public int AverageActigraphCountPer24Hour { get; set; }

        #endregion

        #region TIME measures

        //number of minutes spent in this activity per recording period
        public int TotalSedentaryActivityMins { get; set; }
        public int TotalLowActivityActivityMins { get; set; }
        public int TotalLightActivityMins { get; set; }
        public int TotalModerateActivityMins { get; set; }
        public int TotalVigorousActivityMins { get; set; }
        public int TotalExtraVigorousActivityMins { get; set; }

        //total number of minutes per recording period
        public int TotalDurationOfRecordingMins { get; set; }

        //this much proportion of the activity per minute
        public double NormalizedSedentaryActivityPerMin { get; set; }
        public double NormalizedLowActivityActivityPerMin { get; set; }
        public double NormalizedLightActivityPerMin { get; set; }
        public double NormalizedModerateActivityPerMin { get; set; }
        public double NormalizedVigorousActivityPerMin { get; set; }
        public double NormalizedExtraVigorousActivityPerMin { get; set; }

        //this many minutes spent in activity per 24 hours
        public int AverageSedentaryActivityMinsPer24Hour { get; set; }
        public int AverageLowActivityActivityMinsPer24Hour { get; set; }
        public int AverageLightActivityMinsPer24Hour { get; set; }
        public int AverageModerateActivityMinsPer24Hour { get; set; }
        public int AverageVigorousActivityMinsPer24Hour { get; set; }
        public int AverageExtraVigorousActivityMinsPer24Hour { get; set; }

        public int AverageLightToModerateActivityMinsPer24Hour { get; set; }

        #endregion

        public override string ToString()
        {
            return String.Concat(TotalActigraphCount, ",",
                                 NormalizedActigraphCountPerMin, ",",
                                 AverageActigraphCountPer24Hour, ",",

                                 TotalSedentaryActivityMins, ",",
                                 TotalLowActivityActivityMins, ",",
                                 TotalLightActivityMins, ",",
                                 TotalModerateActivityMins, ",",
                                 TotalVigorousActivityMins, ",",
                                 TotalExtraVigorousActivityMins, ",",

                                 TotalDurationOfRecordingMins, ",",

                                 NormalizedSedentaryActivityPerMin, ",",
                                 NormalizedLowActivityActivityPerMin, ",",
                                 NormalizedLightActivityPerMin, ",",
                                 NormalizedModerateActivityPerMin, ",",
                                 NormalizedVigorousActivityPerMin, ",",
                                 NormalizedExtraVigorousActivityPerMin, ",",

                                 AverageSedentaryActivityMinsPer24Hour, ",",
                                 AverageLowActivityActivityMinsPer24Hour, ",",
                                 AverageLightActivityMinsPer24Hour, ",",
                                 AverageModerateActivityMinsPer24Hour, ",",
                                 AverageVigorousActivityMinsPer24Hour, ",",
                                 AverageExtraVigorousActivityMinsPer24Hour, ",",

                                 AverageLightToModerateActivityMinsPer24Hour);
            
        }

        public static string GetCsvHeader()
        {
            return String.Concat("TotalActigraphCount,",
                                 "NormalizedActigraphCountPerMin,",
                                 "AverageActigraphCountPer24Hour,",

                                 "TotalSedentaryActivityMins,",
                                 "TotalLowActivityActivityMins,", 
                                 "TotalLightActivityMins,", 
                                 "TotalModerateActivityMins,", 
                                 "TotalVigorousActivityMins,", 
                                 "TotalExtraVigorousActivityMins,", 

                                 "TotalDurationOfRecordingMins,", 

                                 "NormalizedSedentaryActivityPerMin,", 
                                 "NormalizedLowActivityActivityPerMin,", 
                                 "NormalizedLightActivityPerMin,", 
                                 "NormalizedModerateActivityPerMin,", 
                                 "NormalizedVigorousActivityPerMin,", 
                                 "NormalizedExtraVigorousActivityPerMin,", 

                                 "AverageSedentaryActivityMinsPer24Hour,", 
                                 "AverageLowActivityActivityMinsPer24Hour,", 
                                 "AverageLightActivityMinsPer24Hour,", 
                                 "AverageModerateActivityMinsPer24Hour,", 
                                 "AverageVigorousActivityMinsPer24Hour,", 
                                 "AverageExtraVigorousActivityMinsPer24Hour,", 

                                 "AverageLightToModerateActivityMinsPer24Hour");
        }
    }
}