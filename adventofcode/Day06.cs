using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    public class Group
    {
       public List<List<char>> Data { get;set; }
       public Dictionary<char, int> Counts  { get;set; }

        public Group()
        {
            Data = new List<List<char>>();
            Counts = new Dictionary<char, int>();
        }

        public static List<Group> ParseAll(List<string> inputs)
        {
            var groups = new List<Group>() 
            {
                new Group()
            };

            foreach(var input in inputs)
            {
                if(input == string.Empty)
                {
                    groups.Add(new Group());
                }
                else
                {
                    var group = groups.Last();
                    var characters = input.ToList();
                
                    foreach(var character in characters)
                    {
                        if(group.Counts.ContainsKey(character))
                        {
                            group.Counts[character] += 1;
                        }
                        else
                        {
                            group.Counts.Add(character,1);
                        }
                    }

                    group.Data.Add(input.ToList());
                }
            }

            return groups;
        }
    }

    public class Day06 : IDay
    {
        private readonly List<string> _input;

        public Day06(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var groups = Group.ParseAll(_input);
            var count = groups.Sum(o => o.Counts.Keys.Count());
            return count.ToString();
        }

        public string ChallengeTwo()
        {
            var total = 0;
            var groups = Group.ParseAll(_input);
            foreach(var group in groups)
            {
                var memberCount = group.Data.Count();
                total += group.Counts.Count(o => o.Value == memberCount);
            }
            return total.ToString();
        }
    }
}