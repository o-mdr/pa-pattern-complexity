using System.Collections.Generic;
using PatternPA.Core.Interfaces;

namespace PatternPA.Core.Model
{
    public class ActivePalAlphabet : IAlphabet
    {
        #region IAlphabet Members

        public IEnumerable<ActivityCodes> GetAlphabet()
        {
            return
                new List<ActivityCodes>
                    {
                        ActivityCodes.Sit,
                        ActivityCodes.Upright,
                        ActivityCodes.Walk
                    };
        }

        #endregion
    }
}