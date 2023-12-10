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

    [TestCase(@".....
.S-7.
.|.|.
.L-J.
.....", "1")]
    public void PuzzleTwoTestSimpleCircle(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));

    [TestCase(@"-L|F7
7S-7|
L|7||
-L-J|
L|-JF", "1")]
    public void PuzzleTwoTestSimpleCircleWithJunkPipes(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));

    [TestCase(@"..F7.
.FJ|.
SJ.L7
|F--J
LJ...", "1")]
    public void PuzzleTwoTestSimpleCurves(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));

    [TestCase(@"...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........", "4")]
    public void PuzzleTwoTestSymmetricCurvesWithConnectionToTheBorder(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));

    [TestCase(@".F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ...", "8")]
    public void PuzzleTwoTestComplexCase(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
    
    [TestCase(@"FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJ7F7FJ-
L---JF-JLJ.||-FJLJJ7
|F|F-JF---7F7-L7L|7|
|FFJF7L7F-JF7|JL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L", "10")]
    public void PuzzleTwoTestComplexCaseWithJunkPipes(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}