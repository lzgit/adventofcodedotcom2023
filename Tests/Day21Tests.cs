using Puzzles;

namespace Tests;

[TestFixture]
public class Day21Tests : DayTestBase<Day21>
{
    [TestCase(@"6
...........
.....###.#.
.###.##..#.
..#.#...#..
....#.#....
.##..S####.
.##..#...#.
.......##..
.##.#.####.
.##..##.##.
...........", "16")]
    public void PuzzleOneTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"", "")]
    public void PuzzleTwoTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}