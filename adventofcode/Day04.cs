using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    public class Passport
    {
        public Passport()
        {
            Data = new Dictionary<string, string>();
        }
        public Dictionary<string,string> Data { get;set; }
 
        public bool IsValid(List<string> criteria, Validator validator = null)
        {
            var isValid = true;
            foreach(var criterion in criteria)
            {
                if(!Data.ContainsKey(criterion))
                {
                    isValid = false;
                }
                else
                {
                    if(validator != null)
                    {
                        if(!validator.IsValid(criterion, Data[criterion]))
                        {
                            isValid = false;
                        }
                    }
                }
            }
            return isValid;
        }
    }

    public class Parser 
    {
        public static List<Passport> Parse(List<string> input)
        {
            var data = new List<Passport>(){
                new Passport()
            };

            foreach(var row in input)
            {
                if(row == string.Empty)
                {
                    data.Add(new Passport());
                }
                else
                {
                    var attributes = row.Split(new [] { ' ' });
                    foreach(var attribute in attributes)
                    {
                        var kvp  = attribute.Split(new char[] { ':'});
                        data.Last().Data.Add(kvp[0], kvp[1]);
                    }
                }
            }
            return data;
        }
    }

    public class Validator
    {
        public bool IsValid(string key, string value)
        {
            bool isValid = false;
            switch(key)
            {
                case "byr":
                    isValid = YearValidator(value,1920,2002);
                    break;
                case "iyr":
                    isValid = YearValidator(value, 2010, 2020);
                    break;
                case "eyr":
                    isValid = YearValidator(value, 2020, 2030);
                    break;
                case "hgt":
                    isValid = HeightValidator(value);
                    break;
                case "hcl":
                    isValid = ColourValidator(value);
                    break;
                case "ecl":
                    isValid = EyeColourValidator(value);
                    break;
                case "pid":
                    isValid = PassportNumberValidator(value);
                    break;
                
            }
            return isValid;
        }

        private bool PassportNumberValidator(string passportNumber)
        {
            var isValid = false;
            if(passportNumber.Length == 9)
            {
                int parsed;
                if(int.TryParse(passportNumber, out parsed))
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        private bool EyeColourValidator(string eyeColour)
        {
            var isValid = false;
            var matches = "amb blu brn gry grn hzl oth".Split(new char[] { ' '});
            if(matches.Contains(eyeColour))
            {
                isValid = true;
            }
            return isValid;
        }

        private bool ColourValidator(string colour)
        {
            var isValid = false;
            if(Regex.Match(colour,"^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
            {
                isValid = true;
            }
            return isValid;
        }

        private bool HeightValidator(string height)
        {
            var isValid = false;
            if(height.EndsWith("in"))
            {
                int parsedHeight;
                if(int.TryParse(height.Replace("in",""), out parsedHeight))
                {
                    if(parsedHeight >= 59 && parsedHeight <= 76)
                    {
                        isValid = true;
                    }
                }
            }
            if(height.EndsWith("cm"))
            {
                int parsedHeight;
                if(int.TryParse(height.Replace("cm",""), out parsedHeight))
                {
                    if(parsedHeight >= 150 && parsedHeight <= 193)
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }
        private  bool YearValidator(string year, int min, int max)
        {
            var isValid = false;
            if(year.Length == 4)
            {
                int parsedYear;
                if(int.TryParse(year, out parsedYear))
                {
                    if(parsedYear >= min && parsedYear <= max)
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }
        
    }

    public class Day04 : IDay
    {
        private readonly List<Passport> _input;

        public Day04(List<string> input)
        {
            _input = Parser.Parse(input); 
        }

        public string ChallengeOne()
        {
            var criteria = new List<string> 
            {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid"
            };

            var count = 0;

            foreach(var passport in _input)
            {
                count += passport.IsValid(criteria) ? 1 : 0;
            }
            return count.ToString();
        }


        public string ChallengeTwo()
        {
            var criteria = new List<string> 
            {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid"
            };

            var count = 0;

            foreach(var passport in _input)
            {
                count += passport.IsValid(criteria, new Validator()) ? 1 : 0;
            }
            return count.ToString();
        }
    }
}