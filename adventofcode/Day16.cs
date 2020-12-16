using System.Collections.Generic;
using System.Linq;
using System;

namespace adventofcode
{
    public class Tickets
    {
        public Tickets()
        {
            Rules = new Dictionary<string, List<(int start, int end)>>();
            ValidNumbers = new HashSet<int>();
            ValidNearbyTickets = new Dictionary<int, List<int>>();
            NearbyTickets = new Dictionary<int, List<int>>();
            SpecificRules = new Dictionary<string, HashSet<int>>();
        }

        public Dictionary<string, List<(int start, int end)>> Rules { get;set; }
        public Dictionary<string, HashSet<int>> SpecificRules { get;set; }

        public HashSet<int> ValidNumbers { get;set; }

        public List<int> MyTicket { get;set;}

        public Dictionary<int, List<int>> NearbyTickets { get;set;}

        public Dictionary<int, List<int>> ValidNearbyTickets { get;set;}

        public void Parse(List<string> inputs)
        {
            var mode = "Rules";
            var count = 0;
            foreach(var input in inputs)
            {
                if(input != "")
                {
                    if(input == "your ticket:")
                    {
                        mode = "My Ticket";
                    }
                    else if(input == "nearby tickets:")
                    {
                        mode = "Nearby Tickets";
                    }
                    else
                    {
                        switch(mode)
                        {
                            case "Rules":
                                var ruleSplit = input.Split(new [] { ": ", "-", " or "  }, System.StringSplitOptions.RemoveEmptyEntries);
                                Rules.Add(ruleSplit[0], new List<(int start, int end)> { ( int.Parse(ruleSplit[1]), int.Parse(ruleSplit[2]) ), ( int.Parse(ruleSplit[3]), int.Parse(ruleSplit[4]) )  });
                                SpecificRules.Add(ruleSplit[0], new HashSet<int>());
                                foreach(var range in Rules[ruleSplit[0]])
                                {
                                    var validNumbers = Enumerable.Range(range.start, (range.end - range.start) + 1);
                                    foreach(var validNumber in validNumbers)
                                    {
                                        if(!ValidNumbers.Contains(validNumber))
                                        {
                                            ValidNumbers.Add(validNumber);
                                        }
                                        if(!SpecificRules[ruleSplit[0]].Contains(validNumber))
                                        {
                                            SpecificRules[ruleSplit[0]].Add(validNumber);
                                        }
                                    }
                                }
                                break;

                            case "My Ticket":
                                MyTicket = input.Split(',').Select(o => int.Parse(o)).ToList();
                                break;

                            case "Nearby Tickets":
                                NearbyTickets.Add(count, input.Split(',').Select(o => int.Parse(o)).ToList());
                                count++;
                                break;
                        }
                    }
                }
            }
        }

        public void GetValidTickets()
        {

            foreach(var ticket in NearbyTickets.Keys)
            {
                bool IsValid = true;
                foreach(var number in NearbyTickets[ticket])
                {
                    if(!ValidNumbers.Contains(number))
                    {
                        IsValid = false;
                    }
                }
                if(IsValid)
                {
                    ValidNearbyTickets.Add(ticket, NearbyTickets[ticket]);
                }
            }
        }

        public long GetErrorRate()
        {
            long sum = 0;
            foreach(var ticket in NearbyTickets.Values)
            {
                foreach(var number in ticket)
                {
                    if(!ValidNumbers.Contains(number))
                    {
                        sum += number;
                    }
                }
            }

            return sum;
        }

        public long DetermineOrder()
        {
            var order = new Dictionary<string, HashSet<int>>();
            order = Rules.Keys.ToDictionary(o => o, o => new HashSet<int>());
            
            
            for(var column = 0; column < ValidNearbyTickets[0].Count; column ++)
            {
                var values = ValidNearbyTickets.Values.Select(o => o[column]);
                foreach(var rule in SpecificRules.Keys)
                {   
                    var except = values.Except(SpecificRules[rule]);
                    if(!except.Any())
                    {
                        order[rule].Add(column);
                    }
                }
            }

            while(order.Values.Where(o => o.Count() > 1).Any())
            {
                foreach(var key in order.Where(o => o.Value.Count() > 1).Select(o => o.Key))
                {
                    var uniques = order.Values.Where(o => o.Count == 1).SelectMany(o => o).ToList();

                    foreach(var column in order[key])
                    {
                        if(uniques.Contains(column))
                        {
                            order[key].Remove(column);
                        }
                    }

                    var checks = order.Where(o => o.Key != key).SelectMany(o => o.Value).ToList();

                    var unique = -1;
                    foreach(var column in order[key])
                    {
                        if(!checks.Contains(column))
                        {
                            unique = column;
                        }
                    }
                    if(unique != -1)
                    {
                        order[key] = new HashSet<int> { unique};
                    }
                }
            }

            var departureColumns = order.Where(o => o.Key.StartsWith("departure")).SelectMany(o => o.Value);
            long total = 1;
            foreach(var column in departureColumns)
            {
                total *= MyTicket[column];
            }

            return total;
        }
                   
    }

    public class Day16 : IDay
    {
        private readonly List<string> _input;

        public Day16(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var tickets = new Tickets();
            tickets.Parse(_input);
            return tickets.GetErrorRate().ToString();
        }

        public string ChallengeTwo()
        {
            var tickets = new Tickets();
            tickets.Parse(_input);
            tickets.GetValidTickets();
            return tickets.DetermineOrder().ToString();
        }
    }
}