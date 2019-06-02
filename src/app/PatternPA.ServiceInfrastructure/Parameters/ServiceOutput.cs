using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PatternPA.ServiceInfrastructure.Patterns;

namespace PatternPA.ServiceInfrastructure.Parameters
{
    [DataContract]
    public class ServiceOutput : AbstractParameter
    {
        public Dictionary<Pattern, IEnumerable<Int64>> PatternOccurance { get; set; }
    }
}