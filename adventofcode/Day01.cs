using System.Collections.Generic;
using System;


namespace adventofcode
{
    public class Day01 : IDay
    {
        private readonly List<string> _input;

        public Day01(List<string> input)
        {
            _input = input;
        }

        public string ChallengeOne()
        {
            foreach(var value in _input)
            {
                foreach(var value2 in _input)
                {
                    if(Convert.ToInt32(value) + Convert.ToInt32(value2) == 2020)
                    {
                        return (Convert.ToInt32(value) * Convert.ToInt32(value2)).ToString();
                    }
                }
            }
            return string.Empty;
        }

        public string ChallengeTwo()
        {
            foreach(var value in _input)
            {
                foreach(var value2 in _input)
                {
                    foreach(var value3 in _input)
                    {
                        if(Convert.ToInt32(value) + Convert.ToInt32(value2)  + Convert.ToInt32(value3) == 2020)
                        {
                            return (Convert.ToInt32(value) * Convert.ToInt32(value2) * Convert.ToInt32(value3)).ToString();
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}