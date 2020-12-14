using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Threading;

namespace adventofcode
{

    public class BusTimes
    {
        public HashSet<int> Buses { get;set;}

        public int TargetTime { get;set;}

        public Dictionary<long, long> BusPattern { get;set; }

        public void Parse(List<string> input)
        {
            TargetTime = int.Parse(input[0]);

            var buses = input[1].Split(new char[] { ','});
            Buses = input[1].Split(new char[] { ','}).Where(o => o != "x").Select(o => int.Parse(o)).ToHashSet();
            BusPattern = new Dictionary<long, long>();
            long count = 0;
            foreach(var bus in buses)
            {
                if(bus != "x")
                {
                    BusPattern.Add(long.Parse(bus), count);
                }
                count++;
            }
        }

        public int GetNearestBus()
        {
            var closestBusMinutes = int.MaxValue;
            var closestBus = 0;
            foreach(var bus in Buses)
            {
                int remainder;
                int quotient = Math.DivRem(TargetTime, bus, out remainder);
                var nearestTime = ((TargetTime - remainder) + bus) - TargetTime;
                if(nearestTime < closestBusMinutes)
                {
                    closestBusMinutes = nearestTime;
                    closestBus = bus;
                }
            }

            return closestBus * closestBusMinutes;
        }

        public long GetMatchingPattern()
        {
            var currentFrequency = BusPattern.Keys.Max();
            long currentNumber = BusPattern.Keys.Max();
            long previousBus = BusPattern.Keys.Max();
            long firstBus = BusPattern.Keys.Max();
            long tempRemainder;

            foreach(var currentBus in BusPattern.Keys.Where(o => o < BusPattern.Keys.Max()).OrderByDescending(o => o))
            {
                var diff = BusPattern[currentBus] - BusPattern[firstBus];
                long startOfCheck = 0;
                while(currentNumber < long.MaxValue)
                {   
                    var checkNumber = currentNumber + diff;
                    var smallCheck = Math.DivRem(checkNumber, currentBus, out tempRemainder);
                    if(tempRemainder == 0)
                    {   
                        if(startOfCheck == 0)
                        {
                            if(currentBus == BusPattern.Keys.Min())
                            {
                                return checkNumber - BusPattern[currentBus];
                            }
                            else
                            {
                                startOfCheck = currentNumber;
                            }
                        }   
                        else
                        {                      
                            currentFrequency = currentNumber - startOfCheck;
                            
                            currentNumber = startOfCheck;
                            previousBus = currentBus;
                            break;
                        }
                    }
                         
                    currentNumber += currentFrequency;
                }
            }
            
            return 0;
        }
    }

    public class Day13 : IDay
    {
        private readonly List<string> _input;

        public Day13(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var busTimes  = new BusTimes();
            busTimes.Parse(_input);
            return busTimes.GetNearestBus().ToString();
        }

        public string ChallengeTwo()
        {            
            var busTimes  = new BusTimes();
            busTimes.Parse(_input);
            return busTimes.GetMatchingPattern().ToString();
        }
    }
}