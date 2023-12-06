using Puzzles;
using static NUnit.Framework.Assert;

namespace Tests
{
    [TestFixture]
    public class Day01Tests: DayTestBase<Day01>
    {
        [TestCase(@"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet", "142")]
        public void PuzzleOneTest(string input, string output) => That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

        [TestCase(@"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen", "281")]
        public void PuzzleTwoTest(string input, string output) => That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
    }
}
//12 red cubes, 13 green cubes, and 14 blue