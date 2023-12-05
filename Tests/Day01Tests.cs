using System.Runtime.CompilerServices;
using Puzzles;

namespace Tests
{
    [TestFixture]
    public class Day01Tests: DayTestBase<Day01>
    {
        [TestCase(@"1",@"2")]
        public void PuzzleOneTest(string input, string output)
        {
            var puzzle = GetPuzzle(input);
            puzzle.GetPuzzleOneSolution();
            puzzle.GetPuzzleOneSolution();
            Assert.Pass();
        }

        [TestCase(@"1", @"2")]
        public void PuzzleTwoTest(string input, string output)
        {
            Assert.Pass();
        }
    }
}