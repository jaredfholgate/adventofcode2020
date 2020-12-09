using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    public class Decipher
    {
        public Dictionary<int,long> Numbers { get;set;}

        public void Parse(List<string> inputs)
        {
            Numbers = new Dictionary<int, long>();
            for(int i = 0; i < inputs.Count; i++)
            {
                Numbers.Add(i, long.Parse(inputs[i]));
            }
        }

        public long FindContiguous(long numberToFind = 31161678)
        {
            foreach(var key in Numbers.Keys)
            {
                long sum = 0;
                int position = key;
                while(sum < numberToFind)
                {
                    sum += Numbers[position];
                    position += 1;
                }

                if(sum == numberToFind)
                {
                    var smallest = long.MaxValue;
                    long largest = -1;

                    for(int i = key; i <= position -1; i++)
                    {
                        if(Numbers[i] < smallest)
                        {
                            smallest = Numbers[i];
                        }
                        if(Numbers[i] > largest)
                        {
                            largest = Numbers[i];
                        }
                    }
                    return smallest + largest;
                }
            }

            return -1;
        }

        public long FindFirstFail()
        {
            for(var position = 25; position < Numbers.Count; position ++)
            {
                var number = Numbers[position];
                var range = Enumerable.Range(position - 25,25);

                bool success = false;
                foreach(var check in range)
                {
                    foreach(var check2 in range)
                    {
                        if(check != check2)
                        {
                            if(Numbers[check] + Numbers[check2] == number)
                            {
                                success = true;
                            }
                        }
                        if(success)
                        {
                            break;
                        }
                    }
                    if(success)
                    {
                        break;
                    }
                }
                if(!success)
                {
                    return number;
                }
            }

            return -1;
        }
    }

    public class Day09 : IDay
    {
        private readonly List<string> _input;

        public Day09(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var decipher = new Decipher();
            decipher.Parse(_input);
            return decipher.FindFirstFail().ToString();
        }

        public string ChallengeTwo()
        {
            var decipher = new Decipher();
            decipher.Parse(_input);
            return decipher.FindContiguous().ToString();
        }
    }
}