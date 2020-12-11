using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace adventofcode
{

    public class Day11 : IDay
    {
        private readonly List<string> _input;

        public Day11(List<string> input)
        {
            _input = input; 
        }

               public class FerrySeats
        {
            public int[,] Seats { get ;set;}

            public int TotalRows { get;set;}

            public int SeatsPerRow { get;set;}

            public void Parse(List<string> input)
            {
                TotalRows = input.Count;
                SeatsPerRow = input[0].Length;
                Seats = new int[TotalRows, SeatsPerRow];
                for(var row = 0; row < TotalRows; row++)
                {
                    var rowString = input[row].ToCharArray();
                    for(var seat = 0; seat < SeatsPerRow; seat++)
                    {
                         Seats[row,seat] = rowString[seat] == '.' ? -1 : 0;
                    }
                }
                ShowSeats(0);
            }

                        public int GetSeated()
            {
                var currentHash = GetHashOfOccupied();
                var lastHash = -1;
                var count = 0;

                
                while(currentHash != lastHash)
                {
                    count++;
                    lastHash = currentHash;

                    var copy = (int[,])Seats.Clone();

                    for(var row = 0; row < TotalRows; row++)
                    {
                        for(var seat = 0; seat < SeatsPerRow; seat++)
                        {
                            if(Seats[row,seat] != -1)
                            {
                                var currentValue = Seats[row,seat];
                                if(currentValue == 1 && HasOccupiedSurroundingSeats(row, seat, 4))
                                {
                                    copy[row,seat] = 0;
                                }
                                else if(currentValue == 0 && !HasOccupiedSurroundingSeats(row, seat, 1))
                                {
                                    copy[row,seat] = 1;
                                }  
                            }
                        }
                    }

                    Seats = copy;
                    currentHash = GetHashOfOccupied();
                    ShowSeats(count);
                }

                return GetSumOFOccupied();
            }

            public int GetSeatedVisible()
            {
                var currentHash = GetHashOfOccupied();
                var lastHash = -1;
                var count = 0;

                
                while(currentHash != lastHash)
                {
                    count++;
                    lastHash = currentHash;

                    var copy = (int[,])Seats.Clone();

                    for(var row = 0; row < TotalRows; row++)
                    {
                        for(var seat = 0; seat < SeatsPerRow; seat++)
                        {
                            if(Seats[row,seat] != -1)
                            {
                                var currentValue = Seats[row,seat];
                                if(currentValue == 1 && HasVisibleOccupiedSurroundingSeats(row, seat, 5))
                                {
                                    copy[row,seat] = 0;
                                }
                                else if(currentValue == 0 && !HasVisibleOccupiedSurroundingSeats(row, seat, 1))
                                {
                                    copy[row,seat] = 1;
                                }  
                            }
                        }
                    }

                    Seats = copy;
                    currentHash = GetHashOfOccupied();
                    ShowSeats(count);
                }

                return GetSumOFOccupied();
            }

            private void ShowSeats(int iteration)
            {
                var lines = new List<string>();
                for(var row = 0; row < TotalRows; row++)
                {
                    var build = new StringBuilder();
                    for(var seat = 0; seat < SeatsPerRow; seat++)
                    {
                        var parsedSeat = Seats[row,seat].ToString().Replace("-1",".").Replace("1","#").Replace("0","L");
                        build.Append(parsedSeat);
                    }
                    lines.Add(build.ToString());

                }
                File.WriteAllLines(@$"c:\Test\Debug{iteration}.txt", lines);
            }

            private int GetHashOfOccupied()
            {
                var build = new StringBuilder();
                for(var row = 0; row < TotalRows; row++)
                {
                    for(var seat = 0; seat < SeatsPerRow; seat++)
                    {
                        build.Append(Seats[row,seat].ToString());
                    }
                }
                
                return build.ToString().GetHashCode();
            }

            private int GetSumOFOccupied()
            {
                var sum = 0;
                for(var row = 0; row < TotalRows; row++)
                {
                    for(var seat = 0; seat < SeatsPerRow; seat++)
                    {
                        sum += Seats[row,seat] == 1 ? 1 : 0;;
                    }
                }
                
                return sum;
            }

            private bool HasVisibleOccupiedSurroundingSeats(int row, int seat, int occupiedMaxCount)
            {
                var foundASeat = new HashSet<int>();
                var occupiedCount = 0;
                for(var iteration = 1; iteration < SeatsPerRow; iteration++)
                {
                    var seatsToCheck = new Dictionary<int,Tuple<int, int>>();
                    if(!foundASeat.Contains(0))
                    {
                        seatsToCheck.Add(0,new Tuple<int, int>(row - iteration, seat - iteration));
                    }
                    if(!foundASeat.Contains(1))
                    {
                        seatsToCheck.Add(1,new Tuple<int, int>(row - iteration, seat));
                    }
                    if(!foundASeat.Contains(2))
                    {
                        seatsToCheck.Add(2,new Tuple<int, int>(row - iteration, seat + iteration));
                    }
                    if(!foundASeat.Contains(3))
                    {
                        seatsToCheck.Add(3,new Tuple<int, int>(row, seat - iteration));
                    }
                    if(!foundASeat.Contains(4))
                    {
                        seatsToCheck.Add(4,new Tuple<int, int>(row, seat + iteration));
                    }
                    if(!foundASeat.Contains(5))
                    {
                        seatsToCheck.Add(5,new Tuple<int, int>(row + iteration, seat - iteration));
                    }
                    if(!foundASeat.Contains(6))
                    {
                        seatsToCheck.Add(6,new Tuple<int, int>(row + iteration, seat));
                    }
                    if(!foundASeat.Contains(7))
                    {
                        seatsToCheck.Add(7,new Tuple<int, int>(row + iteration, seat + iteration));    
                    }          

                    foreach(var key in seatsToCheck.Keys)
                    {
                        if(seatsToCheck[key].Item1 >= 0 && seatsToCheck[key].Item1 < TotalRows  && seatsToCheck[key].Item2 >= 0 && seatsToCheck[key].Item2 < SeatsPerRow)
                        {
                            if(Seats[seatsToCheck[key].Item1, seatsToCheck[key].Item2] != -1)
                            {
                                foundASeat.Add(key);
                            }
                            if(Seats[seatsToCheck[key].Item1, seatsToCheck[key].Item2] == 1)
                            {
                                occupiedCount += 1;
                            }
                        }
                    }
                    if(occupiedCount >= occupiedMaxCount)
                    {
                        return true;
                    }
                }
                return occupiedCount >= occupiedMaxCount;
            }

            private bool HasOccupiedSurroundingSeats(int row, int seat, int occupiedMaxCount)
            {
                var rows = Enumerable.Range(row - 1, 3);
                var seats = Enumerable.Range(seat - 1, 3);

                var occupiedCount = 0;

                foreach(var testRow in rows)
                {
                    foreach(var testSeat in seats)
                    {
                        if(testRow >= 0 && testRow < TotalRows  && testSeat >= 0 && testSeat < SeatsPerRow)
                        {
                            if(!(testRow == row && testSeat == seat))
                            {
                                if(Seats[testRow, testSeat] == 1)
                                {
                                    occupiedCount += 1;
                                }
                            }
                        }
                    }
                }

                return occupiedCount >= occupiedMaxCount;
            }
        }

        public string ChallengeOne()
        {
            var ferrySeats = new FerrySeats();
            ferrySeats.Parse(_input);
            return ferrySeats.GetSeated().ToString();
        }

        public string ChallengeTwo()
        {
            var ferrySeats = new FerrySeats();
            ferrySeats.Parse(_input);
            return ferrySeats.GetSeatedVisible().ToString();
        }
    }
}