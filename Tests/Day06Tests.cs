using Puzzles;

namespace Tests;

[TestFixture]
public class Day06Tests : DayTestBase<Day06>
{
    [TestCase(@"Time:      7  15   30
Distance:  9  40  200", "288")]
    public void PuzzleOneTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"Time:      7  15   30
Distance:  9  40  200", "71503")]
    public void PuzzleTwoTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}