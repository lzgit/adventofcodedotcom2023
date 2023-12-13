using Puzzles;

namespace Tests;

[TestFixture]
public class Day13Tests : DayTestBase<Day13>
{
    [TestCase(@"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#", "405")]
    public void PuzzleOneTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));
    
    [TestCase(@"##...##
..#.#..
#...#..
.###.##
##.#.##
##..###
##.....
.#.##..
.#..#..", "6")]
    public void PuzzleOneTestSmallEndColumnMirror(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"#...####.
#.##.#.#.
####.####
...##.#..
....###.#
....###.#
...##.#..
.###.####
#.##.#.#.
#...####.
..#.#..#.
####.#...
##.####.#
##.####.#
####.#...", "1300")]
    public void PuzzleOneTestSmallEndRowMirror(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#", "400")]
    public void PuzzleTwoTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}