using System.Collections.Generic;
using System.Linq;

namespace adventofcode
{
    public class Homework
    {
        public Dictionary<string, long> Sums { get;set;}

        public void Parse(List<string> inputs)
        {
            Sums = new Dictionary<string, long>();

            foreach(var input in inputs)
            {
                Sums.Add(input,0);
            }
        }

        public long Calculate()
        {
            
            foreach(var sum in Sums.Keys)
            {
                Sums[sum] = Calculate(sum);
            }
            return Sums.Values.Sum();
        }


        private long Calculate(string sum)
        {
            sum = sum.Replace(" ","");

            var partDepths = new List<(string part, int  depth)>();

            var depth = 0;
            foreach(var part in sum.ToCharArray())
            {
                if(part == '(')
                {
                    depth++;
                }
                if(part != '(' && part != ')')
                {
                    partDepths.Add((part.ToString(), depth));
                }
                if(part == ')')
                {
                    depth--;
                }
            }

            partDepths.Add(("end",-1));

            for(var i = partDepths.Select(o => o.depth).Max(); i >=0; i--)
            {
                var newParts = new List<(string part, int  depth)>(); 
                var previousDepth = -1;

                long total = 0;
                var lastOperator = string.Empty;

                foreach(var part in partDepths)
                {
                    if(part.depth != i)
                    {
                        if(previousDepth == i || part.part == "end")
                        {
                            if(previousDepth == i)
                            {
                                newParts.Add((total.ToString(), i -1 ));
                            }
                            if(part.part == "end")
                            {
                                newParts.Add(("end",-1));
                            }
                            else
                            {
                                newParts.Add(part);
                            }
                        }
                        else
                        {
                            newParts.Add(part);
                        }
                    }
                    if(part.depth == i)
                    {
                        if(part.depth != previousDepth)
                        {
                            total = 0;
                            lastOperator = string.Empty;
                        }
      
                        if(part.part == "*" || part.part == "+")
                        {
                            lastOperator = part.part;
                        }
                        else
                        {
                            if(string.IsNullOrEmpty(lastOperator))
                            {
                                total = long.Parse(part.part);
                            }
                            else
                            {
                                total = lastOperator == "*" ? total * long.Parse(part.part) : total + long.Parse(part.part);
                            }
                        }
                    }

                    previousDepth = part.depth;
                }

                partDepths = newParts;
            }

            return long.Parse(partDepths[0].part);
        }

        public long Calculate2()
        {
            
            foreach(var sum in Sums.Keys)
            {
                Sums[sum] = Calculate2(sum);
            }
            return Sums.Values.Sum();
        }

        public class SumPart
        {
            public string Part { get;set;}

            public int Depth { get;set;}
            
        }

        private long Calculate2(string sum)
        {
            sum = sum.Replace(" ","");
            
            var partDepths = new Dictionary<int, SumPart>();
            partDepths.Add(0, new SumPart { Part ="start", Depth = 0});

            var depth = 0;
            var array = sum.ToCharArray();

            var count = 1;
            foreach(var part in array)
            {
                if(part == '(')
                {
                    depth++;
                }
                if(part != '(' && part != ')')
                {
                    partDepths.Add(count, new SumPart { Part = part.ToString(), Depth = depth });
                    count++;
                }
                if(part == ')')
                {
                    depth--;
                }
            }

            partDepths.Add(count, new SumPart { Part ="end", Depth =-1 });


            //((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2 
            //  2 2 2 2 2  1  2 2 2 2 2 2 2  1 1  0 0 0 0 0 0
            //  3 3 3 2 2  1  3 3 3 2 3 3 3  1 1  0 0 0 0 0 0


            var parts = partDepths.Values.Where(o => o.Part != "start").ToList();

            var currentDepth = parts.Select(o => o.Depth).Max();

            while(currentDepth >= 0)
            {
                var lastPart = new SumPart();
                bool lastWasAdd = false;
                bool beforeLastWasAdd = false;
         
                foreach(var part in parts.Where(o => o.Depth == currentDepth))
                {
                    if(part.Part == "+")
                    {
                        if(!beforeLastWasAdd)
                        {
                            lastPart.Depth += 1;
                        }
                        else
                        {
                            beforeLastWasAdd = false;
                        }

                        part.Depth += 1;
                        lastWasAdd = true;
                    }
                    else if(lastWasAdd)
                    {
                        part.Depth += 1;
                        lastWasAdd = false;
                        beforeLastWasAdd = true;
                    }
                    else if(beforeLastWasAdd)
                    {
                        beforeLastWasAdd = false;
                    }
                    
                    lastPart = part;
                }

                currentDepth = parts.Select(o => o.Depth).Max();
                
                var newParts = new List<SumPart>(); 
                var previousDepth = -1;

                long total = 0;
                var lastOperator = string.Empty;

                foreach(var part in parts)
                {
                    if(part.Depth != currentDepth)
                    {
                        if(previousDepth == currentDepth || part.Part == "end")
                        {
                            if(previousDepth == currentDepth)
                            {
                                newParts.Add(new SumPart { Part = total.ToString(), Depth = currentDepth -1 });
                            }
                            if(part.Part == "end")
                            {
                                newParts.Add(new SumPart { Part =  "end", Depth = -1});
                            }
                            else
                            {
                                newParts.Add(part);
                            }
                        }
                        else
                        {
                            newParts.Add(part);
                        }
                    }
                    if(part.Depth == currentDepth)
                    {
                        if(part.Depth != previousDepth)
                        {
                            total = 0;
                            lastOperator = string.Empty;
                        }
      
                        if(part.Part == "*" || part.Part == "+")
                        {
                            lastOperator = part.Part;
                        }
                        else
                        {
                            if(string.IsNullOrEmpty(lastOperator))
                            {
                                total = long.Parse(part.Part);
                            }
                            else
                            {
                                total = lastOperator == "*" ? total * long.Parse(part.Part) : total + long.Parse(part.Part);
                            }
                        }
                    }

                    previousDepth = part.Depth;
                }
                
                parts = newParts;
                currentDepth = parts.Select(o => o.Depth).Max();
            }

            return long.Parse(parts[0].Part);
        }
    }

    public class Day18 : IDay
    {
        private readonly List<string> _input;

        public Day18(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var homework = new Homework();
            homework.Parse(_input);
            return homework.Calculate().ToString();
        }

        public string ChallengeTwo()
        {
            var homework = new Homework();
            homework.Parse(_input);
            return homework.Calculate2().ToString();
        }
    }
}