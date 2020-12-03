using System.Collections.Generic;
using System;
using System.Linq;

namespace adventofcode
{
    public class Day03Data
    {
        public string Data { get;set;}
 
        public static Day03Data Parse(string input)
        {
            return new Day03Data
            {
                Data = input
            };
        }

        public bool IsTree(int position)
        {
            int checkPosition = position % Data.Length;
            return Data[checkPosition] == '#';
        }
    }

    public class Day03 : IDay
    {
        private readonly List<Day03Data> _input;

        public Day03(List<string> input)
        {
            _input = input.Select(o => Day03Data.Parse(o)).ToList();
        }

        public string ChallengeOne()
        {
            var count = 0;
            var horizontalPosition = 0;

            foreach(var row in _input)
            {
                count += row.IsTree(horizontalPosition) ? 1 : 0;
                horizontalPosition += 3;
            }
            return count.ToString();
        }

        public string ChallengeTwo()
        {
            var slopes = new List<Tuple<int,int>> {
                new Tuple<int, int>(1,1),
                new Tuple<int, int>(3,1),
                new Tuple<int, int>(5,1),
                new Tuple<int, int>(7,1),
                new Tuple<int, int>(1,2),
            };

            long total = 1;
            foreach(var slope in slopes)
            {
                var count = 0;
                var horizontalPosition = 0;
                var rowCount = 0;
                foreach(var row in _input)
                {
                    if(slope.Item2 == 1 || rowCount % slope.Item2 == 0)
                    {
                        count += row.IsTree(horizontalPosition) ? 1 : 0;
                        horizontalPosition += slope.Item1;
                    }
                    rowCount++;
                }
                total = total * count;
            }
            return total.ToString();
        }
    }
}