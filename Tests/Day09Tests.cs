using Puzzles;

namespace Tests;

[TestFixture]
public class Day09Tests : DayTestBase<Day09>
{
    [TestCase(@"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45", "114")]
    public void PuzzleOneTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45", "2")]
    public void PuzzleTwoTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}