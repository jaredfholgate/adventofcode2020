using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

namespace adventofcode
{
    public class Day15 : IDay
    {
        private readonly List<string> _input;

        public Day15(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var numberCounts = new Dictionary<int, int>();
            var count = 1;
            var inputs = _input[0].Split(new [] { ','}).Select(o => int.Parse(o)).ToList();
            foreach(var input in inputs)
            {
                numberCounts.Add(input, count);
                count++;
            }

            var lastSpoken = inputs.Last();
            while(count <= 2020)
            {
                var keepLastSpoken = lastSpoken;
                if(numberCounts.ContainsKey(keepLastSpoken) && (numberCounts[keepLastSpoken] < count - 1))
                {
                    lastSpoken = (count - 1) - numberCounts[keepLastSpoken];
                }
                else
                {
                    lastSpoken = 0;
                }

                if(!numberCounts.ContainsKey(keepLastSpoken))
                {
                    numberCounts.Add(keepLastSpoken, count -1);
                }
                else
                {
                    numberCounts[keepLastSpoken] = count -1;
                }

                count++;
            }

            return lastSpoken.ToString();
        }

        public string ChallengeTwo()
        {
            var numberCounts = new Dictionary<int, int>();
            var count = 1;
            var inputs = _input[0].Split(new [] { ','}).Select(o => int.Parse(o)).ToList();
            foreach(var input in inputs)
            {
                numberCounts.Add(input, count);
                count++;
            }

            var lastSpoken = inputs.Last();
            while(count <= 30000000)
            {
                var keepLastSpoken = lastSpoken;
                if(numberCounts.ContainsKey(keepLastSpoken) && (numberCounts[keepLastSpoken] < count - 1))
                {
                    lastSpoken = (count - 1) - numberCounts[keepLastSpoken];
                }
                else
                {
                    lastSpoken = 0;
                }

                if(!numberCounts.ContainsKey(keepLastSpoken))
                {
                    numberCounts.Add(keepLastSpoken, count -1);
                }
                else
                {
                    numberCounts[keepLastSpoken] = count -1;
                }

                count++;
            }

            return lastSpoken.ToString();
        }
    }
}