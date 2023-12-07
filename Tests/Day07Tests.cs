using Puzzles;

namespace Tests;

[TestFixture]
public class Day07Tests : DayTestBase<Day07>
{
    [TestCase(@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483", "6440")]
    public void PuzzleOneTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));
    
    [TestCase(@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483", "5905")]
    public void PuzzleTwoTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}