using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode
{
    public class Joltage
    {
        public Dictionary<int, int> Jolts { get;set;}

        public void Parse(List<string> inputs)
        {
            Jolts = new Dictionary<int, int>();
            var orderedJolts = inputs.Select(o => int.Parse(o)).OrderBy(o => o);

            int count = 1;
            Jolts.Add(0,0);
            foreach(var input in orderedJolts)
            {
                Jolts.Add(input,count);
                count += 1;
            }
            Jolts.Add(orderedJolts.Max() + 3, count);
        }

        public int GetDifferences()
        {
            var lastJolt = 0;
            foreach(var key in Jolts.Keys)
            {
                Jolts[key] = key - lastJolt;
                lastJolt = key;
            }

            return Jolts.Count(o => o.Value == 1) * Jolts.Count(o => o.Value == 3);
        }

        private const int _maxJolt = 167;

        private class Edge
        {
            public int Start { get;set; }
            public int End { get;set; }
        }

        private class Graph
        {
            public Graph()
            {
                Edges = new List<Edge>();
            }
            public List<Edge> Edges { get;set; }
        }

        public long CountArrangements()
        {
            var graph  = new Graph();
            foreach(var jolt in Jolts.Keys.OrderBy(o => o))
            {
                foreach(var child in Jolts.Keys.Where(o => o > jolt && o <= jolt + 3))
                {
                    graph.Edges.Add(new Edge {  Start = Jolts[jolt], End = Jolts[child] });
                }
            }

            int[, ] graph2 = new int[V,V];
            
            for(int start = 0; start < V; start++)
            {
                for(int end = 0; end < V; end++)
                {
                    graph2[start,end] = graph.Edges.Count(o => o.Start == start && o.End == end);
                }
            }
           
            int u = 0, v = 99, k = 100;
    
            var count = CountWalks(graph2, u, v, k);
            return count;
        }

        static int V = 100;
 
        // Method slightly butcherd, but stolen from hwre: https://www.geeksforgeeks.org/count-number-edges-undirected-graph/
        private long CountWalks(int[, ] graph, int start, int end, int maximumEdges)
        {
            // Table to be filled up using DP. The
            // value count[i][j][e] will/ store
            // count of possible walks from i to
            // j with exactly k edges
            long[,, ] count = new long[V, V, maximumEdges + 1];
    
            // Loop for number of edges from 0 to k
            for (int e = 0; e <= maximumEdges; e++) {
    
                // for source
                for (int i = 0; i < V; i++) {
    
                    // for destination
                    for (int j = 0; j < V; j++) {
                        // initialize value
                        count[i, j, e] = 0;
    
                        // from base cases
                        if (e == 0 && i == j)
                            count[i, j, e] = 1;
                        if (e == 1 && graph[i, j] != 0)
                            count[i, j, e] = 1;
    
                        // go to adjacent only when
                        // number of edges
                        // is more than 1
                        if (e > 1) {
                            // adjacent of i
                            for (int a = 0; a < V; a++)
                                if (graph[i, a] != 0)
                                    count[i, j, e] += count[a, j, e - 1];
                        }
                    }
                }
            }
    
            long sum = 0;
            for(int i = 0; i <= 100; i++)
            {
                sum += count[0,99,i];
            }
            return sum;
        }
    }

    public class Day10 : IDay
    {
        private readonly List<string> _input;

        public Day10(List<string> input)
        {
            _input = input; 
        }

        public string ChallengeOne()
        {
            var joltage = new Joltage();
            joltage.Parse(_input);
            return joltage.GetDifferences().ToString();
        }

        public string ChallengeTwo()
        {
            var joltage = new Joltage();
            joltage.Parse(_input);
            return joltage.CountArrangements().ToString();
        }
    }
}