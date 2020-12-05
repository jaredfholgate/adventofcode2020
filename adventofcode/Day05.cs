using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    public class Seat
    {
        public int Row { get;set; }

        public int Column { get;set; }

        public static Seat Parse(string input)
        {
            var rowLetters = input.Substring(0,7).Replace("F","0").Replace("B", "1");
            var columnnLetters = input.Substring(7,3).Replace("L","0").Replace("R", "1");

            return new Seat 
            {
                Row = Convert.ToInt32(rowLetters, 2),
                Column = Convert.ToInt32(columnnLetters, 2)
            };
        }

        public int Id()
        {
            return (Row * 8) + Column;
        }
    }

    public class Day05 : IDay
    {
        private readonly List<string> _input;

        public Day05(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var maxID = -1;
            foreach(var input in _input)
            {
                var id = Seat.Parse(input).Id();
                if(id > maxID)
                {
                    maxID = id;
                }
            }
            return maxID.ToString();
        }


        public string ChallengeTwo()
        {
            var seats = _input.Select(o => Seat.Parse(o)).OrderBy(o => o.Id()).ToList();

            var missingId = -1;
            var lastId = seats[0].Id() -1;
            foreach(var seat in seats)
            {
                var currentId = seat.Id();
                if(lastId != currentId -1)
                {
                    missingId = currentId -1;
                }
                lastId = currentId;
            }
            return missingId.ToString();
        }
    }
}