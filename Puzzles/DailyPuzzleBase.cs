namespace Puzzles;

public abstract class DailyPuzzleBase(string input)
{
    protected string Input = input;
    public abstract string GetPuzzleOneSolution();
    public abstract string GetPuzzleTwoSolution();
}