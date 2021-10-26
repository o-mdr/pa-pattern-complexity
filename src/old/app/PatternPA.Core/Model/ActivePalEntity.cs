using System;
using System.Configuration;

namespace PatternPA.Core.Model
{
    public class ActivePalEntity
    {
        public static string DefaultGroupDataPath = ConfigurationManager.AppSettings["DefaultGroupDataPath"];
        public static string DefaultFolderPath = ConfigurationManager.AppSettings["DefaultFolderPath"];
        public static TimeSpan DefaultCheckpoint = new TimeSpan(0, 0, 0, 1);

        protected ActivePalEntity(TimeSpan checkpointRate)
        {
            CheckpointRate = checkpointRate == new TimeSpan() ? DefaultCheckpoint : checkpointRate;
        }

        public object Id { get; set; }
        public string DataFilePath { get; set; }
        public string DataFolderPath { get; set; }
        public TimeSpan CheckpointRate { get; set; }
    }
}