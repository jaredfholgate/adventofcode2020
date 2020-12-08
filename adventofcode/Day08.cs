using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    
    public class CodeLine
    {
        public bool HasRun { get; set;}
        public string ActionType { get;set;}

        public int ActionNumber { get;set;}
    }

    public class CodeRunner
    {
        public CodeRunner()
        {
            
            Code = new Dictionary<int, CodeLine>();
        }
        public int Accumulator { get;set;}

        public Dictionary<int, CodeLine> Code { get;set;}

        public void Parse(List<string> input)
        {
            for(var i = 0; i < input.Count; i++)
            {
                var split = input[i].Split(' ');
                var CodeLine = new CodeLine 
                {
                    ActionType = split[0],
                    ActionNumber = int.Parse(split[1])
                };
                Code.Add(i, CodeLine);
            }        
        }

        public bool RunUntilDuplicate()
        {
            var success = false;
            var position = 0;
            Accumulator = 0;
            while(Code.ContainsKey(position) && !Code[position].HasRun)
            {
                var codeLine = Code[position];
                codeLine.HasRun = true;

                switch (codeLine.ActionType)
                {
                    case "acc":
                        Accumulator += Code[position].ActionNumber;
                        position += 1;
                        break;
                    case "jmp":
                        position += Code[position].ActionNumber;
                        break;
                    case "nop":
                        position += 1;
                         break;
                }
            }

            if(!Code.ContainsKey(position))
            {
                if(position == Code.Count)
                {
                    success = true;
                }
            }

            return success;
        }

        public void FixAndRun(List<string> input)
        {
            foreach(var position in Code.Keys)
            {
                if(Code[position].ActionType == "nop" || Code[position].ActionType == "jmp")
                {
                    var testCode = new CodeRunner();
                    testCode.Parse(input);
                    testCode.Code[position].ActionType = testCode.Code[position].ActionType == "nop" ? "jmp" : "nop";
                    var success = testCode.RunUntilDuplicate();
                    if(success)
                    {
                        Accumulator = testCode.Accumulator;
                        return;
                    }
                }
            }

        }
    }

    public class Day08 : IDay
    {
        private readonly List<string> _input;

        public Day08(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var runner = new CodeRunner();
            runner.Parse(_input);
            runner.RunUntilDuplicate();
            return runner.Accumulator.ToString();
        }

        public string ChallengeTwo()
        {
            var runner = new CodeRunner();
            runner.Parse(_input);
            runner.FixAndRun(_input);
            return runner.Accumulator.ToString();
        }
    }
}