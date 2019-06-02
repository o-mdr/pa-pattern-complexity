using System;
using System.Collections.Generic;

namespace PatternPA.Core.Model
{
    public class Group : ActivePalEntity
    {
        public Group() : this(null) { }

        public Group(string folderPath) : this(folderPath, new TimeSpan(), null) { }

        public Group(string folderPath, TimeSpan checkpointRate, string dataPath)
            : base(checkpointRate)
        {
            if (String.IsNullOrEmpty(folderPath))
            {
                folderPath = DefaultFolderPath;
            }

            DataFolderPath = folderPath;
            DataFilePath = String.IsNullOrEmpty(dataPath) ? DefaultGroupDataPath : dataPath;
        }

        public IEnumerable<Person> People { get; set; }
        public IDictionary<string, decimal> CommonWordsProbability { get; set; }
    }
}