using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace adventofcode
{
    public class ConwayCubes3D
    {
        public Dictionary<int,Dictionary<int,Dictionary<int, int>>> Space { get;set;}

        public void Parse(List<string> inputs)
        {
            Space = new Dictionary<int, Dictionary<int, Dictionary<int, int>>>();

            for(var y = 0; y < inputs.Count; y++)
            {
                for(var x = 0; x < inputs[y].Length; x++)
                {
                    var line = inputs[y].ToCharArray();
                    CheckAndAdd(x, y, 0, line[x] == '.' ? 0 : 1, Space);
                }
            }
        }

        private void CheckAndAdd(int x, int y, int z, int value, Dictionary<int, Dictionary<int, Dictionary<int, int>>> space)
        {
            if(!space.ContainsKey(x))
            {
                space.Add(x, new Dictionary<int, Dictionary<int, int>>());
            }

            if(!space[x].ContainsKey(y))
            {
                space[x].Add(y,new Dictionary<int, int>());
            }

            if(!space[x][y].ContainsKey(z))
            {
                space[x][y].Add(z,0);
            }

            space[x][y][z] = value;
        }

        

        public void Iterate()
        {
            var newSpace = new Dictionary<int,Dictionary<int,Dictionary<int, int>>>();

            var xMin = Space.Keys.Min();
            var xMax = Space.Keys.Max();
            var yMin = Space[0].Keys.Min();
            var yMax = Space[0].Keys.Max();
            var zMin = Space[0][0].Keys.Min();
            var zMax = Space[0][0].Keys.Max();


            for(var x = xMin - 1; x <= xMax + 1; x++)
            {
                for(var y = yMin - 1; y <= yMax + 1; y++)
                {
                    for(var z = zMin - 1; z <= zMax + 1; z++)
                    {
                        var currentValue = GetValue(x,y,z, Space);
                        var count = CountSurrounding(x, y, z, xMin, xMax, yMin, yMax, zMin, zMax);
                        if(currentValue == 1 && !(count == 2 || count == 3))
                        {
                            CheckAndAdd(x,y,z,0,newSpace);
                        }
                        else if(currentValue == 0 && count == 3)
                        {
                            CheckAndAdd(x,y,z,1,newSpace);
                        }
                        else
                        {
                            CheckAndAdd(x,y,z,currentValue, newSpace);
                        }
                    }
                }
            }

            Space = newSpace;
        }

       public void Print(int iteration)
       {
           var lines = new List<string>();


            foreach(var z in Space[0][0].Keys)
            {
                lines.Add("");
                lines.Add($"z:{z}");

                foreach(var y in Space[0].Keys)
                {   
                    var builder = new StringBuilder();
                    foreach(var x in Space.Keys)
                    {
                        builder.Append(GetValue(x,y,z,Space) == 1 ? '#' : '.');                        
                    }
                    lines.Add(builder.ToString());   
                }  
                           
            }

            File.WriteAllLines($@"c:\test\iteration{iteration}.txt", lines);

            
        }

        public long Count()
        {
            long count = 0;

            foreach(var x in Space.Keys)
            {
                foreach(var y in Space[x].Keys)
                {
                    foreach(var z in Space[x][y].Keys)
                    {
                        count += GetValue(x,y,z,Space);
                    }
                }
            }

            return count;
        }

        private int GetValue(int x,int y, int z, Dictionary<int,Dictionary<int,Dictionary<int, int>>> space)
        {
            if(space.ContainsKey(x) && space[x].ContainsKey(y) && space[x][y].ContainsKey(z))
            {
                return space[x][y][z];
            }
            return 0;
        }

        public int CountSurrounding(int x, int y, int z, int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
        {
            var xRange = Enumerable.Range(x-1, 3);
            var yRange = Enumerable.Range(y-1, 3);
            var zRange = Enumerable.Range(z-1, 3);

            var count = 0;
            var checkCount = 0;

            foreach(var xCheck in xRange)
            {
                foreach(var yCheck in yRange)
                {
                    foreach(var zCheck in zRange)
                    {
                        checkCount++;
                        bool skip = false;
                        if(xCheck == x && yCheck == y && zCheck == z)
                        {
                            skip = true;
                        }
                        
                        if(xCheck < xMin || xCheck > xMax)
                        {
                            skip = true;
                        }

                        if(yCheck < yMin || yCheck > yMax)
                        {
                            skip = true;
                        }

                        if(zCheck < zMin || zCheck > zMax)
                        {
                            skip = true;
                        }

                        if(!skip)
                        {
                            count += Space[xCheck][yCheck][zCheck];
                        }
                    }
                }
            }

            return count;

        }
        

    }

    public class ConwayCubes4D
    {
        public Dictionary<int,Dictionary<int,Dictionary<int, Dictionary<int, int>>>> Space { get;set;}

        public void Parse(List<string> inputs)
        {
            Space  = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, int>>>>();
            for(var y = 0; y < inputs.Count; y++)
            {
                for(var x = 0; x < inputs[y].Length; x++)
                {
                    var line = inputs[y].ToCharArray();
                    CheckAndAdd(x, y, 0, 0, line[x] == '.' ? 0 : 1, Space);
                }
            }
        }

        private void CheckAndAdd(int x, int y, int z, int w, int value, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int,int>>>> space)
        {
            if(!space.ContainsKey(x))
            {
                space.Add(x, new Dictionary<int, Dictionary<int, Dictionary<int,int>>>());
            }

            if(!space[x].ContainsKey(y))
            {
                space[x].Add(y,new Dictionary<int, Dictionary<int,int>>());
            }

            if(!space[x][y].ContainsKey(z))
            {
                space[x][y].Add(z,new Dictionary<int, int>());
            }

            if(!space[x][y][z].ContainsKey(z))
            {
                space[x][y][z].Add(w,0);
            }

            space[x][y][z][w] = value;
        }

        

        public void Iterate()
        {
            var newSpace = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int,int>>>>();

            var xMin = Space.Keys.Min();
            var xMax = Space.Keys.Max();
            var yMin = Space[0].Keys.Min();
            var yMax = Space[0].Keys.Max();
            var zMin = Space[0][0].Keys.Min();
            var zMax = Space[0][0].Keys.Max();
            var wMin = Space[0][0][0].Keys.Min();
            var wMax = Space[0][0][0].Keys.Max();


            for(var x = xMin - 1; x <= xMax + 1; x++)
            {
                for(var y = yMin - 1; y <= yMax + 1; y++)
                {
                    for(var z = zMin - 1; z <= zMax + 1; z++)
                    {
                        for(var w = wMin - 1; w <= wMax + 1; w++)
                        {
                            var currentValue = GetValue(x,y,z,w, Space);
                            var count = CountSurrounding(x, y, z,w, xMin, xMax, yMin, yMax, zMin, zMax, wMin, wMax);
                            if(currentValue == 1 && !(count == 2 || count == 3))
                            {
                                CheckAndAdd(x,y,z,w,0,newSpace);
                            }
                            else if(currentValue == 0 && count == 3)
                            {
                                CheckAndAdd(x,y,z,w,1,newSpace);
                            }
                            else
                            {
                                CheckAndAdd(x,y,z,w,currentValue, newSpace);
                            }
                        }
                    }
                }
            }

            Space = newSpace;
        }

        public long Count()
        {
            long count = 0;

            foreach(var x in Space.Keys)
            {
                foreach(var y in Space[x].Keys)
                {
                    foreach(var z in Space[x][y].Keys)
                    {
                        foreach(var w in Space[x][y][z].Keys)
                        {
                            count += GetValue(x,y,z,w,Space);
                        }
                    }
                }
            }

            return count;
        }

        private int GetValue(int x,int y, int z, int w, Dictionary<int,Dictionary<int,Dictionary<int, Dictionary<int,int>>>> space)
        {
            if(space.ContainsKey(x) && space[x].ContainsKey(y) && space[x][y].ContainsKey(z) && space[x][y][z].ContainsKey(w))
            {
                return space[x][y][z][w];
            }
            return 0;
        }

        public int CountSurrounding(int x, int y, int z, int w, int xMin, int xMax, int yMin, int yMax, int zMin, int zMax,  int wMin, int wMax)
        {
            var xRange = Enumerable.Range(x-1, 3);
            var yRange = Enumerable.Range(y-1, 3);
            var zRange = Enumerable.Range(z-1, 3);
            var wRange = Enumerable.Range(w-1, 3);

            var count = 0;
            var checkCount = 0;

            foreach(var xCheck in xRange)
            {
                foreach(var yCheck in yRange)
                {
                    foreach(var zCheck in zRange)
                    {
                        foreach(var wCheck in wRange)
                        {
                            checkCount++;
                            bool skip = false;
                            if(xCheck == x && yCheck == y && zCheck == z && wCheck == w)
                            {
                                skip = true;
                            }
                            
                            if(xCheck < xMin || xCheck > xMax)
                            {
                                skip = true;
                            }

                            if(yCheck < yMin || yCheck > yMax)
                            {
                                skip = true;
                            }

                            if(zCheck < zMin || zCheck > zMax)
                            {
                                skip = true;
                            }

                            if(wCheck < wMin || wCheck > wMax)
                            {
                                skip = true;
                            }

                            if(!skip)
                            {
                                count += Space[xCheck][yCheck][zCheck][wCheck];
                            }
                        }
                    }
                }
            }

            return count;

        }
        

    }

    public class Day17 : IDay
    {
        private readonly List<string> _input;

        public Day17(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var cubes = new ConwayCubes3D();
            cubes.Parse(_input);
            cubes.Print(0);
            cubes.Iterate();
            cubes.Print(1);
            cubes.Iterate();
            cubes.Print(2);
            cubes.Iterate();
            cubes.Print(3);
            cubes.Iterate();
            cubes.Print(4);
            cubes.Iterate();
            cubes.Print(5);
            cubes.Iterate();
            cubes.Print(6);
            return cubes.Count().ToString();
        }

        public string ChallengeTwo()
        {
            var cubes = new ConwayCubes4D();
            cubes.Parse(_input);
            cubes.Iterate();
            cubes.Iterate();
            cubes.Iterate();
            cubes.Iterate();
            cubes.Iterate();
            cubes.Iterate();
            return cubes.Count().ToString();
        }
    }
}