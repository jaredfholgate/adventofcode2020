using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Diagnostics;

namespace adventofcode.unittests
{
    [TestClass]
    public class Day13Tests
    {
        [TestMethod]
        public void ChallengeOne()
        {
            var input = File.ReadAllLines("Day13.txt");
            var day = new Day13(input.ToList());
            var result = day.ChallengeOne();
        }
         
        [TestMethod]
        public void ChallengeTwo()
        {
            var input = File.ReadAllLines("Day13.txt");
            var day = new Day13(input.ToList());
            var result = day.ChallengeTwo();
        }
    }
}
