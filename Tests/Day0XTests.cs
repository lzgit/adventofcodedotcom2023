using Puzzles;

namespace Tests;

[TestFixture]
public class Day0XTests : DayTestBase<Day0X>
{
    [TestCase(@"", "")]
    public void PuzzleOneTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"", "")]
    public void PuzzleTwoTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}