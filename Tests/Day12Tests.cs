using Puzzles;

namespace Tests;

[TestFixture]
public class Day12Tests : DayTestBase<Day12>
{
    [TestCase(@"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1", "21")]
    public void PuzzleOneTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"?###???????? 3,2,1", "10")]
    public void PuzzleOneTestOneRow(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"..?????#?? 4,1", "2")]
    public void PuzzleOneTestOneRow1(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));

    [TestCase(@"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1", "525152")]
    public void PuzzleTwoTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}