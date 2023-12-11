using Puzzles;

namespace Tests;

[TestFixture]
public class Day11Tests : DayTestBase<Day11>
{
    [TestCase(@"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....", "374")]
    public void PuzzleOneTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"", "")]
    public void PuzzleTwoTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}