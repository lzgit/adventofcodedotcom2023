using Puzzles;

namespace Tests;

[TestFixture]
public class Day10Tests : DayTestBase<Day10>
{
    [TestCase(@"-L|F7
7S-7|
L|7||
-L-J|
L|-JF", "4")]
    public void PuzzleOneTestShort(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"..F7.
.FJ|.
SJ.L7
|F--J
LJ...", "8")]
    public void PuzzleOneTestLong(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"", "")]
    public void PuzzleTwoTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}