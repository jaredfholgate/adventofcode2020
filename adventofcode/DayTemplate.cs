using System.Collections.Generic;

namespace adventofcode
{
    public class DayTemplate : IDay
    {
        private readonly List<string> _input;

        public DayTemplate(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            return string.Empty;
        }

        public string ChallengeTwo()
        {
            return string.Empty;
        }
    }
}