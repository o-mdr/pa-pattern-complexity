using System.Collections.Generic;

namespace PatternPA.Core.Interfaces
{
    public interface IRandomEventGenerator
    {
        /// <summary>
        /// Creates a random enumerable sequence of events
        /// </summary>
        /// <param name="availableEvents">Random values can only be taken from this range,
        /// sequence must not contain gaps, e.g 0, 1, 2, 4, 6 is not accaptable</param>
        /// <param name="lenght">the lenght of the output</param>
        /// <returns></returns>
        IEnumerable<int> GenerateRandomEvents(IEnumerable<int> availableEvents, int lenght);


        IEnumerable<int> GenerateRandomEventsWithDegreeOfRandomess(
            IEnumerable<int> availableEvents, int lenght,
            double degreeOfRandomness, int nonRandomEvent);

        IEnumerable<int> GenerateLineOfOneEvent(int eventCode, int lenght);
    }
}