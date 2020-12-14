using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

namespace adventofcode
{
    public class Action
    {
        public string ActionType { get;set;}

        public string Mask { get; set;}

        public long Address { get;set;}

        public long Input { get;set;}
    }

    public class Addresses
    {

        public Addresses()
        {
            Memory = new Dictionary<long, long>();
            Actions = new List<Action>();
        }

        public Dictionary<long, long> Memory { get;set;}
        public List<Action> Actions { get;set;}

        public void Parse(List<string> inputs)
        {
            foreach(var input in inputs)
            {
                if(input.StartsWith("mask"))
                {
                    Actions.Add(new Action { ActionType = "Mask", Mask = input.Replace("mask = ", "") }) ;
                }
                else
                {
                    var split = input.Split(new [] { "[", "]", "=" }, System.StringSplitOptions.RemoveEmptyEntries);
                    Actions.Add(new Action { ActionType = "Allocation", Address = int.Parse(split[1]), Input = int.Parse(split[3]) });
                }
            }
        }

        public long GetTotal()
        {
            Memory = Actions.Where(o => o.ActionType == "Allocation").Select(o => o.Address).Distinct().ToDictionary(o => o, o => (long)0);

            var currentMask = string.Empty;
            foreach(var action in Actions)
            {
                if(action.ActionType == "Mask")
                {
                    currentMask = action.Mask;
                }
                if(action.ActionType == "Allocation")
                {
                    var binary = Convert.ToString(action.Input, 2);
                    binary = binary.PadLeft(36,'0');
                    
                    var newString = new StringBuilder();
                    for(var i = 0; i < 36; i++)
                    {
                        if(currentMask[i] != 'X')
                        {
                            newString.Append(currentMask[i]); ;
                        }
                        else
                        {
                            newString.Append(binary[i]);
                        }
                    }

                    Memory[action.Address] = Convert.ToInt64(newString.ToString(), 2);
                }
            }

            return Memory.Values.Sum();
        }

        public long GetTotalFloating()
        {
            var currentMask = string.Empty;
            var currentMasks = new List<string>();

            foreach(var action in Actions)
            {
                if(action.ActionType == "Mask")
                {
                    currentMask = action.Mask;
                    currentMasks = new List<string>();

                    var floatCount = currentMask.Where(o => o == 'X').Count();

                    var topOfRange = Convert.ToInt32(string.Empty.PadLeft(floatCount, '1'),2);
                   
                    var range = Enumerable.Range(0, topOfRange + 1);

                    foreach(var mask in range)
                    {
                        var floats = Convert.ToString(mask,2);
                        floats = floats.PadLeft(floatCount,'0');
                        var newString = new StringBuilder();

                        var maskCount = 0;
                        for(var i = 0; i <36; i++)
                        {
                            if(currentMask[i] == 'X')
                            {
                                newString.Append(floats[maskCount] == '0' ? 'F' : 'T');
                                maskCount++;
                            }
                            else
                            {   
                                newString.Append(currentMask[i]);
                            }
                        }

                        currentMasks.Add(newString.ToString());
                    }                   
                }

                if(action.ActionType == "Allocation")
                {
                    var binary = Convert.ToString(action.Address, 2);
                    binary = binary.PadLeft(36,'0');

                    foreach(var mask in currentMasks)
                    {
                        var newString = new StringBuilder();
                        for(var i = 0; i < 36; i++)
                        {
                            if(mask[i] == 'F')
                            {
                                newString.Append(0);
                            }
                            else if(mask[i] == '1' || mask[i] == 'T')
                            {
                                newString.Append(1);
                            }
                            else
                            {
                                newString.Append(binary[i]);
                            }
                        }
                        var address = Convert.ToInt64(newString.ToString(), 2);
                        if(!Memory.ContainsKey(address))
                        {
                            Memory.Add(address, 0);
                        }
                        Memory[address] = action.Input;
                    }
                }
            }

            return Memory.Values.Sum();
        }
    }

    public class Day14 : IDay
    {
        private readonly List<string> _input;

        public Day14(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var memory = new Addresses();
            memory.Parse(_input);
            return memory.GetTotal().ToString();
        }

        public string ChallengeTwo()
        {
            var memory = new Addresses();
            memory.Parse(_input);
            return memory.GetTotalFloating().ToString();
        }
    }
}