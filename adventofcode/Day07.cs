using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    public class Bags
    {
        public Bags()
        {
            Relationships = new Dictionary<string, BagData>();
            AllBags = new HashSet<string>();
        }

        public Dictionary<string, BagData> Relationships { get;set; }
        public HashSet<string> AllBags { get;set; }

        public void Parse(List<string> inputs)
        {
            foreach(var input in inputs)
            {
                var split = input.Split(new [] { "bags contain", "," }, StringSplitOptions.RemoveEmptyEntries);
                var bagName = split[0].Trim();
                if(!AllBags.Contains(bagName))
                {
                    AllBags.Add(bagName);
                }

                if(split.Skip(1).ToList()[0].Trim() != "no other bags.")
                {
                    var bagData = new BagData();
                    
                    foreach(var child in split.Skip(1))
                    {
                        var count = Int16.Parse(child.Trim().Substring(0,1));
                        var name = child.Trim().Substring(2, child.Trim().Length - 2).Replace("bags","").Replace("bag","").Replace(".","").Trim();
                        if(!AllBags.Contains(name))
                        {
                            AllBags.Add(bagName);
                        }
                        bagData.Children.Add(name, count);
                    }

                    Relationships.Add(bagName, bagData);
                }
            }
        }
    }


    public class BagData
    {
       public Dictionary<string, int> Children { get;set; }

        public BagData()
        {
            Children = new Dictionary<string, int>();
        }
    }

    public class BagFinder
    {
        public static bool FindChildBag(string bagName, List<string> children, Dictionary<string, BagData> allBags)
        {
            foreach(var child in children)
            {
                if(bagName == child)
                {
                    return true;
                }

                if(allBags.ContainsKey(child))
                {
                    if(FindChildBag(bagName, allBags[child].Children.Keys.ToList(), allBags))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int CountChildBag(BagData bag, Dictionary<string, BagData> allBags)
        {
            var count = 0;
            foreach(var child in bag.Children.Keys)
            {
                count += bag.Children[child];

                if(allBags.ContainsKey(child))
                {
                    count += bag.Children[child] * CountChildBag(allBags[child], allBags);
                }
            }
            return count;
        }
    }

    public class Day07 : IDay
    {
        private readonly List<string> _input;

        public Day07(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var bags = new Bags();
            bags.Parse(_input);
            var count = 0;

            foreach(var bag in bags.Relationships)
            {
                count += BagFinder.FindChildBag("shiny gold", bag.Value.Children.Keys.ToList(), bags.Relationships) ? 1 : 0;
            }    

            return count.ToString();
        }

        public string ChallengeTwo()
        {
            var bags = new Bags();
            bags.Parse(_input);
            var count = 0;

            var bag = bags.Relationships["shiny gold"];

            count = new BagFinder().CountChildBag(bag, bags.Relationships);
            return count.ToString();
        }
    }
}