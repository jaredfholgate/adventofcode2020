using System.Collections.Generic;
using System;
using System.Linq;

namespace adventofcode
{
    public class PasswordData
    {
        public string Password { get;set;}
        public int Low {get;set;}
        public int High {get;set;}

        public char Letter { get;set;}

        public static PasswordData Parse(string data)
        {
            var split = data.Split(new [] { "-"," ",":" }, StringSplitOptions.RemoveEmptyEntries);
            return new PasswordData 
            {
                Password = split[3],
                Low = Convert.ToInt32(split[0]),
                High = Convert.ToInt32(split[1]),
                Letter = Convert.ToChar(split[2])
            };
        }

        public bool IsValid()
        {
            var count = 0;
            foreach(var letter in Password)
            {
                count += (letter == Letter ? 1 : 0);
            }
            return count >= Low && count <= High;
        }
        
        public bool IsValid2()
        {
            return (Password[Low -1] == Letter || Password[High-1] == Letter) && (Password[Low -1] != Password[High-1]) ;
        }
    }

    public class Day02 : IDay
    {
        private readonly List<PasswordData> _input;

        public Day02(List<string> input)
        {
            _input = input.Select(o => PasswordData.Parse(o)).ToList();
        }

        public string ChallengeOne()
        {
            var count = 0;
            foreach(var password in _input)
            {
                count += password.IsValid() ? 1 : 0;
            }
            return count.ToString();
        }

        public string ChallengeTwo()
        {
                        var count = 0;
            foreach(var password in _input)
            {
                count += password.IsValid2() ? 1 : 0;
            }
            return count.ToString();
        }
    }
}