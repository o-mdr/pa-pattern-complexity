using System;
using System.Collections.Generic;
using System.Linq;
using PatternPA.Core.Interfaces;

namespace PatternPA.Utils
{
    public class RandomEventGenerator : IRandomEventGenerator
    {
        static Random random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Creates a random enumerable sequence of events
        /// </summary>
        /// <param name="availableEvents">Random values can only be taken from this range,
        /// sequence must not contain gaps, e.g 0, 1, 2, 4, 6 is not accaptable</param>
        /// <param name="lenght">the lenght of the output</param>
        /// <returns></returns>
        public IEnumerable<int> GenerateRandomEvents(IEnumerable<int> availableEvents, int lenght)
        {
            if (lenght < 0)
                throw new ArgumentOutOfRangeException("lenght");

            int min = availableEvents.ToList().Min();
            int max = availableEvents.ToList().Max();
            

            var result = new List<int>();
            for (int i = 0; i < lenght; i++)
            {
                result.Add(random.Next(min, max + 1));
            }

            return result;
        }

        /// <summary>
        /// Creates a random enumerable sequence of events
        /// </summary>
        /// <param name="availableEvents">Random values can only be taken from this range,
        /// sequence must not contain gaps, e.g 0, 1, 2, 4, 6 is not accaptable</param>
        /// <param name="lenght">the lenght of the output</param>
        /// <param name="degreeOfRandomness">determines how often a non random event will be inserted</param>
        /// /// <param name="nonRandomEvent">value to be inserted on the </param>
        /// <returns></returns>
        public IEnumerable<int> GenerateRandomEventsWithDegreeOfRandomess(
            IEnumerable<int> availableEvents, int lenght, 
            double degreeOfRandomness, int nonRandomEvent)
        {
            if (lenght < 0)
                throw new ArgumentOutOfRangeException("lenght");
            if (degreeOfRandomness < 0 || degreeOfRandomness > 1)
                throw new ArgumentOutOfRangeException("Allowed values are between 0 and 1");

            int min = availableEvents.ToList().Min();
            int max = availableEvents.ToList().Max();
            int count = 0;

            var result = new List<int>();
            for (int i = 0; i < lenght; i++)
            {
                double tmp = random.NextDouble();
                bool useRandom = tmp < degreeOfRandomness;
                                
                if (useRandom)
                {
                    //random event
                    result.Add(random.Next(min, max + 1));   
                }
                else
                {
                    //non random event
                    result.Add(nonRandomEvent);
                    count = 0;                    
                }
            }

            return result;
        }

        public IEnumerable<int> GenerateLineOfOneEvent(int eventCode, int lenght)
        {
            if (lenght < 0)
                throw new ArgumentOutOfRangeException("lenght");

            var result = new List<int>();
            for (int i = 0; i < lenght; i++)
            {
                result.Add(eventCode);
            }

            return result;
        }
    }
}